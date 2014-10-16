using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KSPFlightPlanner.Program;
using KSPFlightPlanner.Program.NodeDataTypes;
using KSPFlightPlanner.Program.Nodes;
namespace KSPFlightPlanner
{
    public class ProgramDrawer
    {
        private bool inputLocked = false;
        private Vector2 nodeViewScrollPos;

        private Dictionary<Type, NodeInfo> RegisteredNodeTypes;
        private FlightProgram program;
        private Rect windowRect;
        private float smallGap = 2;
        private Color defaultColor = Color.white;
        private Node draggedNode;
        private NodeConnectionOut draggedConnection;
        private Vector2 dragInfo;
        private bool mouseDown = false;
        private bool mousePressed = false;
        private bool mouseReleased = false;
        private Dictionary<NodeConnectionIn, Vector2[]> connections;
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
            RegisteredNodeTypes = new Dictionary<Type, NodeInfo>();
            RegisteredNodeTypes.Add(typeof(NodeTick), new NodeInfo("Tick", Color.red, new Vector2(180, 50)));
			RegisteredNodeTypes.Add(typeof(NodePreLaunch), new NodeInfo("PreLaunch", Color.red, new Vector2(180, 50)));
			RegisteredNodeTypes.Add(typeof(NodeAltitudeReached), new NodeInfo("Height reached (m)", Color.red, new Vector2(280, 130)));
            RegisteredNodeTypes.Add(typeof(NodeTriggerStage), new NodeInfo("Activate next stage", Color.green, new Vector2(150, 100)));
			RegisteredNodeTypes.Add(typeof(NodeSetThrottle), new NodeInfo("Set throttle", Color.green, new Vector2(150, 90)));
			RegisteredNodeTypes.Add(typeof(NodeAltitude), new NodeInfo("Altitude (m)", Color.blue, new Vector2(180, 50)));
			RegisteredNodeTypes.Add(typeof(NodeDelay), new NodeInfo("Delay (s)", Color.cyan, new Vector2(150, 100)));
			connections = new Dictionary<NodeConnectionIn, Vector2[]>();

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
            foreach (var n in RegisteredNodeTypes)
            {
               // Debug.Log("Node: " + n.ToString());
                
                GUI.backgroundColor = n.Value.Color;
                if (GUI.Button(new Rect(smallGap, smallGap + cy, 140, h), n.Value.Name))
                {
					Log.Write("Added node: " + n.Value);
					draggedNode = program.AddNode(n.Key, GUIUtility.GUIToScreenPoint(mousePos));
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
					draggedNode = null;
				else
				{
					var vec = GUIUtility.ScreenToGUIPoint(mousePos) - dragInfo;
					draggedNode.Position = new float[] {vec.x, vec.y};
				}
            }
            foreach (var n in program.Nodes)
                DrawNode(n);
            
            GUI.EndScrollView();
            GUI.backgroundColor = defaultColor;
        }
        private void DrawNodeConnections()
        {
			GUI.depth = -150;
            foreach (var c in connections)
            {
                if (windowRect.Contains(c.Value[0]) || windowRect.Contains(c.Value[1]))
                    GUIHelper.DrawLine(GUIUtility.ScreenToGUIPoint(c.Value[0]), GUIUtility.ScreenToGUIPoint(c.Value[1]), Color.white, 4);
                
            }
        }
        private void DrawNode(Node node)
        {
            float baseSize = 25;
            NodeInfo info = RegisteredNodeTypes[node.GetType()];
            GUI.BeginGroup(new Rect(node.Position[0], node.Position[1], info.Size.x, info.Size.y));
            GUI.backgroundColor = info.Color;
            GUI.Box(new Rect(0, 0, info.Size.x, info.Size.y), info.Name);
            if (PointerAvailable && mousePressed)
            {
                if (new Rect(0,0, info.Size.x, 20).Contains(GUIUtility.ScreenToGUIPoint(mousePos)))
                {
                    draggedNode = node;
                    dragInfo = GUIUtility.ScreenToGUIPoint(mousePos);
                }
            }
            DrawNodeInputs(node.Inputs, baseSize, info.Size);
            DrawNodeOutputs(node.Outputs, baseSize, info.Size);
            GUI.backgroundColor = defaultColor;
            GUI.EndGroup();
            
        }
		void DrawNodeInputs(Dictionary<string, NodeConnectionIn> inputs, float baseSize, Vector2 size)
		{
			float y = 15;
			foreach (var inp in inputs)
			{
				bool wasConnected = connections.ContainsKey(inp.Value);
				if (wasConnected)
				{
					connections[inp.Value][1] = GUIUtility.GUIToScreenPoint(GetNodeAnchor(smallGap, y));
				}
				bool isConnected = GUI.Toggle(new Rect(smallGap, y, size.x / 2, baseSize), wasConnected, inp.Key);
				if (inp.Value.DataType == typeof(double))
				{
					if (inp.Value.DataBuffer == null)
						inp.Value.DataBuffer = 0.0;

					inp.Value.DataBuffer = GUI.TextField(new Rect(smallGap, y + baseSize + smallGap, size.x / 2, baseSize), inp.Value.DataBuffer.ToString());
					y += baseSize + smallGap;
				}
				else if (inp.Value.DataType == typeof(float))
				{
					if (inp.Value.DataBuffer == null)
						inp.Value.DataBuffer = 0.0f;

					inp.Value.DataBuffer = GUI.TextField(new Rect(smallGap, y + baseSize + smallGap, size.x / 2, baseSize), inp.Value.DataBuffer.ToString());
					y += baseSize + smallGap;
				}
				else if (inp.Value.DataType == typeof(bool))
				{
					if (inp.Value.DataBuffer == null)
						inp.Value.DataBuffer = false;
					inp.Value.DataBuffer = GUI.Toggle(new Rect(smallGap, y + baseSize + smallGap, size.x / 2, baseSize), (bool)inp.Value.GetBufferAsBool(), inp.Value.DataBuffer.ToString());
					y += baseSize + smallGap;
				}
				y += baseSize + smallGap;
				if (!wasConnected && isConnected)
				{
					if (draggedConnection != null)
					{
						Log.Write("Trying to connect " + draggedConnection.Owner + " to " + inp.Value.Owner + "(" + draggedConnection.DataType + "==" + inp.Value.DataType + ")");
						if (draggedConnection.DataType == inp.Value.DataType)
						{
							Log.Write("Connected " + draggedConnection.Owner + " to " + inp.Value.Owner);
							draggedConnection.Connect(inp.Value);
							draggedConnection = null;

						}
					}
				}
			}
		}
        void DrawNodeOutputs(Dictionary<string, NodeConnectionOut> outputs, float baseSize, Vector2 size)
        {
            float y = 15;
            foreach (var outp in outputs)
            {
                bool wasConnected = outp.Value.ConnectedTo != null;
                if (wasConnected)
                {
                    if (!connections.ContainsKey(outp.Value.ConnectedTo))
                    {
                        connections.Add(outp.Value.ConnectedTo, new Vector2[] { GUIUtility.GUIToScreenPoint(GetNodeAnchor(size.x / 2, y)), new Vector2() });
                    }
                    else
                    {
                        connections[outp.Value.ConnectedTo][0] = GUIUtility.GUIToScreenPoint(GetNodeAnchor(size.x / 2, y));
                    }
                }
                bool isConnected = GUI.Toggle(new Rect(size.x / 2, y, size.x / 2, baseSize), wasConnected, outp.Key);
                if (!wasConnected && isConnected)
                {
                    if (PointerAvailable)
                    {
                        dragInfo = GUIUtility.GUIToScreenPoint(GetNodeAnchor(size.x / 2, y));
                        draggedConnection = outp.Value;
                    }
                }
                else if (wasConnected && !isConnected)
                {
                    if (PointerAvailable)
                    {
                        //dragInfo = GUIUtility.GUIToScreenPoint(GetNodeAnchor(size.x / 2, y));
                        connections.Remove(outp.Value.ConnectedTo);
                        outp.Value.Connect(null);
                        //draggedConnection = outp.Value;
                    }
                }
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
        Vector2 GetNodeAnchor(float x, float y)
        {
            return new Vector2(x + 16, y + 16);
        }
    }
}
