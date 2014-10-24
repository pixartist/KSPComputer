using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
namespace DefaultNodes
{
    [Serializable]
    public class NodeSetNumericValue : ExecutableNode
    {
        private double currentValue = 0;
        protected override void OnCreate()
        {
            In<double>("Value");
            Out<double>("Value");
        }
        protected override void OnExecute(ConnectorIn input)
        {
            currentValue = In("Value").AsDouble();
            ExecuteNext();
        }
        protected override void OnUpdateOutputData()
        {
            Out("Value", currentValue);
        }
    }
}