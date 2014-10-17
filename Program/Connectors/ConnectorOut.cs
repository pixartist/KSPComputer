using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPFlightPlanner.Program.Nodes;
namespace KSPFlightPlanner.Program.Connectors
{
    [Serializable]
    public abstract class ConnectorOut : Connector
    {
        public ConnectorOut(Type dataType, bool allowMultipleConnections = true)
            : base(dataType, allowMultipleConnections)
        {

        }
        public void SendData(Object data)
        {
            foreach (var c in connections)
            {
                (c as ConnectorIn).SetData(data);
            }
        }
        internal void Execute()
        {
            foreach (var c in (from cn in connections where cn.Node is ExecutableNode select cn.Node as ExecutableNode))
            {
                c.Execute();
            }
        }
    }
}