using System;
using System.Collections.Generic;
using System.Linq;
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
            Out<SVector2>("Vector2");
            In<double>("X");
            In<double>("Y");
        }
        protected override void OnUpdateOutputData()
        {
            var x = In("X").AsFloat();
            var y = In("Y").AsFloat();
            Out("Vector2", new SVector2(x, y));
        }
    }
}
