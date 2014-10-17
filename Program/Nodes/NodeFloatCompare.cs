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
            AddConnectorIn("A", new FloatConnectorIn());
            AddConnectorIn("B", new FloatConnectorIn());
            AddConnectorOut("<", new ExecConnectorOut());
            AddConnectorOut("<=", new ExecConnectorOut());
            AddConnectorOut("==", new ExecConnectorOut());
            AddConnectorOut(">=", new ExecConnectorOut());
            AddConnectorOut(">", new ExecConnectorOut());
        }
        protected override void OnExecute()
        {
            ExecuteNext();

            var a = GetConnectorIn("A").GetBufferAsFloat();
            var b = GetConnectorIn("B").GetBufferAsFloat();
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
