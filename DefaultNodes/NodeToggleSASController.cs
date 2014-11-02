using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
namespace DefaultNodes
{
    [Serializable]
    public class NodeToggleSASController : DefaultExecutableNode
    {
        protected override void OnCreate()
        {
            In<bool>("Enabled");
        }
        protected override void OnExecute(ConnectorIn input)
        {
            SASController.SASControlEnabled = In("Enabled").AsBool();
            ExecuteNext();
        }
    }
}