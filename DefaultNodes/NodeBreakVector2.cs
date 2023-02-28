using System;
using System.Collections.Generic;
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
            In<SVector2d>("Vector2");
            Out<double>("X E/W Long");
            Out<double>("Y N/S Lat");
        }
        protected override void OnUpdateOutputData()
        {
            var v = In("Vector2").AsVector2();
            Out("X E/W Long", v.x);
            Out("Y N/S Lat", v.y);
        }
    }
}
