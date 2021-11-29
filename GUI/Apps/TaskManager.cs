using Mosa.External.x86.Drawing.Fonts;
using Mosa.External.x86.Drawing;
using System.Drawing;
using Mosa.Runtime.x86;
using Mosa.External.x86;

namespace csOS.GUI.Apps
{
    class TaskManager : App
    {

        public App_taskmanager() {
            X=500;
            Y=300;
            Width=325;
            Height=325;
            Title="Task Manager";
        }

        public override void UIUpdate()
        {
            if (windowGraphics == null) windowGraphics = new VirtualGraphics(Height, Width);
            windowGraphics.Clear(Color.White.ToArgb());
            
            for (int i=0; i < Core.System.OpenApps.Count; i++)
            {
                windowGraphics.DrawACS16String(Color.Black.ToArgb(), Core.System.OpenApps[i].Title + " - State: Open, ID: " + Core.System.OpenApps.IndexOf(Core.System.OpenApps[i]), 5, i * 16);
            }
            
            windowGraphics.Update();
            Program.graphics.DrawImageASM(windowGraphics.bitmap, X, Y);
        }
    }
}
