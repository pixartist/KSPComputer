using System.Linq;

namespace KSPComputer.Helpers {
    public static class PartHelper {
        public static double CountMaxResources(this Part part, params DefaultResources[] resources) {
            if (part.IsUnfiredDecoupler() || part.IsSepratron())
                return 0.0;

            return (from PartResource r in part.Resources where resources.Contains((DefaultResources)r.info.id) select r.maxAmount).Sum();
        }
        public static double CountRemainingResources(this Part part, params DefaultResources[] resources) {
            if (part.IsUnfiredDecoupler() || part.IsSepratron())
                return 0.0;
            return (from PartResource r in part.Resources where resources.Contains((DefaultResources)r.info.id) && r.amount > 0.01 select r.amount).Sum();
        }
        public static bool CheckEngineHasFuel(this Part part) {
            if (!part.IsEngine())
                return false;
            else if (part.IsSepratron())
                return false;
            else {
                foreach (PartModule m in part.Modules) {
                    ModuleEngines eng = m as ModuleEngines;
                    if (eng != null) return !eng.getFlameoutState;

                    ModuleEnginesFX engFX = m as ModuleEnginesFX;
                    if (engFX != null) return !engFX.getFlameoutState;
                }
            }
            return false;
        }
        public static float GetMaxThrust(this Part part) {
            float t = 0;
            foreach (PartModule pm in part.Modules) {
                if (pm is ModuleEngines)
                    t += (pm as ModuleEngines).maxThrust;
                if (pm is ModuleEnginesFX)
                    t += (pm as ModuleEnginesFX).maxThrust;
            }
            return t;
        }

        public static double CountMaxResourcesInChildren(this Part part, params DefaultResources[] resources) {
            return CountMaxResources(part, resources) + (from Part p in part.children select p.CountMaxResourcesInChildren(resources)).Sum();
        }
        public static double CountRemainingResourcesInChildren(this Part part, params DefaultResources[] resources) {
            return CountRemainingResources(part, resources) + (from Part p in part.children select p.CountRemainingResourcesInChildren(resources)).Sum();
        }
        public static bool CheckEnginesHaveFuelInChildren(this Part part) {
            if (part.CheckEngineHasFuel())
                return true;
            foreach (var c in part.children) {
                if (c.CheckEnginesHaveFuelInChildren())
                    return true;
            }
            return false;
        }
        public static float CountMaxThrustInChildren(this Part part) {
            return part.GetMaxThrust() + (from Part p in part.children select p.CountMaxThrustInChildren()).Sum();
        }

        public static bool IsUnfiredDecoupler(this Part part) {
            foreach (PartModule m in part.Modules) {

                ModuleDecouple mDecouple = m as ModuleDecouple;
                if (mDecouple != null) {
                    if (!mDecouple.isDecoupled)
                        return true;
                    break;
                }
                ModuleAnchoredDecoupler mAnchoredDecoupler = m as ModuleAnchoredDecoupler;
                if (mAnchoredDecoupler != null) {
                    if (!mAnchoredDecoupler.isDecoupled)
                        return true;
                    break;
                }
            }
            return false;
        }
        public static bool IsStackSeperator(this Part part) {

            return part.ActivatesEvenIfDisconnected
                && !part.IsEngine()
                && part.IsDecoupledInStage(part.inverseStage)
                && part.isControlSource > 0;
        }
        public static bool IsSepratron(this Part part) {
            return part.ActivatesEvenIfDisconnected
                && part.IsEngine()
                && part.IsDecoupledInStage(part.inverseStage)
                && part.isControlSource > 0;
        }
        public static bool IsEngine(this Part part) {
            foreach (PartModule module in part.Modules) {
                if (module is ModuleEngines || module is ModuleEnginesFX)
                    return true;
            }
            return false;
        }
        public static bool IsDecoupledInStage(this Part part, int stage) {
            if ((part.IsUnfiredDecoupler() || part.IsLaunchClamp()) && part.inverseStage == stage)
                return true;
            if (part.parent == null)
                return false;
            return part.parent.IsDecoupledInStage(stage);
        }
        public static bool IsLaunchClamp(this Part part) {
            foreach (PartModule m in part.Modules) {
                if (m is LaunchClamp)
                    return true;
            }
            return false;
        }
    }
}
