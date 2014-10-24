using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KSPComputer.Types;
using KSPComputer.Connectors;
namespace KSPComputer.Nodes
{

    [Serializable]
    public abstract class Node
    {
        public SVector2 Position;
        public int InputCount
        {
            get
            {
                return inputs.Count;
            }
        }
        public int OutputCount
        {
            get
            {
                return outputs.Count;
            }
        }
        public FlightProgram Program { get; private set; }
        private Dictionary<string, ConnectorIn> inputs;
        private Dictionary<string, ConnectorOut> outputs;
        public KeyValuePair<string, ConnectorIn>[] Inputs
        {
            get
            {
                return inputs.ToArray();
            }
        }
        public KeyValuePair<string, ConnectorOut>[] Outputs
        {
            get
            {
                return outputs.ToArray();
            }
        }
        public Node()
        {
            Position = new SVector2();
            inputs = new Dictionary<string, ConnectorIn>();
            outputs = new Dictionary<string, ConnectorOut>();
        }
        internal virtual void Init(FlightProgram program)
        {
            Program = program;
            OnCreate();
        }
        protected void In<T>(string name, bool allowMultipleConnections = false)
        {
            var connector = new ConnectorIn(typeof(T), default(T), allowMultipleConnections);
            connector.Init(this);
            inputs.Add(name, connector);
        }
        protected void Out<T>(string name, bool allowMultipleConnections = true)
        {
            var connector = new ConnectorOut(typeof(T), allowMultipleConnections);
            connector.Init(this);
            outputs.Add(name, connector);
        }
        protected void Out(string name, object value)
        {
            ConnectorOut o;
            if (outputs.TryGetValue(name, out o))
            {
                if (o.Connected)
                {
                    o.SendData(value);
                }
            }
        }
        protected ConnectorOut GetOuput(string name, bool connected = true)
        {
            ConnectorOut o;
            if (outputs.TryGetValue(name, out o))
            {
                if (o.Connected || !connected)
                {
                    return o;
                }
            }
            return null;
        }
        protected ConnectorIn In(string name)
        {
            ConnectorIn o;
            if (inputs.TryGetValue(name, out o))
            {
                return o;
            }
            return null;
        }
        public void UpdateOutputData()
        {
            
            RequestInputUpdates();
            OnUpdateOutputData();
        }
        protected virtual void OnUpdateOutputData()
        { }
        protected virtual void OnCreate()
        { }
        protected virtual void OnDestroy()
        { }
        protected void RequestInputUpdates()
        {
            foreach (var i in inputs.Values)
            {
                i.FreshData = false;
            }
            foreach (var i in inputs.Values)
            {
                if (!i.FreshData)
                {
                    i.RequestData();
                }
            }
        }
        private void DisconnectAllInputs()
        {
            var keys = inputs.Keys;
            foreach (var k in keys)
                inputs[k].DisconnectAll();
            inputs.Clear();
        }
        private void DisconnectAllOutputs()
        {
            var keys = outputs.Keys;
            foreach (var k in keys)
                outputs[k].DisconnectAll();
        }
        public void Destroy()
        {
           
            OnDestroy();
            DisconnectAllInputs();
            DisconnectAllOutputs();
        }
        public IEnumerable<Connector> GetConnectedConnectors()
        {
            return (from c in inputs.Values where c.Connected select c as Connector).Concat(from c in outputs.Values where c.Connected select c as Connector);
        }
        public IEnumerable<Connector> GetConnectedConnectorsIn()
        {
            return (from c in inputs.Values where c.Connected select c as Connector);
        }
        public IEnumerable<Connector> GetConnectedConnectorsOut()
        {
            return (from c in outputs.Values where c.Connected select c as Connector);
        }
    }
}
