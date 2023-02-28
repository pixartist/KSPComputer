using System;
using KSPComputer.Types;
using KSPComputer.Nodes;
using KSPComputer.Helpers;
namespace DefaultNodes
{
    [Serializable]
    public class NodeGetHeading : Node
    {
        protected override void OnCreate()
        {
            Out<SVector3d>("Direction");
        }
        protected override void OnUpdateOutputData()
        {
            Out("Direction", new SVector3d(VesselController.NavballHeading));
        }
    }
}
