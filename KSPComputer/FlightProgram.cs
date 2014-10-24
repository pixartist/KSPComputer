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
using KSPComputer.Variables;
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
        public Dictionary<string, Variable> Variables { get; private set; }
        public List<Node> Nodes { get; private set; }
        public FlightProgram()
        {
            Nodes = new List<Node>();
            Variables = new Dictionary<string, Variable>();
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
        public Node AddVariableNode(Vector2 position, string variable)
        {
            Variable var;
            if(Variables.TryGetValue(variable, out var))
            {
                Log.Write("Adding variable node for " + variable);
                var type = typeof(VariableNode<>).MakeGenericType(var.Type);
                Log.Write("Type: " + type);
                Node n = Activator.CreateInstance(type) as Node;
                Log.Write("Node: " + n);
                var mi = type.GetMethod("SetVariable", BindingFlags.Instance | BindingFlags.NonPublic);
                Log.Write("Method: " + mi);
                mi.Invoke(n, new object[] { var });
                Log.Write("Init");
                n.Init(this);
                n.Position = new SVector2(position.x, position.y);
                Nodes.Add(n);
                return n;
            }
            else
            {
                throw (new Exception("Variable " + variable + " does not exist"));
            }
        }
        public bool AddVariable(Type t, string name)
        {
            if(!Variables.ContainsKey(name))
            {
                Variables.Add(name, new Variable(t));
                return true;
            }
            else
            {
                return false;
            }
        }
        public void RemoveVariable(string name)
        {
            Variable var;
            if (Variables.TryGetValue(name, out var))
            {
                Variables.Remove(name);
                List<Node> toRemove = new List<Node>();
                foreach(var n in Nodes)
                {
                    var t = n.GetType();
                    if (t.IsGenericType)
                    {
                        if (t.GetGenericTypeDefinition() == typeof(VariableNode<>))
                        {
                            if (t.GetProperty("Variable").GetValue(n, null) == var)
                            {
                                toRemove.Add(n);
                            }
                        }
                    }
                }
                foreach(var n in toRemove)
                {
                    RemoveNode(n);
                }
            }
            else
            {
                throw (new Exception("Variable " + name + " does not exist"));
            }
        }
        public void RemoveNode(Node node)
        {
            
            Nodes.Remove(node);
            node.Destroy();
            
            
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
