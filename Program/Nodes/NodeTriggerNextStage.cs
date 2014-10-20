using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPFlightPlanner.Program.Connectors;
namespace KSPFlightPlanner.Program.Nodes
{
    [Serializable]
    public class NodeTriggerNextStage : ExecutableNode
    {
        protected override void OnExecute(ConnectorIn input)
        {
            Staging.ActivateNextStage();
            ExecuteNext();
        }
    }
}