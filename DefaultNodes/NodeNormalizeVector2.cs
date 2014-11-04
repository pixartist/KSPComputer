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
    public class NodeNormalizeVector2 : Node
    {
        protected override void OnCreate()
        {
            In<SVector2d>("Vector2");
            Out<SVector2d>("Vector2");
        }
        protected override void OnUpdateOutputData()
        {
            var v = In("Vector2").AsVector2();
            double m = Math.Sqrt(v.x * v.x + v.y * v.y);
            if (m == 0)
                m = 1;
            Out("Vector2", new SVector2d(v.x/m, v.y/m));
        }
    }
}
