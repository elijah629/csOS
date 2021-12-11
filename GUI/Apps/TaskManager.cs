using CSOS.GUI.Core;
using Mosa.External.x86.Drawing;
using Mosa.External.x86.Drawing.Fonts;
using System.Drawing;

namespace CSOS.GUI.Apps
{
    public class TaskManager : App
    {

        public TaskManager()
        {
            Width = 325;
            Height = 325;
            if (windowGraphics == null)
            {
                windowGraphics = new VirtualGraphics(Height, Width);
            }

            Title = "Task Manager";
        }

        public override void UIUpdate()
        {
            windowGraphics.Clear(Color.White.ToArgb());

            for (int i = 0; i < Core.System.OpenApps.Count; i++)
            {
                windowGraphics.DrawACS16String(Color.Black.ToArgb(), $"{Core.System.OpenApps[i].Title} - State: {(Core.System.OpenApps[i].Visible ? (Core.System.OpenApps[i].Activated ? "Active" : "Open") : "Minimized")}", 5, i * 16);
            }

            windowGraphics.Update();
            Program.graphics.DrawImageASM(windowGraphics.bitmap, X, Y);
        }
    }
}
