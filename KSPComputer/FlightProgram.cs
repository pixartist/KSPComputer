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
        public static Vessel Vessel
        {
            get
            {
                return KSPOperatingSystem.VesselController.Vessel;
            }
        }
        public static VesselController VesselController
        {
            get
            {
                return KSPOperatingSystem.VesselController;
            }
        }
        public Dictionary<string, Variable> Variables { get; private set; }
        public List<Node> Nodes { get; private set; }
        public FlightProgram()
        {
            Nodes = new List<Node>();
            Variables = new Dictionary<string, Variable>();
        }
        public void Launch()
        {
            foreach (var n in Nodes)
                n.OnLaunch();
        }
        public void CustomAction(int action)
        {
            foreach (var n in Nodes)
                n.OnCustomAction(action);
        }
        public T AddNode<T>(Vector2 position) where T : Node
        {
            return (T)AddNode(typeof(T), position);
        }
        public Node AddNode(Type nodeType, Vector2 position)
        {
            if (nodeType.IsSubclassOf(typeof(Node)))
            {
                Node n = Activator.CreateInstance(nodeType) as Node;
                n.OnRequestRemoval += OnNodeRemovalRequest;
                n.Position = new SVector2(position.x, position.y);
                Nodes.Add(n);
                n.Create();
                n.OnInit();
                return n;
            }
            else
            {
                throw (new Exception("Type " + nodeType + " is not a valid node type"));
            }
        }

        void OnNodeRemovalRequest(Node n)
        {
            RemoveNode(n);
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
                n.OnRequestRemoval += OnNodeRemovalRequest;
                n.Create();
                n.OnInit();
                n.Position = new SVector2(position.x, position.y);
                Nodes.Add(n);
                return n;
            }
            else
            {
                throw (new Exception("Variable " + variable + " does not exist"));
            }
        }
        public Node AddSubRoutineNode(Vector2 position, string subroutine)
        {
            SubroutineNode n = Activator.CreateInstance(typeof(SubroutineNode)) as SubroutineNode;
            n.OnRequestRemoval += OnNodeRemovalRequest;
            n.Position = new SVector2(position.x, position.y);
            n.SetSubRoutine(subroutine);
            Nodes.Add(n);
            n.Create();
            n.OnInit();
            
            return n;
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
        public void RemoveSubRoutineNodes(string name)
        {
            SubroutineNode sr;
            List<Node> toRemove = new List<Node>();
            foreach (var n in Nodes)
            {
                if(n is SubroutineNode)
                {
                    sr = n as SubroutineNode;
                    if(sr.SubRoutineBlueprint == name)
                    {
                        toRemove.Add(n);
                    }
                }
            }
            foreach(var n in toRemove)
            {
                RemoveNode(n);
            }
        }
        public virtual void RemoveNode(Node node)
        {
            Nodes.Remove(node);
            node.Destroy();
        }
        public void Update()
        {
            foreach (var n in Nodes)
                n.OnUpdate();
        }
        internal void Init()
        {
            foreach (var n in Nodes)
                n.OnInit();
        }
    }
}
