using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPFlightPlanner.Program.Connectors;
namespace KSPFlightPlanner.Program.Nodes
{
    [Serializable]
    public class NodeToggleSAS : ExecutableNode
    {
        protected override void OnCreate()
        {
            In<bool>("On");
        }
        protected override void OnExecute(ConnectorIn input)
        {
            var on = In("On").AsBool();
            Program.SASController.SASEnabled = on;
            ExecuteNext();
        }
    }
}