using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
namespace DefaultNodes
{
    [Serializable]
    public class NodeSetWheelSteer : DefaultExecutableNode
    {
        protected override void OnCreate()
        {
            In<double>("Steer");
        }
        protected override void OnExecute(ConnectorIn input)
        {
            Vessel.ctrlState.wheelSteer = (float)In("Steer").AsDouble();
            ExecuteNext();
        }
    }
}