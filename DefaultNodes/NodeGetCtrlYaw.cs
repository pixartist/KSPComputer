using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
namespace DefaultNodes
{
    [Serializable]
    public class NodeGetCtrlYaw : Node
    {
        protected override void OnCreate()
        {
            Out<double>("Yaw");
        }
        protected override void OnUpdateOutputData()
        {
            Out("Yaw", Vessel.ctrlState.yaw);
        }
    }
}