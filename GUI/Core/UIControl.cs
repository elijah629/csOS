namespace CSOS.GUI.Core
{
    public class UIControl
    {
        public bool Initilized = false;
        public virtual void Init() { }
        public virtual void Update(Window app) { }
        public UIControl()
        {
            Init();
            Initilized = true;
        }
    }
}