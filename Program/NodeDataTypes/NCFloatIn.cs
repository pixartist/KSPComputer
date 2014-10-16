using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KSPFlightPlanner.Program.NodeDataTypes
{
	[Serializable]
    public class NCFloatIn : NodeConnectionIn
    {
		public NCFloatIn(Node owner)
            : base(typeof(float), owner)
        {
        }
    }
}
