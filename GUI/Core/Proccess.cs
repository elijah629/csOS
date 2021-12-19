namespace CSOS.GUI.Core
{
    public class Process
    {
        public string Title = "Process";

        public void Delete()
        {
            System.OpenProcs.Remove(this);
        }
        public void Start()
        {
            System.OpenProcs.Add(this);
        }
        public virtual void Update() { }
    }
}
