using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPFlightPlanner.Program.Connectors;
namespace KSPFlightPlanner.Program.Nodes
{
    [Serializable]
    public class NodeDoubleToFloat : Node
    {
        public new static string Name = "Double to Float";
        public new static string Description = "Converts a double input to a float output";
        public new static SVector3 Color = new SVector3(0.2f, 1f, 1f);
        public new static SVector2 Size = new SVector2(190, 100);
        protected override void OnCreate()
        {
            AddConnectorIn("Double", new DoubleConnectorIn());
            AddConnectorOut("Float", new FloatConnectorOut());
        }
        protected override void OnUpdateOutputData()
        {
            var d = GetConnectorIn("Double");
            if (d != null)
            {
                var f = GetConnectorOut("Float");
                if (f != null)
                    f.SendData((float)d.GetBufferAsDouble());
            }
        }
    }
}
