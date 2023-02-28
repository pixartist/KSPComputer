using System;
using System.Collections.Generic;

using System.Text;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
namespace DefaultNodes
{
    [Serializable]
    public class NodeSelectNumber : Node
    {
        protected override void OnCreate()
        {
            In<bool>("Selector");
            In<double>("True");
            In<double>("False");
            Out<double>("Out");
        }
        protected override void OnUpdateOutputData()
        {
            var a = In("True").AsDouble();
            var b = In("False").AsDouble();
            Out("Out", In("Selector").AsBool() ? a : b);
        }
    }
}

