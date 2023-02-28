using System;
using System.Collections.Generic;

using System.Text;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
namespace DefaultNodes
{
    [Serializable]
    public class NodeDelta : Node
    {
        double lastValue;
        protected override void OnCreate()
        {
            lastValue = 0;
            In<double>("Value");
            Out<double>("Delta");
        }
        protected override void OnUpdateOutputData()
        {
            double v = In("Value").AsDouble();
            Out("Delta", (v - lastValue));
            lastValue = v;
        }
    }
}
