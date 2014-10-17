using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPFlightPlanner.Program.Connectors;
namespace KSPFlightPlanner.Program.Nodes
{
    [Serializable]
    public class NodeSetThrottle : ExecutableNode
    {
        public new static string Name = "Set throttle";
        public new static string Description = "Sets the throttle to the given amount";
        public new static SVector3 Color = new SVector3(1, 1, 0.2f);
        public new static SVector2 Size = new SVector2(150, 100);
        protected override void OnCreate()
        {
            AddConnectorIn("Throttle", new FloatConnectorIn());
        }
        protected override void OnExecute()
        {
            FlightInputHandler.state.mainThrottle = GetConnectorIn("Throttle").GetBufferAsFloat();
            ExecuteNext();
        }
    }
}