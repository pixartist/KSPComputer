using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPComputer.Types;
using KSPComputer.Nodes;
namespace DefaultNodes
{
    [Serializable]
    public class NodeBreakVector2 : Node
    {
        protected override void OnCreate()
        {
            In<SVector2>("Vector2");
            Out<double>("X");
            Out<double>("Y");
        }
        protected override void OnUpdateOutputData()
        {
            var v = In("Vector2").AsVector2();
            Out("X", v.x);
            Out("Y", v.y);
        }
    }
}
