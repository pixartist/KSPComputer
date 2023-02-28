using System;
using System.Collections.Generic;

using System.Text;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
namespace DefaultNodes
{
    [Serializable]
    public class NodeIncrement : DefaultExecutableNode
    {
        int count = 0;
        protected override void OnCreate()
        {
            In<double>("Start");
            In<double>("Step");
            Out<double>("Value");
            In<Connector.Exec>("Reset", true);
        }
        protected override void OnExecute(ConnectorIn input)
        {
            if (input == In("Reset"))
                count = 0;
            else
                count++;
            ExecuteNext();
        }
        protected override void OnUpdateOutputData()
        {
            Out("Value", In("Start").AsDouble() + count * In("Step").AsDouble());
        }
    }
}