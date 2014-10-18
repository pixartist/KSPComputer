using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPFlightPlanner.Program.Connectors;
namespace KSPFlightPlanner.Program.Nodes
{
    [Serializable]
    public class NodeFloatCompare : ExecutableNode
    {
        public new static string Name = "Compare (Float)";
        public new static string Description = "Compares two floats";
        public new static SVector3 Color = new SVector3(1f, 1f, 1f);
        public new static SVector2 Size = new SVector2(190, 200);
        protected override void OnCreate()
        {
            In<float>("A");
            In<float>("B");
            Out<Connector.Exec>("<", false);
            Out<Connector.Exec>("<=", false);
            Out<Connector.Exec>("==", false);
            Out<Connector.Exec>(">=", false);
            Out<Connector.Exec>(">", false);
        }
        protected override void OnExecute()
        {
            ExecuteNext();

            var a = In("A").AsFloat();
            var b = In("B").AsFloat();
            if (a < b)
                ExecuteNext("<");
            if (a <= b)
                ExecuteNext("<=");
            if (a == b)
                ExecuteNext("==");
            if (a >= b)
                ExecuteNext(">=");
            if (a > b)
                ExecuteNext(">");
            
         
        }
    }
}
