using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace KSPComputer.Helpers
{
    public static class MathfHelper
    {
        public static float SignedAngle(this Vector3 normal, Vector3 a, Vector3 b)
        {
            return Mathf.Atan2(
                Vector3.Dot(normal, Vector3.Cross(a, b)),
                Vector3.Dot(a, b)) * Mathf.Rad2Deg;
        }
    }
}
