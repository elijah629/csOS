using CSOS.GUI.Core;
using Mosa.External.x86.Drawing;
using Mosa.External.x86.Drawing.Fonts;
using Mosa.External.x86.Driver;
using Mosa.External.x86.Driver.Input;
using System;

namespace CSOS.GUI.UI
{
    public class Button
    {
        public int X = 0;
        public int Y = 0;
        public string Text = "";
        private int Height = 20;
        private int Width = 0;
        private uint Current_Color;
        public uint Background_Color = Core.System.Color2;
        public int Border_Radius = 0;
        private bool PressLock = false;
        public Action OnClick;
        public void Init()
        {
            Current_Color = Background_Color;
            Width = Width < BitFont.Calculate(Core.System.DefaultFontName, Text) * 2 ? BitFont.Calculate(Core.System.DefaultFontName, Text) * 2 : Width;
        }
        public void Update(VirtualGraphics graphics, int winX, int winY)
        {
            graphics.DrawFilledRoundedRectangle(Current_Color, X, Y, Width, Height, Border_Radius);
            graphics.DrawBitFontString(Core.System.DefaultFontName, Core.System.TextColor, Text, X, Y);
            if (PS2Mouse.X > X + winX && PS2Mouse.X < X + winX + Width && PS2Mouse.Y > Y + winY && PS2Mouse.Y < Y + winY + Height)
            {
                CursorManager.SetCursor("pointer");
                Current_Color = Core.System.Color1;
            }
            else
            {
                Current_Color = Background_Color;
                CursorManager.SetCursor("normal");
            }
            if (PS2Mouse.MouseStatus == MouseStatus.Left)
            {
                if (!PressLock)
                {
                    if (PS2Mouse.X > X + winX && PS2Mouse.X < X + winX + Width && PS2Mouse.Y > Y + winY && PS2Mouse.Y < Y + winY + Height)
                    {
                        OnClick.Invoke();
                        PressLock = true;
                    }
                }
            } else
            {
                PressLock = false;
                Current_Color = Background_Color;
            }
        }
        public void Update()
        {
            Graphics graphics = Program.graphics;
            graphics.DrawFilledRoundedRectangle(Current_Color, X, Y, Width, Height, Border_Radius);
            graphics.DrawBitFontString(Core.System.DefaultFontName, Core.System.TextColor, Text, X, Y);
            if (PS2Mouse.X > X && PS2Mouse.X < X + Width && PS2Mouse.Y > Y && PS2Mouse.Y < Y + Height)
            {
                CursorManager.SetCursor("pointer");
                Current_Color = Core.System.Color1;
            }
            else
            {
                CursorManager.SetCursor("normal");
            }
            if (PS2Mouse.MouseStatus == MouseStatus.Left)
            {
                if (!PressLock)
                {
                    if (PS2Mouse.X > X && PS2Mouse.X < X + Width && PS2Mouse.Y > Y && PS2Mouse.Y < Y + Height)
                    {
                        OnClick.Invoke();
                        PressLock = true;
                    }
                }
            } else
            {
                PressLock = false;
                Current_Color = Background_Color;
            }
        }
    }
}
