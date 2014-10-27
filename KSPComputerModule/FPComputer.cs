using System;
using System.Collections;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using KSP;
using UnityEngine;
using KSPComputer;
using System.IO;
namespace KSPComputerModule
{
    public class FPComputer : PartModule
    {
        private FlightProgram activeProgram;
        private ProgramDrawer drawer;
        private bool started = false;
        private double startTime = 0;
        private float fps = 0;
        public PartModule.StartState LastStartState { get; private set; }
        #region customActions
        [KSPAction("Custom Action 1")]
        public void CustomAction1(KSPActionParam param)
        {
            if (activeProgram != null && started) activeProgram.TriggerAction(1);
        }
        [KSPAction("Custom Action 2")]
        public void CustomAction2(KSPActionParam param)
        {
            if (activeProgram != null && started) activeProgram.TriggerAction(2);
        }
        [KSPAction("Custom Action 3")]
        public void CustomAction3(KSPActionParam param)
        {
            if (activeProgram != null && started) activeProgram.TriggerAction(3);
        }
        [KSPAction("Custom Action 4")]
        public void CustomAction4(KSPActionParam param)
        {
            if (activeProgram != null && started) activeProgram.TriggerAction(4);
        }
        [KSPAction("Custom Action 5")]
        public void CustomAction5(KSPActionParam param)
        {
            if (activeProgram != null && started) activeProgram.TriggerAction(5);
        }
        [KSPAction("Custom Action 6")]
        public void CustomAction6(KSPActionParam param)
        {
            if (activeProgram != null && started) activeProgram.TriggerAction(6);
        }
        [KSPAction("Custom Action 7")]
        public void CustomAction7(KSPActionParam param)
        {
            if (activeProgram != null && started) activeProgram.TriggerAction(7);
        }
        [KSPAction("Custom Action 8")]
        public void CustomAction8(KSPActionParam param)
        {
            if (activeProgram != null && started) activeProgram.TriggerAction(8);
        }
        #endregion
        public override void OnStart(PartModule.StartState state)
        {
            LastStartState = state;
            //Log.Write("TAC Examples-SimplePartModule [" + this.GetInstanceID().ToString("X") + "][" + Time.time.ToString("0.0000") + "]: OnStart: " + state);
            Log.Write("Starting " + state);
            if (activeProgram == null)
            {
                activeProgram = new FlightProgram();
                drawer = new ProgramDrawer(this, activeProgram);
            }
            
            if ((LastStartState & StartState.PreLaunch) == StartState.PreLaunch)
            {
                StartCoroutine("StartDelay");  
                started = false;
                startTime = 0;
            }
            
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
            Log.Write("FPS: " + fps);
            startTime = Planetarium.GetUniversalTime();
            Log.Write("Start time: " + startTime);
            double t = startTime;
            while (t  < (startTime + 1))
            {
                //Log.Write("Vessel not ready " + Planetarium.GetUniversalTime());
                t = Planetarium.GetUniversalTime();
                yield return null;
            }
            Log.Write("Vessel ready " + Planetarium.GetUniversalTime());
            
            activeProgram.Init(vessel);
            activeProgram.Launch();
            started = true;
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
            
            if (started && activeProgram != null)
            {
                activeProgram.Update();
            }
            

        }
        public void OnGUI()
        {
            if (LastStartState == StartState.Editor && drawer.Show)
                EditorTooltip.Instance.HideToolTip();
            if (drawer != null)
                drawer.Draw();
        }

        public override void OnLoad(ConfigNode node)
        {
            //Log.Write("TAC Examples-SimplePartModule [" + this.GetInstanceID().ToString("X") + "][" + Time.time.ToString("0.0000") + "]: OnLoad: " + node);
            Log.Write("Loading program " + LastStartState + ", Vessel :" + vessel);
            if (node.HasValue("FlightProgram"))
            {
                string v = node.GetValue("FlightProgram");
                //Log.Write("Node Value found: " + v);
                v = v.Replace('_', '/');
                try
                {
                    BinaryFormatter f = new BinaryFormatter();
                    using (MemoryStream fs = new MemoryStream(Convert.FromBase64String(v)))
                    {
                        activeProgram = (FlightProgram)f.Deserialize(fs);
                        Log.Write("Program loaded from file");
                    }
                }
                catch (Exception e)
                {
                    Log.Write("Error loading program: " + e.Message + " (State: " + LastStartState + ")");
                    activeProgram = new FlightProgram();
                }
                
            }
            else
            {
                activeProgram = new FlightProgram();
            }
            drawer = new ProgramDrawer(this, activeProgram);
        }
        public override void OnSave(ConfigNode node)
        {
            //Log.Write("TAC Examples-SimplePartModule [" + this.GetInstanceID().ToString("X") + "][" + Time.time.ToString("0.0000") + "]: OnSave: " + node);
            Log.Write("Saving program " + LastStartState);
            if (activeProgram != null)
            {
                try
                {
                    BinaryFormatter f = new BinaryFormatter();
                    using (MemoryStream fs = new MemoryStream())
                    {
                        f.Serialize(fs, activeProgram);
                        string data = Convert.ToBase64String(fs.ToArray()).Replace('/', '_'); ;
                        node.AddValue("FlightProgram", data);
                       // Log.Write("Saved program: " + data + " (State: " + currentState + ")");

                    }


                }
                catch (Exception e)
                {
                    Log.Write("Could not save flight program: " + e.Message + " (State: " + LastStartState + ")");
                }
            }
        }
    }
}
