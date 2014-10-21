using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using KSPComputer.Helpers;
using KSPComputer.Nodes;
using KSPComputer.Types;
namespace KSPComputer
{
    [Serializable]
    public class FlightProgram
    {
        public delegate void FlightEventHandler();
        public event FlightEventHandler OnTick;
        public event FlightEventHandler OnLaunch;
        [NonSerialized]
        private Vessel vessel;
        [NonSerialized]
        private SASController sasController;
        [NonSerialized]
        private VesselInformation vesselInformation;
        public Vessel Vessel
        {
            get
            {
                return vessel;
            }
        }

        public SASController SASController
        {
            get
            {
                return sasController;
            }
        }
        public VesselInformation VesselInfo
        {
            get
            {
                return vesselInformation;
            }
        }
        
        public List<Node> Nodes { get; private set; }
        public FlightProgram()
        {
            Nodes = new List<Node>();
        }
        public void Init(Vessel vessel)
        {
            this.vessel = vessel;
            this.sasController= new SASController(this);
            this.vesselInformation = new VesselInformation(this);
            Log.Write("Initializing flight program");
        }
        public void Launch()
        {
            Log.Write("Vehicle launching");
            if (SASController == null || Vessel == null)
            {
                Log.Write("Something went wrong, was init called ? (FlightProgram)");
                if (SASController == null)
                    Log.Write("SASController = null");
                if (Vessel == null)
                    Log.Write("Vessel = null");
            }
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
            //Log.Write("Vessel: " + Vessel.ToString());
            SASController.Update();
            VesselInfo.Update();
            if (OnTick != null)
                OnTick();
        }
    }
}
