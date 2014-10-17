using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KSPFlightPlanner.Program.Connectors
{
    [Serializable]
    public class Vector3ConnectorIn : ConnectorIn
    {
        public Vector3ConnectorIn()
            : base(typeof(SVector3), false)
        {
        }
    }
}