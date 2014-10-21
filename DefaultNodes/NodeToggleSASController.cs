using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
namespace DefaultNodes
{
    [Serializable]
    public class NodeToggleSASController : ExecutableNode
    {
        protected override void OnCreate()
        {
            In<bool>("Enabled");
        }
        protected override void OnExecute(ConnectorIn input)
        {
            Program.SASController.SASControlEnabled = In("Enabled").AsBool();
            ExecuteNext();
        }
    }
}