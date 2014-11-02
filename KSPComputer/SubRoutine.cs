using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPComputer.Helpers;
using KSPComputer.Nodes;
using KSPComputer.Types;
using KSPComputer.Variables;
using UnityEngine;
namespace KSPComputer
{
    [Serializable]
    public class SubRoutine : FlightProgram
    {
        public SubRoutineEntry EntryNode { get; private set; }
        public SubRoutineExit ExitNode { get; private set; }
        public SubRoutine()
            : base()
        {
            EntryNode = AddNode<SubRoutineEntry>(new Vector2(100, 100));
            ExitNode = AddNode<SubRoutineExit>(new Vector2(300, 100));
        }
        public override void RemoveNode(Node node)
        {
            if(!(node == EntryNode || node == ExitNode))
                base.RemoveNode(node);
        }
    }
}
