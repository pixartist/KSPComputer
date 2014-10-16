using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KSPFlightPlanner.Program.NodeDataTypes
{
	[Serializable]
    public class NCDoubleIn : NodeConnectionIn
    {
		public NCDoubleIn(Node owner)
            : base(typeof(double), owner)
        {
        }
    }
}
