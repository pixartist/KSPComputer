using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
namespace DefaultNodes
{
    [Serializable]
    public class NodeSetCtrlYaw : DefaultExecutableNode
    {
        protected override void OnCreate()
        {
            In<double>("Yaw");
        }
        protected override void OnExecute(ConnectorIn input)
        {
            Vessel.ctrlState.yaw = (float)In("Yaw").AsDouble();
            ExecuteNext();
        }
    }
}