﻿using System;
using System.Collections.Generic;
using System.Text;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
namespace DefaultNodes
{
    [Serializable]
    public class NodeSASModeStabilityAssist : DefaultExecutableNode
    {
        protected override void OnExecute(ConnectorIn input)
        {
            SASController.SASEnabled = true;
            VesselController.Vessel.Autopilot.SetMode(VesselAutopilot.AutopilotMode.StabilityAssist);
            ExecuteNext();
        }
    }
}