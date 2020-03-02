using KSPComputer.Connectors;
using KSPComputer.Nodes;
using System;
namespace DefaultNodes {
    [Serializable]
    public class NodeToggleSASController : DefaultExecutableNode {
        protected override void OnCreate() {
            In<bool>("Enabled");
        }
        protected override void OnExecute(ConnectorIn input) {
            SASController.SASEnabled = In("Enabled").AsBool();
            ExecuteNext();
        }
    }
}