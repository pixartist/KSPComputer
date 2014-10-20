using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPFlightPlanner.Program.Connectors;
namespace KSPFlightPlanner.Program.Nodes
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
            var v = In("Value").AsDouble();
            Out("Delta", v - lastValue);
            lastValue = v;
        }
    }
}
