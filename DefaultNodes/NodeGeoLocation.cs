using System;
using System.Collections.Generic;

using System.Text;
using KSPComputer.Types;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
namespace DefaultNodes
{
    [Serializable]
    public class NodeGeoLocation : Node
    {
        protected override void OnCreate()
        {
            Out<SVector3d>("GeoLoc");
        }
        protected override void OnUpdateOutputData()
        {
            Out("GeoLoc", new SVector3d(VesselController.WorldToGeo(VesselController.CenterOfMass)));
        }
    }
}
