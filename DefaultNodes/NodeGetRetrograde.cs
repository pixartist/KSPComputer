using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KSPComputer.Types;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
using KSPComputer.Helpers;
namespace DefaultNodes
{
    [Serializable]
    public class NodeGetRetrograde : Node
    {
        protected override void OnCreate()
        {
            Out<SVector3d>("Direction");
        }
        protected override void OnUpdateOutputData()
        {
            Out("Direction", new SVector3d(VesselController.WorldToReference(VesselController.Prograde, VesselController.FrameOfReference.Navball) * -1f));
        }
    }
}
