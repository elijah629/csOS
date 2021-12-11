using Mosa.External.x86.Drawing;
using Mosa.External.x86.Drawing.Fonts;
using Mosa.External.x86.Driver;

namespace CSOS.GUI.UI
{
    public class Button
    {
        public uint BackgroundColor = 0xFFFFFF;
        public uint OverBackgroundColor = 0x808080;
        public uint ClickedBackgroundColor = 0x36454F;
        public uint TextColor = 0x0;
        public uint LineColor = 0x0;
        private uint CurrentColor;
        public int X = 0;
        public int Y = 0;
        public int Width = "Button".Length * 8 + 10;
        public int Height = 20;
        public string Text = "Button";
        public string Font = "asc16";
        public VirtualGraphics vgraphics;
        public Graphics graphics;

        private bool PressLock = false;

        public Button()
        {
            CurrentColor = BackgroundColor;
        }

        public void Draw(VirtualGraphics graphics)
        {
            graphics.DrawFilledRoundedRectangle(CurrentColor, X, Y, Width, Height, 5);
            if (Font.ToLower() == "asc16")
            {
                graphics.DrawACS16String(TextColor, Text, X + 5, Y + 5);
            }
            else
            {
                graphics.DrawBitFontString(Font, TextColor, Text, X + 5, Y + 5);
            }
        }

        public void Draw(Graphics graphics)
        {
            graphics.DrawFilledRoundedRectangle(CurrentColor, X, Y, Width, Height, 5);
            if (Font.ToLower() == "asc16")
            {
                graphics.DrawACS16String(TextColor, Text, X + 5, Y + 5);
            }
            else
            {
                graphics.DrawBitFontString(Font, TextColor, Text, X + 5, Y + 5);
            }
        }

        public void Update(int scrX, int scrY)
        {
            Draw(vgraphics);
            if (PS2Mouse.X > X + scrX && PS2Mouse.X < X + scrX + Width && PS2Mouse.Y > Y + scrY && PS2Mouse.Y < Y + scrY + Height)
            {
                CurrentColor = OverBackgroundColor;
            }
            if (PS2Mouse.MouseStatus == Mosa.External.x86.Driver.Input.MouseStatus.Left)
            {
                if (!PressLock)
                {
                    if (PS2Mouse.X > X + scrX && PS2Mouse.X < X + scrX + Width && PS2Mouse.Y > Y + scrY && PS2Mouse.Y < Y + scrY + Height)
                    {
                        CurrentColor = ClickedBackgroundColor;
                        Pressed();
                        PressLock = true;
                    }
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
            Draw(graphics);
            if (PS2Mouse.X > X && PS2Mouse.X < X + Width && PS2Mouse.Y > Y && PS2Mouse.Y < Y + Height)
            {
                CurrentColor = OverBackgroundColor;
            }
            else if (PS2Mouse.MouseStatus == Mosa.External.x86.Driver.Input.MouseStatus.Left)
            {
                if (!PressLock)
                {
                    if (PS2Mouse.X > X && PS2Mouse.X < X + Width && PS2Mouse.Y > Y && PS2Mouse.Y < Y + Height)
                    {
                        CurrentColor = ClickedBackgroundColor;
                        Pressed();
                        PressLock = true;
                    }
                }
            }
            else
            {
                PressLock = false;
                CurrentColor = BackgroundColor;
            }
        }

        public virtual void Pressed() { }
    }
}