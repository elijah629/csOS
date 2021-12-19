using CSOS.GUI.Core;
using Mosa.External.x86.Drawing.Fonts;
using Mosa.External.x86.Driver;

namespace CSOS.GUI.Apps
{
    public class Dock : Process
    {
        public const int Radius = 8;
        public const int ApplicationIconSize = 48;
        public const int Devide = 10;
        private int X = 0;
        private readonly int Y = 0;
        private readonly int Height = 0;
        private int Width = 0;
        public Dock()
        {
            Title = "Dock";
            Height = ApplicationIconSize + (Devide * 2);
            Y = Program.ScreenHeight - Height - Devide * 2;
        }

        public bool PressLock = false;
        public override void Update()
        {
            Width = Devide + Core.System.OpenApps.Count * (ApplicationIconSize + Devide) + Core.System.ClosedApps.Count * (ApplicationIconSize + Devide);

            X = (Program.ScreenWidth / 2) - (Width / 2);

            Program.graphics.DrawFilledRoundedRectangle(ThemeManager.CurrentTheme.DockBackgroundColor, X, Y, Width, Height, Radius);
            int aX = X + Devide;
            for (int i = 0; i < Core.System.OpenApps.Count; i++)
            {

                if (Core.System.OpenApps[i].Icon_48 == null)
                {
                    Program.graphics.DrawFilledRoundedRectangle(ThemeManager.CurrentTheme.NoDockIcon, aX, Y + Devide, ApplicationIconSize, ApplicationIconSize, Radius);
                }
                else
                {
                    Program.graphics.DrawImage(Core.System.OpenApps[i].Icon_48, aX, Y + Devide, true);
                }

                if (Core.System.OpenApps[i].Activated && Core.System.OpenApps[i].Visible)
                {
                    Program.graphics.DrawFilledCircle(0x23afa1, aX + (ApplicationIconSize / 2), Y + Devide + ApplicationIconSize + (Devide / 4), 2);
                }
                else
                {
                    Program.graphics.DrawFilledCircle(0xFFF0F3F4, aX + (ApplicationIconSize / 2), Y + Devide + ApplicationIconSize + (Devide / 4), 2);
                }

                Core.System.OpenApps[i].X_Dock = aX;

                aX += ApplicationIconSize + Devide;
                int TipWidth = (ApplicationIconSize * 2) + BitFont.Calculate(Core.System.DefaultFontName, Core.System.OpenApps[i].Title);

                if (PS2Mouse.X > Core.System.OpenApps[i].X_Dock && PS2Mouse.X < Core.System.OpenApps[i].X_Dock + ApplicationIconSize && PS2Mouse.Y > Y + Devide && PS2Mouse.Y < Y + Devide + ApplicationIconSize)
                {
                    int TX = Core.System.OpenApps[i].X_Dock - ((TipWidth / 2) - (ApplicationIconSize / 2));
                    int TY = Y - (int)((ApplicationIconSize + Devide) * 0.8f);
                    int TL = BitFont.Calculate(Core.System.DefaultFontName, Core.System.OpenApps[i].Title);
                    Program.graphics.DrawFilledRoundedRectangle(ThemeManager.CurrentTheme.FocusedColor, TX, TY, TipWidth, 8 + Devide * 2, Radius);
                    Program.graphics.DrawBitFontString(Core.System.DefaultFontName, ThemeManager.CurrentTheme.TextColor, Core.System.OpenApps[i].Title, TX + ((TipWidth / 2) - (TL / 2)) - 8, TY + (((8 + Devide * 2) / 2) - 8));
                    if (PS2Mouse.MouseStatus == Mosa.External.x86.Driver.Input.MouseStatus.Left && !PressLock)
                    {

                        if (!Core.System.OpenApps[i].Activated || !Core.System.OpenApps[i].Visible)
                        {
                            Core.System.OpenApps[i].Activate();
                            Core.System.OpenApps[i].Visible = true;
                        }
                        else if (Core.System.OpenApps[i].Visible)
                        {
                            Core.System.OpenApps[i].Visible = false;
                        }
                        PressLock = true;
                    }
                }
                else if (PS2Mouse.MouseStatus == Mosa.External.x86.Driver.Input.MouseStatus.None)
                {
                    PressLock = false;
                }
            }
            for (int i = 0; i < Core.System.ClosedApps.Count; i++)
            {

                if (Core.System.ClosedApps[i].Icon_48 == null)
                {
                    Program.graphics.DrawFilledRoundedRectangle(ThemeManager.CurrentTheme.NoDockIcon, aX, Y + Devide, ApplicationIconSize, ApplicationIconSize, Radius);
                }
                else
                {
                    Program.graphics.DrawImage(Core.System.ClosedApps[i].Icon_48, aX, Y + Devide, true);
                }

                Core.System.ClosedApps[i].X_Dock = aX;

                aX += ApplicationIconSize + Devide;
                int TipWidth = (ApplicationIconSize * 2) + BitFont.Calculate(Core.System.DefaultFontName, Core.System.ClosedApps[i].Title);

                if (PS2Mouse.X > Core.System.ClosedApps[i].X_Dock && PS2Mouse.X < Core.System.ClosedApps[i].X_Dock + ApplicationIconSize && PS2Mouse.Y > Y + Devide && PS2Mouse.Y < Y + Devide + ApplicationIconSize)
                {
                    int TX = Core.System.ClosedApps[i].X_Dock - ((TipWidth / 2) - (ApplicationIconSize / 2));
                    int TY = Y - (int)((ApplicationIconSize + Devide) * 0.8f);
                    int TL = BitFont.Calculate(Core.System.DefaultFontName, Core.System.ClosedApps[i].Title);
                    Program.graphics.DrawFilledRoundedRectangle(ThemeManager.CurrentTheme.FocusedColor, TX, TY, TipWidth, 8 + Devide * 2, Radius);
                    Program.graphics.DrawBitFontString(Core.System.DefaultFontName, ThemeManager.CurrentTheme.TextColor, Core.System.ClosedApps[i].Title, TX + ((TipWidth / 2) - (TL / 2)) - 8, TY + (((8 + Devide * 2) / 2) - 8));
                    if (PS2Mouse.MouseStatus == Mosa.External.x86.Driver.Input.MouseStatus.Left && !PressLock)
                    {
                        Core.System.ClosedApps[i].Open();
                        PressLock = true;
                    }
                }
                else if (PS2Mouse.MouseStatus == Mosa.External.x86.Driver.Input.MouseStatus.None)
                {
                    PressLock = false;
                }
            }
        }
    }
}