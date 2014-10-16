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
    public class NodeTick : Node
    {
        public override void OnCreate()
        {
            Outputs.Add("Action", new NCActionOut(this));
            Program.OnTick += Program_OnTick;
        }
        void Program_OnTick(FlightProgram.FlightEvent e)
        {
            Execute();
        }
        protected override void OnExecute()
        {
			TriggerOutput();
        }
    }
}
