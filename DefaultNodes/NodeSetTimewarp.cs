using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
namespace DefaultNodes
{
    [Serializable]
    public class NodeSetTimewarp : ExecutableNode
    {
        protected override void OnCreate()
        {
            In<double>("Multiplier");
        }
        protected override void OnExecute(ConnectorIn input)
        {
            var m = Math.Max(1, In("Multiplier").AsInt());
            TimeWarp.SetRate(m, true);
            ExecuteNext();
        }
    }
}