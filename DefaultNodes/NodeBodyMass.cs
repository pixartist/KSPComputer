using System;
using System.Collections.Generic;
using System.Text;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
namespace DefaultNodes
{
    [Serializable]
    public class NodeBodyMass : Node
    {
        protected override void OnCreate()
        {
            Out<double>("Mass");
        }
        protected override void OnUpdateOutputData()
        {
            Out("Mass", VesselController.Vessel.mainBody.Mass);
        }
    }
}
