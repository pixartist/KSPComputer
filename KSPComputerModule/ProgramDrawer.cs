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
using KSPComputer.Variables;
using DefaultNodes;
using System.IO;
namespace KSPComputerModule
{
    public class ProgramDrawer
    {
        private NodeCategories nodeCats;
        private bool inputLocked = false;
        private Vector2 nodeViewScrollPos;
        private Vector2 menuScrollPos;
        private Dictionary<Connector, Vector2> connections;
        private int selectedProgram = 0;
        private SubRoutine currentSubroutine;
        private string currentSubroutineName = "";
        private FlightProgram Program
        {
            get
            {
                if (currentSubroutine != null)
                    return currentSubroutine;
                if (KSPOperatingSystem.ProgramCount < 1)
                    return null;
                if (selectedProgram < 0)
                    selectedProgram = 0;
                if (selectedProgram >= KSPOperatingSystem.ProgramCount)
                    selectedProgram = KSPOperatingSystem.ProgramCount - 1;
                return KSPOperatingSystem.GetProgram(selectedProgram);
            }
        }
        private string[] subRoutines;
        private FPComputer computer;
        private Rect windowRect;
        
        private float baseElementHeight = 26;
        private float toolbarWidth = 220;
        private Color defaultColor = Color.white;
        private Node draggedNode;
        private Connector draggedConnection;
        private Vector2 dragInfo;
        private bool mouseDown = false;
        private bool mousePressed = false;
        private bool mouseReleased = false;
        private const float inactiveCol = 0.7f;
        private Type selectedVariableType = typeof(double);
        private Type selectedParameterType = typeof(double);
        private string currentVarName = "";
        private string currentParamName = "";
        private bool showNodes = true;
        private bool showVariables = true;
        private bool showSubRoutines = true;
        private bool showLog = false;
        private bool autoScrollLog = true;
        private Vector2 logScrollPos;
        private Dictionary<string, Type> selectableVariableTypes = new Dictionary<string, Type>
        {
            {"Number", typeof(double)},
            {"Bool", typeof(bool)},
            {"Vector3", typeof(SVector3)},
            {"Quaternion",  typeof(SQuaternion)},
            {"Vector2", typeof(SVector2)}
        };
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
        public ProgramDrawer(FPComputer computer)
        {
            this.computer = computer;
            Show = false;
            connections = new Dictionary<Connector, Vector2>();
            windowRect = new Rect(0, 0, Screen.width, Screen.height);
            nodeCats = NodeCategories.Instance;

        }
        public void Draw()
        {
            var oldPadding = GUI.skin.button.padding;
            GUI.skin.button.padding = new RectOffset(2, 2, 2, 2);
            if (Show)
            {
                if (subRoutines == null)
                    ReloadSubRoutines();
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
                if (draggedConnection != null && Event.current.button == 1)
                {
                    draggedConnection = null;
                }
                GUI.skin = HighLogic.Skin;

                GUI.skin.box.alignment = TextAnchor.UpperCenter;
                GUI.backgroundColor = defaultColor;

                GUI.Window(667, windowRect, OnDrawWindow, "Flight Program");
                if (Program != null)
                    DrawNodeConnections();
                GUI.skin = null;

            }
            else
            {
                GUI.skin = HighLogic.Skin;
                KSPOperatingSystem.SmallWindowRect.height = showLog ? 300 : 70;
                KSPOperatingSystem.SmallWindowRect.width = showLog ? 600 : 300;
                KSPOperatingSystem.SmallWindowRect = GUI.Window(223, KSPOperatingSystem.SmallWindowRect, OnDrawButtonWindow, "Flight Computer");
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
            GUI.skin.button.padding = oldPadding;
        }
        private void OnDrawButtonWindow(int id)
        {
            GUI.DragWindow(new Rect(0, 0, KSPOperatingSystem.SmallWindowRect.width - 25, 20));
            if (GUI.Button(new Rect(KSPOperatingSystem.SmallWindowRect.width - 20, 0, 20, 20), "_"))
            {
                showLog = !showLog;
            }
            GUILayout.BeginVertical();
            if (GUILayout.Button("Flight Program"))
                Show = true;
            if (showLog)
            {
                autoScrollLog = GUILayout.Toggle(autoScrollLog, "Autoscroll");
                if (autoScrollLog)
                    logScrollPos.y = Mathf.Infinity;
                logScrollPos = GUILayout.BeginScrollView(logScrollPos, GUILayout.Width(KSPOperatingSystem.SmallWindowRect.width - 20), GUILayout.Height(190));
                GUILayout.TextArea(Log.LogData);
                GUILayout.EndScrollView();
            }
            GUILayout.EndVertical();
            
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
            if (Program != null)
                DrawNodes(new Rect(toolbarWidth + 20, 0, windowRect.width - toolbarWidth, windowRect.height));

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

            menuScrollPos = GUILayout.BeginScrollView(menuScrollPos, GUILayout.Width(toolbarWidth), GUILayout.Height(windowRect.height - 40));
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("<"))
                selectedProgram--;
            GUILayout.Label("Program " + selectedProgram + "/" + KSPOperatingSystem.ProgramCount);
            if (GUILayout.Button(">"))
                selectedProgram++;
            GUILayout.EndHorizontal();
            #region back button
            //draw back button

            if (GUILayout.Button(nodeCats.TopCategory ? "---- Nodes ----" : "---- Back ----"))
            {
                if (nodeCats.TopCategory)
                {
                    showNodes = !showNodes;
                }
                else
                {
                    nodeCats.CategoryUp();
                }
            }

            #endregion
            #region nodes
            if (showNodes)
            {
                //Draw categories
                var cats = nodeCats.ListSubCategories();
                if (cats.Length > 0)
                {
                    foreach (var cat in cats)
                    {
                        GUI.backgroundColor = cat.color;
                        if (GUILayout.Button(cat.name))
                        {
                            nodeCats.SelectSubCategory(cat.name);
                            return;
                        }
                    }

                }

                //Draw nodes
                var nodes = nodeCats.ListNodes();
                foreach (var node in nodes)
                {
                    GUI.backgroundColor = node.color;
                    if (GUILayout.Button(node.name) && Program != null)
                    {
                        //Log.Write("Button pressed: " + node.name);
                        draggedNode = Program.AddNode(node.type, GUIUtility.GUIToScreenPoint(mousePos));
                        dragInfo = new Vector2(0, 0);
                    }
                }
            }


            #endregion
            GUILayout.Space(baseElementHeight);
            #region variables
            GUI.backgroundColor = defaultColor;
            //Draw add variable
            if (Program != null)
            {
                if (GUILayout.Button("---- Variables ----"))
                {
                    showVariables = !showVariables;
                }
                if (showVariables)
                {
                    MethodInfo addMethod = Program.GetType().GetMethod("AddVariable");

                    GUILayout.Label("Add variables");
                    foreach (var k in selectableVariableTypes)
                    {
                        GUI.backgroundColor = TypeColor(k.Value);
                        if (GUILayout.Toggle(selectedVariableType == k.Value, k.Key))
                            selectedVariableType = k.Value;
                    }
                    GUI.backgroundColor = defaultColor;
                    GUILayout.Label("Name");
                    currentVarName = GUILayout.TextField(currentVarName);
                    if (GUILayout.Button("Add"))
                    {
                        if (!string.IsNullOrEmpty(currentVarName))
                            Program.AddVariable(selectedVariableType, currentVarName);
                    }
                    GUILayout.Label("Variables");
                    //draw variables
                    foreach (var v in Program.Variables)
                    {
                        GUILayout.BeginHorizontal();
                        GUI.backgroundColor = TypeColor(v.Value.Type);
                        if (GUILayout.Button(v.Key))
                        {
                            //add variableNode
                            draggedNode = Program.AddVariableNode(GUIUtility.GUIToScreenPoint(mousePos), v.Key);
                            dragInfo = new Vector2(0, 0);
                        }
                        if (GUILayout.Button("X", GUILayout.MaxWidth(baseElementHeight)))
                        {
                            //remove variable
                            Program.RemoveVariable(v.Key);
                        }
                        GUILayout.EndHorizontal();
                    }
                }
            }
            GUI.backgroundColor = defaultColor;

            #endregion
            GUILayout.Space(baseElementHeight);
            #region subroutines

            if (GUILayout.Button("---- Subroutines ----"))
            {
                showSubRoutines = !showSubRoutines;
            }
            if (showSubRoutines)
            {
                foreach (var s in subRoutines)
                {
                    if (GUILayout.Button(s))
                    {
                        draggedNode = Program.AddSubRoutineNode(GUIUtility.GUIToScreenPoint(mousePos), s);
                        dragInfo = new Vector2(0, 0);
                    }
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("Edit"))
                    {
                        currentSubroutine = KSPOperatingSystem.LoadSubRoutine(s, true);
                        currentSubroutineName = s;
                    }
                    GUI.backgroundColor = Color.red;
                    if (GUILayout.Button("Delete"))
                    {
                        KSPOperatingSystem.DeleteSubRoutine(s);
                        ReloadSubRoutines();
                    }
                    GUI.backgroundColor = defaultColor;
                    GUILayout.EndHorizontal();
                }
                if (currentSubroutine == null)
                {
                    if (GUILayout.Button("Create Subroutine"))
                    {
                        currentSubroutine = new SubRoutine();
                    }
                }
                else
                {
                    GUILayout.Label("Add parameter");
                    if (GUILayout.Toggle(selectedParameterType == typeof(Connector.Exec), "Exec"))
                        selectedParameterType = typeof(Connector.Exec);
                    foreach (var k in selectableVariableTypes)
                    {
                        GUI.backgroundColor = TypeColor(k.Value);
                        if (GUILayout.Toggle(selectedParameterType == k.Value, k.Key))
                            selectedParameterType = k.Value;
                    }
                    GUI.backgroundColor = defaultColor;
                    GUILayout.Label("Name");
                    currentParamName = GUILayout.TextField(currentParamName);
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("Add Input") && !string.IsNullOrEmpty(currentParamName))
                    {
                        currentSubroutine.EntryNode.AddRoutineInput(currentParamName, selectedParameterType);
                    }
                    if (GUILayout.Button("Add Output") && !string.IsNullOrEmpty(currentParamName))
                    {
                        currentSubroutine.ExitNode.AddRoutineOuput(currentParamName, selectedParameterType);
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.Space(baseElementHeight);
                    currentSubroutineName = GUILayout.TextField(currentSubroutineName);
                    if (GUILayout.Button("Save") && !string.IsNullOrEmpty(currentSubroutineName))
                    {
                        KSPOperatingSystem.SaveSubRoutine(currentSubroutineName, currentSubroutine, true);
                        currentSubroutine = null;
                        currentSubroutineName = "";
                        ReloadSubRoutines();
                        KSPOperatingSystem.InitPrograms();
                    }
                    if (GUILayout.Button("Cancel"))
                    {
                        currentSubroutine = null;
                        currentSubroutineName = "";
                    }
                }
            }

