using System;
using System.Collections.Generic;

using System.Text;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
namespace DefaultNodes
{
    [Serializable]
    public class NodeGetCtrlPitch : Node
    {
        protected override void OnCreate()
        {
            Out<double>("Pitch");
        }
        protected override void OnUpdateOutputData()
        {
            Out("Pitch", Vessel.ctrlState.pitch);
        }
    }
}