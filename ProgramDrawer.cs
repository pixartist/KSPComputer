using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Reflection;
using KSPComputer;
using KSPComputer.Types;
using KSPComputer.Connectors;
using KSPComputer.Nodes;
namespace KSPFlightPlanner
{
    public class ProgramDrawer
    {
        private NodeCategories nodeCats;
        private bool inputLocked = false;
        private Vector2 nodeViewScrollPos;
        private Dictionary<Connector, Vector2> connections;
        private FlightProgram program;
        private FPComputer computer;
        private Rect windowRect;
        private float baseElementHeight = 26;
        private float toolbarWidth = 150;
        private Color defaultColor = Color.white;
        private Node draggedNode;
        private Connector draggedConnection;
        private Vector2 dragInfo;
        private bool mouseDown = false;
        private bool mousePressed = false;
        private bool mouseReleased = false;
        private const float inactiveCol = 0.7f;
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
        public ProgramDrawer(FPComputer computer, FlightProgram program)
        {
            this.computer = computer;
            this.program = program;
            Show = false;
            connections = new Dictionary<Connector, Vector2>();
            windowRect = new Rect(0, 0, Screen.width, Screen.height);
            nodeCats = new NodeCategories(Environment.CurrentDirectory + "\\GameData\\FlightPlanner\\Nodes\\");
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
                    if (computer.LastStartState == PartModule.StartState.Editor)
                    {
                        EditorLogic.fetch.Unlock("FlightControl_noclick");
                        inputLocked = false;
                    }
                    else
                    {
                        InputLockManager.RemoveControlLock("FlightControl_noclick");
                        ManeuverGizmo.HasMouseFocus = false;
                        inputLocked = false;
                    }
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

            DrawNodeToolbar();
            DrawNodes(new Rect(toolbarWidth, 0, windowRect.width - toolbarWidth, windowRect.height));

            if (computer.LastStartState == PartModule.StartState.Editor)
                PreventEditorClickthrough();
            else
                PreventInFlightClickthrough();

            if (GUI.Button(new Rect(windowRect.width - 50, 10, 30, 30), "X"))
            {
                Show = false;
            }

        }
        private void DrawNodeToolbar()
        {
            float cy = baseElementHeight;
            GUI.BeginGroup(new Rect(0, 0, toolbarWidth, windowRect.height), "Nodes");
            if (GUI.Button(new Rect(0, 0 + cy, toolbarWidth, baseElementHeight), "Back"))
            {
                nodeCats.CategoryUp();
                return;
            }
            cy += baseElementHeight * 2;
            var cats = nodeCats.ListSubCategories();
            foreach(var cat in cats)
            {
                GUI.backgroundColor = cat.color;
                if (GUI.Button(new Rect(0, 0 + cy, toolbarWidth, baseElementHeight), cat.name))
                {
                    nodeCats.SelectSubCategory(cat.name);
                    return;
                }
                cy += baseElementHeight;
            }

            cy += baseElementHeight;
            var nodes = nodeCats.ListNodes();
            foreach (var node in nodes)
            {
                GUI.backgroundColor = node.color;
                if (GUI.Button(new Rect(0, cy, toolbarWidth, baseElementHeight), node.name))
                {
                    Log.Write("Button pressed: " + node.name);
                    draggedNode = program.AddNode(node.type, GUIUtility.GUIToScreenPoint(mousePos));
                    dragInfo = new Vector2(0, 0);
                }
                cy += baseElementHeight;
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
                    
                    if(mousePos.x < 140)
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
            
            var info = nodeCats.GetNodeInfo(node.GetType());
            //Log.Write("Drawing node " + node + " width: " + info.width);
            float height = Math.Max(node.InputCount * 2, node.OutputCount) * baseElementHeight + baseElementHeight; 
            GUI.BeginGroup(new Rect(node.Position.x, node.Position.y, info.width, height));
            GUI.backgroundColor = info.color;
            GUI.Box(new Rect(0, 0, info.width, height), info.name);
            if (PointerAvailable && mousePressed)
            {
                if (new Rect(0, 0, info.width, 20).Contains(GUIUtility.ScreenToGUIPoint(mousePos)))
                {
                    draggedNode = node;
                    dragInfo = GUIUtility.ScreenToGUIPoint(mousePos);
                }
            }
            DrawNodeInputs(node.Inputs, info.width);
            DrawNodeOutputs(node.Outputs, info.width);
            GUI.backgroundColor = defaultColor;
            GUI.EndGroup();

        }
        void DrawNodeInputs(KeyValuePair<string, ConnectorIn>[] inputs, float width)
        {
            float y = baseElementHeight;
            foreach (var inp in inputs)
            {
                
                bool wasConnected = inp.Value.Connected;
                GUI.backgroundColor = ConnectionColor(inp.Value) * (wasConnected ? 1f : inactiveCol);
                bool buttonPressed = GUI.Button(new Rect(0, y, width / 2, baseElementHeight), inp.Key);
                Vector2 anchor = GUIUtility.GUIToScreenPoint(GetNodeAnchor(0, y, 0));
                connections.Add(inp.Value, anchor);
                y += baseElementHeight;
                var infoRect = new Rect(0, y, width / 2, baseElementHeight);
                if (inp.Value.DataType == typeof(double))
                {
                    inp.Value.Set(GUI.TextField(infoRect, inp.Value.AsString()));
                    y += baseElementHeight;
                }
                else if (inp.Value.DataType == typeof(float))
                {

                    inp.Value.Set(GUI.TextField(infoRect, inp.Value.AsString()));
                    y += baseElementHeight;
                }
                else if (inp.Value.DataType == typeof(bool))
                {
                    inp.Value.Set(GUI.Toggle(infoRect, (bool)inp.Value.AsBool(), inp.Value.AsBool().ToString()));
                    y += baseElementHeight;
                }
                
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
        void DrawNodeOutputs(KeyValuePair<string, ConnectorOut>[] outputs, float width)
        {
            float y = baseElementHeight;
            foreach (var outp in outputs)
            {
                
                bool wasConnected = outp.Value.Connected;
                GUI.backgroundColor = ConnectionColor(outp.Value) * (wasConnected ? 1f : inactiveCol);
                bool buttonPressed = GUI.Button(new Rect(width/2, y, width / 2, baseElementHeight), outp.Key);
                Vector2 anchor = GUIUtility.GUIToScreenPoint(GetNodeAnchor(width / 2, y, width/2));
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
                            dragInfo = GUIUtility.GUIToScreenPoint(GetNodeAnchor(width / 2, y, width/2));
                            draggedConnection = outp.Value;
                            
                        }
                    }
                }
                y += baseElementHeight;
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
                return new Color(0.2f, 1f, 0.3f);
            if (t == typeof(double))
                return new Color(0.2f, 1f, 0.0f);
            if (t == typeof(bool))
                return new Color(0.2f, 0.2f, 1f);
            if (t == typeof(Quaternion))
                return new Color(0.2f, 1.0f, 1.0f);
            if (t == typeof(SVector3))
                return new Color(1.0f, 0.2f, 0.2f);
            if (t == typeof(SVector3))
                return new Color(0.8f, 0.5f, 0.2f);
            return Color.white;
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