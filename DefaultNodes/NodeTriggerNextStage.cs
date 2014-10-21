using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
namespace DefaultNodes
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