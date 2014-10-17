using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KSPFlightPlanner.Program.Connectors
{
    [Serializable]
    public class ExecConnectorOut : ConnectorOut
    {
        public ExecConnectorOut()
            : base(null, false)
        {
        }
    }
}