using System;
using System.Collections.Generic;

using System.Text;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
namespace DefaultNodes
{
    [Serializable]
    public class NodeOr : Node
    {
        protected override void OnCreate()
        {
            In<bool>("A");
            In<bool>("B");
            Out<bool>("Or");
        }
        protected override void OnUpdateOutputData()
        {
            var a = In("A").AsBool();
            var b = In("B").AsBool();
            Out("Or", a || b);
        }
    }
}

