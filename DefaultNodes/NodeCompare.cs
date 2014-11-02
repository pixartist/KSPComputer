using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
namespace DefaultNodes
{
    [Serializable]
    public class NodeCompare : DefaultExecutableNode
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
