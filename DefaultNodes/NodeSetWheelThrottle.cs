using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
namespace DefaultNodes
{
    [Serializable]
    public class NodeSetWheelThrottle : ExecutableNode
    {
        protected override void OnCreate()
        {
            In<double>("Throttle");
        }
        protected override void OnExecute(ConnectorIn input)
        {
            Program.Vessel.ctrlState.wheelThrottle = (float)In("Throttle").AsDouble();
            ExecuteNext();
        }
    }
}