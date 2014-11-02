using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
namespace DefaultNodes
{
    [Serializable]
    public class NodeDT : Node
    {
        protected override void OnCreate()
        {
            Out<double>("Delta Time");
        }
        protected override void OnUpdateOutputData()
        {
            Out("Delta Time", (double)Time.deltaTime);
        }
    }
}
