using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using KSPFlightPlanner.Program.Connectors;
using KSPFlightPlanner.Program.Nodes;
namespace KSPFlightPlanner.Program
{
    [Serializable]
    public class FlightProgram
    {
        [NonSerialized]
        private FPComputer module;
        [NonSerialized]
        public SASController SASController;
        public Vessel Vessel
        {
            get
            {
                if (module == null)
                    return null;
                return module.vessel;
            }
        }
        public delegate void FlightEventHandler();
        public event FlightEventHandler OnTick;
        public event FlightEventHandler OnLaunch;
        public List<Node> Nodes { get; private set; }
        public FlightProgram()
        {
            Nodes = new List<Node>();
        }
        public void Init(FPComputer module)
        {
            this.module = module;
            SASController = new SASController(this);
        }
        public void Launch()
        {
            SASController.SASTarget = Vessel.ReferenceTransform.rotation;
            if (OnLaunch != null)
                OnLaunch();
        }
        public Node AddNode(Type nodeType, Vector2 position)
        {
            if (nodeType.IsSubclassOf(typeof(Node)))
            {
                Node n = Activator.CreateInstance(nodeType) as Node;
                n.Init(this);
                n.Position = new SVector2(position.x, position.y);
                Nodes.Add(n);
                return n;
            }
            else
            {
                throw (new Exception("Type " + nodeType + " is not a valid node type"));
            }
        }
        public void RemoveNode(Node node)
        {
            foreach (var i in node.Inputs)
                i.Value.DisconnectAll();
            foreach (var i in node.Outputs)
                i.Value.DisconnectAll();
            Nodes.Remove(node);
        }
        public void Update()
        {
            SASController.Update();
            if (OnTick != null)
                OnTick();
        }
    }
}
