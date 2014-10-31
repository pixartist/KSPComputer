﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPComputer;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
namespace DefaultNodes
{
    [Serializable]
    public class NodeActionGroup : ExecutableNode
    {
        protected int inputID;
        protected const KSPActionGroup[] AGroups = new KSPActionGroup[]
        {
            KSPActionGroup.Custom01,
            KSPActionGroup.Custom02,
            KSPActionGroup.Custom03,
            KSPActionGroup.Custom04,
            KSPActionGroup.Custom05,
            KSPActionGroup.Custom06,
            KSPActionGroup.Custom07,
            KSPActionGroup.Custom08,
            KSPActionGroup.Custom09,
            KSPActionGroup.Custom10,
            KSPActionGroup.Abort,
            KSPActionGroup.Brakes,
            KSPActionGroup.Gear,
            KSPActionGroup.Light,
            KSPActionGroup.RCS,
            KSPActionGroup.SAS,
            KSPActionGroup.Stage
        };
        protected override void OnCreate()
        {
            In<double>("ActionID");
            In<bool>("Set");
            Out<bool>("Enabled");
        protected override void OnExecute(ConnectorIn input)
        {

            int inputID = In("ActionID").AsInt();
            if (inputID < 0)
            {
                inputID = 0;
            }
            else if (inputID > AGroups.Length)
            {
                inputID = AGroups.Length;
            }

            Program.Vessel.ActionGroups.SetGroup(SelectedGroup(), In("Set").AsBool());
        }

        private KSPActionGroup SelectedGroup()
        {
            int inputID = In("ActionID").AsInt() - 1;
            if (inputID < 0)
                inputID = 0;
            else if (inputID > AGroups.Length)
                inputID = AGroups.Length;
            return AGroups[inputID];
        }

        protected override void OnUpdateOutputData()
        {
            Out("Enabled", Program.Vessel.ActionGroups[AGroups[inputID]]);
        }
    }
}