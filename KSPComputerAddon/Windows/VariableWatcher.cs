using KSPComputer;
using UnityEngine;
namespace KSPComputerModule.Windows {
    public class VariableWatcher : GUIWindow {
        private Vector2 scrollPosition;
        private const int MAXCOLS = 8;
        public override string Title {
            get { return "Watched values"; }
        }
        public override Vector2 MinSize {
            get { return new Vector2(200, 100); }
        }
        public override void Draw() {
            base.Draw();
            GUILayout.BeginVertical();
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);

            GUILayout.BeginHorizontal();
            var values = KSPOperatingSystem.GetWatchedValues();
            GUILayout.BeginVertical();
            for (int i = 0; i < values.Length; i++) {
                if (i > 0 && i % MAXCOLS == 0) {
                    GUILayout.EndVertical();
                    GUILayout.BeginVertical();
                }
                GUILayout.TextField(values[i], GUIController.CustomStyles);
            }
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.EndScrollView();
            GUILayout.Space(GUIController.ElSize);
            GUILayout.EndVertical();
        }
    }
}
