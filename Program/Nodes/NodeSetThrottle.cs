using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPFlightPlanner.Program;
using KSPFlightPlanner.Program.NodeDataTypes;
namespace KSPFlightPlanner.Program.Nodes
{

	[Serializable]
    public class NodeSetThrottle : Node
    {
        public override void OnCreate()
        {
			Inputs.Add(DefaultExecName, new NCActionIn(this));
			Inputs.Add("Throttle", new NCFloatIn(this));
			Outputs.Add(DefaultExecName, new NCActionOut(this));
        }
        protected override void OnExecute()
		{
			FlightInputHandler.state.mainThrottle = Inputs["Throttle"].GetBufferAsFloat();
			TriggerOutput();
        }
    }
}