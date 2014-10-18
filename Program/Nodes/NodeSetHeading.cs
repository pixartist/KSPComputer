using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KSPFlightPlanner.Program.Connectors;
namespace KSPFlightPlanner.Program.Nodes
{
    [Serializable]
    public class NodeSetHeading : ExecutableNode
    {
        public new static string Name = "Set heading";
        public new static string Description = "Sets the heading relative to the orbited body";
        public new static SVector3 Color = new SVector3(1, 1, 0.2f);
        public new static SVector2 Size = new SVector2(150, 100);
        protected override void OnCreate()
        {
            In<SQuaternion>("Quaternion");
        }
        protected override void OnExecute()
        {
            SQuaternion h = In("Quaternion").AsQuaternion();
            Program.SASController.SASTarget = h.GetQuaternion();
            ExecuteNext();
        }
    }
}