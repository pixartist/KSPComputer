﻿using System;
using System.Collections.Generic;

using System.Text;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
namespace DefaultNodes
{
    [Serializable]
    public class NodeToggleRCS : DefaultExecutableNode
    {
        protected override void OnCreate()
        {
            In<bool>("On");
        }
        protected override void OnExecute(ConnectorIn input)
        {
            var on = In("On").AsBool();
            SASController.RCSEnabled = on;
            ExecuteNext();
        }
    }
}