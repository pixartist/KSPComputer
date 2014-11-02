using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KSPComputer.Connectors;
namespace KSPComputer.Nodes
{
    [Serializable]
    public abstract class BaseExecutableNode : Node
    {
        public float LastExecution { get; protected set; }
        public virtual void Execute(ConnectorIn input)
        {
            LastExecution = Time.time;
            //Log.Write(this.GetType() + " executing");
            try
            {
                RequestInputUpdates();
                OnExecute(input);
            }
            catch (Exception e)
            {
                Log.Write("Node " + this.GetType() + " execution threw exception. Removing node. Exception: " + e.Message);
                //Program.RemoveNode(this);
                RequestRemoval();
            }
        }
        protected virtual void OnExecute(ConnectorIn input)
        {
            ExecuteNext();
        }
        protected void ExecuteNext(string name = DefaultExecName)
        {
            var c = GetOuput(name);
            if (c != null)
            {
                c.Execute();
            }
        }
    }
}
