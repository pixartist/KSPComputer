﻿using System;
using System.Collections.Generic;

using System.Text;
using UnityEngine;
using KSPComputer.Helpers;
using KSPComputer.Types;
using KSPComputer.Connectors;
namespace KSPComputer.Nodes
{

    [Serializable]
    public abstract class Node
    {
        public static VesselController VesselController
        {
            get
            {
                return KSPOperatingSystem.VesselController;
            }
        }
        public static Vessel Vessel
        {
            get
            {
                return KSPOperatingSystem.VesselController.Vessel;
            }
        }
        public static SASController SASController
        {
            get
            {
                return KSPOperatingSystem.VesselController.SASController;
            }
        }
        public const string DefaultExecName = "Exec";
        public delegate void RequestRemovalHandler(Node n);
        public event RequestRemovalHandler OnRequestRemoval;
        public SVector2 Position;
        
        internal Dictionary<string, ConnectorIn> inputs;
        internal Dictionary<string, ConnectorOut> outputs;
        public int InputCount
        {
            get
            {
                return inputs.Count;
            }
        }
        public int InputExecCount
        {
            get
            {
                int k = 0;
                foreach(var i in inputs) {
                    if(i.Value.DataType == typeof(Connector.Exec)) {
                        k++;
                    }
                }
                return k;
            }
        }
        public int OutputCount
        {
            get
            {
                return outputs.Count;
            }
        }
        public KeyValuePair<string, ConnectorIn>[] Inputs
        {
            get
            {
                return ToArray(inputs);
            }
        }
        public KeyValuePair<string, ConnectorOut>[] Outputs
        {
            get
            {
                return ToArray(outputs);
            }
        }
        public string[] OutputNames
        {
            get
            {
                return ToArray(outputs.Keys);
            }
        }
        public string[] InputNames
        {
            get
            {
                return ToArray(inputs.Keys);
            }
        }
        public Node()
        {
            Position = new SVector2();
            inputs = new Dictionary<string, ConnectorIn>();
            outputs = new Dictionary<string, ConnectorOut>();
        }
        internal void Create()
        {
            OnCreate();
        }
        protected void RequestRemoval()
        {
            if (OnRequestRemoval != null)
                OnRequestRemoval(this);
        }
        internal void In(string name, Type t, bool allowMultipleConnections = false)
        {
            name = name.Trim();
            if (!inputs.ContainsKey(name))
            {
                var connector = new ConnectorIn(t, name, t.IsValueType ? Activator.CreateInstance(t) : null, allowMultipleConnections);
                connector.Init(this);
                inputs.Add(name, connector);
            }
        }
        internal void Out(string name, Type t, bool allowMultipleConnections = true)
        {
            name = name.Trim();
            if (!outputs.ContainsKey(name))
            {
                var connector = new ConnectorOut(t, name, allowMultipleConnections);
                connector.Init(this);
                outputs.Add(name, connector);
            }
        }
        protected void In<T>(string name, bool allowMultipleConnections = false)
        {
            In(name, typeof(T), allowMultipleConnections);
        }
        protected void Out<T>(string name, bool allowMultipleConnections = true)
        {
            Out(name, typeof(T), allowMultipleConnections);
        }
        public void RemoveInput(string name)
        {
            if(inputs.ContainsKey(name))
            {
                inputs[name].DisconnectAll();
                inputs.Remove(name);
            }
        }
        public void RemoveOutput(string name)
        {
            if (outputs.ContainsKey(name))
            {
                outputs[name].DisconnectAll();
                outputs.Remove(name);
            }
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
        public virtual void UpdateOutputData()
        {
            RequestInputUpdates();
            OnUpdateOutputData();
        }
        public virtual void OnInit()
        { }
        public virtual void OnLaunch()
        { }
        public virtual void OnUpdate()
        { }
        public virtual void OnCustomAction(int action)
        { }
        protected virtual void OnUpdateOutputData()
        { }
        protected virtual void OnCreate()
        { }
        protected virtual void OnDestroy()
        { }
        public virtual void OnAnomaly()
        { }
        protected void RequestInputUpdate(string node)
        {
            ConnectorIn c;
            if(inputs.TryGetValue(node, out c))
            {
                c.RequestData();
            }
        }
        protected void RequestInputUpdates()
        {
            foreach (var i in inputs.Values)
            {
                i.FreshData = false;
            }
            foreach (var i in inputs.Values)
            {
                if (!i.FreshData && !(i.DataType == typeof(Connector.Exec)))
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
            List<Connector> result = new List<Connector>();
            result.AddRange(GetConnectedConnectorsIn());
            result.AddRange(GetConnectedConnectorsOut());
            return result;
        }
        public IEnumerable<Connector> GetConnectedConnectorsIn()
        {
            List<Connector> result = new List<Connector>();
            foreach(var c in inputs.Values) {
                if(c.Connected) {
                    result.Add(c);
                }
            }
            return result;
        }
        public IEnumerable<Connector> GetConnectedConnectorsOut()
        {
            List<Connector> result = new List<Connector>();
            foreach(var c in outputs.Values) {
                if(c.Connected) {
                    result.Add(c);
                }
            }
            return result;
        }

        private static KeyValuePair<A,B>[] ToArray<A, B>(Dictionary<A,B> input) {
            var keys = input.Keys;
            var output = new List<KeyValuePair<A,B>>();
            foreach(var key in keys) {
                output.Add(new KeyValuePair<A, B>(key, input[key]));
            }
            return output.ToArray();
        }

        private static T[] ToArray<T>(ICollection<T> input) {
            var output = new List<T>();
            foreach(var entry in input) {
                output.Add(entry);
            }
            return output.ToArray();
        }
    }
}
