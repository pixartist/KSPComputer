using KSPComputer;
using UnityEngine;
namespace KSPComputerModule.Windows {
    public class LogWindow : GUIWindow {
        private Vector2 scrollPosition;
        private bool autoScroll = true;
        private int lastLength = 0;
        private Rect lastSize;
        private bool wasAutoScroll = true;
        public override string Title {
            get { return "Log window"; }
        }
        public override Vector2 MinSize {
            get { return new Vector2(400, 200); }
        }
        public override void Draw() {
            base.Draw();
            int newLength = Log.LogData.Length;
            GUILayout.BeginVertical();
            autoScroll = GUILayout.Toggle(autoScroll, "Auto scroll");
            if (autoScroll) {
                if (newLength != lastLength || WinRect != lastSize || !wasAutoScroll)
                    scrollPosition.y = float.PositiveInfinity;
            }
            var newScrollPosition = GUILayout.BeginScrollView(scrollPosition);
            if (newScrollPosition.y < scrollPosition.y && scrollPosition.y != float.PositiveInfinity)
                autoScroll = false;
            scrollPosition = newScrollPosition;
            lastLength = newLength;
            lastSize = WinRect;
            GUILayout.TextArea(Log.LogData);
            GUILayout.EndScrollView();
            GUILayout.Space(GUIController.ElSize);
            GUILayout.EndVertical();
        }
    }
}
