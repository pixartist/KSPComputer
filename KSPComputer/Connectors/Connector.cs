using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPComputer.Nodes;
namespace KSPComputer.Connectors
{
    [Serializable]
    public abstract class Connector
    {
        [Serializable]
        public struct Exec
        {
        }
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
        public string Name
        {
            get;
            private set;
        }
        public Node Node { get; private set; }
        public bool AllowMultipleConnections { get; private set; }
        public Type DataType { get; private set; }
        public Connector(Type dataType, string name, bool allowMultipleConnections)
        {
            Name = name;
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
            var arr = connections.ToArray();
            foreach (var c in arr)
                DisconnectFrom(c);
        }
    }
}
