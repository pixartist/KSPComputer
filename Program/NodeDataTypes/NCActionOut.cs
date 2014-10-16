using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KSPFlightPlanner.Program.NodeDataTypes
{
	[Serializable]
    public class NCActionOut : NodeConnectionOut
    {
		public NCActionOut(Node owner)
            : base(null, owner, false)
        {
        }
    }
}
