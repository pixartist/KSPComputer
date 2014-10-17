using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KSPFlightPlanner.Program
{
    public static class PartHelper
    {
        /*
         * TODO
         * Most of this is deprecated
         * Use Part.resources
         * 
         * */
        public static bool CanBeSavelySeperated(this Part part, List<int> activeResources)
        {
            if(!part.IsSepratron())
            {
            if(part.State == PartStates.ACTIVE || part.State == PartStates.IDLE)
            {
                if(part.IsEngine())
                {
                    if (part.EngineHasFuel())
                        return false;
                }
            }
            if(part is FuelTank)
            {
                if((part as FuelTank).fuel > 0)
                {
                    return false;
                }
            }
            if (!part.IsSepratron())
            {
                foreach (PartResource r in part.Resources)
                {
                    if (r.amount > 0 && r.info.name != "ElectricCharge" && activeResources.Contains(r.info.id))
                    {
                        return false;
                    }
                }
            }
            }
            foreach(var c in part.children)
            {
                if (!c.CanBeSavelySeperated(activeResources))
                    return false;
            }
            return true;
        }
        public static bool IsEngine(this Part part)
        {
            foreach (PartModule m in part.Modules)
            {
                if (m is ModuleEngines || m is ModuleEnginesFX)
                    return true;
            }
            return false;
        }
        public static bool EngineHasFuel(this Part part)
        {
            foreach (PartModule m in part.Modules)
            {
                ModuleEngines engines = m as ModuleEngines;
                if (engines != null) 
                    return !engines.getFlameoutState;

                ModuleEnginesFX engineFX = m as ModuleEnginesFX;
                if (engineFX != null)
                    return !engineFX.getFlameoutState;
            }
            return false;
        }
        public static bool IsUnfiredDecoupler(this Part part)
        {
            foreach (PartModule m in part.Modules)
            {
                ModuleDecouple mDecouple = m as ModuleDecouple;
                if (mDecouple != null)
                {
                    if (!mDecouple.isDecoupled) 
                        return true;
                    break;
                }
                ModuleAnchoredDecoupler mAnchoredDecoupler = m as ModuleAnchoredDecoupler;
                if (mAnchoredDecoupler != null)
                {
                    if (!mAnchoredDecoupler.isDecoupled) 
                        return true;
                    break;
                }
            }
            return false;
        }
        public static bool IsSepratron(this Part part)
        {
            return part.ActivatesEvenIfDisconnected
                && part.IsEngine()
                && part.IsDecoupledInStage(part.inverseStage)
                && !part.isControlSource;
        }
        public static bool IsDecoupledInStage(this Part part, int stage)
        {
            if ((part.IsUnfiredDecoupler() || part.IsLaunchClamp()) && part.inverseStage == stage)
                return true;
            if (part.parent == null) 
                return false;
            return part.parent.IsDecoupledInStage(stage);
        }
        public static bool IsLaunchClamp(this Part part)
        {
            foreach (PartModule m in part.Modules)
            {
                if (m is LaunchClamp) 
                    return true;
            }
            return false;
        }
    }
}
