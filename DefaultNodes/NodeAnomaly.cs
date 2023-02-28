using System;
using System.Collections.Generic;
using System.Text;
using KSPComputer;
using KSPComputer.Nodes;
namespace DefaultNodes
{
    [Serializable]
    public class NodeAnomaly : DefaultRootNode
    {
        public override void OnAnomaly()
        {
            Execute(null);
        }
    }
}
