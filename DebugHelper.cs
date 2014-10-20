using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace KSPFlightPlanner
{
    static class DebugHelper
    {
        public static LineRenderer AddLine(Vessel v, Color c)
        {
            var obj = new GameObject("line");
            obj.transform.parent = v.gameObject.transform;
            var line = obj.AddComponent<LineRenderer>();

            line.material = new Material(Shader.Find("Particles/Additive"));
            line.SetColors(c, c);
            line.enabled = true;
            line.SetWidth(0.5f, 0.5f);
            line.SetVertexCount(2);
            return line;
        }
        public static void UpdateLine(LineRenderer line, Vector3 origin, Vector3 direction)
        {
            line.SetPosition(0, origin);
            line.SetPosition(1, origin + direction);
        }
    }
}
