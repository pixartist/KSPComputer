using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KSPFlightPlanner.Program.Connectors;
namespace KSPFlightPlanner.Program.Nodes
{
    [Serializable]
    public abstract class RootNode : Node
    {
        public const string DefaultExecName = "Exec";
        public RootNode()
            : base()
        {

            AddConnectorOut(DefaultExecName, new ExecConnectorOut());
        }
        public void Execute()
        {

            //Log.Write(this.GetType() + " executing");
            RequestInputUpdates();
            OnExecute();
        }
        protected virtual void OnExecute()
        {
            ExecuteNext();
        }
        protected void ExecuteNext(string name = DefaultExecName)
        {
            var c = GetConnectorOut(name);
            if (c != null)
                c.Execute();
        }
    }
}