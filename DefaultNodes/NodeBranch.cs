using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
namespace DefaultNodes
{
    [Serializable]
    public class NodeBranch : DefaultExecutableNode
    {
        protected override void OnCreate()
        {
            In<bool>("Input");
            Out<Connector.Exec>("True", false);
            Out<Connector.Exec>("False", false);
        }
        protected override void OnExecute(ConnectorIn input)
        {
            bool isTrue = In("Input").AsBool();
            //Log.Write("Checking branch: " + isTrue);
            ExecuteNext();
            if(isTrue)
                ExecuteNext("True");
            else
                ExecuteNext("False");
        }
    }
}
