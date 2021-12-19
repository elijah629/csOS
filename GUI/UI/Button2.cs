using CSOS.GUI.Core;
using Mosa.External.x86.Drawing;
using Mosa.External.x86.Drawing.Fonts;
using Mosa.External.x86.Driver;
using Mosa.External.x86.Driver.Input;
using System;

namespace CSOS.GUI.UI
{
    public class Button : UIControl
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

        public Button() : base() { }
        public override void Update(Window app)
        {
            VirtualGraphics graphics = app.WindowGraphics;
            graphics.DrawFilledRoundedRectangle(CurrentColor, X, Y, Width, Height, Border_Radius);
            graphics.DrawBitFontString(Core.System.DefaultFontName, ThemeManager.CurrentTheme.TextColor, Text, X, Y);
            if (PS2Mouse.X > X + app.X && PS2Mouse.X < X + app.X + Width && PS2Mouse.Y > Y + app.Y && PS2Mouse.Y < Y + app.Y + Height)
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
                if (!PressLock && PS2Mouse.X > X + app.X && PS2Mouse.X < X + app.X + Width && PS2Mouse.Y > Y + app.Y && PS2Mouse.Y < Y + app.Y + Height)
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
        public override void Init()
        {
            CurrentColor = BackgroundColor;
        }
    }
}