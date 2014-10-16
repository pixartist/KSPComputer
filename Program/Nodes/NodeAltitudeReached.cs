using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KSPFlightPlanner.Program;
using KSPFlightPlanner.Program.NodeDataTypes;
namespace KSPFlightPlanner.Program.Nodes
{
	[Serializable]
    public class NodeAltitudeReached : Node
    {
		private double lastHeight;
        public override void OnCreate()
        {
            Inputs.Add("Altitude", new NCDoubleIn(this));
			Inputs.Add("Down", new NCBoolIn(this));
			lastHeight = double.MinValue;
			Outputs.Add(DefaultExecName, new NCActionOut(this));
            Program.OnTick += Program_OnTick;
        }

        void Program_OnTick(FlightProgram.FlightEvent e)
        {
			double v = Inputs["Altitude"].GetBufferAsDouble();
			bool down = Inputs["Down"].GetBufferAsBool();
			double alt = Program.Vessel.altitude;
			if (down)
			{
				if (lastHeight > v)
				{
					if (alt <= v)
					{
						Execute();
					}
				}
			}
			else
			{
				if (lastHeight < v)
				{
					if (alt >= v)
					{
						Execute();
					}
				}
			}
			lastHeight = alt;
        }
		protected override void OnExecute()
		{
			TriggerOutput();
		}
    }
}
