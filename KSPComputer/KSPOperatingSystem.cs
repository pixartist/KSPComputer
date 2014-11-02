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
namespace KSPComputer
{
    public static class KSPOperatingSystem
    {
        public static Rect SmallWindowRect = new Rect(270, 45, 300, 300);
        public static string PluginPath { get; private set; }
        public static int ProgramCount
        {
            get
            {
                return loadedPrograms.Count;
            }
        }
        private static List<FlightProgram> loadedPrograms = new List<FlightProgram>();
        public static VesselController VesselController { get; private set; }
        public static void Boot(string pluginPath)
        {
            PluginPath = pluginPath;
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
        }
        public static void CustomAction(int action)
        {
            foreach (var p in loadedPrograms)
                p.CustomAction(action);
        }
        public static void InitPrograms()
        {
            foreach (var p in loadedPrograms)
                p.Init();
        }
        public static void AddProgram()
        {
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
            loadedPrograms.Clear();
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
            //temp
            if(loadedPrograms.Count > 1)
                loadedPrograms.RemoveRange(1, loadedPrograms.Count -1);
            InitPrograms();
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
