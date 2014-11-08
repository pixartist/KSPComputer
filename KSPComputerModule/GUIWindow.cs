using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace KSPComputerModule
{
    public abstract class GUIWindow
    {
        
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Opened { get; set; }
        public abstract string Title { get; }
        public Rect WinRect;
        public abstract Vector2 MinSize { get; }
        public abstract void Draw();
        public virtual void Start() { }
    }
}
