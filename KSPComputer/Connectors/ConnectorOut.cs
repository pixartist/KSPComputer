using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPComputer.Nodes;
namespace KSPComputer.Connectors
{
    [Serializable]
    public class ConnectorOut : Connector
    {
        internal ConnectorOut(Type dataType, string name, bool allowMultipleConnections = true)
            : base(dataType, name, allowMultipleConnections)
        {

        }
        internal void SendData(Object data)
        {
            foreach (var c in connections)
            {
                (c as ConnectorIn).Set(data);
            }
        }
        internal void Execute()
        {
            //Log.Write("Trying to execute next node from connector");
            //Log.Write("Connection count: " + connections.Count);
            foreach (var c in (from cn in connections where cn.Node is BaseExecutableNode select cn))
            {
                //Log.Write("Executing next node from connector");
                (c.Node as BaseExecutableNode).Execute(c as ConnectorIn);
            }
        }

    }
}