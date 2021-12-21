using csOS.GUI.Apps;
using Mosa.External.x86;
using Mosa.External.x86.Drawing.Fonts;
using Mosa.External.x86.Driver;
using System.Collections.Generic;

namespace csOS.GUI.Core
{
    public static class System
    {
        public const string DefaultFontName = "LucidaConsoleCustomCharset16";
        public static int[] cursor;
        public static List<Window> OpenApps = new List<Window>();
        public static List<Window> ClosedApps = new List<Window>();
        public static List<Process> OpenProcs = new List<Process>();
        public static unsafe void Boot()
        {
            BitFont.RegisterBitFont(new BitFontDescriptor(DefaultFontName, "01234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ:;'\"[]{}\\|,<.>/?!@#$%^&*()_+-=", ResourceManager.GetObject("LucidaConsoleCustomCharset16.bin"), 16));
            
            CursorManager.Init();
            PS2Mouse.Initialize(Program.ScreenWidth, Program.ScreenHeight);
            CursorManager.SetCursor("normal");
            ThemeManager.SetTheme(new ThemeManager.Theme()
            {
                TextColor = 0xFFF0F3F4,
                BackgroundColor = 0x313131,
                DockBackgroundColor = 0x141414,
                CursorFillColor = 0xFFFFFF,
                CursorOutlineColor = 0x0,
                WindowCloseButtonColor = 0xFF0000,
                WindowMinimizeButtonColor = 0xFFFFFF,
                FocusedColor = 0x141414,
                DefaultAppBackground = 0xFFF0F3F4,
                NoAppIcon = 0x0000FF,
                NoDockIcon = 0x0000FF,
                HoverColor = 0x414141,
                UnFocusedColor = 0x414141,
            });
            new TaskBar().Start();
            new Dock().Start();
            new TaskManager().Close();
        }
        public static void Update()
        {
            Program.graphics.Clear(ThemeManager.CurrentTheme.BackgroundColor);
            Program.graphics.DrawBitFontString(DefaultFontName, ThemeManager.CurrentTheme.TextColor, "FPS: " + FPSMeter.FPS, 10, 46);
            for (int i = 0; i < OpenApps.Count; i++)
            {
                OpenApps[i].Update();
            }
            for (int i = 0; i < OpenProcs.Count; i++)
            {
                OpenProcs[i].Update();
            }
            DrawCursor(PS2Mouse.X, PS2Mouse.Y);
            Program.graphics.Update();
            FPSMeter.Update();
        }
        public static void DrawCursor(int x, int y)
        {
            for (int h = 0; h < 21; h++)
            {
                for (int w = 0; w < 12; w++)
                {
                    if (cursor[h * 12 + w] == 1)
                    {
                        Program.graphics.DrawPoint(ThemeManager.CurrentTheme.CursorOutlineColor, w + x, h + y);
                    }

                    if (cursor[h * 12 + w] == 2)
                    {
                        Program.graphics.DrawPoint(ThemeManager.CurrentTheme.CursorFillColor, w + x, h + y);
                    }
                }
            }
        }
    }
}