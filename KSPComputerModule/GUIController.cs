using KSPComputer;
using KSPComputerModule.Windows;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace KSPComputerModule {
    public static class GUIController {
        public static PartModule.StartState lastStartState;
        public static bool InEditor {
            get {
                return (lastStartState & PartModule.StartState.Editor) == PartModule.StartState.Editor;
            }
        }
        public static Dictionary<int, GUIWindow> windows = new Dictionary<int, GUIWindow>();
        private static bool inputLocked = false;
        private static bool mouseOver = false;
        private static int cId = 1337;
        private static Rect mwRect = new Rect(250, 45, 150, 100);
        public const float ElSize = 26;
        public static readonly GUILayoutOption[] CustomStyles =
        {
            GUILayout.Height(ElSize)
        };
        public static readonly Color DefaultColor = Color.white;
        private static Texture2D resizeTex;
        public static Vector2 mousePos;
        public static bool mouseWasDown, mousePressed, mouseReleased;
        private static int resizingWindow = -1;

        static GUIController() {

            string p = Path.Combine(KSPOperatingSystem.PluginPath, "resize.png");
            try {
                resizeTex = new WWW("file://" + p).texture;
            } catch (Exception e) {
                Log.Write("Failed loading " + p + ", " + e.Message);
            }
            AddWindow("ProgramEditor", new ProgramEditor());
            AddWindow("ActionButtons", new ActionButtons());
            AddWindow("VariableWatcher", new VariableWatcher());
            AddWindow("Log", new LogWindow());
        }
        public static void Start() {
            foreach (var w in windows.Values)
                w.Start();
        }
        public static void Draw() {
            mouseOver = false;
            GUI.skin = HighLogic.Skin;
            mousePos = new Vector2(Input.mousePosition.x, (Screen.height - Input.mousePosition.y));
            bool isMouseDown = Input.GetMouseButton(0);
            mouseReleased = false;
            mousePressed = false;
            if (!mouseWasDown && isMouseDown) {
                mouseWasDown = true;
                mousePressed = true;
            } else if (mouseWasDown && !isMouseDown) {
                mouseReleased = true;
                mouseWasDown = false;
            }
            mwRect.height = (windows.Count + 2) * ElSize;
            mwRect = GUI.Window(2300, mwRect, OnDrawMainWindow, "Flight Computer");
            mouseOver = mwRect.Contains(mousePos);
            foreach (var w in windows) {
                if (w.Value.Opened) {
                    w.Value.WinRect = GUI.Window(w.Key, w.Value.WinRect, OnDrawWindow, w.Value.Title);
                    mouseOver |= w.Value.WinRect.Contains(mousePos);
                }
            }
            if (InEditor)
                PreventEditorClickthrough();
            else
                PreventInFlightClickthrough();
        }
        private static void OnDrawMainWindow(int id) {
            GUI.DragWindow(new Rect(0, 0, mwRect.width, ElSize));
            GUILayout.BeginVertical();
            foreach (var w in windows) {
                if (GUILayout.Button(w.Value.Title, CustomStyles)) {
                    w.Value.Opened = !w.Value.Opened;
                }
            }
            GUILayout.EndVertical();
        }
        private static void OnDrawWindow(int id) {
            var window = windows[id];
            //Log.Write("Drawing window " + window.Name + ", " + window.WinRect);
            GUI.DragWindow(new Rect(0, 0, mwRect.width, ElSize));
            //draw window
            window.Draw();

            Rect rect = new Rect(
                window.WinRect.width - ElSize,
                 window.WinRect.height - ElSize,
                ElSize,
                ElSize
                );
            GUI.DrawTexture(rect, resizeTex);
            rect.x += window.WinRect.x;
            rect.y += window.WinRect.y;
            if (mouseWasDown) {
                //Log.Write("Checking resize, rect: " + rect + ", mouse: " + mp);
                if (mousePressed && resizingWindow < 0) {
                    if (rect.Contains(mousePos)) {
                        resizingWindow = id;
                    }
                }
                if (resizingWindow == id) {
                    window.WinRect.width = Math.Max(window.MinSize.x, ElSize / 2 + mousePos.x - window.WinRect.x);
                    window.WinRect.height = Math.Max(window.MinSize.y, ElSize / 2 + mousePos.y - window.WinRect.y);
                }
            } else if (resizingWindow >= 0)
                resizingWindow = -1;
            if (GUI.Button(new Rect(window.WinRect.width - GUIController.ElSize, 0, GUIController.ElSize, GUIController.ElSize), "X"))
                window.Opened = false;
            GUI.DragWindow(new Rect(0, 0, window.WinRect.width - ElSize, ElSize));
        }
        public static void LoadState(ConfigNode node) {
            Log.Write("Loading window states");
            foreach (var w in windows.Values) {
                Log.Write("Loading state for window " + w.Name);
                LoadWindowState(w, node);
            }
        }
        public static void SaveState(ConfigNode node) {
            foreach (var w in windows.Values) {
                SaveWindowState(w, node);
            }
        }
        public static void AddWindow(string name, GUIWindow window) {

            window.Id = cId;
            window.Name = name;
            window.WinRect = new Rect(300, 50, window.MinSize.x, window.MinSize.y);
            windows.Add(cId, window);
            cId++;

        }
        private static void LoadWindowState(GUIWindow window, ConfigNode node) {
            if (node != null) {
                string nodeName = "Window" + window.Name;
                if (node.HasNode(nodeName)) {
                    node = node.GetNode(nodeName);
                    Log.Write("Loading node value " + nodeName);
                    window.WinRect = LoadRect(node, window.WinRect);
                    if (node.HasValue("Opened")) {
                        window.Opened = bool.Parse(node.GetValue("Opened"));
                        Log.Write("Window Opened: " + node.GetValue("Opened") + " -> " + window.Opened);
                    }
                }
            }
        }
        private static void SaveWindowState(GUIWindow window, ConfigNode node) {
            if (node != null) {
                string nodeName = "Window" + window.Name;
                node = node.AddNode(nodeName);
                SaveRect(node, window.WinRect);
                node.AddValue("Opened", window.Opened.ToString());
            }
        }
        public static Rect LoadRect(ConfigNode node, Rect defaultSize) {
            if (node.HasValue("RectX"))
                defaultSize.x = float.Parse(node.GetValue("RectX"));
            if (node.HasValue("RectY"))
                defaultSize.y = float.Parse(node.GetValue("RectY"));
            if (node.HasValue("RectW"))
                defaultSize.width = float.Parse(node.GetValue("RectW"));
            if (node.HasValue("RectH"))
                defaultSize.height = float.Parse(node.GetValue("RectH"));
            return defaultSize;
        }
        public static void SaveRect(ConfigNode node, Rect rect) {
            node.AddValue("RectX", rect.x.ToString());
            node.AddValue("RectY", rect.y.ToString());
            node.AddValue("RectW", rect.width.ToString());
            node.AddValue("RectH", rect.height.ToString());
        }
        static void PreventEditorClickthrough() {
            if (!inputLocked && mouseOver) {

                EditorLogic.fetch.Lock(true, true, true, "FlightControl_noclick");
                inputLocked = true;

            }
            if (inputLocked && !mouseOver) {
                EditorLogic.fetch.Unlock("FlightControl_noclick");
                inputLocked = false;
            }
        }
        static void PreventInFlightClickthrough() {
            if (!inputLocked && mouseOver) {
                InputLockManager.SetControlLock(ControlTypes.CAMERACONTROLS | ControlTypes.MAP | ControlTypes.ACTIONS_ALL, "FlightControl_noclick");
                // Setting this prevents the mouse wheel to zoom in/out while in map mode
                ManeuverGizmo.HasMouseFocus = true;
                inputLocked = true;
            }
            if (inputLocked && !mouseOver) {
                InputLockManager.RemoveControlLock("FlightControl_noclick");
                ManeuverGizmo.HasMouseFocus = false;
                inputLocked = false;
            }
        }
    }
}
