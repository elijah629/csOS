using CSOS.GUI.Core;
using Mosa.External.x86;
using Mosa.External.x86.Drawing;
using Mosa.External.x86.Drawing.Fonts;
using Mosa.External.x86.Driver;
using Mosa.Kernel.x86;
using Mosa.Runtime.x86;
using System.Drawing;

class TerminalCore
{
    private int x = 0;
    private int y = 0;
    public uint ForegroundColor;
    public uint BackgroundColor;
    public readonly int fontWidth = 8;
    public readonly int fontHeight = 16;
    private int scrW;
    private int scrH;

    public VirtualGraphics windowGraphics;
    public bool CursorVisible = true;

    public TerminalCore(VirtualGraphics windowGraphics, uint ForegroundColor, uint BackgroundColor)
    {
        this.windowGraphics = windowGraphics;
        this.ForegroundColor = ForegroundColor;
        this.BackgroundColor = BackgroundColor;
        scrW = windowGraphics.Width;
        scrH = windowGraphics.Height;
    }

    public void SetCursorPosition(int cx, int cy)
    {
        x = cx * fontWidth;
        y = cy * fontHeight;
    }

    public void Next()
    {
        x += fontWidth;

        if (x >= scrW)
        {
            x = 0;
            y += fontHeight;
        }
        else if (y + fontHeight > scrH)
        {
            Clear();
        }
        windowGraphics.Update();
    }

    public void Previous()
    {
        if (x == 0 && y == 0)
        {
            return;
        }


        if (x != 0)
        {
            x -= fontWidth;
        }
        else
        {
            y -= fontHeight;
            x = scrH - fontWidth;
        }
    }

    public void NewLine()
    {
        x = 0;
        y += fontHeight;
    }

    public void Write(char c, uint colour)
    {
        windowGraphics.DrawACS16String(colour, c + "", x, y);
        Next();
        windowGraphics.Update();
    }
    public void WriteLine(char c, uint colour)
    {
        Write(c, colour);
        NewLine();
    }
    public void Write(string s, uint colour)
    {
        for (int i = 0; i < s.Length; i++)
        {
            Write(s[i], colour);
        }
    }
    public void WriteLine(string s, uint colour)
    {
        Write(s, colour);
        NewLine();
    }

    public void Write(char c)
    {
        Write(c, ForegroundColor);
    }
    public void WriteLine(char c)
    {
        WriteLine(c, ForegroundColor);
    }
    public void Write(string s)
    {
        Write(s, ForegroundColor);
    }
    public void WriteLine(string s)
    {
        WriteLine(s, ForegroundColor);
    }

    /*public string ReadLine()
    {
        string S = "";
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
                    Previous();
                    windowGraphics.DrawFilledRectangle(BackgroundColor, x, y, fontWidth, fontHeight);
                    Line = Line.Substring(0, Line.Length - 1);
                    windowGraphics.Update();
                }
            }
            else
            {
                if (PS2Keyboard.IsCapsLock)
                {
                    S = PS2Keyboard.KeyCodeToString(code);
                    Line += S.ToUpper();
                    Write(S.ToUpper());
                }
                else
                {
                    S = PS2Keyboard.KeyCodeToString(code).ToLower();
                    Line += S.ToUpper().ToLower();
                    Write(S.ToUpper().ToLower());
                }
                if (CursorVisible)
                {
                    Write("_");
                }
            }
        }
        NewLine();
        return Line;
    }*/
    public KeyCode ReadKey(bool WaitForKey = true)
    {
        if (WaitForKey)
        {
            KeyCode code;
            for (; ; )
            {
                Native.Hlt();
                code = Console.ReadKey();
                if (code != 0) break;
            }
            return code;
        }
        else
        {
            return Console.ReadKey();
        }
    }

    public void ToTop()
    {
        x = 0;
        y = 0;
    }

    public void Clear()
    {
        windowGraphics.Clear(BackgroundColor);
        ToTop();
    }
}
namespace CSOS.GUI.Apps
{
    public class Terminal : App
    {
        private TerminalCore terminal;
        public Terminal()
        {
            Width = 300;
            Title = "Terminal";
            Icon_16 = new Bitmap(ResourceManager.GetObject("terminal_16.bmp"));
            Icon_48 = new Bitmap(ResourceManager.GetObject("terminal_48.bmp"));
            if (windowGraphics == null) windowGraphics = new VirtualGraphics(Width, Height);
            terminal = new TerminalCore(windowGraphics, Color.White.ToArgb(), Color.Black.ToArgb());
            terminal.SetCursorPosition(10, 10);
            terminal.WriteLine("Hello!, I am in development");
        }
        public override void InputUpdate()
        {
            
        }
        public override void UIUpdate()
        {
            Program.graphics.DrawImageASM(windowGraphics.bitmap, X, Y);
        }
    }
}