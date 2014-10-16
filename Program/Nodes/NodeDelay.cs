using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPFlightPlanner.Program;
using KSPFlightPlanner.Program.NodeDataTypes;
using System.Threading;
namespace KSPFlightPlanner.Program.Nodes
{

	[Serializable]
    public class NodeDelay : Node
    {
        public override void OnCreate()
        {
			Inputs.Add(DefaultExecName, new NCActionIn(this));
			Inputs.Add("Delay", new NCFloatIn(this));
			Outputs.Add(DefaultExecName, new NCActionOut(this));
        }
        protected override void OnExecute()
		{
			float d = Inputs["Delay"].GetBufferAsFloat();
			Thread t = new Thread(new ParameterizedThreadStart((n) => WaitThread(d)));
			t.Start();
        }
		private void WaitThread(float delay)
		{
			Thread.Sleep((int)(delay * 1000));
			TriggerOutput();
		}
    }
}