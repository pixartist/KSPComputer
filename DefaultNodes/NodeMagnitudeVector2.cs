using System;
using System.Collections.Generic;

using System.Text;
using UnityEngine;
using KSPComputer.Nodes;
using KSPComputer.Types;
namespace DefaultNodes
{
    [Serializable]
    public class NodeMagnitudeVector2 : Node
    {
        protected override void OnCreate()
        {
            In<SVector2d>("Vector2");
            Out<double>("Magnitude");
        }
        protected override void OnUpdateOutputData()
        {
            var v = In("Vector2").AsVector2();
            double m = Math.Sqrt(v.x * v.x + v.y * v.y);
            Out("Magnitude", m);
        }
    }
}
