using System;
using System.Collections.Generic;

using System.Text;
using UnityEngine;
using KSPComputer.Nodes;
using KSPComputer.Types;
namespace DefaultNodes
{
    [Serializable]
    public class NodeScaleVector2 : Node
    {
        protected override void OnCreate()
        {
            In<SVector2d>("Vector");
            In<double>("Scale");
            Out<SVector2d>("Result");
        }
        protected override void OnUpdateOutputData()
        {
            var v = In("Vector").AsVector2();
            var s = In("Scale").AsDouble();
            Out("Result", new SVector2d(v.x * s, v.y * s));
        }
    }
}
