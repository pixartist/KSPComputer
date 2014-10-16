using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPFlightPlanner.Program;
using KSPFlightPlanner.Program.NodeDataTypes;
using UnityEngine;
namespace KSPFlightPlanner.Program.Nodes
{

	[Serializable]
	public class NodeSetHeading : Node
    {
        public override void OnCreate()
        {
			Inputs.Add(DefaultExecName, new NCActionIn(this));
			Inputs.Add("Heading", new NCVector3In(this));
			Outputs.Add(DefaultExecName, new NCActionOut(this));
        }
        protected override void OnExecute()
		{
			var vec = Inputs["Heading"].GetBufferAsVector3();
			Program.Vessel.VesselSAS.LockHeading(Quaternion.Euler(vec.GetVec3()));
			TriggerOutput();
        }
    }
}