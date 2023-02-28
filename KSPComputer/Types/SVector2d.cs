using System;
using System.Collections.Generic;

using System.Text;
using UnityEngine;
namespace KSPComputer.Types
{
    [Serializable]
    public struct SVector2d
    {
        public static explicit operator SVector2d(SVector2 b) 
        {
            SVector2d v = new SVector2d(b.x, b.y);
            return v;
        }
        public double x, y;
        public SVector2d(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
        public Vector2d GetVec2()
        {
            return new Vector2d(x, y);
        }
        public override string ToString()
        {
            return String.Format("SVector2: {0}, {1}", x, y);
        }
    }
}