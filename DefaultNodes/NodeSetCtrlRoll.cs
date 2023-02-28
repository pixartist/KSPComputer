using System;
using System.Collections.Generic;

using System.Text;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
namespace DefaultNodes
{
    [Serializable]
    public class NodeSetCtrlRoll : DefaultExecutableNode
    {
        protected override void OnCreate()
        {
            In<double>("Roll");
        }
        protected override void OnExecute(ConnectorIn input)
        {
            Vessel.ctrlState.roll = (float)In("Roll").AsDouble();
            ExecuteNext();
        }
    }
}