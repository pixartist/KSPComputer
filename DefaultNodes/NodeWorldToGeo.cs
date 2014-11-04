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
    public class NodeWorldToGeo : Node
    {
        protected override void OnCreate()
        {
            In<SVector3d>("WorldLocation");
            Out<SVector3d>("GeoLocation");
        }
        protected override void OnUpdateOutputData()
        {
            var worldDeref = VesselController.ReferenceToWorld(In("WorldLocation").AsVector3().GetVec3(), VesselController.FrameOfReference.Navball);
            Out("GeoLocation", new SVector3d(VesselController.WorldToGeo(worldDeref)));
        }
    }
}
