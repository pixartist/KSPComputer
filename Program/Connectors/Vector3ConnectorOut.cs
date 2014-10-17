using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KSPFlightPlanner.Program.Connectors
{
    [Serializable]
    public class Vector3ConnectorOut : ConnectorOut
    {
        public Vector3ConnectorOut()
            : base(typeof(SVector3), true)
        {
        }
    }
}