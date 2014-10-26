using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
namespace DefaultNodes
{
    [Serializable]
    public class NodeAbs : Node
    {
        protected override void OnCreate()
        {
            In<double>("Value");
            Out<double>("Abs");
        }
        protected override void OnUpdateOutputData()
        {
            var a = In("Value").AsDouble();
            Out("Abs", Math.Abs(a));
        }
    }
}

