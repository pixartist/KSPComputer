using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
namespace DefaultNodes
{
    [Serializable]
    public class NodeSetCtrlYaw : ExecutableNode
    {
        protected override void OnCreate()
        {
            In<double>("Yaw");
        }
        protected override void OnExecute(ConnectorIn input)
        {
            Program.Vessel.ctrlState.yaw = (float)In("Yaw").AsDouble();
            ExecuteNext();
        }
    }
}