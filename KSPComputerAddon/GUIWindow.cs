using UnityEngine;
namespace KSPComputerModule {
    public abstract class GUIWindow {
        public bool LmbPressed { get; private set; }
        public bool LmbDown { get; private set; }
        public bool LmbReleased { get; private set; }
        public bool RmbPressed { get; private set; }
        public bool RmbDown { get; private set; }
        public bool RmbReleased { get; private set; }
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
            bool lmbDown = Input.GetMouseButton(0);
            this.LmbPressed = lmbDown && !this.LmbDown;
            this.LmbReleased = !lmbDown && this.LmbDown;
            this.LmbDown = lmbDown;

            bool rmbDown = Input.GetMouseButton(3);
            this.RmbPressed = rmbDown && !this.RmbDown;
            this.RmbReleased = rmbDown && this.RmbDown;
            this.RmbDown = rmbDown;
        }
        public virtual void Start() {
            this.LmbDown = false;
            this.LmbPressed = false;
            this.LmbReleased = false;
            this.RmbDown = false;
            this.RmbPressed = false;
            this.RmbReleased = false;
        }
    }
}
