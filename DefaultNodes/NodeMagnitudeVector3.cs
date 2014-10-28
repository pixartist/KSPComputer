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
    public class NodeMagnitudeVector3 : Node
    {
        protected override void OnCreate()
        {
            In<SVector3>("Vector3");
            Out<double>("Magnitude");
        }
        protected override void OnUpdateOutputData()
        {
            var v = In("Vector2").AsVector3();
            float m = Mathf.Sqrt(v.x * v.x + v.y * v.y + v.z * v.z);
            Out("Magnitude", m);
        }
    }
}