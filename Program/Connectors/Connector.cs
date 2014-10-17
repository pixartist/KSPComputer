using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPFlightPlanner.Program.Nodes;
namespace KSPFlightPlanner.Program.Connectors
{
    [Serializable]
    public abstract class Connector
    {
        protected List<Connector> connections;
        public bool Connected
        {
            get
            {
                return connections.Count > 0;
            }
        }
        public Connector[] Connections
        {
            get
            {
                return connections.ToArray();
            }
        }
        public Node Node { get; private set; }
        public bool AllowMultipleConnections { get; private set; }
        public Type DataType { get; private set; }
        public Connector(Type dataType, bool allowMultipleConnections)
        {
            connections = new List<Connector>();
            AllowMultipleConnections = allowMultipleConnections;
            DataType = dataType;
        }
        internal void Init(Node owner)
        {
            Node = owner;
        }
        private bool RemoveConnection(Connector other)
        {
            if (connections.Contains(other))
            {
                connections.Remove(other);
                return true;
            }
            return false;
        }
        private void AddConnection(Connector other)
        {
            //cut single connection
            if (!AllowMultipleConnections)
            {
                foreach (var c in connections)
                    DisconnectFrom(c);
            }
            connections.Add(other);
        }
        public void ConnectTo(Connector other)
        {
            if (other != null)
            {
                if (other.DataType == DataType)
                {
                    if ((this is ConnectorIn || other is ConnectorIn) && (this is ConnectorOut || other is ConnectorOut))
                    {
                        if (!connections.Contains(other))
                        {
                            AddConnection(other);
                            other.AddConnection(this);
                        }
                    }
                }
            }
        }
        public void DisconnectFrom(Connector other)
        {
            if (RemoveConnection(other))
            {
                other.RemoveConnection(this);
            }
        }
        public void DisconnectAll()
        {
            foreach (var c in connections)
                DisconnectFrom(c);
        }
    }
}
