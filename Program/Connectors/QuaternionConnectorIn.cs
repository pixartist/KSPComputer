using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KSPFlightPlanner.Program.Connectors
{
    [Serializable]
    public class QuaternionConnectorIn : ConnectorIn
    {
        public QuaternionConnectorIn()
            : base(typeof(SQuaternion), false)
        {
        }
    }
}