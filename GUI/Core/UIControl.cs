namespace csOS.GUI.Core
{
    public class UIControl
    {
        public virtual void Init() { }
        public virtual void Update(Window app) { }
        public UIControl()
        {
            Init();
        }
    }
}