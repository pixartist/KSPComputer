using System;
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
        protected KSPActionGroup[] AGroups;
        protected int inputID;
        protected override void OnCreate()
        {
            In<double>("ActionID");
            In<bool>("Set");
            Out<bool>("Enabled");

            AGroups = new KSPActionGroup[17];
            AGroups[0] = KSPActionGroup.Custom01;
            AGroups[1] = KSPActionGroup.Custom02;
            AGroups[2] = KSPActionGroup.Custom03;
            AGroups[3] = KSPActionGroup.Custom04;
            AGroups[4] = KSPActionGroup.Custom05;
            AGroups[5] = KSPActionGroup.Custom06;
            AGroups[6] = KSPActionGroup.Custom07;
            AGroups[7] = KSPActionGroup.Custom08;
            AGroups[8] = KSPActionGroup.Custom09;
            AGroups[9] = KSPActionGroup.Custom10;
            AGroups[10] = KSPActionGroup.Abort;
            AGroups[11] = KSPActionGroup.Brakes;
            AGroups[12] = KSPActionGroup.Gear;
            AGroups[13] = KSPActionGroup.Light;
            AGroups[14] = KSPActionGroup.RCS;
            AGroups[15] = KSPActionGroup.SAS;
            AGroups[16] = KSPActionGroup.Stage;
        }
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

            if (In("Set").Connected)
            {
                Program.Vessel.ActionGroups.SetGroup(AGroups[inputID], In("Set").AsBool());
            }

        }

        protected override void OnUpdateOutputData()
        {
            Out("Enabled", Program.Vessel.ActionGroups[AGroups[inputID]]);
        }
    }
}