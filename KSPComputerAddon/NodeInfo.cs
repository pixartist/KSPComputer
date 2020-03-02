using System;
using UnityEngine;
namespace KSPComputerAddon {
    public struct NodeInfo {
        public string name;
        public Type type;
        public string description;
        public Color color;
        public float width;
        public NodeInfo(string name, Type type, string description, Color color, float width) {
            this.name = name;
            this.type = type;
            this.description = description;
            this.color = color;
            this.width = width;
        }
    }
}
