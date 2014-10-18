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

            Out<Connector.Exec>(DefaultExecName, false);
        }
        public void Execute()
        {

            Log.Write(this.GetType() + " executing");
            RequestInputUpdates();
            OnExecute();
        }
        protected virtual void OnExecute()
        {
            ExecuteNext();
        }
        protected void ExecuteNext(string name = DefaultExecName)
        {
            
           // Log.Write("Trying to execute next node: " + name);
            var c = GetOuput(name);
            if (c != null)
            {
                //Log.Write("Executing next node");
                c.Execute();
            }
            /*else
            {
                Log.Write("Next node is null!");
            }*/
        }
    }
}