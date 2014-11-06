using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using KSP;
using UnityEngine;
using KSPComputer;
using KSPComputer.Helpers;
using System.IO;
using System.IO.Compression;
namespace KSPComputerModule
{
    public class FPComputer : PartModule
    {
        private ProgramDrawer drawer;
        private double startTime = 0;
        private float fps = 0;
        private Rect loadedWindowRect = new Rect(200, 200, 800, 600);
        private Rect smallWindowRect = new Rect(270, 45, 300, 70);
        private string loadedPrograms = null;
        private bool programsCompressed = true;
        public PartModule.StartState LastStartState { get; private set; }
        #region customActions
        [KSPAction("Custom Action 1")]
        public void CustomAction1(KSPActionParam param)
        {
            KSPOperatingSystem.CustomAction(1);
        }
        [KSPAction("Custom Action 2")]
        public void CustomAction2(KSPActionParam param)
        {
            KSPOperatingSystem.CustomAction(2);
        }
        [KSPAction("Custom Action 3")]
        public void CustomAction3(KSPActionParam param)
        {
            KSPOperatingSystem.CustomAction(3);
        }
        [KSPAction("Custom Action 4")]
        public void CustomAction4(KSPActionParam param)
        {
            KSPOperatingSystem.CustomAction(4);
        }
        [KSPAction("Custom Action 5")]
        public void CustomAction5(KSPActionParam param)
        {
            KSPOperatingSystem.CustomAction(5);
        }
        [KSPAction("Custom Action 6")]
        public void CustomAction6(KSPActionParam param)
        {
            KSPOperatingSystem.CustomAction(6);
        }
        [KSPAction("Custom Action 7")]
        public void CustomAction7(KSPActionParam param)
        {
            KSPOperatingSystem.CustomAction(7);
        }
        [KSPAction("Custom Action 8")]
        public void CustomAction8(KSPActionParam param)
        {
            KSPOperatingSystem.CustomAction(8);
        }
        #endregion
        public override void OnStart(PartModule.StartState state)
        {
            KSPOperatingSystem.Boot(Path.Combine(Path.Combine(Environment.CurrentDirectory, "GameData"), "FlightComputer"));
            
            GameEvents.onPartExplode.Add(OnExplosion);
            LastStartState = state;
            //Log.Write("TAC Examples-SimplePartModule [" + this.GetInstanceID().ToString("X") + "][" + Time.time.ToString("0.0000") + "]: OnStart: " + state);
            Log.Write("Starting with state: " + state);

            StartCoroutine("StartDelay");  
            startTime = 0;
            
            
            drawer = new ProgramDrawer(this, loadedWindowRect, smallWindowRect);
            if (loadedPrograms == null)
                KSPOperatingSystem.AddProgram();
            else
            {
                try
                {
                    KSPOperatingSystem.LoadStateBase64(loadedPrograms, programsCompressed);
                    Log.Write(KSPOperatingSystem.ProgramCount + " programs loaded from file");
                }
                catch (Exception e)
                {
                    KSPOperatingSystem.AddProgram();
                    Log.Write("Error loading program: " + e.Message + " (State: " + LastStartState + ")");
                }
                loadedPrograms = null;
            }
            loadedPrograms = null;
        }
        private bool CheckVesselReady()
        {
            if ((LastStartState & StartState.PreLaunch) == StartState.PreLaunch)
                if (Math.Abs(vessel.orbit.timeToAp) > 0.1)
                    return false;
            return true;
        }
        private IEnumerator StartDelay()
        {
            while (!CheckVesselReady())
            {
                //Log.Write("Vessel not ready");
                yield return null;
            }
            //wait for stable fps:
            while (vessel == null ? true : !FlightGlobals.ready)
            {
                yield return null;
            }
            KSPOperatingSystem.SetVessel(vessel);
            //Log.Write("FPS: " + fps);
            startTime = Planetarium.GetUniversalTime();
            Log.Write("Start time: " + startTime);
            double t = startTime;
            while (t  < (startTime + 2))
            {
                //Log.Write("Vessel not ready " + Planetarium.GetUniversalTime());
                t = Planetarium.GetUniversalTime();
                yield return null;
            }
            //Log.Write("Vessel ready " + Planetarium.GetUniversalTime());
            if ((LastStartState & StartState.PreLaunch) == StartState.PreLaunch)
                KSPOperatingSystem.Launch();
        }
        public override void OnAwake()
        {
            base.OnAwake();
        }
        /*
         * Called every frame
         */
        public override void OnUpdate()
        {
            float t = Time.time;
            fps = 0.95f * fps + 0.05f * (1 / Time.deltaTime);

            KSPOperatingSystem.Update();
        }
        public void OnGUI()
        {
            
            if (drawer != null)
            {
                if (LastStartState == StartState.Editor && drawer.Show)
                    EditorTooltip.Instance.HideToolTip();
                drawer.Draw();
            }
        }
        void OnExplosion(GameEvents.ExplosionReaction r)
        {
            
            KSPOperatingSystem.Anomaly();
        }
        public override void OnLoad(ConfigNode node)
        {
            loadedPrograms = null;
            KSPOperatingSystem.Boot(Path.Combine(Path.Combine(Environment.CurrentDirectory, "GameData"), "FlightComputer"));
            //Log.Write("TAC Examples-SimplePartModule [" + this.GetInstanceID().ToString("X") + "][" + Time.time.ToString("0.0000") + "]: OnLoad: " + node);
            Log.Write("Loading program, State: " + LastStartState + ", Vessel: " + vessel);
            if (node.HasValue("FlightProgram"))
            {
                loadedPrograms = node.GetValue("FlightProgram");
                programsCompressed = node.HasValue("IsCompressed");
            }
            else
            {
                Log.Write("No program found!");
            }
            if(node.HasValue("WindowRectX"))
                loadedWindowRect.x = float.Parse(node.GetValue("WindowRectX"));
            if (node.HasValue("WindowRectY"))
                loadedWindowRect.y = float.Parse(node.GetValue("WindowRectY"));
            if (node.HasValue("WindowRectW"))
                loadedWindowRect.width = float.Parse(node.GetValue("WindowRectW"));
            if (node.HasValue("WindowRectH"))
                loadedWindowRect.height = float.Parse(node.GetValue("WindowRectH"));
            if (node.HasValue("SmallWindowX"))
                smallWindowRect.x = float.Parse(node.GetValue("SmallWindowX"));
            if (node.HasValue("SmallWindowY"))
                smallWindowRect.y = float.Parse(node.GetValue("SmallWindowY"));
            if (node.HasValue("SmallWindowW"))
                smallWindowRect.width = float.Parse(node.GetValue("SmallWindowW"));
            if (node.HasValue("SmallWindowH"))
                smallWindowRect.height = float.Parse(node.GetValue("SmallWindowH"));
        }
        public override void OnSave(ConfigNode node)
        {
            //Log.Write("TAC Examples-SimplePartModule [" + this.GetInstanceID().ToString("X") + "][" + Time.time.ToString("0.0000") + "]: OnSave: " + node);
            Log.Write("Saving program " + LastStartState);

            try
            {
                if (loadedPrograms != null)
                {
                    node.AddValue("FlightProgram", loadedPrograms);
                    Log.Write("Program string saved, state: " + LastStartState);
                }
                else
                {
                    string data = KSPOperatingSystem.SaveStateBase64(true);
                    node.AddValue("FlightProgram", data);
                    Log.Write(KSPOperatingSystem.ProgramCount + " programs saved, state: " + LastStartState);
                }
                node.AddValue("IsCompressed", "yes");
                if (drawer != null)
                {
                    node.AddValue("WindowRectX", drawer.windowRect.x.ToString());
                    node.AddValue("WindowRectY", drawer.windowRect.y.ToString());
                    node.AddValue("WindowRectW", drawer.windowRect.width.ToString());
                    node.AddValue("WindowRectH", drawer.windowRect.height.ToString());
                    node.AddValue("SmallWindowX", drawer.smallWindowRect.x.ToString());
                    node.AddValue("SmallWindowY", drawer.smallWindowRect.y.ToString());
                    node.AddValue("SmallWindowW", drawer.smallWindowRect.width.ToString());
                    node.AddValue("SmallWindowH", drawer.smallWindowRect.height.ToString());
                }

                
            }
            catch (Exception e)
            {
                Log.Write("Could not save flight program: " + e.Message + " at " + e.StackTrace + " (State: " + LastStartState + ")");
            }
        }
    }
}
