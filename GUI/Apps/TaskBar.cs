using CSOS.GUI.Core;
using Mosa.External.x86;
using Mosa.External.x86.Drawing.Fonts;
using Mosa.External.x86.Driver;
using Mosa.Kernel.x86;
using Mosa.Runtime;
using System.Drawing;

namespace CSOS.GUI.Apps
{
    internal class TaskBar : App
    {
        public TaskBar()
        {
            X = 0;
            Y = 0;

            Width = Program.ScreenWidth;

            Title = "TaskBar";

            isWindow = false;

            Height = TaskBarHeight;
        }

        private string time;

        public const int TaskBarHeight = 30;
        public override void InputUpdate()
        {
            if (PS2Mouse.MouseStatus == Mosa.External.x86.Driver.Input.MouseStatus.Left)
                if (PS2Mouse.X >= Width - (TaskBarHeight / 2) - 7 && PS2Mouse.X <= Width - (TaskBarHeight / 2) + 7)
                    if (PS2Mouse.Y >= (TaskBarHeight / 2) - 7 && PS2Mouse.Y <= (TaskBarHeight / 2) + 7)
                        Power.Shutdown();
        }
        public override void UIUpdate()
        {
            Program.graphics.DrawFilledRectangle(Core.System.Color2, X, Y, Width, Height);
            Program.graphics.DrawFilledCircle(Color.Red.ToArgb(), Width - TaskBarHeight / 2, TaskBarHeight / 2, 7);
            time = CMOS.Hour + ":" + CMOS.Minute.ToString().PadLeft(2, '0');
            Program.graphics.DrawBitFontString(Core.System.DefaultFontName, 0xFFF0F3F4, time, (Width / 2) - (BitFont.Calculate(Core.System.DefaultFontName, time) / 2), Y + (Height / 2 - 8));
            time.Dispose();
        }
    }
}