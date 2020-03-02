using KSPComputer.Connectors;
using KSPComputer.Nodes;
using System;
namespace DefaultNodes {
    [Serializable]
    public class NodeTriggerNextStage : DefaultExecutableNode {
        protected override void OnExecute(ConnectorIn input) {
            KSP.UI.Screens.StageManager.ActivateNextStage();
            ExecuteNext();
        }
    }
}