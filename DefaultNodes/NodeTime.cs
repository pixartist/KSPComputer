using System;
using System.Collections.Generic;

using System.Text;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
namespace DefaultNodes
{
    [Serializable]
    public class NodeTime : Node
    {
        protected override void OnCreate()
        {
            Out<double>("Time");
        }
        protected override void OnUpdateOutputData()
        {
            Out("Time", Planetarium.GetUniversalTime());
        }
    }
}
