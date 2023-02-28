using System;
using System.Collections.Generic;

using System.Text;
using KSPComputer.Nodes;
using KSPComputer.Types;
namespace DefaultNodes
{
    [Serializable]
    public class NodeMakeVector2 : Node
    {
        protected override void OnCreate()
        {
            Out<SVector2d>("Vector2");
            In<double>("X E/W Long");
            In<double>("Y N/S Lat");
        }
        protected override void OnUpdateOutputData()
        {
            var x = In("X E/W Long").AsDouble();
            var y = In("Y N/S Lat").AsDouble();
            Out("Vector2", new SVector2d(x, y));
        }
    }
}
