using System;
using System.Collections.Generic;

using System.Text;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
namespace DefaultNodes
{
    [Serializable]
    public class NodeSetTimewarp : DefaultExecutableNode
    {
        protected override void OnCreate()
        {
            In<double>("Multiplier");
        }
        protected override void OnExecute(ConnectorIn input)
        {
            var m = Math.Min(TimeWarp.fetch.warpRates.Length-1, Math.Max(1, In("Multiplier").AsInt())-1);
            TimeWarp.SetRate(m, true);
            ExecuteNext();
        }
    }
}