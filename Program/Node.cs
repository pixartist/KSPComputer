using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KSPFlightPlanner.Program.Nodes;
namespace KSPFlightPlanner.Program
{

	[Serializable]
    public abstract class Node
    {
		public const string DefaultExecName = "Exec";
		internal delegate void PreExecuteHandler();
		internal event PreExecuteHandler PreExecute;
        public FlightProgram Program { get; private set; }
        public Dictionary<string, NodeConnectionOut> Outputs { get; protected set; }
		public Dictionary<string, NodeConnectionIn> Inputs { get; protected set; }
        public float[] Position { get; set; }
        protected Node()
        {
			Inputs = new Dictionary<string, NodeConnectionIn>();
			Outputs = new Dictionary<string, NodeConnectionOut>();
        }
        internal void Init(FlightProgram program)
        {
			Log.Write(this + " Initialized");
            Program = program;
            OnCreate();
        }
		public virtual void Execute()
		{
			Log.Write("Node Executed: " + this.GetType());
			if (PreExecute != null)
				PreExecute();
			OnExecute();
		}
        public virtual void OnCreate()
        { }
        public virtual void OnDestroy()
        { }
        protected abstract void OnExecute();
        
        public override string ToString()
        {
            return "[Node]: " + this.GetType();
        }
		protected void TriggerOutput(string name = DefaultExecName)
		{
			NodeConnectionOut target;
			if(Outputs.TryGetValue(name, out target))
			{
				if (target.ConnectedTo != null)
					target.ConnectedTo.Owner.Execute();
			}
		}
    }
}
