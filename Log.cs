using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace KSPFlightPlanner
{
    public class Log
    {
        public static void Write(string info)
        {
            Debug.Log("[FlightPlanner]: " + info);
        }
    }
}