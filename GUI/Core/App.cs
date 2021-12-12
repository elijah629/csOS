using CSOS.GUI.Apps;
using Mosa.External.x86.Drawing;
using Mosa.External.x86.Drawing.Fonts;
using Mosa.External.x86.Driver;
using System;

namespace CSOS.GUI.Core
{
    public class App
    {
        public int Index = 0;
        public bool Dock = true;
        public int X;
        public int Y;
        public int Width = 200;
        public int Height = 125;
        private bool Moving = false;
        private int OffsetX;
        private int OffsetY;
        public string Title = "App";
        private const int BarHeight = 20;
        public VirtualGraphics windowGraphics;
        public Bitmap Icon_48 = null;
        public Bitmap Icon_16 = null;
        public bool Visible = true;
        public bool isWindow = true;

        public int X_Dock = -1;
        public int Y_Dock = -1;

        public bool Moveble = true;

        public bool Activated => Index == System.OpenApps.Count - 1;

        public App()
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

            X = (Program.ScreenWidth + Width) / 2;
            Y = (Program.ScreenHeight + Height) / 2;
        }

        public static void MoveWindowToEnd(App window)
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

            if (isWindow)
            {
                int Rad = (BarHeight / 2) - 5;
                if (PS2Mouse.MouseStatus == Mosa.External.x86.Driver.Input.MouseStatus.Left)
                {
                    if (PS2Mouse.X > X && PS2Mouse.X < X + Width && PS2Mouse.Y > Y - BarHeight && PS2Mouse.Y < Y && !Moving && Moveble)
                    {
                        if (PS2Mouse.X > X + Width - (Rad * 3) && Activated && isWindow)
                        {
                            System.OpenApps.Remove(this);
                            if (Dock)
                            {
                                System.ClosedApps.Add(this);
                            }

                            return;
                        }
                        if (PS2Mouse.X > X + Width - (Rad * 6) && Activated && isWindow)
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
                    Y = Math.Clamp(PS2Mouse.Y - OffsetY, BarHeight + TaskBar.TaskBarHeight, Program.ScreenHeight - (Height + (BarHeight - (5 * 2))));
                }

                if (Activated)
                {
                    Program.graphics.DrawFilledRectangle(ThemeManager.GetTheme().FocusedColor, X, Y - BarHeight + 5, Width, BarHeight - 5);
                    Program.graphics.DrawFilledRoundedRectangle(ThemeManager.GetTheme().FocusedColor, X, Y - BarHeight, Width, BarHeight, 5);

                    Program.graphics.DrawFilledRoundedRectangle(ThemeManager.GetTheme().FocusedColor, X, Y + Height - 5, Width, BarHeight - 5, 5);
                }
                else
                {
                    Program.graphics.DrawFilledRectangle(ThemeManager.GetTheme().UnFocusedColor, X, (Y - BarHeight) + 5, Width, BarHeight - 5);
                    Program.graphics.DrawFilledRoundedRectangle(ThemeManager.GetTheme().UnFocusedColor, X, Y - BarHeight, Width, BarHeight, 5);

                    Program.graphics.DrawFilledRoundedRectangle(ThemeManager.GetTheme().UnFocusedColor, X, Y + Height - 5, Width, BarHeight - 5, 5);
                }
                if (Icon_16 == null)
                {
                    Program.graphics.DrawFilledRectangle(ThemeManager.GetTheme().NoAppIcon, X + ((BarHeight / 2) - 8), Y - BarHeight / 2 - (16 / 2), 16, 16);
                }
                else
                {
                    Program.graphics.DrawImage(Icon_16, X + ((BarHeight / 2) - 8), Y - BarHeight / 2 - (16 / 2), true);
                }
                Program.graphics.DrawBitFontString(System.DefaultFontName, ThemeManager.GetTheme().TextColor, Title, X + ((BarHeight / 2) - 8) + 21, Y - BarHeight / 2 - (16 / 2));

                Program.graphics.DrawFilledRectangle(ThemeManager.GetTheme().AppBackground, X, Y, Width, Height);

                Program.graphics.DrawFilledCircle(ThemeManager.GetTheme().WindowCloseButtonColor, X + Width - (Rad * 2), Y - BarHeight / 2, Rad);

                Program.graphics.DrawFilledCircle(ThemeManager.GetTheme().WindowMinimizeButtonColor, X + Width - (Rad * 6), Y - BarHeight / 2, Rad);


            }
            if (Activated || !isWindow)
            {
                InputUpdate();
            }

            UIUpdate();

            if (isWindow)
            {
                Program.graphics.DrawLine(Activated ? ThemeManager.GetTheme().FocusedColor : ThemeManager.GetTheme().UnFocusedColor, X, Y, X, Y + Height);
                Program.graphics.DrawLine(Activated ? ThemeManager.GetTheme().FocusedColor : ThemeManager.GetTheme().UnFocusedColor, X + Width - 1, Y, X + Width - 1, Y + Height);
            }
        }
        public void Close()
        {
            System.OpenApps.Remove(this);
            System.ClosedApps.Add(this);
        }
        public void Hide()
        {
            Visible = false;
        }
        public void Open()
        {
            if (System.ClosedApps.Contains(this))
            {
                System.ClosedApps.Remove(this);
            }

            Index = System.OpenApps.Count;
            System.OpenApps.Add(this);
            Activate();
        }
        public virtual void UIUpdate() { }
        public virtual void InputUpdate() { }
    }
}
