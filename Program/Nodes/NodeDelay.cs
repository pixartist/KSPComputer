using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using KSPFlightPlanner.Program.Connectors;
namespace KSPFlightPlanner.Program.Nodes
{
    [Serializable]
    public class NodeDelay : ExecutableNode
    {
        public new static string Name = "Delay (s)";
        public new static string Description = "Delays incoming executions";
        public new static SVector3 Color = new SVector3(1, 1, 1);
        public new static SVector2 Size = new SVector2(150, 100);
        protected override void OnCreate()
        {
            AddConnectorIn("Delay", new FloatConnectorIn());
        }
        protected override void OnExecute()
        {
            
            float d = GetConnectorIn("Delay").GetBufferAsFloat();
            Log.Write("Delay Executed: " + d + " secs");
            Thread t = new Thread(new ParameterizedThreadStart((n) => WaitThread(d)));
            t.Start();
        }
        private void WaitThread(float delay)
        {
            Log.Write("Delay Thread started: " + delay + " secs");
            Thread.Sleep(Math.Max(0,(int)(delay * 1000)));
            
            ExecuteNext();
            Log.Write("Delay Thread ended: " + delay + " secs");
        }
    }
}