using System;
using System.Collections;
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
            KSPOperatingSystem.SetVessel(null);
            LastStartState = state;
            //Log.Write("TAC Examples-SimplePartModule [" + this.GetInstanceID().ToString("X") + "][" + Time.time.ToString("0.0000") + "]: OnStart: " + state);
            Log.Write("Starting with state: " + state);
            if ((LastStartState & StartState.PreLaunch) == StartState.PreLaunch)
            {
                StartCoroutine("StartDelay");  
                startTime = 0;
            }
            drawer = new ProgramDrawer(this);
            if (KSPOperatingSystem.ProgramCount < 1)
                KSPOperatingSystem.AddProgram();
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
            while (fps < 10 || vessel == null)
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

        public override void OnLoad(ConfigNode node)
        {
            KSPOperatingSystem.Boot(Path.Combine(Path.Combine(Environment.CurrentDirectory, "GameData"), "FlightComputer"));
            //Log.Write("TAC Examples-SimplePartModule [" + this.GetInstanceID().ToString("X") + "][" + Time.time.ToString("0.0000") + "]: OnLoad: " + node);
            Log.Write("Loading program, State: " + LastStartState + ", Vessel: " + vessel);
            if (node.HasValue("FlightProgram"))
            {
                string v = node.GetValue("FlightProgram");
                try
                {
                    KSPOperatingSystem.LoadStateBase64(v, node.HasValue("IsCompressed"));
                    Log.Write("Program loaded from file");
                }
                catch (Exception e)
                {
                    KSPOperatingSystem.ClearPrograms();
                    KSPOperatingSystem.AddProgram();
                    Log.Write("Error loading program: " + e.Message + " (State: " + LastStartState + ")");
                }
            }
        }
        public override void OnSave(ConfigNode node)
        {
            //Log.Write("TAC Examples-SimplePartModule [" + this.GetInstanceID().ToString("X") + "][" + Time.time.ToString("0.0000") + "]: OnSave: " + node);
            Log.Write("Saving program " + LastStartState);

            try
            {
                string data = KSPOperatingSystem.SaveStateBase64(true);
                node.AddValue("FlightProgram", data);
                node.AddValue("IsCompressed", "yes");
            }
            catch (Exception e)
            {
                Log.Write("Could not save flight program: " + e.Message + " at " + e.StackTrace + " (State: " + LastStartState + ")");
            }
        }
    }
}
