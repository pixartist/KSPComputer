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
        private SubRoutine subRoutineInstance;
        public SubRoutine SubRoutineInstance
        {
            get
            {
                return subRoutineInstance;
            }
        }
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
            Log.Write("Reloading subroutine " + SubRoutineBlueprint);
            if (this.subRoutineInstance != null)
            {
                this.subRoutineInstance.Destroy();
                this.subRoutineInstance.ExitNode.OnExecuted -= ExitNode_OnExecuted;
                this.subRoutineInstance.EntryNode.OnRequestData -= EntryNode_OnRequestData;
                this.subRoutineInstance = null;
            }
            try
            {
                this.subRoutineInstance = KSPOperatingSystem.LoadSubRoutine(SubRoutineBlueprint, true);
            }
            catch(Exception e)
            {
                Log.Write("Failed to load subroutine: " + e.Message);
            }
            if (this.subRoutineInstance == null)
            {
                Log.Write("Could not load SubRoutine " + SubRoutineBlueprint + ", deleting node");
                RequestRemoval();
            }
            else
            {
                this.subRoutineInstance.ExitNode.OnExecuted += ExitNode_OnExecuted;
                this.subRoutineInstance.EntryNode.OnRequestData += EntryNode_OnRequestData;
                SyncInputs();
                SyncOutputs();
            }
        }

        
        private void SyncOutputs()
        {
            List<string> toRemove = new List<string>(OutputNames);
            ConnectorOut tmp;
            foreach (var o in subRoutineInstance.ExitNode.inputs)
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
            foreach (var o in subRoutineInstance.ExitNode.inputs)
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
            foreach (var i in subRoutineInstance.EntryNode.outputs)
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
            foreach (var i in subRoutineInstance.EntryNode.outputs)
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
            subRoutineInstance.EntryNode.Execute(input);
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
                    if (subRoutineInstance.EntryNode.outputs.TryGetValue(i.Key, out tmp))
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
                subRoutineInstance.ExitNode.UpdateOutputData();
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
                    if (subRoutineInstance.ExitNode.inputs.TryGetValue(o.Key, out tmp))
                    {
                        Out(o.Key, tmp.buffer);
                    }
                }
            }
        }
        public override void OnUpdate()
        {
            subRoutineInstance.Update();
        }
        public override void OnLaunch()
        {
            subRoutineInstance.Launch();
        }
        protected override void OnDestroy()
        {
            subRoutineInstance.Destroy();
        }
    }
}
