using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KSPFlightPlanner.Program.NodeDataTypes
{
	[Serializable]
    public class NCBoolOut : NodeConnectionOut
    {
		public NCBoolOut(Node owner)
            : base(typeof(bool), owner, true)
        {
        }
    }
}
