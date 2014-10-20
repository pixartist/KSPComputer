using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using KSP;
using UnityEngine;
using KSPComputer;
using System.IO;
using KSPComputer;
namespace KSPFlightPlanner
{
    public class FPComputer : PartModule
    {
        private FlightProgram activeProgram;
        private ProgramDrawer drawer;
        public PartModule.StartState LastStartState { get; private set; }
        
        public override void OnStart(PartModule.StartState state)
        {
            LastStartState = state;
            //Log.Write("TAC Examples-SimplePartModule [" + this.GetInstanceID().ToString("X") + "][" + Time.time.ToString("0.0000") + "]: OnStart: " + state);
            Log.Write("Starting " + state);
            if (activeProgram == null)
            {
                activeProgram = new FlightProgram();
                activeProgram.Init(vessel);
                drawer = new ProgramDrawer(this, activeProgram);
            }
            if ((LastStartState & StartState.PreLaunch) == StartState.PreLaunch)
            {
                activeProgram.Launch();
            }
            
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
            if (LastStartState != StartState.Editor && activeProgram != null)
                activeProgram.Update();
            

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
            Log.Write("Loading program " + LastStartState);
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
                        
                    }
                }
                catch (Exception e)
                {
                    Log.Write("Error loading program: " + e.Message + " (State: " + LastStartState + ")");
                    activeProgram = new FlightProgram();
                }
                activeProgram.Init(vessel);
                drawer = new ProgramDrawer(this, activeProgram);
            }
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
