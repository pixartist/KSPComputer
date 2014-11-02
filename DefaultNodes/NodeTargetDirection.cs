using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KSPComputer;
using KSPComputer.Types;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
using KSPComputer.Helpers;
namespace DefaultNodes
{
    [Serializable]
    public class NodeTargetDirection : Node
    {
        protected override void OnCreate()
        {
            Out<SVector3>("Direction");
        }
        protected override void OnUpdateOutputData()
        {
            if (Vessel.targetObject != null)
            {
                Vector3 pDelta = Vessel.targetObject.GetTransform().position - VesselController.WorldPosition;
                pDelta.Normalize();
                Out("Direction", new SVector3(VesselController.WorldToReference(pDelta, VesselController.FrameOfReference.Navball)));
            }
            else
            {
                Out("Direction", new SVector3());
            }
        }
    }
}
