using KSPComputerModule;
using UnityEngine;

namespace KSPComputerAddon {
    public struct CatInfo {
        public string name;
        public Color color;
        public CatInfo(string name, Color color) {
            this.name = name;
            this.color = color;
        }
        public CatInfo(CategoryModel token) {
            this.name = token.name;
            this.color = Tools.FromRGB(token.r, token.g, token.b);
        }
    }
}
