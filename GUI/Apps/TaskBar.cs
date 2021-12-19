using CSOS.GUI.Core;
using Mosa.External.x86.Drawing.Fonts;
using Tools = CSOS.Helper.Tools;

namespace CSOS.GUI.Apps
{
    public class TaskBar : Process
    {
        public const int Height = 30;
        public static int Width = Program.ScreenWidth;
        public TaskBar()
        {
            Title = "TaskBar";
        }
        public override void Update()
        {
            Program.graphics.DrawFilledRectangle(ThemeManager.CurrentTheme.DockBackgroundColor, 0, 0, Width, Height);
            Program.graphics.DrawBitFontString(Core.System.DefaultFontName, ThemeManager.CurrentTheme.TextColor, Tools.GetTime(), (Width / 2) - (BitFont.Calculate(Core.System.DefaultFontName, Tools.GetTime()) / 2), Height / 2 - 8);
        }
    }
}