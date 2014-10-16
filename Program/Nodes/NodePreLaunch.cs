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
    public class NodePreLaunch : Node
    {
        public override void OnCreate()
        {
            Outputs.Add("Action", new NCActionOut(this));
			Program.OnLoad += Program_OnLoad;
        }

		void Program_OnLoad(FlightProgram.FlightEvent e)
		{
			if(e == FlightProgram.FlightEvent.PreLaunch)
				Execute();
		}
        protected override void OnExecute()
        {
			TriggerOutput();
        }
    }
}
