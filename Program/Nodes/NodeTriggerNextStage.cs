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
        public new static string Name = "Trigger stage";
        public new static string Description = "Triggers the next stage";
        public new static SVector3 Color = new SVector3(1, 1, 0.2f);
        public new static SVector2 Size = new SVector2(150, 50);
        protected override void OnExecute(ConnectorIn input)
        {
            Staging.ActivateNextStage();
            ExecuteNext();
        }
    }
}