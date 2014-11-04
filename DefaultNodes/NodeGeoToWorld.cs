using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPComputer.Helpers;
using KSPComputer.Types;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
namespace DefaultNodes
{
    [Serializable]
    public class NodeGeoToWorld : Node
    {
        protected override void OnCreate()
        {
            Out<SVector3d>("WorldLocation");
            In<SVector3d>("GeoLocation");
        }
        protected override void OnUpdateOutputData()
        {
            var worldLoc = VesselController.GeoToWorld(In("GeoLocation").AsVector3().GetVec3());
            Out("WorldLocation", new SVector3d(VesselController.WorldToReference(worldLoc, VesselController.FrameOfReference.Navball)));
        }
    }
}
