using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPFlightPlanner.Program.Connectors;
namespace KSPFlightPlanner.Program.Nodes
{
    [Serializable]
    public class NodeHeading : Node
    {
        public new static string Name = "Heading";
        public new static string Description = "Returns the current heading";
        public new static SVector3 Color = new SVector3(0.5f, 1f, 0.5f);
        public new static SVector2 Size = new SVector2(190, 50);
        protected override void OnCreate()
        {
            Out<SQuaternion>("Heading");
        }
        protected override void OnUpdateOutputData()
        {
            Out("Heading", new SQuaternion(Program.Vessel.ReferenceTransform.rotation));
        }
    }
}
