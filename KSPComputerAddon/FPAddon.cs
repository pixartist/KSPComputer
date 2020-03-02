using KSPComputer;
using System;
using System.IO;
using UnityEngine;
namespace KSPComputerModule {
    public class FPComputer : VesselModule {
        //private ProgramDrawer drawer;
        private bool isAwake = false;
        private double startTime = 0;
        private float fps = 0;
        //private Rect loadedWindowRect = new Rect(200, 200, 800, 600);
        //private Rect smallWindowRect = new Rect(270, 45, 300, 70);
        private string loadedPrograms = null;
        private bool programsCompressed = true;
        public PartModule.StartState LastStartState { get; private set; }
        #region customActions
        [KSPAction("Custom Action 1")]
        public void CustomAction1(KSPActionParam param) {
            KSPOperatingSystem.CustomAction(1);
        }
        [KSPAction("Custom Action 2")]
        public void CustomAction2(KSPActionParam param) {
            KSPOperatingSystem.CustomAction(2);
        }
        [KSPAction("Custom Action 3")]
        public void CustomAction3(KSPActionParam param) {
            KSPOperatingSystem.CustomAction(3);
        }
        [KSPAction("Custom Action 4")]
        public void CustomAction4(KSPActionParam param) {
            KSPOperatingSystem.CustomAction(4);
        }
        [KSPAction("Custom Action 5")]
        public void CustomAction5(KSPActionParam param) {
            KSPOperatingSystem.CustomAction(5);
        }
        [KSPAction("Custom Action 6")]
        public void CustomAction6(KSPActionParam param) {
            KSPOperatingSystem.CustomAction(6);
        }
        [KSPAction("Custom Action 7")]
        public void CustomAction7(KSPActionParam param) {
            KSPOperatingSystem.CustomAction(7);
        }
        [KSPAction("Custom Action 8")]
        public void CustomAction8(KSPActionParam param) {
            KSPOperatingSystem.CustomAction(8);
        }
        #endregion
        public override void OnLoadVessel() {
            Log.Write("Loaded Vessel");
            base.OnAwake();
            KSPOperatingSystem.Boot(Path.Combine(Path.Combine(Environment.CurrentDirectory, "GameData"), "FlightComputer"));
            KSPOperatingSystem.SetVessel(Vessel);
            if (loadedPrograms == null)
                KSPOperatingSystem.AddProgram();
            else {
                try {
                    KSPOperatingSystem.LoadStateBase64(loadedPrograms, programsCompressed);
                    Log.Write(KSPOperatingSystem.ProgramCount + " programs loaded from file");
                } catch (Exception e) {
                    KSPOperatingSystem.AddProgram();
                    Log.Write("Error loading program: " + e.Message + " (State: " + LastStartState + ")");
                }
                loadedPrograms = null;
            }
            loadedPrograms = null;
            isAwake = true;
            KSPOperatingSystem.Launch();
        }
        public override void OnUnloadVessel() {
            Log.Write("Unloaded Vessel");
            base.OnUnloadVessel();
            isAwake = false;
        }
        protected override void OnStart() {
            Log.Write("Starting");
            GUIController.Start();
            GameEvents.onPartDie.Add(OnExplosion);
            /*
             GameEvents.onPartDie.Add(OnExplosion);
             LastStartState = state;
             GUIController.lastStartState = state;
             GUIController.Start();
             //Log.Write("TAC Examples-SimplePartModule [" + this.GetInstanceID().ToString("X") + "][" + Time.time.ToString("0.0000") + "]: OnStart: " + state);
             Log.Write("Starting with state: " + state);

             StartCoroutine("StartDelay");
             startTime = 0;


             //drawer = new ProgramDrawer(this, loadedWindowRect, smallWindowRect);
             if (loadedPrograms == null)
                 KSPOperatingSystem.AddProgram();
             else {
                 try {
                     KSPOperatingSystem.LoadStateBase64(loadedPrograms, programsCompressed);
                     Log.Write(KSPOperatingSystem.ProgramCount + " programs loaded from file");
                 } catch (Exception e) {
                     KSPOperatingSystem.AddProgram();
                     Log.Write("Error loading program: " + e.Message + " (State: " + LastStartState + ")");
                 }
                 loadedPrograms = null;
             }
             loadedPrograms = null;
         }
         private bool CheckVesselReady() {
             if ((LastStartState & StartState.PreLaunch) == StartState.PreLaunch)
                 if (Math.Abs(vessel.orbit.timeToAp) > 0.1)
                     return false;
             return true;
         }
         private IEnumerator StartDelay() {
             while (!CheckVesselReady()) {
                 //Log.Write("Vessel not ready");
                 yield return null;
             }
             //wait for stable fps:
             while (vessel == null ? true : !FlightGlobals.ready) {
                 yield return null;
             }
             KSPOperatingSystem.SetVessel(vessel);
             //Log.Write("FPS: " + fps);
             startTime = Planetarium.GetUniversalTime();
             Log.Write("Start time: " + startTime);
             double t = startTime;
             while (t < (startTime + 2)) {
                 //Log.Write("Vessel not ready " + Planetarium.GetUniversalTime());
                 t = Planetarium.GetUniversalTime();
                 yield return null;
             }
             //Log.Write("Vessel ready " + Planetarium.GetUniversalTime());
             if ((LastStartState & StartState.PreLaunch) == StartState.PreLaunch)
                 KSPOperatingSystem.Launch();
                 */
        }
        /*
         * Called every frame
         */
        private void Update() {
            if (isAwake) {
                float t = Time.time;
                fps = 0.95f * fps + 0.05f * (1 / Time.deltaTime);
                KSPOperatingSystem.Update();
            }
        }
        public void OnGUI() {
            if (isAwake) {
                GUIController.Draw();
            }
        }
        void OnExplosion(Part p) {
            if (isAwake) {
                if (p.inverseStage < KSPOperatingSystem.VesselController.Vessel.currentStage) {
                    KSPOperatingSystem.Anomaly();
                }
            }
        }
        protected override void OnLoad(ConfigNode node) {
            loadedPrograms = null;
            KSPOperatingSystem.Boot(Path.Combine(Path.Combine(Environment.CurrentDirectory, "GameData"), "FlightComputer"));
            //Log.Write("TAC Examples-SimplePartModule [" + this.GetInstanceID().ToString("X") + "][" + Time.time.ToString("0.0000") + "]: OnLoad: " + node);
            Log.Write("Loading program, State: " + LastStartState + ", Vessel: " + vessel);
            if (node.HasValue("FlightProgram")) {
                loadedPrograms = node.GetValue("FlightProgram");
                programsCompressed = node.HasValue("IsCompressed");
            } else {
                Log.Write("No program found!");
            }
            GUIController.LoadState(node);
        }
        protected override void OnSave(ConfigNode node) {
            //Log.Write("TAC Examples-SimplePartModule [" + this.GetInstanceID().ToString("X") + "][" + Time.time.ToString("0.0000") + "]: OnSave: " + node);
            Log.Write("Saving program " + LastStartState);

            try {
                if (loadedPrograms != null) {
                    node.AddValue("FlightProgram", loadedPrograms);
                    Log.Write("Program string saved, state: " + LastStartState);
                } else {
                    string data = KSPOperatingSystem.SaveStateBase64(true);
                    node.AddValue("FlightProgram", data);
                    Log.Write(KSPOperatingSystem.ProgramCount + " programs saved, state: " + LastStartState);
                }
                node.AddValue("IsCompressed", "yes");
                GUIController.SaveState(node);
            } catch (Exception e) {
                Log.Write("Could not save flight program: " + e.Message + " at " + e.StackTrace + " (State: " + LastStartState + ")");
            }
        }
    }
}
