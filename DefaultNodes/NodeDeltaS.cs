using System;
using System.Collections.Generic;

using System.Text;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
namespace DefaultNodes
{
    [Serializable]
    public class NodeDeltaS : Node
    {
        double lastTime = -1;
        double lastValue;
        protected override void OnCreate()
        {
            lastValue = 0;
            In<double>("Value");
            Out<double>("Delta");
        }
        protected override void OnUpdateOutputData()
        {
            double t = Planetarium.GetUniversalTime();
            double v = In("Value").AsDouble();
            if (lastTime >= 0)
            {
                Out("Delta", (v - lastValue) / (t - lastTime));
            }
            lastTime = t;
            lastValue = v;
        }
    }
}
