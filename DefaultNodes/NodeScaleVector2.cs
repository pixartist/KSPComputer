﻿using System;
using System.Collections.Generic;
using System.Linq;
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
            In<SVector2>("Vector");
            In<double>("Scale");
            Out<SVector2>("Result");
        }
        protected override void OnUpdateOutputData()
        {
            var v = In("Vector").AsVector2();
            var s = (float)In("Scale").AsDouble();
            Out("Result", new SVector2(v.x * s, v.y * s));
        }
    }
}
