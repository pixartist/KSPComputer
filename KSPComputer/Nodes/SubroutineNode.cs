using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPComputer.Connectors;
using UnityEngine;
namespace KSPComputer.Nodes
{
    [Serializable]
    public class SubroutineNode : DefaultExecutableNode
    {
        public string SubRoutineBlueprint {get; private set;}

        [NonSerialized]
        private SubRoutine subRoutine;
        public void SetSubRoutine(string subRoutine)
        {
            this.SubRoutineBlueprint = subRoutine;
        }
        public override void OnInit()
        {
            ReloadSubroutine();
        }
        private void ReloadSubroutine()
        {
            //Log.Write("Reloading subroutine");
            if (this.subRoutine != null)
            {
                this.subRoutine.ExitNode.OnExecuted -= ExitNode_OnExecuted;
                this.subRoutine = null;
            }
            this.subRoutine = KSPOperatingSystem.LoadSubRoutine(SubRoutineBlueprint, true);
            if (this.subRoutine == null)
            {
                Log.Write("Could not load SubRoutine " + SubRoutineBlueprint + ", deleting node");
                RequestRemoval();
            }
            else
            {
                this.subRoutine.ExitNode.OnExecuted += ExitNode_OnExecuted;
                this.subRoutine.EntryNode.OnRequestData += EntryNode_OnRequestData;
                SyncInputs();
                SyncOutputs();
            }
        }

        
        private void SyncOutputs()
        {
            List<string> toRemove = new List<string>(OutputNames);
            ConnectorOut tmp;
            foreach (var o in subRoutine.ExitNode.inputs)
            {
                if (outputs.TryGetValue(o.Key, out tmp))
                {
                    if (tmp.DataType == o.Value.DataType)
                        toRemove.Remove(o.Key);
                }
            }
            foreach (var t in toRemove)
            {
                RemoveOutput(t);
            }
            foreach (var o in subRoutine.ExitNode.inputs)
            {
                if (!outputs.ContainsKey(o.Key))
                {
                    Out(o.Key, o.Value.DataType, true);
                }
            }
        }
        private void SyncInputs()
        {
            List<string> toRemove = new List<string>(InputNames);
            ConnectorIn tmp;
            foreach (var i in subRoutine.EntryNode.outputs)
            {
                if(inputs.TryGetValue(i.Key, out tmp))
                {
                    if(tmp.DataType == i.Value.DataType)
                        toRemove.Remove(i.Key);
                }
            }
            foreach (var t in toRemove)
            {
                RemoveInput(t);
            }
            foreach (var i in subRoutine.EntryNode.outputs)
            {
                if (!inputs.ContainsKey(i.Key))
                {
                    In(i.Key, i.Value.DataType, false);
                }
            }
        }
        public override void Execute(ConnectorIn input)
        {
            LastExecution = Time.time;
            subRoutine.EntryNode.Execute(input);
        }
        void ExitNode_OnExecuted(string name)
        {
            LastExecution = Time.time;
            ExecuteNext(name);
        }
        void EntryNode_OnRequestData()
        {
            RequestInputUpdates();
            ConnectorOut tmp;
            foreach (var i in inputs)
            {
                if (i.Value.DataType != typeof(Connector.Exec))
                {
                    if (subRoutine.EntryNode.outputs.TryGetValue(i.Key, out tmp))
                    {
                        tmp.SendData(i.Value.buffer);
                    }
                }
            }
        }
        public override void UpdateOutputData()
        {
            //Log.Write("Subroutine " + SubRoutineBlueprint + " requests ouput (" + outputs.Count + " outputs)");
            try
            {
                subRoutine.ExitNode.UpdateOutputData();
            }
            catch(Exception e)
            {
                Log.Write("Subroutine failed to update output data! " + e.Message);
                throw (new Exception("Subroutine failed to update output data! " + e.Message, e));
            }
            ConnectorIn tmp;
            foreach (var o in outputs)
            {
                if (o.Value.DataType != typeof(Connector.Exec))
                {
                    if (subRoutine.ExitNode.inputs.TryGetValue(o.Key, out tmp))
                    {
                        Out(o.Key, tmp.buffer);
                    }
                }
            }
        }
        public override void OnUpdate()
        {
            subRoutine.Update();
        }
        public override void OnLaunch()
        {
            subRoutine.Launch();
        }
    }
}
