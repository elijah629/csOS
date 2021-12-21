using csOS.GUI.Core;
using Mosa.External.x86.Drawing.Fonts;
using System.Drawing;

namespace csOS.GUI.Apps
{
    public class TaskManager : Window
    {
        public TaskManager() : base()
        {
            Width = 325;
            Height = 325;
            Title = "Task Manager";
        }

        public override void UIUpdate()
        {
            for (int i = 0; i < Core.System.OpenApps.Count; i++)
            {
                WindowGraphics.DrawACS16String(Color.Black.ToArgb(), $"{Core.System.OpenApps[i].Title} - State: {(Core.System.OpenApps[i].Visible ? (Core.System.OpenApps[i].Activated ? "Active" : "Open") : "Minimized")}", 5, i * 16);
            }
        }
    }
}
