using UnityEngine;
namespace KSPComputerModule {
    public abstract class GUIWindow {
        public bool MousePressed { get; private set; }
        public bool MouseDown { get; private set; }
        public bool MouseReleased { get; private set; }
        public Vector2 MouseLocation { get; private set; }
        public Vector2 GuiMouseLocation { get; private set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Opened { get; set; }
        public abstract string Title { get; }
        public Rect WinRect;
        public abstract Vector2 MinSize { get; }
        public virtual void Draw() {
            this.MouseLocation = new Vector2(Input.mousePosition.x, (Screen.height - Input.mousePosition.y));
            this.GuiMouseLocation = GUIUtility.ScreenToGUIPoint(this.MouseLocation);
            bool mouseDown = Input.GetMouseButton(0);
            this.MousePressed = false;
            this.MouseReleased = false;
            if (!this.MouseDown && mouseDown) {
                this.MousePressed = true;
            }
            if (this.MouseDown && !mouseDown) {
                this.MouseReleased = true;
            }
            this.MouseDown = mouseDown;
        }
        public virtual void Start() {
            this.MouseDown = false;
            this.MousePressed = false;
            this.MouseReleased = false;
        }
    }
}
