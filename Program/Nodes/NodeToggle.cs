using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;
using KSPFlightPlanner.Program.Connectors;
namespace KSPFlightPlanner.Program.Nodes
{
    [Serializable]
    public class NodeToggle : ExecutableNode
    {
        private bool enabled;
        protected override void OnCreate()
        {
            enabled = false;
            In<Connector.Exec>("Enable", true);
            In<Connector.Exec>("Disable", true);
        }

        protected override void OnExecute(ConnectorIn input)
        {
            if (input == In("Enable"))
                enabled = true;
            else if (input == In("Disable"))
                enabled = false;
            else if(enabled)
                ExecuteNext();
        }
  
    }
}