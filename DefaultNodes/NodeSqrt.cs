using System;
using System.Collections.Generic;

using System.Text;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
namespace DefaultNodes
{
    [Serializable]
    public class NodeSqrt : Node
    {
        protected override void OnCreate()
        {
            In<double>("Value");
            Out<double>("Value");
        }
        protected override void OnUpdateOutputData()
        {
            var a = In("Value").AsDouble();
            Out("Value", Math.Sqrt(a));
        }
    }
}

