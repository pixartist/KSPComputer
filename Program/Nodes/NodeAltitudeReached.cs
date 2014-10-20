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
        private double lastAltitude;
        protected override void OnCreate()
        {
            In<double>("Altitude");
            In<bool>("Down");
            Program.OnTick += Program_OnTick;
            lastAltitude = double.MinValue;
        }

        void Program_OnTick()
        {
            Execute(null);
        }
        protected override void OnExecute(ConnectorIn input)
        {
            double v = In("Altitude").AsDouble();
            bool down = In("Down").AsBool();
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