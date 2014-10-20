using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPFlightPlanner.Program.Nodes;
namespace KSPFlightPlanner.Program.Connectors
{
    [Serializable]
    public class ConnectorOut : Connector
    {
        internal ConnectorOut(Type dataType, bool allowMultipleConnections = true)
            : base(dataType, allowMultipleConnections)
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
            foreach (var c in (from cn in connections where cn.Node is ExecutableNode select cn))
            {
                //Log.Write("Executing next node from connector");
                (c.Node as ExecutableNode).Execute(c as ConnectorIn);
            }
        }
    }
}