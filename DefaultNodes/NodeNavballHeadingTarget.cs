using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KSPComputer;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
using KSPComputer.Helpers;
namespace DefaultNodes
{
    [Serializable]
    public class NodeNavballHeadingTarget : Node
    {
        protected override void OnCreate()
        {
            Out<double>("N/S");
            Out<double>("E/W");
            Out<double>("U/D");
        }
        protected override void OnUpdateOutputData()
        {
            if (Program.Vessel.targetObject != null)
            {
                Vector3 pDelta = Program.Vessel.targetObject.GetTransform().position - Program.VesselInfo.WorldPosition;
                pDelta.Normalize();
                Vector3 hRel = Program.VesselInfo.WorldToReference(pDelta, VesselInformation.FrameOfReference.Navball);
                Out("N/S", (double)hRel.z);
                Out("E/W", (double)hRel.x);
                Out("U/D", (double)hRel.y);
            }
            else
            {
                Out("N/S", 0.0);
                Out("E/W", 0.0);
                Out("U/D", 0.0);
            }
        }
    }
}
