using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KSPFlightPlanner.Program.NodeDataTypes
{
	[Serializable]
    public class NCDoubleOut : NodeConnectionOut
    {
		public NCDoubleOut(Node owner)
            : base(typeof(double), owner, true)
        {
        }
    }
}
