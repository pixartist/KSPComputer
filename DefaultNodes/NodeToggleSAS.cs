﻿using System;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
namespace DefaultNodes
{
    [Serializable]
    public class NodeToggleSAS : DefaultExecutableNode
    {
        protected override void OnCreate()
        {
            In<bool>("On");
        }
        protected override void OnExecute(ConnectorIn input)
        {
            var on = In("On").AsBool();
            SASController.SASEnabled = on;
            ExecuteNext();
        }
    }
}