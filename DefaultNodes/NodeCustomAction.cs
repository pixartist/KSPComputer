using System;
using System.Collections.Generic;

using System.Text;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
namespace DefaultNodes
{
    [Serializable]
    public class NodeCustomAction : EventNode
    {
        private int lastAction;
        protected override void OnCreate()
        {
            base.OnCreate();
            In<double>("Action id");
        }

        public override void OnCustomAction(int action)
        {
            lastAction = action;
            Execute(null);
        }
        protected override void OnExecute(ConnectorIn input)
        {
            if (lastAction == Math.Round(In("Action id").AsDouble()))
                ExecuteNext();
        }
    }
}