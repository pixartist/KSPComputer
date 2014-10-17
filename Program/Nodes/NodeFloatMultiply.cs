using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPFlightPlanner.Program.Connectors;
namespace KSPFlightPlanner.Program.Nodes
{
    [Serializable]
    public class NodeFloatMultiply : Node
    {
        public new static string Name = "Multiply (Float)";
        public new static string Description = "Multiplies two floats";
        public new static SVector3 Color = new SVector3(0.2f, 1f, 1f);
        public new static SVector2 Size = new SVector2(190, 150);
        protected override void OnCreate()
        {
            AddConnectorIn("A", new FloatConnectorIn());
            AddConnectorIn("B", new FloatConnectorIn());
            AddConnectorOut("Out", new FloatConnectorOut());
        }
        protected override void OnUpdateOutputData()
        {
            var o = GetConnectorOut("Out");
            if (o != null)
            {
                var a = GetConnectorIn("A").GetBufferAsFloat();
                var b = GetConnectorIn("B").GetBufferAsFloat();
                o.SendData(a * b);
            }
         
        }
    }
}
