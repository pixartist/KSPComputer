using System;
using System.Collections.Generic;

using System.Text;
using KSPComputer;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
using KSPComputer.Helpers;
namespace DefaultNodes
{
    [Serializable]
    public class NodeStageFuel : Node
    {
        protected override void OnCreate() {
            Out<double>("Fuel");
        }
         protected override void OnUpdateOutputData() {
            Out("Fuel", Vessel.CurrentStageFuelRemaining(DefaultResources.LiquidFuel, DefaultResources.Oxidizer, DefaultResources.SolidFuel));
        }
    }
}