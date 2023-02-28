using System;
using System.Collections.Generic;

using System.Text;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
namespace DefaultNodes
{
    [Serializable]
    public class NodeDivide : Node
    {
        protected override void OnCreate()
        {
            In<double>("A");
            In<double>("B");
            Out<double>("Out");
        }
        protected override void OnUpdateOutputData()
        {
            var a = In("A").AsDouble();
            var b = In("B").AsDouble();
            if (b != 0)
                Out("Out", a / b);
            else
               Out("Out", 0);
        }
    }
}
