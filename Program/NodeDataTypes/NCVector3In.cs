using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KSPFlightPlanner.Program.NodeDataTypes
{
	[Serializable]
    public class NCVector3In : NodeConnectionIn
    {
		public NCVector3In(Node owner)
            : base(typeof(SVector3), owner)
        {
        }
    }
}
