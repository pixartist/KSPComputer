using KSPComputer.Connectors;
using System;
using UnityEngine;
namespace KSPComputer.Nodes {
    [Serializable]
    public abstract class BaseExecutableNode : Node {
        public float LastExecution { get; protected set; }
        public virtual void Execute(ConnectorIn input) {
            LastExecution = Time.time;
            //Log.Write(this.GetType() + " executing");
            try {
                RequestInputUpdates();
                OnExecute(input);
            } catch (Exception e) {
                Log.Write("Node " + this.GetType() + " execution threw exception. Removing node. Exception: " + e.Message);
                Log.Write(e.StackTrace);
                //Program.RemoveNode(this);
                RequestRemoval();
            }
        }
        protected virtual void OnExecute(ConnectorIn input) {
            ExecuteNext();
        }
        protected void ExecuteNext(string name = DefaultExecName) {
            var c = GetOuput(name);
            if (c != null) {
                c.Execute();
            }
        }
    }
}
