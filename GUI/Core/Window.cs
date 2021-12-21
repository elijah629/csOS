using csOS.GUI.Apps;
using Mosa.External.x86.Drawing;
using Mosa.External.x86.Drawing.Fonts;
using Mosa.External.x86.Driver;
using System;
using System.Collections.Generic;

namespace csOS.GUI.Core
{
    public class Window
    {
        private int _Index = 0;
        private bool _Pinned = true;
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
        private uint _BackgroundColor = ThemeManager.CurrentTheme.DefaultAppBackground;
        private int _Y_Dock = -1;
        private bool _Moveble = true;
        private List<UIControl> _Children = new List<UIControl>();
        public bool Activated => _Index == System.OpenApps.Count - 1;
        // Props:
        public int Index { get => _Index; set => _Index = value; }
        public bool Pinned { get => _Pinned; set => _Pinned = value; }
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
        public uint BackgroundColor { get => _BackgroundColor; set => _BackgroundColor = value; }
        public int Y_Dock { get => _Y_Dock; set => _Y_Dock = value; }
        public bool Moveble { get => _Moveble; set => _Moveble = value; }
        public List<UIControl> Children { get => _Children; set => _Children = value; }

        public Window() {
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
            if (WindowGraphics == null)
            {
                WindowGraphics = new VirtualGraphics(Width, Height);
            }
            WindowGraphics.Clear(BackgroundColor);

            int Rad = (BarHeight / 2) - 5;
            if (PS2Mouse.MouseStatus == Mosa.External.x86.Driver.Input.MouseStatus.Left)
            {
                if (PS2Mouse.X > X && PS2Mouse.X < X + Width && PS2Mouse.Y > Y - BarHeight && PS2Mouse.Y < Y && !Moving && Moveble)
                {
                    if (PS2Mouse.X > X + Width - (Rad * 3) && Activated)
                    {
                        Close();

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
                X = Math.Clamp(PS2Mouse.X - OffsetX, 0, Program.ScreenWidth - Width);
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
            Program.graphics.DrawFilledCircle(ThemeManager.CurrentTheme.WindowCloseButtonColor, X + Width - (Rad * 2), Y - BarHeight / 2, Rad);

            Program.graphics.DrawFilledCircle(ThemeManager.CurrentTheme.WindowMinimizeButtonColor, X + Width - (Rad * 6), Y - BarHeight / 2, Rad);

            if (Activated) InputUpdate();

            UIUpdate();

            foreach (UIControl uIControl in Children)
                uIControl.Update(this);
            WindowGraphics.Update();
            Program.graphics.DrawImageASM(WindowGraphics.bitmap, X, Y);
        }
        public void Close()
        {
            System.OpenApps.Remove(this);
            if (Pinned)
            {
                System.ClosedApps.Add(this);
            }
        }
        public void Hide()
        {
            Visible = false;
        }

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
