using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
namespace DefaultNodes
{
    [Serializable]
    public class NodeSign : Node
    {
        protected override void OnCreate()
        {
            In<double>("Value");
            Out<double>("Sign");
        }
        protected override void OnUpdateOutputData()
        {
            var a = In("Value").AsDouble();
            Out("Sign", Math.Sign(a));
        }
    }
}

