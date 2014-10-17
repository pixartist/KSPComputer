using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KSPFlightPlanner.Program.Connectors
{
    [Serializable]
    public class QuaternionConnectorOut : ConnectorOut
    {
        public QuaternionConnectorOut()
            : base(typeof(SQuaternion), true)
        {
        }
    }
}