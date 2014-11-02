using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
namespace DefaultNodes
{
    [Serializable]
    public class NodeSurfaceLocation : Node
    {
        protected override void OnCreate()
        {
            Out<double>("Lat");
            Out<double>("Long");
        }
        protected override void OnUpdateOutputData()
        {
            Out("Lat", Vessel.mainBody.GetLatitude(VesselController.WorldPosition));
            Out("Long", Vessel.mainBody.GetLongitude(VesselController.WorldPosition));
        }
    }
}
