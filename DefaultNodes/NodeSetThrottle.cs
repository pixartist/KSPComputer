using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
namespace DefaultNodes
{
    [Serializable]
    public class NodeSetThrottle : ExecutableNode
    {
        protected override void OnCreate()
        {
            In<double>("Throttle");
        }
        protected override void OnExecute(ConnectorIn input)
        {
            FlightInputHandler.state.mainThrottle = In("Throttle").AsFloat();
            ExecuteNext();
        }
    }
}