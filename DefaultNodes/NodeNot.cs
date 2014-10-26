using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
namespace DefaultNodes
{
    [Serializable]
    public class NodeNot : Node
    {
        protected override void OnCreate()
        {
            In<bool>("A");
            Out<bool>("!A");
        }
        protected override void OnUpdateOutputData()
        {
            var a = In("A").AsBool();
            Out("!A", !a);
        }
    }
}

