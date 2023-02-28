using System;
using System.Collections.Generic;

using System.Text;
using UnityEngine;
namespace KSPComputer
{
    public class Log
    {
        public static string LogData = "";
        public static void Write(string info)
        {
            string s = "[FlightComputer]: " + info;
            LogData += Environment.NewLine + s;
            Debug.Log(s);
        }
    }
}