using System;
using System.Collections.Generic;

using System.Text;
using UnityEngine;
namespace KSPComputer.Types
{
    [Serializable]
    public struct SVector2
    {
        public static explicit operator SVector2(SVector2d b)
        {
            SVector2 v = new SVector2((float)b.x, (float)b.y);
            return v;
        }
        public float x, y;
        public SVector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
        public Vector2 GetVec2()
        {
            return new Vector2(x, y);
        }
        public override string ToString()
        {
            return String.Format("SVector2: {0}, {1}", x, y);
        }
    }
}