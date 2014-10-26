﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
namespace DefaultNodes
{
    [Serializable]
    public class NodeGreater : Node
    {
        protected override void OnCreate()
        {
            In<double>("A");
            In<double>("B");
            Out<bool>(">");
        }
        protected override void OnUpdateOutputData()
        {
            var a = In("A").AsDouble();
            var b = In("B").AsDouble();
            Out("Out", a>b);
        }
    }
}

