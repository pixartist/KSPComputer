using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
namespace DefaultNodes
{
    [Serializable]
    public class NodeSetSASControllerStrength : DefaultExecutableNode
    {
        protected override void OnCreate()
        {
            In<double>("Strength");
        }
        protected override void OnExecute(ConnectorIn input)
        {
            SASController.SASControllerStrength = In("Strength").AsFloat();
            ExecuteNext();
        }
    }
}