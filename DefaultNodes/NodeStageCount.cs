using KSPComputer.Nodes;
using System;
namespace DefaultNodes {
    [Serializable]
    public class NodeStageCount : Node {
        protected override void OnCreate() {
            Out<double>("Stage");
        }
        protected override void OnUpdateOutputData() {
            Out("Stage", KSP.UI.Screens.StageManager.GetStageCount(Vessel.parts));
        }
    }
}
