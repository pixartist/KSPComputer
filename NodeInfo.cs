using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace KSPFlightPlanner
{
    public class NodeInfo
    {
        public string Name { get; private set; }
        public Color Color { get; private set; }
        public Vector2 Size { get; private set; }
        public NodeInfo(string name, Color color, Vector2 size)
        {
            Name = name;
            Color = color;
            Size = size;
        }
        public override string ToString()
        {
            return "Node [" + Name + "]";
        }
    }
}
