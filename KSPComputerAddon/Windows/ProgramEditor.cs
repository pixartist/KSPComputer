using KSPComputer;
using KSPComputer.Connectors;
using KSPComputer.Nodes;
using KSPComputer.Types;
using KSPComputer.Variables;
using KSPComputerAddon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace KSPComputerModule.Windows {
    public class ProgramEditor : GUIWindow {
        private const float inactiveCol = 0.7f;
        private Node draggedNode;
        private Connector draggedConnection;
        private bool dragging = false;
        private Vector2 dragInfo;
        private float toolbarWidth = 220;
        private Vector2 toolbarScrollPos;
        private Vector2 nodeViewScrollPos;
        private int selectedProgram = 0;
        private NodeCategories nodeCats = NodeCategories.Instance;
        private Dictionary<Connector, Vector2> connections = new Dictionary<Connector, Vector2>();
        private Rect programRect = new Rect(0, 0, 4000, 2000);
        private bool showNodes = true;
        private bool showVariables = true;
        private bool showSubRoutines = true;
        private Type selectedVariableType = typeof(double);
        private Type selectedParameterType = typeof(double);
        private string currentVarName = "";
        private string currentParamName = "";
        private string[] subRoutines = KSPOperatingSystem.ListSubRoutines();
        private SubRoutine currentSubroutine;
        private string currentSubroutineName = "";

        private FlightProgram Program {
            get {
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
        private Dictionary<string, Type> selectableVariableTypes = new Dictionary<string, Type>
        {
            {"Number", typeof(double)},
            {"Bool", typeof(bool)},
            {"Vector3", typeof(SVector3d)},
            {"Quaternion",  typeof(SQuaternion)},
            {"Vector2", typeof(SVector2d)}
        };
        private bool PointerAvailable {
            get {
                return draggedNode == null && draggedConnection == null;
            }
        }
        public override string Title {
            get { return "Program Editor"; }
        }
        public override Vector2 MinSize {
            get { return new Vector2(400, 400); }
        }
        public override void Start() {
            base.Start();
            currentSubroutine = null;
        }
        public override void Draw() {
            base.Draw();
            GUI.depth = 150;
            if (draggedConnection != null && dragInfo != this.MouseLocation) {
                GUILine.DrawLine(GUIUtility.ScreenToGUIPoint(dragInfo), GUIUtility.ScreenToGUIPoint(this.MouseLocation), Color.red, 2, true);
            }
            if (Program != null)
                DrawNodes(new Rect(toolbarWidth + 20, GUIController.ElSize, WinRect.width - (toolbarWidth + GUIController.ElSize + 20), WinRect.height - GUIController.ElSize * 2));
            DrawNodeToolbar();
        }

        private void DrawNodeToolbar() {

            toolbarScrollPos = GUILayout.BeginScrollView(toolbarScrollPos, GUILayout.Width(toolbarWidth), GUILayout.Height(WinRect.height - 40));
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("<", GUIController.CustomStyles))
                selectedProgram--;
            GUILayout.Label("Program " + (selectedProgram + 1) + "/" + KSPOperatingSystem.ProgramCount);
            if (GUILayout.Button(">", GUIController.CustomStyles))
                selectedProgram++;
            GUILayout.EndHorizontal();
            #region back button
            //draw back button

            if (GUILayout.Button(nodeCats.TopCategory ? "---- Nodes ----" : "---- Back ----", GUIController.CustomStyles)) {
                if (nodeCats.TopCategory) {
                    showNodes = !showNodes;
                } else {
                    nodeCats.CategoryUp();
                }
            }

            #endregion

            #region nodes
            if (showNodes) {
                //Draw categories
                var cats = nodeCats.ListSubCategories();
                if (cats.Length > 0) {
                    foreach (var cat in cats) {
                        GUI.backgroundColor = cat.color;
                        if (GUILayout.Button(cat.name, GUIController.CustomStyles)) {
                            nodeCats.SelectSubCategory(cat.name);
                            return;
                        }
                    }

                }

                //Draw nodes
                var nodes = nodeCats.ListNodes();
                foreach (var node in nodes) {
                    GUI.backgroundColor = node.color;
                    if (GUILayout.Button(node.name, GUIController.CustomStyles) && Program != null) {
                        Log.Write("Adding node: " + node.name);
                        draggedNode = Program.AddNode(node.type, GUIUtility.GUIToScreenPoint(this.MouseLocation));
                        dragInfo = new Vector2(0, 0);
                    }
                }
            }


            #endregion

            GUILayout.Space(GUIController.ElSize);
            #region variables
            GUI.backgroundColor = GUIController.DefaultColor;
            //Draw add variable
            if (Program != null) {
                if (GUILayout.Button("---- Variables ----", GUIController.CustomStyles)) {
                    showVariables = !showVariables;
                }
                if (showVariables) {
                    MethodInfo addMethod = Program.GetType().GetMethod("AddVariable");

                    GUILayout.Label("Add variables");
                    foreach (var k in selectableVariableTypes) {
                        GUI.backgroundColor = TypeColor(k.Value);
                        if (GUILayout.Toggle(selectedVariableType == k.Value, k.Key, GUIController.CustomStyles))
                            selectedVariableType = k.Value;
                    }
                    GUI.backgroundColor = GUIController.DefaultColor;
                    GUILayout.Label("Name");
                    currentVarName = GUILayout.TextField(currentVarName, GUIController.CustomStyles);
                    if (GUILayout.Button("Add", GUIController.CustomStyles)) {
                        if (!string.IsNullOrEmpty(currentVarName))
                            Program.AddVariable(selectedVariableType, currentVarName);
                    }
                    GUILayout.Label("Variables");
                    //draw variables
                    foreach (var v in Program.Variables) {
                        GUILayout.BeginHorizontal();
                        GUI.backgroundColor = TypeColor(v.Value.Type);
                        if (GUILayout.Button(v.Key, GUIController.CustomStyles)) {
                            //add variableNode
                            draggedNode = Program.AddVariableNode(GUIUtility.GUIToScreenPoint(this.MouseLocation), v.Key);
                            dragInfo = new Vector2(0, 0);
                        }
                        if (GUILayout.Button("X", GUILayout.MaxWidth(GUIController.ElSize))) {
                            //remove variable
                            Program.RemoveVariable(v.Key);
                        }
                        GUILayout.EndHorizontal();
                    }
                }
            }
            GUI.backgroundColor = GUIController.DefaultColor;

            #endregion

            GUILayout.Space(GUIController.ElSize);
            #region subroutines

            if (GUILayout.Button("---- Subroutines ----", GUIController.CustomStyles)) {
                showSubRoutines = !showSubRoutines;
            }
            if (showSubRoutines && Program != null) {
                foreach (var s in subRoutines) {
                    if (GUILayout.Button(s, GUIController.CustomStyles)) {
                        draggedNode = Program.AddSubRoutineNode(GUIUtility.GUIToScreenPoint(this.MouseLocation), s);
                        dragInfo = new Vector2(0, 0);
                    }
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("Edit", GUIController.CustomStyles)) {
                        currentSubroutine = KSPOperatingSystem.LoadSubRoutine(s, true);
                        currentSubroutineName = s;
                    }
                    GUI.backgroundColor = Color.red;
                    if (GUILayout.Button("Delete", GUIController.CustomStyles)) {
                        KSPOperatingSystem.DeleteSubRoutine(s);
                        ReloadSubRoutines();
                    }
                    GUI.backgroundColor = GUIController.DefaultColor;
                    GUILayout.EndHorizontal();
                }
                if (currentSubroutine == null) {
                    if (GUILayout.Button("Create Subroutine", GUIController.CustomStyles)) {
                        currentSubroutine = new SubRoutine();
                    }
                } else {
                    GUILayout.Label("Add parameter");
                    if (GUILayout.Toggle(selectedParameterType == typeof(Connector.Exec), "Exec", GUIController.CustomStyles))
                        selectedParameterType = typeof(Connector.Exec);
                    foreach (var k in selectableVariableTypes) {
                        GUI.backgroundColor = TypeColor(k.Value);
                        if (GUILayout.Toggle(selectedParameterType == k.Value, k.Key, GUIController.CustomStyles))
                            selectedParameterType = k.Value;
                    }
                    GUI.backgroundColor = GUIController.DefaultColor;
                    GUILayout.Label("Name", GUIController.CustomStyles);
                    currentParamName = GUILayout.TextField(currentParamName, GUIController.CustomStyles);
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("Add Input", GUIController.CustomStyles) && !string.IsNullOrEmpty(currentParamName)) {
                        currentSubroutine.EntryNode.AddRoutineInput(currentParamName, selectedParameterType);
                    }
                    if (GUILayout.Button("Add Output", GUIController.CustomStyles) && !string.IsNullOrEmpty(currentParamName)) {
                        currentSubroutine.ExitNode.AddRoutineOuput(currentParamName, selectedParameterType);
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.Space(GUIController.ElSize);
                    currentSubroutineName = GUILayout.TextField(currentSubroutineName, GUIController.CustomStyles);
                    if (GUILayout.Button("Save", GUIController.CustomStyles) && !string.IsNullOrEmpty(currentSubroutineName)) {
                        KSPOperatingSystem.SaveSubRoutine(currentSubroutineName, currentSubroutine, true);
                        currentSubroutine = null;
                        currentSubroutineName = "";
                        ReloadSubRoutines();
                        KSPOperatingSystem.InitPrograms();
                    }
                    if (GUILayout.Button("Cancel", GUIController.CustomStyles)) {
                        currentSubroutine = null;
                        currentSubroutineName = "";
                    }
                }
            }

            #endregion

            GUILayout.EndVertical();
            GUILayout.EndScrollView();
            GUI.backgroundColor = GUIController.DefaultColor;

        }
        private void DrawNodes(Rect area) {
            nodeViewScrollPos = GUI.BeginScrollView(area, nodeViewScrollPos, programRect);
            if (draggedNode != null) {
                if (this.MouseReleased) {
                    if (this.MouseLocation.x < toolbarWidth) {
                        Log.Write("Removing node: " + draggedNode);
                        Program.RemoveNode(draggedNode);
                    }
                    draggedNode = null;
                } else {
                    var vec = GUIUtility.ScreenToGUIPoint(this.MouseLocation) - dragInfo;
                    draggedNode.Position = new SVector2(Mathf.Max(0, Mathf.Min(programRect.width - 120, vec.x)), Mathf.Max(0, Mathf.Min(programRect.height - 120, vec.y)));
                }
            }
            connections.Clear();
            GUI.skin.box.alignment = TextAnchor.UpperLeft;
            GUI.skin.box.padding = new RectOffset(2, 2, 2, 2);
            foreach (var n in Program.Nodes)
                DrawNode(n);
            GUI.skin.box.padding = new RectOffset(0, 0, 0, 0);
            GUI.skin.box.alignment = TextAnchor.UpperCenter;
            DrawNodeConnections(new Rect(toolbarWidth + 20, GUIController.ElSize, WinRect.width - toolbarWidth - GUIController.ElSize, WinRect.height - GUIController.ElSize));
            GUI.EndScrollView();
            if (this.MousePressed) {
                if (draggedConnection == null && draggedNode == null && !dragging) {
                    if (area.Contains(this.GuiMouseLocation)) {
                        dragging = true;
                        dragInfo = this.MouseLocation;
                    }
                }
            }
            if (dragging) {
                if (this.MouseReleased)
                    dragging = false;
                else {
                    nodeViewScrollPos -= this.MouseLocation - dragInfo;
                    dragInfo = this.MouseLocation;
                }
            }
            GUI.backgroundColor = GUIController.DefaultColor;
        }
        private void DrawNodeConnections(Rect container) {
            GUI.depth = -150;
            foreach (var n in Program.Nodes) {
                foreach (var c in n.Outputs) {
                    if (c.Value.Connected) {
                        if (connections.ContainsKey(c.Value)) {
                            foreach (var c2 in c.Value.Connections) {
                                if (connections.ContainsKey(c2)) {
                                    Vector2 a = connections[c.Value];
                                    Vector2 b = connections[c2];
                                    GUILine.DrawLine(GUIUtility.ScreenToGUIPoint(a), GUIUtility.ScreenToGUIPoint(b), ConnectionColor(c.Value), 2, true);

                                }
                            }
                        }
                    }
                }
            }
        }
        private void DrawNode(Node node) {
            bool drawInputs = true;
            bool drawOutputs = true;
            bool isEditable = false;
            bool isVariableNode = false;
            SubRoutine subRoutine = null;
            string variableName = "";
            //find variable node
            NodeInfo info = new NodeInfo();
            var nodeType = node.GetType();
            if (nodeType.IsGenericType) {
                if (nodeType.GetGenericTypeDefinition() == typeof(VariableNode<>)) {
                    isVariableNode = true;
                    Variable v = node.GetType().GetProperty("Variable").GetValue(node, null) as Variable;
                    var n = from va in Program.Variables where va.Value == v select va.Key;
                    if (n.Count() > 0) {
                        variableName = n.First();
                        info = new NodeInfo(variableName, v.Type, "Variable node", TypeColor(v.Type), 120);
                    }
                }
            } else if (node is SubroutineNode) {
                var sr = node as SubroutineNode;
                subRoutine = sr.SubRoutineInstance;

                info = new NodeInfo(sr.SubRoutineBlueprint, typeof(SubRoutine), "Subroutine", Color.cyan, 220);
            } else if (node is SubRoutineEntry) {
                drawInputs = false;
                isEditable = true;
                info = new NodeInfo("Entry", typeof(SubRoutineEntry), "Entry", Color.cyan, 220);
            } else if (node is SubRoutineExit) {
                drawOutputs = false;
                isEditable = true;
                info = new NodeInfo("Exit", typeof(SubRoutineExit), "Exit", Color.cyan, 220);
            } else
                info = nodeCats.GetNodeInfo(node.GetType());
            info.width = Mathf.Max(info.width, GUI.skin.label.CalcSize(new GUIContent(info.name)).x + GUIController.ElSize * 2);
            //Log.Write("Drawing node " + node + " width: " + info.width);
            float height = Math.Max(node.InputCount * 2 - node.InputExecCount, node.OutputCount) * GUIController.ElSize;
            height += GUIController.ElSize;

            GUI.BeginGroup(new Rect(node.Position.x, node.Position.y, info.width, height));
            if (GUI.Button(new Rect(info.width - GUIController.ElSize * 1.5f, -5f, GUIController.ElSize, GUIController.ElSize), "x")) {
                if (draggedConnection != null)
                    if (draggedConnection.Node == node)
                        draggedConnection = null;
                Program.RemoveNode(node);
                return;
            }
            if (node is BaseExecutableNode) {
                if (subRoutine != null) {
                    GUI.skin.label.alignment = TextAnchor.LowerCenter;
                    if (GUI.Button(new Rect(info.width - GUIController.ElSize * 2.5f, -5f, GUIController.ElSize, GUIController.ElSize), "e")) {
                        currentSubroutineName = (node as SubroutineNode).SubRoutineBlueprint;
                        currentSubroutine = subRoutine;
                    }
                }
                float c = Mathf.Max(0, Mathf.Min(1, (Time.time - (node as BaseExecutableNode).LastExecution) * 3f));
                GUI.skin.label.alignment = TextAnchor.UpperRight;
                GUI.color = new Color(c, 1 - c, 0);
                GUI.Label(new Rect(info.width - GUIController.ElSize, -5f, GUIController.ElSize, GUIController.ElSize), "■");
                GUI.color = Color.white;
                GUI.skin.label.alignment = TextAnchor.UpperCenter;
            }


            GUI.backgroundColor = info.color;
            GUI.Box(new Rect(0, 0, info.width, height), info.name);
            if (PointerAvailable && this.MouseDown) {
                if (this.MouseLocation.x > toolbarWidth) {
                    if (new Rect(0, 0, info.width, 20).Contains(GUIUtility.ScreenToGUIPoint(this.MouseLocation))) {
                        if (Event.current.control) {
                            if (!(node is SubRoutineEntry || node is SubRoutineExit)) {
                                if (node is SubroutineNode) {
                                    draggedNode = Program.AddSubRoutineNode(node.Position.GetVec2(), (node as SubroutineNode).SubRoutineBlueprint);
                                    dragInfo = GUIUtility.ScreenToGUIPoint(this.MouseLocation);
                                } else if (isVariableNode) {
                                    draggedNode = Program.AddVariableNode(node.Position.GetVec2(), variableName);
                                    dragInfo = GUIUtility.ScreenToGUIPoint(this.MouseLocation);
                                } else {
                                    draggedNode = Program.AddNode(nodeType, node.Position.GetVec2());
                                    dragInfo = GUIUtility.ScreenToGUIPoint(this.MouseLocation);
                                }
                            }
                        } else {
                            draggedNode = node;
                            dragInfo = GUIUtility.ScreenToGUIPoint(this.MouseLocation);
                        }
                    }
                }
            }

            if (drawInputs)
                DrawNodeInputs(node.Inputs, info.width, isEditable);
            if (drawOutputs)
                DrawNodeOutputs(node.Outputs, info.width, isEditable);
            GUI.backgroundColor = GUIController.DefaultColor;
            GUI.EndGroup();

        }
        void DrawNodeInputs(KeyValuePair<string, ConnectorIn>[] inputs, float width, bool removable) {
            float y = GUIController.ElSize;
            foreach (var inp in inputs) {

                bool wasConnected = inp.Value.Connected;
                GUI.backgroundColor = ConnectionColor(inp.Value) * (wasConnected ? 1f : inactiveCol);
                bool buttonPressed = GUI.Button(new Rect(0, y, width / 2, GUIController.ElSize), inp.Key);
                if (removable) {
                    if (GUI.Button(new Rect(width / 2, y, width / 2, GUIController.ElSize), "Remove")) {
                        inp.Value.Node.RemoveInput(inp.Key);
                        return;
                    }
                }
                Vector2 anchor = GUIUtility.GUIToScreenPoint(GetNodeAnchor(0, y, 0));
                connections.Add(inp.Value, anchor);
                y += GUIController.ElSize;
                var infoRect = new Rect(0, y, width / 2, GUIController.ElSize);
                if (inp.Value.DataType == typeof(double)) {
                    inp.Value.Set(GUI.TextField(infoRect, inp.Value.AsString()));
                    y += GUIController.ElSize;
                } else if (inp.Value.DataType == typeof(float)) {

                    inp.Value.Set(GUI.TextField(infoRect, inp.Value.AsString()));
                    y += GUIController.ElSize;
                } else if (inp.Value.DataType == typeof(string)) {

                    inp.Value.Set(GUI.TextField(infoRect, inp.Value.AsString()));
                    y += GUIController.ElSize;
                } else if (inp.Value.DataType == typeof(bool)) {
                    inp.Value.Set(GUI.Toggle(infoRect, (bool)inp.Value.AsBool(), inp.Value.AsBool().ToString()));
                    y += GUIController.ElSize;
                }


                if (buttonPressed) {
                    if (!wasConnected) {
                        if (draggedConnection != null) {
                            //Log.Write("Trying to connect " + draggedConnection.Node + " to " + inp.Value.Node + "(" + draggedConnection.DataType + "==" + inp.Value.DataType + ")");
                            if (draggedConnection.ConnectTo(inp.Value)) {
                                //Log.Write("Connected " + draggedConnection.Node + " to " + inp.Value.Node);
                                draggedConnection = null;
                            }
                        }
                    } else if (wasConnected) {
                        if (Event.current.button == 1) {
                            inp.Value.DisconnectAll();
                        } else {
                            if (!inp.Value.AllowMultipleConnections)
                                inp.Value.DisconnectAll();
                            if (draggedConnection != null) {
                                //Log.Write("Trying to connect " + draggedConnection.Node + " to " + inp.Value.Node + "(" + draggedConnection.DataType + "==" + inp.Value.DataType + ")");
                                if (draggedConnection.DataType == inp.Value.DataType) {
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
        void DrawNodeOutputs(KeyValuePair<string, ConnectorOut>[] outputs, float width, bool removable) {
            float y = GUIController.ElSize;
            foreach (var outp in outputs) {

                bool wasConnected = outp.Value.Connected;
                GUI.backgroundColor = ConnectionColor(outp.Value) * (wasConnected ? 1f : inactiveCol);
                bool buttonPressed = GUI.Button(new Rect(width / 2, y, width / 2, GUIController.ElSize), outp.Key);
                if (removable) {
                    if (GUI.Button(new Rect(0, y, width / 2, GUIController.ElSize), "Remove")) {
                        outp.Value.Node.RemoveOutput(outp.Key);
                        return;
                    }
                }
                Vector2 anchor = GUIUtility.GUIToScreenPoint(GetNodeAnchor(width / 2, y, width / 2));
                connections.Add(outp.Value, anchor);
                if (buttonPressed && PointerAvailable) {
                    if (!wasConnected) {
                        dragInfo = anchor;
                        draggedConnection = outp.Value;
                    } else if (wasConnected) {
                        if (Event.current.button == 1) {
                            outp.Value.DisconnectAll();
                        } else {
                            if (!outp.Value.AllowMultipleConnections)
                                outp.Value.DisconnectAll();
                            dragInfo = GUIUtility.GUIToScreenPoint(GetNodeAnchor(width / 2, y, width / 2));
                            draggedConnection = outp.Value;

                        }
                    }
                }
                y += GUIController.ElSize;
            }
        }
        Color ConnectionColor(Connector c) {
            var t = c.DataType;
            return TypeColor(t);
        }
        Color TypeColor(Type t) {
            if (t == typeof(float))
                return new Color(0.2f, 1f, 0.3f);
            if (t == typeof(double))
                return new Color(0.2f, 1f, 0.0f);
            if (t == typeof(bool))
                return new Color(0.2f, 0.2f, 1f);
            if (t == typeof(Quaternion))
                return new Color(0.2f, 1.0f, 1.0f);
            if (t == typeof(SVector3d))
                return new Color(1.0f, 0.7f, 0.0f);
            if (t == typeof(SVector2d))
                return new Color(1.0f, 1.0f, 0.0f);
            if (t == typeof(string))
                return new Color(0.5f, 0.7f, 0.95f);
            return Color.white;
        }
        public void ReloadSubRoutines() {
            subRoutines = KSPOperatingSystem.ListSubRoutines();
        }
        Vector2 GetNodeAnchor(float x, float y, float addX) {
            return new Vector2(x + addX + (addX > 0 ? -5 : 5), y + GUIController.ElSize / 2);
        }
    }
}
