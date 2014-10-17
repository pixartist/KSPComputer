using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPFlightPlanner.Program.Connectors;
namespace KSPFlightPlanner.Program.Nodes
{
    [Serializable]
    public class NodeAltitudeReached : RootNode
    {
        public new static string Name = "Altitude reached";
        public new static string Description = "Called when the specified altitude is reached";
        public new static SVector3 Color = new SVector3(0.2f, 0.2f, 1f);
        public new static SVector2 Size = new SVector2(170, 130);
        private double lastAltitude;
        protected override void OnCreate()
        {
            AddConnectorIn("Altitude", new DoubleConnectorIn());
            AddConnectorIn("Down", new BoolConnectorIn());
            Program.OnTick += Program_OnTick;
            lastAltitude = double.MinValue;
        }

        void Program_OnTick()
        {
            Execute();
        }
        protected override void OnExecute()
        {
            double v = GetConnectorIn("Altitude").GetBufferAsDouble();
            bool down = GetConnectorIn("Down").GetBufferAsBool();
            double alt = Program.Vessel.altitude;
            if (down)
            {
                if (lastAltitude > v)
                {
                    if (alt <= v)
                    {
                        ExecuteNext();
                    }
                }
            }
            else
            {
                if (lastAltitude < v)
                {
                    if (alt >= v)
                    {
                        ExecuteNext();
                    }
                }
            }
            lastAltitude = alt;
        }
    }
}