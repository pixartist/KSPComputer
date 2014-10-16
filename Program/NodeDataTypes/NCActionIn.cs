using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KSPFlightPlanner.Program.NodeDataTypes
{
	[Serializable]
    public class NCActionIn : NodeConnectionIn
    {
		public NCActionIn(Node owner)
            : base(null, owner)
        {

        }
    }
}
