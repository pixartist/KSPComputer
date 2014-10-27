using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
namespace DefaultNodes
{
    [Serializable]
    public class NodeSetCtrlRoll : ExecutableNode
    {
        protected override void OnCreate()
        {
            In<double>("Roll");
        }
        protected override void OnExecute(ConnectorIn input)
        {
            Program.Vessel.ctrlState.roll = (float)In("Roll").AsDouble();
            ExecuteNext();
        }
    }
}