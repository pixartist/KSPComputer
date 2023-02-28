using System;
using System.Collections.Generic;

using System.Text;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
namespace DefaultNodes
{
    [Serializable]
    public class NodeSetWheelThrottle : DefaultExecutableNode
    {
        protected override void OnCreate()
        {
            In<double>("Throttle");
        }
        protected override void OnExecute(ConnectorIn input)
        {
            Vessel.ctrlState.wheelThrottle = (float)In("Throttle").AsDouble();
            ExecuteNext();
        }
    }
}