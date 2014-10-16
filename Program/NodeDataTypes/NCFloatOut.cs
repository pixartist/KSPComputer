using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KSPFlightPlanner.Program.NodeDataTypes
{
	[Serializable]
    public class NCFloatOut : NodeConnectionOut
    {
		public NCFloatOut(Node owner)
            : base(typeof(float), owner, true)
        {
        }
    }
}
