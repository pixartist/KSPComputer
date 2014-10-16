using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KSPFlightPlanner.Program.NodeDataTypes
{
	[Serializable]
    public class NCBoolIn : NodeConnectionIn
    {
		public NCBoolIn(Node owner)
            : base(typeof(bool), owner)
        {
        }
    }
}
