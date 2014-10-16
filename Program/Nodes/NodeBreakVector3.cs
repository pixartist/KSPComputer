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
    public class NodeBreakVector3 : Node
    {
        public override void OnCreate()
        {
            Outputs.Add("X", new NCFloatOut(this));
			Outputs.Add("Y", new NCFloatOut(this));
			Outputs.Add("Z", new NCFloatOut(this));
        }
        void Program_OnTick(FlightProgram.FlightEvent e)
        {
            Execute();
        }
        protected override void OnExecute()
        {
			if (Outputs["Altitude"].ConnectedTo != null)
            {
				Outputs["Altitude"].ConnectedTo.DataBuffer = Program.Vessel.altitude;
            }
        }
    }
}
