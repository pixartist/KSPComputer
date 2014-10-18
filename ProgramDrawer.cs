using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Reflection;
using KSPFlightPlanner.Program;
using KSPFlightPlanner.Program.Connectors;
using KSPFlightPlanner.Program.Nodes;
namespace KSPFlightPlanner
{
    public class ProgramDrawer
    {
        private bool inputLocked = false;
        private Vector2 nodeViewScrollPos;
        private Dictionary<Connector, Vector2> connections;
        private FlightProgram program;
        private Rect windowRect;
        private float smallGap = 2;
        private Color defaultColor = Color.white;
        private Node draggedNode;
        private Connector draggedConnection;
        private Vector2 dragInfo;
        private bool mouseDown = false;
        private bool mousePressed = false;
        private bool mouseReleased = false;
        private bool PointerAvailable
        {
            get
            {
                return draggedNode == null && draggedConnection == null;
            }
        }
        Vector2 mousePos;
        public bool Show = false;
        public bool MouseOver
        {
            get
            {
                return windowRect.Contains(new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y));
            }
        }
        public ProgramDrawer(FlightProgram program)
        {
            this.program = program;
            Show = false;
            connections = new Dictionary<Connector, Vector2>();
            windowRect = new Rect(0, 0, Screen.width, Screen.height);
        }
        public void Draw()
        {
            if (Show)
            {
                mousePos = new Vector2(Input.mousePosition.x, (Screen.height - Input.mousePosition.y));
                mouseReleased = false;
                mousePressed = false;

                bool isMouseDown = Input.GetMouseButton(0);
                if (!mouseDown && isMouseDown)
                {
                    mouseDown = true;
                    mousePressed = true;
                }
                else if (mouseDown && !isMouseDown)
                {
                    mouseReleased = true;
                    mouseDown = false;
                }
                if (draggedConnection != null && Input.GetMouseButton(2))
                {
                    draggedConnection = null;
                }
                GUI.skin = HighLogic.Skin;
                GUI.skin.box.alignment = TextAnchor.UpperCenter;
                GUI.backgroundColor = defaultColor;

                GUI.Window(667, windowRect, OnDrawWindow, "Flight Program");

                DrawNodeConnections();
                GUI.skin = null;

            }
            else
            {
                GUI.skin = HighLogic.Skin;
                if (GUI.Button(new Rect(270, 50, 120, 30), "Flight Planner"))
                    Show = true;
                GUI.skin = null;
                if (inputLocked)
                {
                    EditorLogic.fetch.Unlock("FlightControl_noclick");
                    inputLocked = false;
                }
            }
        }
        private void OnDrawWindow(int id)
        {
            GUI.depth = 150;
            if (draggedConnection != null && dragInfo != mousePos)
            {
                GUIHelper.DrawLine(GUIUtility.ScreenToGUIPoint(dragInfo), GUIUtility.ScreenToGUIPoint(mousePos), Color.red, 4);
            }
            // GUI.Label(new Rect(200, 0, 100, 50), mousePos.ToString());

            DrawNodeToolbar(smallGap, smallGap);
            DrawNodes(new Rect(smallGap * 2 + 150, smallGap, windowRect.width - (smallGap * 3 + 150), windowRect.height - smallGap * 2));
            PreventEditorClickthrough();

            if (GUI.Button(new Rect(windowRect.width - 50, 10, 30, 30), "X"))
            {
                Show = false;
            }
        }
        private void DrawNodeToolbar(float x, float y)
        {
            float cy = smallGap + 30;
            float h = 30;
            GUI.BeginGroup(new Rect(x + smallGap, y + smallGap, 150, windowRect.height), "Nodes");
            var types = FindAllDerivedTypes<Node>();
            foreach (var n in types)
            {
                Log.Write(n.ToString());
                Color color = GetStaticFieldValue<SVector3>(n, "Color").GetColor();
                GUI.backgroundColor = color;
                if (GUI.Button(new Rect(smallGap, smallGap + cy, 140, h), GetStaticFieldValue<string>(n, "Name")))
                {
                    draggedNode = program.AddNode(n, GUIUtility.GUIToScreenPoint(mousePos));
                    dragInfo = new Vector2(0, 0);
                }
                cy += h;
            }
            GUI.EndGroup();
            GUI.backgroundColor = defaultColor;
        }
        private void DrawNodes(Rect area)
        {

            nodeViewScrollPos = GUI.BeginScrollView(area, nodeViewScrollPos, new Rect(0, 0, 4000, 2000));
            if (draggedNode != null)
            {
                if (mouseReleased)
                {
                    
                    if(GUIUtility.ScreenToGUIPoint(mousePos).x < 140)
                    {
                        program.RemoveNode(draggedNode);
                    }
                    draggedNode = null;
                }
                else
                {
                    var vec = GUIUtility.ScreenToGUIPoint(mousePos) - dragInfo;
                    draggedNode.Position = new SVector2(vec.x, vec.y);
                }
            }
            connections.Clear();
            foreach (var n in program.Nodes)
                DrawNode(n);

            GUI.EndScrollView();
            GUI.backgroundColor = defaultColor;
        }
        private void DrawNodeConnections()
        {
            GUI.depth = -150;
            foreach (var n in program.Nodes)
            {
                foreach (var c in n.Outputs)
                {
                    if (c.Value.Connected)
                    {
                        if (connections.ContainsKey(c.Value))
                        {
                            foreach (var c2 in c.Value.Connections)
                            {
                                if (connections.ContainsKey(c2))
                                {
                                    Vector2 a = connections[c.Value];
                                    Vector2 b = connections[c2];
                                    if (windowRect.Contains(a) || windowRect.Contains(b))
                                        GUIHelper.DrawLine(GUIUtility.ScreenToGUIPoint(a), GUIUtility.ScreenToGUIPoint(b), ConnectionColor(c.Value), 4);

                                }
                            }
                        }
                    }
                }
            }
        }
        private void DrawNode(Node node)
        {
            float baseSize = 25;
            Vector2 size = GetStaticFieldValue<SVector2>(node.GetType(), "Size").GetVec2();
            Color color = GetStaticFieldValue<SVector3>(node.GetType(), "Color").GetColor();
            string name = GetStaticFieldValue<string>(node.GetType(), "Name");
            GUI.BeginGroup(new Rect(node.Position.x, node.Position.y, size.x, size.y));
            GUI.backgroundColor = color;
            GUI.Box(new Rect(0, 0, size.x, size.y), name);
            if (PointerAvailable && mousePressed)
            {
                if (new Rect(0, 0, size.x, 20).Contains(GUIUtility.ScreenToGUIPoint(mousePos)))
                {
                    draggedNode = node;
                    dragInfo = GUIUtility.ScreenToGUIPoint(mousePos);
                }
            }
            DrawNodeInputs(node.Inputs, baseSize, size);
            DrawNodeOutputs(node.Outputs, baseSize, size);
            GUI.backgroundColor = defaultColor;
            GUI.EndGroup();

        }
        void DrawNodeInputs(KeyValuePair<string, ConnectorIn>[] inputs, float baseSize, Vector2 size)
        {
            float y = 15;
            foreach (var inp in inputs)
            {
                
                bool wasConnected = inp.Value.Connected;
                GUI.backgroundColor = ConnectionColor(inp.Value) * (wasConnected ? 1f : 0.5f);
                bool buttonPressed = GUI.Button(new Rect(smallGap, y, size.x / 2, baseSize), inp.Key);
                Vector2 anchor = GUIUtility.GUIToScreenPoint(GetNodeAnchor(smallGap, y, 0));
                connections.Add(inp.Value, anchor);
                if (inp.Value.DataType == typeof(double))
                {
                    inp.Value.Set(GUI.TextField(new Rect(smallGap, y + baseSize + smallGap, size.x / 2, baseSize), inp.Value.AsString()));
                    y += baseSize + smallGap;
                }
                else if (inp.Value.DataType == typeof(float))
                {

                    inp.Value.Set(GUI.TextField(new Rect(smallGap, y + baseSize + smallGap, size.x / 2, baseSize), inp.Value.AsString()));
                    y += baseSize + smallGap;
                }
                else if (inp.Value.DataType == typeof(bool))
                {
                    inp.Value.Set(GUI.Toggle(new Rect(smallGap, y + baseSize + smallGap, size.x / 2, baseSize), (bool)inp.Value.AsBool(), inp.Value.AsBool().ToString()));
                    y += baseSize + smallGap;
                }
                y += baseSize + smallGap;
                if (buttonPressed)
                {
                    if (!wasConnected)
                    {
                        if (draggedConnection != null)
                        {
                            Log.Write("Trying to connect " + draggedConnection.Node + " to " + inp.Value.Node + "(" + draggedConnection.DataType + "==" + inp.Value.DataType + ")");
                            if (draggedConnection.DataType == inp.Value.DataType)
                            {
                                Log.Write("Connected " + draggedConnection.Node + " to " + inp.Value.Node);
                                draggedConnection.ConnectTo(inp.Value);
                                draggedConnection = null;

                            }
                        }
                    }
                    else if (wasConnected)
                    {
                        if (Event.current.button == 1)
                        {
                            inp.Value.DisconnectAll();
                        }
                        else
                        {
                            if (!inp.Value.AllowMultipleConnections)
                                inp.Value.DisconnectAll();
                            if (draggedConnection != null)
                            {
                                Log.Write("Trying to connect " + draggedConnection.Node + " to " + inp.Value.Node + "(" + draggedConnection.DataType + "==" + inp.Value.DataType + ")");
                                if (draggedConnection.DataType == inp.Value.DataType)
                                {
                                    Log.Write("Connected " + draggedConnection.Node + " to " + inp.Value.Node);
                                    draggedConnection.ConnectTo(inp.Value);
                                    draggedConnection = null;

                                }
                            }
                        }
                    }
                }

            }
        }
        void DrawNodeOutputs(KeyValuePair<string, ConnectorOut>[] outputs, float baseSize, Vector2 size)
        {
            float y = 15;
            foreach (var outp in outputs)
            {
                
                bool wasConnected = outp.Value.Connected;
                GUI.backgroundColor = ConnectionColor(outp.Value) * (wasConnected ? 1f : 0.5f);
                bool buttonPressed = GUI.Button(new Rect(size.x / 2, y, size.x / 2, baseSize), outp.Key);
                Vector2 anchor = GUIUtility.GUIToScreenPoint(GetNodeAnchor(size.x / 2, y, size.x/2));
                connections.Add(outp.Value, anchor);
                if (buttonPressed && PointerAvailable)
                {
                    if (!wasConnected)
                    {
                        dragInfo = anchor;
                        draggedConnection = outp.Value;
                    }
                    else if (wasConnected)
                    {
                        if (Event.current.button == 1)
                        {
                            outp.Value.DisconnectAll();
                        }
                        else
                        {
                            if (!outp.Value.AllowMultipleConnections)
                                outp.Value.DisconnectAll();
                            dragInfo = GUIUtility.GUIToScreenPoint(GetNodeAnchor(size.x / 2, y, size.x/2));
                            draggedConnection = outp.Value;
                            
                        }
                    }
                }
                y += baseSize + smallGap;
            }
        }
        void PreventEditorClickthrough()
        {
            if (!inputLocked && MouseOver)
            {

                EditorLogic.fetch.Lock(true, true, true, "FlightControl_noclick");
                inputLocked = true;

            }
            if (inputLocked && !MouseOver)
            {
                EditorLogic.fetch.Unlock("FlightControl_noclick");
                inputLocked = false;
            }
        }

