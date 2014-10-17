using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KSPFlightPlanner.Program
{
    public static class VesselHelper
    {
        public static float FuelRemaining(this Vessel v)
        {
            float fuel = 0;
            var parts = v.rootPart.FindChildParts<FuelTank>(true);
            foreach(var tank in parts)
            {
                fuel += tank.fuel;
            }
            return fuel;
        }
        public static bool CanSavelySeperateCurrentStage(this Vessel v)
        {
           // Log.Write("Checking currentStage: " + v.currentStage);
            if(v.currentStage < 1)
                return false;
            var activeResources = v.GetActiveResourceIDs();
            //Log.Write("Active resource count: " + activeResources.Count);
            foreach(var part in v.Parts)
            {
                if (part.inverseStage == v.currentStage - 1)
                {
                    //Log.Write("Found part in next stage: " + part);
                    if(part.IsUnfiredDecoupler())
                    {
                        //Log.Write("Part is not a decoupler: " + part);
                        if (!part.CanBeSavelySeperated(activeResources))
                            return false;
                    }
                }
            }
            return true;
        }
        public static List<int> GetActiveResourceIDs(this Vessel v)
        {
            var activeEngines = from p in v.Parts where p.inverseStage >= v.currentStage && p.IsEngine() select p;
            HashSet<Propellant> activePropellants = new HashSet<Propellant>();
            foreach(var e in activeEngines)
            {
                foreach (ModuleEngines m in e.Modules.OfType<ModuleEngines>())
                    if (!m.getFlameoutState)
                        activePropellants.UnionWith(m.propellants);
                foreach (ModuleEnginesFX m in e.Modules.OfType<ModuleEnginesFX>())
                    if (m.isEnabled && !m.getFlameoutState)
                        activePropellants.UnionWith(m.propellants);     
            }
            return activePropellants.Select(prop => prop.id).ToList();
        }
    }
}
