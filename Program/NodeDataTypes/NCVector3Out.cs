using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KSPFlightPlanner.Program.NodeDataTypes
{
	[Serializable]
    public class NCVector3Out : NodeConnectionOut
    {
		public NCVector3Out(Node owner)
            : base(typeof(SVector3), owner, true)
        {
        }
    }
}
