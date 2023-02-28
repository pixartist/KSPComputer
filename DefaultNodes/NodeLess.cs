using System;
using System.Collections.Generic;

using System.Text;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
namespace DefaultNodes
{
    [Serializable]
    public class NodeLess : Node
    {
        protected override void OnCreate()
        {
            In<double>("A");
            In<double>("B");
            Out<bool>("<");
        }
        protected override void OnUpdateOutputData()
        {
            var a = In("A").AsDouble();
            var b = In("B").AsDouble();
            Out("<", a < b);
        }
    }
}

