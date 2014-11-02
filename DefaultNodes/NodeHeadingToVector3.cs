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
    public class NodeHeadingToVector3 : Node
    {
        protected override void OnCreate()
        {
            Out<SVector3>("Direction");
            In<double>("N/S");
            In<double>("E/W");
            In<double>("U/D");
        }
        protected override void OnUpdateOutputData()
        {
            Vector3 dir = VesselController.ReferenceToWorld(new Vector3(In("E/W").AsFloat(), In("U/D").AsFloat(), In("N/S").AsFloat()), VesselController.FrameOfReference.Navball);
            Out("Direction", new SVector3(dir));
            
        }
    }
}
