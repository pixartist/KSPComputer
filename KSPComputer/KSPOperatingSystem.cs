using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPComputer.Helpers;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
using Ionic.Zlib;
using UnityEngine;
using KSPComputer.Nodes;
namespace KSPComputer
{
    public static class KSPOperatingSystem
    {
        
        public static string PluginPath { get; private set; }
        public static int ProgramCount
        {
            get
            {
                return loadedPrograms.Count;
            }
        }
        private static List<FlightProgram> loadedPrograms = new List<FlightProgram>();
        private static Dictionary<Node, Func<string>> watchedValues = new Dictionary<Node, Func<string>>();
        private static Dictionary<Action, Func<string>> actionButtons = new Dictionary<Action, Func<string>>();
        public static VesselController VesselController { get; private set; }
        public static void Boot(string pluginPath)
        {
            KSPOperatingSystem.ClearPrograms();
            PluginPath = pluginPath;
            SetVessel(null);
        }
        public static void SetVessel(Vessel vessel)
        {
            
            if (vessel == null)
                VesselController = null;
            else
                VesselController = new VesselController(vessel);
        }
        public static void Launch()
        {
            foreach (var p in loadedPrograms)
                p.Launch();
        }
        public static void Update()
        {
            if (VesselController != null)
            {
                VesselController.Update();
                foreach (var p in loadedPrograms)
                    p.Update();
            }
            else
            {
                Log.Write("Error, VesselController is null");
            }
        }
        public static void CustomAction(int action)
        {
            foreach (var p in loadedPrograms)
                p.CustomAction(action);
        }
        public static void Anomaly()
        {
            foreach (var p in loadedPrograms)
                p.Anomaly();
        }
        public static void InitPrograms()
        {
            foreach (var p in loadedPrograms)
                p.Init();
        }
        public static void AddProgram()
        {
            Log.Write("Created empty program");
            var p = new FlightProgram();
            p.Init();
            loadedPrograms.Add(p);
        }
        public static FlightProgram GetProgram(int id)
        {
            return loadedPrograms[id];
        }
        public static void ClearPrograms()
        {
            Log.Write("Cleared programs");
            foreach (var p in loadedPrograms)
                p.Destroy();
            loadedPrograms.Clear();
        }
        public static void AddWatchedValue(Node n, Func<string> getter)
        {
            if(!watchedValues.ContainsKey(n))
                watchedValues.Add(n, getter);
        }
        public static void RemoveWatchedValue(Node n)
        {
            if (watchedValues.ContainsKey(n))
                watchedValues.Remove(n);
        }
        public static string[] GetWatchedValues()
        {
            string[] wv = new string[watchedValues.Count];
            int i = 0;
            foreach(var v in watchedValues.Values)
            {
                wv[i] = v();
                i++;
            }
            return wv;
        }
        public static void AddActionButton(Action onClick, Func<string> nameGetter)
        {
            if (!actionButtons.ContainsKey(onClick))
                actionButtons.Add(onClick, nameGetter);
        }
        public static void RemoveActionButton(Action onClick)
        {
            if (actionButtons.ContainsKey(onClick))
                actionButtons.Remove(onClick);
        }
        public static Dictionary<Action, Func<string>> GetActionButtons()
        {
            return actionButtons;
        }
        public static string SaveStateBase64(bool compressed)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter f = new BinaryFormatter();
                if (compressed)
                {

                    using (DeflateStream gz = new DeflateStream(ms, CompressionMode.Compress))
                    {
                        f.Serialize(gz, loadedPrograms);
                    }
                }
                else
                {
                    f.Serialize(ms, loadedPrograms);
                }
                return Convert.ToBase64String(ms.ToArray()).Replace('/', '_');
            }
            
        }
        public static void LoadStateBase64(string base64, bool compressed)
        {
            
            base64 = base64.Replace('_', '/');
            byte[] data = Convert.FromBase64String(base64);

            using (MemoryStream ms = new MemoryStream(data))
            {
                BinaryFormatter f = new BinaryFormatter();
                if (compressed)
                {

                    using (DeflateStream gz = new DeflateStream(ms, CompressionMode.Decompress))
                    {
                        loadedPrograms = (List<FlightProgram>)f.Deserialize(gz);
                    }
                }
                else
                {
                    loadedPrograms = (List<FlightProgram>)f.Deserialize(ms);
                }
            }
            InitPrograms();
            //temp
        }
        public static string[] ListSubRoutines()
        {
            string subroutineLoc = Path.Combine(PluginPath, "SubRoutines");
            if (!Directory.Exists(subroutineLoc))
                return new string[] { };
            var files =  Directory.GetFiles(subroutineLoc, "*.sr");
            for (int i = 0; i < files.Length; i++)
                files[i] = Path.GetFileNameWithoutExtension(files[i]);
            return files;
        }
        public static void DeleteSubRoutine(string name)
        {
            string subroutineLoc = Path.Combine(PluginPath, "SubRoutines");
            string path = Path.Combine(subroutineLoc, name + ".sr");

            if (File.Exists(path))
                File.Delete(path);
            foreach(var p in loadedPrograms)
            {
                p.RemoveSubRoutineNodes(name);
            }
        }
        public static SubRoutine LoadSubRoutine(string name, bool compressed)
        {
            string subroutineLoc = Path.Combine(PluginPath, "SubRoutines");
            string path = Path.Combine(subroutineLoc, name + ".sr");
            SubRoutine sr = null;
            if(!File.Exists(path))
                return null;
            using (FileStream fs = new FileStream(path, FileMode.Open))
            { 
                BinaryFormatter f = new BinaryFormatter();
                if (compressed)
                {
                    using (DeflateStream gz = new DeflateStream(fs, CompressionMode.Decompress))
                    {
                        sr = (SubRoutine)f.Deserialize(gz);
                    }
                }
                else
                {
                    sr = (SubRoutine)f.Deserialize(fs);
                }
            }
            sr.Init();
            return sr;
        }
        public static void SaveSubRoutine(string name, SubRoutine subRoutine, bool compressed)
        {
            string subroutineLoc = Path.Combine(PluginPath, "SubRoutines");
            if (!Directory.Exists(subroutineLoc))
                Directory.CreateDirectory(subroutineLoc);
            string path = Path.Combine(subroutineLoc, name + ".sr");
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                BinaryFormatter f = new BinaryFormatter();
                if (compressed)
                {
                    using (DeflateStream gz = new DeflateStream(fs, CompressionMode.Compress))
                    {
                        f.Serialize(gz, subRoutine);
                    }
                }
                else
                {
                    f.Serialize(fs, subRoutine);
                }
            }
        }
    }
}
