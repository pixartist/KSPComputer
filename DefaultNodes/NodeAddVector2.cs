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
    public class NodeAddVector2 : Node
    {
        protected override void OnCreate()
        {
            In<SVector2>("A");
            In<SVector2>("B");
            Out<SVector2>("Result");
        }
        protected override void OnUpdateOutputData()
        {
            var v1 = In("A").AsVector2();
            var v2 = In("B").AsVector2();
            Out("Result", new SVector2(v1.x+v2.x, v1.y+v2.y));
        }
    }
}