            #endregion
            GUILayout.EndVertical();
            GUILayout.EndScrollView();
            GUI.backgroundColor = defaultColor;
        }
        private void DrawNodes(Rect area)
        {

            nodeViewScrollPos = GUI.BeginScrollView(area, nodeViewScrollPos, new Rect(0, 0, 4000, 2000));
            if (draggedNode != null)
            {
                if (mouseReleased)
                {

                    if (mousePos.x < toolbarWidth)
                    {
                        Program.RemoveNode(draggedNode);
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
            
            foreach (var n in Program.Nodes)
                DrawNode(n);

            GUI.EndScrollView();
            GUI.backgroundColor = defaultColor;
        }
        private void DrawNodeConnections()
        {
            GUI.depth = -150;
            foreach (var n in Program.Nodes)
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
            bool drawInputs = true;
            bool drawOutputs = true;
            bool isEditable = false;
            bool isVariableNode = false;
            SubRoutine subRoutine = null;
            string variableName = "";
            //find variable node
            NodeCategories.NodeInfo info = new NodeCategories.NodeInfo();
            var nodeType = node.GetType();
            if (nodeType.IsGenericType)
            {
                if (nodeType.GetGenericTypeDefinition() == typeof(VariableNode<>))
                {
                    isVariableNode = true;
                    Variable v = node.GetType().GetProperty("Variable").GetValue(node, null) as Variable;
                    var n = from va in Program.Variables where va.Value == v select va.Key;
                    if (n.Count() > 0)
                    {
                        variableName = n.First();
                        info = new NodeCategories.NodeInfo(variableName, v.Type, "Variable node", TypeColor(v.Type), 120);
                    }
                }
            }
            else if (node is SubroutineNode)
            {
                var sr = node as SubroutineNode;
                subRoutine = sr.SubRoutineInstance;
                
                info = new NodeCategories.NodeInfo(sr.SubRoutineBlueprint, typeof(SubRoutine), "Subroutine", Color.cyan, 220);
            }
            else if (node is SubRoutineEntry)
            {
                drawInputs = false;
                isEditable = true;
                info = new NodeCategories.NodeInfo("Entry", typeof(SubRoutineEntry), "Entry", Color.cyan, 220);
            }
            else if (node is SubRoutineExit)
            {
                drawOutputs = false;
                isEditable = true;
                info = new NodeCategories.NodeInfo("Exit", typeof(SubRoutineExit), "Exit", Color.cyan, 220);
            }
            else
                info = nodeCats.GetNodeInfo(node.GetType());
            //Log.Write("Drawing node " + node + " width: " + info.width);
            float height = Math.Max(node.InputCount * 2 - node.InputExecCount, node.OutputCount) * baseElementHeight;
            height += baseElementHeight;
            
            GUI.BeginGroup(new Rect(node.Position.x, node.Position.y, info.width, height));
            if (node is BaseExecutableNode)
            {
                if (subRoutine != null)
                {
                    GUI.skin.label.alignment = TextAnchor.LowerCenter;
                    if(GUI.Button(new Rect(info.width - baseElementHeight * 1.2f, -5f, baseElementHeight, baseElementHeight), "e"))
                    {
                        currentSubroutineName = (node as SubroutineNode).SubRoutineBlueprint;
                        currentSubroutine = subRoutine;
                    }
                }
                float c = Mathf.Max(0, Mathf.Min(1, (Time.time - (node as BaseExecutableNode).LastExecution) * 3f));
                GUI.skin.label.alignment = TextAnchor.UpperRight;
                GUI.color = new Color(c, 1 - c, 0);
                GUI.Label(new Rect(info.width - baseElementHeight, -5f, baseElementHeight, baseElementHeight), "■");
                GUI.color = Color.white;
                GUI.skin.label.alignment = TextAnchor.UpperCenter;
            } 
            
            
            GUI.backgroundColor = info.color;
            GUI.Box(new Rect(0, 0, info.width, height), info.name);
            if (PointerAvailable && mousePressed)
            {
                if (mousePos.x > toolbarWidth)
                {
                    if (new Rect(0, 0, info.width, 20).Contains(GUIUtility.ScreenToGUIPoint(mousePos)))
                    {
                        if (Event.current.shift)
                        {
                            if (!(node is SubRoutineEntry || node is SubRoutineExit))
                            {
                                if(node is SubroutineNode)
                                {
                                    draggedNode = Program.AddSubRoutineNode(node.Position.GetVec2(), (node as SubroutineNode).SubRoutineBlueprint);
                                    dragInfo = GUIUtility.ScreenToGUIPoint(mousePos);
                                }
                                else if (isVariableNode)
                                {
                                    draggedNode = Program.AddVariableNode(node.Position.GetVec2(), variableName);
                                    dragInfo = GUIUtility.ScreenToGUIPoint(mousePos);
                                }
                                else
                                {
                                    draggedNode = Program.AddNode(nodeType, node.Position.GetVec2());
                                    dragInfo = GUIUtility.ScreenToGUIPoint(mousePos);
                                }
                            }
                        }
                        else
                        {
                            draggedNode = node;
                            dragInfo = GUIUtility.ScreenToGUIPoint(mousePos);
                        }
                    }
                }
            }

            if (drawInputs)
                DrawNodeInputs(node.Inputs, info.width, isEditable);
            if (drawOutputs)
                DrawNodeOutputs(node.Outputs, info.width, isEditable);
            GUI.backgroundColor = defaultColor;
            GUI.EndGroup();

        }
        void DrawNodeInputs(KeyValuePair<string, ConnectorIn>[] inputs, float width, bool removable)
        {
            float y = baseElementHeight;
            foreach (var inp in inputs)
            {

                bool wasConnected = inp.Value.Connected;
                GUI.backgroundColor = ConnectionColor(inp.Value) * (wasConnected ? 1f : inactiveCol);
                bool buttonPressed = GUI.Button(new Rect(0, y, width / 2, baseElementHeight), inp.Key);
                if (removable)
                {
                    if (GUI.Button(new Rect(width / 2, y, width / 2, baseElementHeight), "Remove"))
                    {
                        inp.Value.Node.RemoveInput(inp.Key);
                        return;
                    }
                }
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
                            //Log.Write("Trying to connect " + draggedConnection.Node + " to " + inp.Value.Node + "(" + draggedConnection.DataType + "==" + inp.Value.DataType + ")");
                            if (draggedConnection.DataType == inp.Value.DataType)
                            {
                                //Log.Write("Connected " + draggedConnection.Node + " to " + inp.Value.Node);
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
                                //Log.Write("Trying to connect " + draggedConnection.Node + " to " + inp.Value.Node + "(" + draggedConnection.DataType + "==" + inp.Value.DataType + ")");
                                if (draggedConnection.DataType == inp.Value.DataType)
                                {
                                    //Log.Write("Connected " + draggedConnection.Node + " to " + inp.Value.Node);
                                    draggedConnection.ConnectTo(inp.Value);
                                    draggedConnection = null;

                                }
                            }
                        }
                    }
                }

            }
        }
        void DrawNodeOutputs(KeyValuePair<string, ConnectorOut>[] outputs, float width, bool removable)
        {
            float y = baseElementHeight;
            foreach (var outp in outputs)
            {

                bool wasConnected = outp.Value.Connected;
                GUI.backgroundColor = ConnectionColor(outp.Value) * (wasConnected ? 1f : inactiveCol);
                bool buttonPressed = GUI.Button(new Rect(width / 2, y, width / 2, baseElementHeight), outp.Key);
                if (removable)
                {
                    if (GUI.Button(new Rect(0, y, width / 2, baseElementHeight), "Remove"))
                    {
                        outp.Value.Node.RemoveOutput(outp.Key);
                        return;
                    }
                }
                Vector2 anchor = GUIUtility.GUIToScreenPoint(GetNodeAnchor(width / 2, y, width / 2));
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
                            dragInfo = GUIUtility.GUIToScreenPoint(GetNodeAnchor(width / 2, y, width / 2));
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
            return new Vector2(x + addX + (addX > 0 ? -17 : 17), y + 17);
        }
        Color ConnectionColor(Connector c)
        {
            var t = c.DataType;
            return TypeColor(t);
        }
        Color TypeColor(Type t)
        {
            if (t == typeof(float))
                return new Color(0.2f, 1f, 0.3f);
            if (t == typeof(double))
                return new Color(0.2f, 1f, 0.0f);
            if (t == typeof(bool))
                return new Color(0.2f, 0.2f, 1f);
            if (t == typeof(Quaternion))
                return new Color(0.2f, 1.0f, 1.0f);
            if (t == typeof(SVector3))
                return new Color(1.0f, 0.7f, 0.0f);
            if (t == typeof(SVector2))
                return new Color(1.0f, 1.0f, 0.0f);
            return Color.white;
        }
        public void ReloadSubRoutines()
        {
            subRoutines = KSPOperatingSystem.ListSubRoutines();
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