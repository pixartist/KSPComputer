﻿using System;
using System.Collections.Generic;

using System.Text;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
namespace DefaultNodes
{
    [Serializable]
    public class NodeMax : Node
    {
        protected override void OnCreate()
        {
            In<double>("A");
            In<double>("B");
            Out<double>("Out");
        }
        protected override void OnUpdateOutputData()
        {
            var a = In("A").AsDouble();
            var b = In("B").AsDouble();
            Out("Out", Math.Max(a,b));
        }
    }
}

