using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KSPComputer.Nodes;
using KSPComputer.Types;
namespace DefaultNodes
{
    [Serializable]
    public class NodeDotVector2 : Node
    {
        protected override void OnCreate()
        {
            In<SVector2d>("A");
            In<SVector2d>("B");
            Out<double>("Dot");
        }
        protected override void OnUpdateOutputData()
        {
            var a = In("A").AsVector2().GetVec2();
            var b = In("B").AsVector2().GetVec2();
            Out("Dot", Vector2d.Dot(a,b));
        }
    }
}
