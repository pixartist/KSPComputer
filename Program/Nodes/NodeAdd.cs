using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPFlightPlanner.Program.Connectors;
namespace KSPFlightPlanner.Program.Nodes
{
    [Serializable]
    public class NodeAdd : Node
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
            Out("Out", a + b);
        }
    }
}

