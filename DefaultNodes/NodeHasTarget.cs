using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KSPComputer;
using KSPComputer.Types;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
using KSPComputer.Helpers;
namespace DefaultNodes
{
    [Serializable]
    public class NodeHasTarget : Node
    {
        protected override void OnCreate()
        {
            Out<bool>("Has target");
        }
        protected override void OnUpdateOutputData()
        {
            Out("Has target", Vessel.targetObject != null);
        }
    }
}
