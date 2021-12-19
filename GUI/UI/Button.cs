/*using CSOS.GUI.Core;
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
        public int Height = 20;
        public int Width = 0;
        private uint CurrentColor;
        public uint BackgroundColor = ThemeManager.CurrentTheme.FocusedColor;
        public uint HoverColor = ThemeManager.CurrentTheme.HoverColor;
        public int Border_Radius = 5;
        private bool PressLock = false;
        public Action OnClick;
        public void Init()
        {
            CurrentColor = BackgroundColor;
        }
        public void Update(VirtualGraphics graphics, int winX, int winY)
        {
            graphics.DrawFilledRoundedRectangle(CurrentColor, X, Y, Width, Height, Border_Radius);
            graphics.DrawBitFontString(Core.System.DefaultFontName, ThemeManager.CurrentTheme.TextColor, Text, X, Y);
            if (PS2Mouse.X > X + winX && PS2Mouse.X < X + winX + Width && PS2Mouse.Y > Y + winY && PS2Mouse.Y < Y + winY + Height)
            {
                CursorManager.SetCursor("pointer");
                CurrentColor = HoverColor;
            }
            else
            {
                CurrentColor = BackgroundColor;
                CursorManager.SetCursor("normal");
            }
            if (PS2Mouse.MouseStatus == MouseStatus.Left)
            {
                if (!PressLock && PS2Mouse.X > X + winX && PS2Mouse.X < X + winX + Width && PS2Mouse.Y > Y + winY && PS2Mouse.Y < Y + winY + Height)
                {
                    CurrentColor = ThemeManager.CurrentTheme.FocusedColor;
                    OnClick.Invoke();
                    PressLock = true;
                }
            }
            else
            {
                PressLock = false;
                CurrentColor = BackgroundColor;
            }
        }
        public void Update()
        {
            Graphics graphics = Program.graphics;
            graphics.DrawFilledRoundedRectangle(CurrentColor, X, Y, Width, Height, Border_Radius);
            graphics.DrawBitFontString(Core.System.DefaultFontName, ThemeManager.CurrentTheme.TextColor, Text, X, Y);
            if (PS2Mouse.X > X && PS2Mouse.X < X + Width && PS2Mouse.Y > Y && PS2Mouse.Y < Y + Height)
            {
                CursorManager.SetCursor("pointer");
                CurrentColor = ThemeManager.CurrentTheme.HoverColor;
            }
            else
            {
                CurrentColor = BackgroundColor;
                CursorManager.SetCursor("normal");
            }
            if (PS2Mouse.MouseStatus == MouseStatus.Left)
            {
                if (!PressLock && PS2Mouse.X > X && PS2Mouse.X < X + Width && PS2Mouse.Y > Y && PS2Mouse.Y < Y + Height)
                {
                    CurrentColor = ThemeManager.CurrentTheme.FocusedColor;
                    OnClick.Invoke();
                    PressLock = true;
                }
            }
            else
            {
                PressLock = false;
                CurrentColor = BackgroundColor;
            }
        }
    }
}*/