using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
namespace DefaultNodes
{
    [Serializable]
    public class NodeShowValue : DefaultExecutableNode
    {
        protected override void OnCreate()
        {
            In<double>("Value");
        }
    }
}
