using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPFlightPlanner.Program.Connectors;
namespace KSPFlightPlanner.Program.Nodes
{
    [Serializable]
    public class NodeCompare : ExecutableNode
    {
        protected override void OnCreate()
        {
            In<double>("A");
            In<double>("B");
            Out<Connector.Exec>("<", false);
            Out<Connector.Exec>("<=", false);
            Out<Connector.Exec>("==", false);
            Out<Connector.Exec>(">=", false);
            Out<Connector.Exec>(">", false);
        }
        protected override void OnExecute(ConnectorIn input)
        {
            ExecuteNext();

            var a = In("A").AsDouble();
            var b = In("B").AsDouble();
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
