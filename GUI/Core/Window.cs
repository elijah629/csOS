using CSOS.GUI.Apps;
using Mosa.External.x86.Drawing;
using Mosa.External.x86.Drawing.Fonts;
using Mosa.External.x86.Driver;
using System;
using System.Collections.Generic;

namespace CSOS.GUI.Core
{
    public class Window
    {
        private int _Index = 0;
        private bool _Dock = true;
        private int _X;
        private int _Y;
        private int _Width = 200;
        private int _Height = 125;
        private bool Moving = false;
        private int OffsetX;
        private int OffsetY;
        private string _Title = "App";
        private const int BarHeight = 20;
        private VirtualGraphics _windowGraphics;
        private Bitmap _Icon_48 = null;
        private Bitmap _Icon_16 = null;
        private bool _Visible = true;
        private int _X_Dock = -1;
        private int _Y_Dock = -1;
        private bool _Moveble = true;
        private List<UIControl> _UIControls = new List<UIControl>();
        public bool Activated => _Index == System.OpenApps.Count - 1;
        // Props:
        public int Index { get => _Index; set => _Index = value; }
        public bool Dock { get => _Dock; set => _Dock = value; }
        public int X { get => _X; set => _X = value; }
        public int Y { get => _Y; set => _Y = value; }
        public int Width { get => _Width; set => _Width = value; }
        public int Height { get => _Height; set => _Height = value; }
        public string Title { get => _Title; set => _Title = value; }
        public VirtualGraphics WindowGraphics { get => _windowGraphics; set => _windowGraphics = value; }
        public Bitmap Icon_48 { get => _Icon_48; set => _Icon_48 = value; }
        public Bitmap Icon_16 { get => _Icon_16; set => _Icon_16 = value; }
        public bool Visible { get => _Visible; set => _Visible = value; }
        public int X_Dock { get => _X_Dock; set => _X_Dock = value; }
        public int Y_Dock { get => _Y_Dock; set => _Y_Dock = value; }
        public bool Moveble { get => _Moveble; set => _Moveble = value; }
        public List<UIControl> UIControls { get => _UIControls; set => _UIControls = value; }

        public Window()
        {
            X_Dock = -1;
            Y_Dock = -1;
            if (Height < 100)
            {
                Height = 100;
            }

            if (Width < 100)
            {
                Width = 100;
            }

            X = (Program.ScreenWidth - Width) / 2;
            Y = (Program.ScreenHeight - Height) / 2;
        }

        public static void MoveWindowToEnd(Window window)
        {
            if (window.Index == System.OpenApps.Count - 1)
            {
                return;
            }

            int index = window.Index;
            foreach (var v in System.OpenApps)
            {
                if (v.Index > index)
                {
                    v.Index--;
                }
            }
            window.Index = System.OpenApps.Count - 1;
        }
        public void Activate()
        {
            MoveWindowToEnd(this);
        }
        public void Update()
        {
            if (!Visible)
            {
                return;
            }

            int Rad = (BarHeight / 2) - 5;
            if (PS2Mouse.MouseStatus == Mosa.External.x86.Driver.Input.MouseStatus.Left)
            {
                if (PS2Mouse.X > X && PS2Mouse.X < X + Width && PS2Mouse.Y > Y - BarHeight && PS2Mouse.Y < Y && !Moving && Moveble)
                {
                    if (PS2Mouse.X > X + Width - (Rad * 3) && Activated)
                    {
                        System.OpenApps.Remove(this);
                        if (Dock)
                        {
                            System.ClosedApps.Add(this);
                        }

                        return;
                    }
                    if (PS2Mouse.X > X + Width - (Rad * 6) && Activated)
                    {
                        Visible = false;
                        return;
                    }
                    Moving = true;

                    OffsetX = PS2Mouse.X - X;
                    OffsetY = PS2Mouse.Y - Y;

                    MoveWindowToEnd(this);
                }
            }
            else
            {
                Moving = false;
            }

            if (Moving && Activated)
            {
                X = Math.Clamp(PS2Mouse.X - OffsetX, 1, Program.ScreenWidth - (Width + 1));
                Y = Math.Clamp(PS2Mouse.Y - OffsetY, BarHeight + TaskBar.Height, Program.ScreenHeight - (Height + (BarHeight - (5 * 2))));
            }

            Program.graphics.DrawFilledRectangle(Activated ? ThemeManager.CurrentTheme.FocusedColor : ThemeManager.CurrentTheme.UnFocusedColor, X, Y - BarHeight + 5, Width, BarHeight - 5);
            Program.graphics.DrawFilledRoundedRectangle(Activated ? ThemeManager.CurrentTheme.FocusedColor : ThemeManager.CurrentTheme.UnFocusedColor, X, Y - BarHeight, Width, BarHeight, 5);
            Program.graphics.DrawFilledRoundedRectangle(Activated ? ThemeManager.CurrentTheme.FocusedColor : ThemeManager.CurrentTheme.UnFocusedColor, X, Y + Height - 5, Width, BarHeight - 5, 5);
            if (Icon_16 == null)
            {
                Program.graphics.DrawFilledRectangle(ThemeManager.CurrentTheme.NoAppIcon, X + ((BarHeight / 2) - 8), Y - BarHeight / 2 - (16 / 2), 16, 16);
            }
            else
            {
                Program.graphics.DrawImage(Icon_16, X + ((BarHeight / 2) - 8), Y - BarHeight / 2 - (16 / 2), true);
            }
            Program.graphics.DrawBitFontString(System.DefaultFontName, ThemeManager.CurrentTheme.TextColor, Title, X + ((BarHeight / 2) - 8) + 21, Y - BarHeight / 2 - (16 / 2));

            Program.graphics.DrawFilledRectangle(ThemeManager.CurrentTheme.AppBackground, X, Y, Width, Height);

            Program.graphics.DrawFilledCircle(ThemeManager.CurrentTheme.WindowCloseButtonColor, X + Width - (Rad * 2), Y - BarHeight / 2, Rad);

            Program.graphics.DrawFilledCircle(ThemeManager.CurrentTheme.WindowMinimizeButtonColor, X + Width - (Rad * 6), Y - BarHeight / 2, Rad);

            if (Activated) InputUpdate();

            UIUpdate();

            Program.graphics.DrawLine(Activated ? ThemeManager.CurrentTheme.FocusedColor : ThemeManager.CurrentTheme.UnFocusedColor, X, Y, X, Y + Height);
            Program.graphics.DrawLine(Activated ? ThemeManager.CurrentTheme.FocusedColor : ThemeManager.CurrentTheme.UnFocusedColor, X + Width - 1, Y, X + Width - 1, Y + Height);
            /**
             * Update UI Controls
             */
            foreach (UIControl uIControl in UIControls)
                if (uIControl.Initilized) uIControl.Update(this);
        }
        public void Close()
        {
            System.OpenApps.Remove(this);
            System.ClosedApps.Add(this);
        }
        public void Hide() => Visible = false;
        public void Open()
        {
            System.ClosedApps.Remove(this);
            Index = System.OpenApps.Count;
            System.OpenApps.Add(this);
            Activate();
        }
        public virtual void UIUpdate() { }
        public virtual void InputUpdate() { }
    }
}
