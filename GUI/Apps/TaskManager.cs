using CSOS.GUI.Core;
using CSOS.GUI.UI;
using Mosa.External.x86;
using Mosa.External.x86.Drawing;
using Mosa.External.x86.Drawing.Fonts;
using System.Drawing;

namespace CSOS.GUI.Apps
{
    public class TaskManager : Window
    {
        public Button btn;
        public int cc = 0;
        public TaskManager()
        {
            Width = 325;
            Height = 325;
            if (WindowGraphics == null)
            {
                WindowGraphics = new VirtualGraphics(Height, Width);
            }
            Title = "Task Manager";
            btn = new Button()
            {
                Text = "Hello",
                OnClick = () =>
                {
                    btn.Text = cc++.ToString();
                },
                X = 5,
                Y = 5,
                Width = 90,
                Height = 20
            };
            btn.Init();
        }
        
        public override void UIUpdate()
        {
            WindowGraphics.Clear(ThemeManager.CurrentTheme.AppBackground);
            btn.Update(this);
            /*for (int i = 0; i < Core.System.OpenApps.Count; i++)*/
            /*{*/
            /*    windowGraphics.DrawACS16String(Color.Black.ToArgb(), $"{Core.System.OpenApps[i].Title} - State: {(Core.System.OpenApps[i].Visible ? (Core.System.OpenApps[i].Activated ? "Active" : "Open") : "Minimized")}", 5, i * 16);*/
            /*}*/
            WindowGraphics.Update();
            Program.graphics.DrawImageASM(WindowGraphics.bitmap, X, Y);
        }
    }
}