        void PreventInFlightClickthrough()
        {
            if (!inputLocked && MouseOver)
            {
                InputLockManager.SetControlLock(ControlTypes.CAMERACONTROLS | ControlTypes.MAP | ControlTypes.ACTIONS_ALL, "FlightControl_noclick");
                // Setting this prevents the mouse wheel to zoom in/out while in map mode
                ManeuverGizmo.HasMouseFocus = true;
                inputLocked = true;
            }
            if (inputLocked && !MouseOver)
            {
                InputLockManager.RemoveControlLock("FlightControl_noclick");
                ManeuverGizmo.HasMouseFocus = false;
                inputLocked = false;
            }
        }
        Vector2 GetNodeAnchor(float x, float y, float addX)
        {
            return new Vector2(x + addX + (addX > 0 ?  -17 : 17), y + 17);
        }
        Color ConnectionColor(Connector c)
        {
            var t = c.DataType;
            if (t == typeof(float))
                return new Color(0.2f, 1.0f, 0.2f);
            if (t == typeof(double))
                return new Color(0.2f, 0.0f, 1.0f);
            if (t == typeof(bool))
                return new Color(0.2f, 0.7f, 0.3f);
            if (t == typeof(Quaternion))
                return new Color(0.2f, 1.0f, 1.0f);
            if (t == typeof(SVector3))
                return new Color(1.0f, 0.2f, 0.2f);
            if (t == typeof(SVector3))
                return new Color(0.8f, 0.5f, 0.2f);
            return Color.white;
        }
        public static List<Type> FindAllDerivedTypes<T>()
        {
            return FindAllDerivedTypes<T>(Assembly.GetAssembly(typeof(T)));
        }

        public static List<Type> FindAllDerivedTypes<T>(Assembly assembly)
        {
            var derivedType = typeof(T);
            return assembly
                .GetTypes()
                .Where(t =>
                    derivedType.IsAssignableFrom(t) && !t.IsAbstract
                    ).ToList();

        }
        public static T GetStaticFieldValue<T>(Type t, string fieldName)
        {
            FieldInfo field = t.GetField(fieldName, BindingFlags.Public | BindingFlags.Static);
            T ret = default(T);
            if (field != null)
                ret = (T)field.GetValue(null);
            return ret;
        }
    }
}