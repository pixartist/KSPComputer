using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
namespace DefaultNodes
{
    [Serializable]
    public class NodeSetCtrlPitch : DefaultExecutableNode
    {
        protected override void OnCreate()
        {
            In<double>("Pitch");
        }
        protected override void OnExecute(ConnectorIn input)
        {
            Vessel.ctrlState.pitch = (float)In("Pitch").AsDouble();
            ExecuteNext();
        }
    }
}