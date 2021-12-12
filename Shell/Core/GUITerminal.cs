using Mosa.External.x86.Drawing;
using Mosa.External.x86.Drawing.Fonts;
using Mosa.External.x86.Driver;
using Mosa.Kernel.x86;
using Mosa.Runtime.x86;

namespace CSOS.Shell.Core
{
    public static unsafe class GUITerminal
    {
        public static int X { private set; get; }
        public static int Y { private set; get; }
        public static uint Background { private set; get; }
        public static readonly int fontWidth = 8;
        public static readonly int fontHeight = 16;
        public static uint Defaultcolour;


        public static Graphics graphics { private set; get; }

        public static void Init(Graphics graphics, uint background)
        {
            GUITerminal.graphics = graphics;
            Background = background;
        }
        public static void SetBackgroundColour(uint colour)
        {
            Background = colour;
        }
        public static void SetCursorPosition(int x, int y)
        {
            X = x * fontWidth;
            Y = y * fontHeight;
        }
        public static void Next()
        {
            X += fontWidth;

            if (X >= VBE.VBEModeInfo->ScreenWidth)
            {
                X = 0;
                Y += fontHeight;
            }
        }
        public static void Previous()
        {
            if (X == 0 && Y == 0)
            {
                return;
            }


            if (X != 0)
            {
                X -= fontWidth;
            }
            else
            {
                Y -= fontHeight;
                X = VBE.VBEModeInfo->ScreenWidth - fontWidth;
            }
        }
        public static void NewLine()
        {
            X = 0;
            Y += fontHeight;
        }


        public static void Write(char c, uint colour)
        {
            graphics.DrawACS16String(colour, c + "", X, Y);
            Next();
            graphics.Update();
        }
        public static void WriteLine(char c, uint colour)
        {
            Write(c, colour);
            NewLine();
        }
        public static void Write(string s, uint colour)
        {
            for (int i = 0; i < s.Length; i++)
            {
                Write(s[i], colour);
            }
        }
        public static void WriteLine(string s, uint colour)
        {
            Write(s, colour);
            NewLine();
        }

        public static void Write(char c)
        {
            Write(c, Defaultcolour);
        }
        public static void WriteLine(char c)
        {
            WriteLine(c, Defaultcolour);
        }
        public static void Write(string s)
        {
            Write(s, Defaultcolour);
        }
        public static void WriteLine(string s)
        {
            WriteLine(s, Defaultcolour);
        }

        public static string ReadLine()
        {
            string Line = "";
            KeyCode code;
            for (; ; )
            {
                code = ReadKey();

                if (code == KeyCode.Enter)
                {
                    break;
                }
                else if (code == KeyCode.Delete)
                {
                    if (Line.Length != 0)
                    {
                        Line = Line.Substring(0, Line.Length - 1);
                        graphics.DrawFilledRectangle(Background, X - 8, Y, fontWidth * 2, fontHeight);
                        if (X != 0)
                        {
                            SetCursorPosition((X / fontWidth) - 1, (Y / fontHeight));
                        }
                        else
                        {
                            SetCursorPosition(Program.ScreenWidth - 3, (Y / fontHeight) - 1);
                        }

                        graphics.Update();
                    }
                }
                else if (code != 0)
                {
                    Line += code.KeyCodeToString();
                    if (X == Program.ScreenWidth - fontWidth * 2)
                    {
                        Program.graphics.DrawFilledRectangle(Background, X, Y, fontWidth, fontHeight);
                        NewLine();
                    }
                    Program.graphics.DrawFilledRectangle(Background, X, Y, fontWidth, fontHeight);
                    Write(code.KeyCodeToString() + '_');
                    SetCursorPosition((X / fontWidth) - 1, (Y / fontHeight));
                }
            }
            graphics.DrawFilledRectangle(Background, X, Y, fontWidth, fontHeight);
            NewLine();
            return Line;
        }
        public static KeyCode ReadKey(bool WaitForKey = true)
        {
            if (WaitForKey)
            {
                KeyCode code;
                for (; ; )
                {
                    Native.Hlt();
                    code = PS2Keyboard.GetKeyPressed();
                    if (code != 0)
                    {
                        break;
                    }
                }
                return code;
            }
            else
            {
                return PS2Keyboard.GetKeyPressed();
            }
        }

        public static void ToTop()
        {
            X = 0;
            Y = 0;
        }

        public static void Clear()
        {
            graphics.Clear(Background);
            ToTop();
        }
    }
}