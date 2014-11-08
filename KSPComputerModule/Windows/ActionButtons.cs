using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPComputer;
using UnityEngine;
namespace KSPComputerModule.Windows
{
    public class ActionButtons : GUIWindow
    {
        private Vector2 scrollPosition;
        private const int MAXCOLS = 8;
        public override string Title
        {
            get { return "Action buttons"; }
        }
        public override Vector2 MinSize
        {
            get { return new Vector2(200, 100); }
        }
        public override void Draw()
        {
            GUILayout.BeginVertical();
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);
            
            GUILayout.BeginHorizontal();
            var values = KSPOperatingSystem.GetActionButtons();
            GUILayout.BeginVertical();
            int i = 0;
            foreach(var a in values)
            {
                if(i > 0 && i%MAXCOLS == 0)
                {
                    GUILayout.EndVertical();
                    GUILayout.BeginVertical();
                }
                if (GUILayout.Button(a.Value()) && !GUIController.InEditor)
                    a.Key();
            }
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.EndScrollView();
            GUILayout.Space(GUIController.ElSize);
            GUILayout.EndVertical();
        }
    }
}
