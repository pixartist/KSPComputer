using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPFlightPlanner.Program.Connectors;
namespace KSPFlightPlanner.Program.Nodes
{
    [Serializable]
    public class NodeFloatDivide : Node
    {
        public new static string Name = "Divide (Float)";
        public new static string Description = "Divides two floats";
        public new static SVector3 Color = new SVector3(0.2f, 1f, 1f);
        public new static SVector2 Size = new SVector2(190, 150);
        protected override void OnCreate()
        {
            In<float>("A");
            In<float>("B");
            Out<float>("Out");
        }
        protected override void OnUpdateOutputData()
        {
            var a = In("A").AsFloat();
            var b = In("B").AsFloat();
            if (b != 0)
                Out("Out", a / b);
            else
               Out("Out", 0f);
        }
    }
}
