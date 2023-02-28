using System;
using System.Collections.Generic;
using System.Text;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
namespace DefaultNodes
{
    [Serializable]
    public class NodeClamp : Node
    {
        protected override void OnCreate()
        {
            In<double>("Min");
            In<double>("Value");
            In<double>("Max");
            Out<double>("Out");
        }
        protected override void OnUpdateOutputData()
        {
            var min = In("Min").AsDouble();
            var v = In("Value").AsDouble();
            var max = In("Max").AsDouble();
            Out("Out", Math.Min(max, Math.Max(min, v)));
        }
    }
}

