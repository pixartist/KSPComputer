using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
namespace DefaultNodes
{
    [Serializable]
    public class NodeCustomAction : RootNode
    {
        private int lastAction;
        protected override void OnCreate()
        {
            In<double>("Action id");
            Program.OnCustomAction += Program_OnCustomAction;
        }

        void Program_OnCustomAction(int action)
        {
            lastAction = action;
            Execute(null);
        }
        protected override void OnExecute(ConnectorIn input)
        {
            if (lastAction == Math.Round(In("Action id").AsDouble()))
                ExecuteNext();
        }
        protected override void OnDestroy()
        {
            Program.OnCustomAction -= Program_OnCustomAction;
        }
    }
}