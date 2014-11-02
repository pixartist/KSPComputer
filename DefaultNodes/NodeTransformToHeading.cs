using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KSPComputer.Helpers;
using KSPComputer.Types;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
namespace DefaultNodes
{
    [Serializable]
    public class NodeTransformToHeading : Node
    {
        protected override void OnCreate()
        {
            In<SVector3>("Direction");
            Out<double>("N/S");
            Out<double>("E/W");
            Out<double>("U/D");
        }
        protected override void OnUpdateOutputData()
        {
            Vector3 dir = VesselController.WorldToReference(In("Direction").AsVector3().GetVec3().normalized, VesselController.FrameOfReference.Navball);
            Out("N/S", (double)dir.z);
            Out("E/W", (double)dir.x);
            Out("U/D", (double)dir.y);
            
        }
    }
}
