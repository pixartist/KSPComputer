using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace KSPFlightPlanner.Program
{
	[Serializable]
    public class FlightProgram
    {
		[NonSerialized]
        private PartModule module;
        public Vessel Vessel 
        {
            get
            {
                if (module == null)
                    return null;
                return module.vessel;
            }
        }
        public enum FlightEvent
        {
            Tick,
			PreLaunch
        }
        public delegate void FlightEventHandler(FlightEvent e);
        public event FlightEventHandler OnTick;
		public event FlightEventHandler OnLoad;
        public List<Node> Nodes { get; private set; }
        public FlightProgram()
        {
            Nodes = new List<Node>();
        }
		public void Init(PartModule module, PartModule.StartState state)
        {
            this.module = module;
			if (OnLoad != null)
			{
				switch(state)
				{ 
					case PartModule.StartState.PreLaunch:
						OnLoad(FlightEvent.PreLaunch);
						break;
					default:
						break;
				}
			}
        }
        public Node AddNode(Type nodeType, Vector2 position)
        {
            if (nodeType.IsSubclassOf(typeof(Node)))
            {
                Node n = Activator.CreateInstance(nodeType) as Node;
				n.Init(this);
				n.Position = new float[] { position.x, position.y };
                Nodes.Add(n);
                
                return n;
                
            }
            else
            {
                throw (new Exception("Type " + nodeType + " is not a valid node type"));
            }
        }
		
        public void Update()
        {
            if (OnTick != null)
                OnTick(FlightEvent.Tick);
        }
    }
}
