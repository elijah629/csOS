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
        CpuInfo cpuinfo;
        
        public Terminal()
        {
            Icon_16 = new Bitmap(ResourceManager.GetObject("terminal_16.bmp"));
            Icon_48 = new Bitmap(ResourceManager.GetObject("terminal_48.bmp"));
            X = 50;
            isWindow = true;
            Height = 500;

            Title="Terminal";

            Y = 50;

            Width = 600;

            if (windowGraphics == null)
            {
                windowGraphics = new VirtualGraphics(Width, Height);
            }
            xterm = new Terminal(System.graphics, windowGraphics, Color.White.ToArgb(), Color.Black.ToArgb(), Width, Height, 0, 0, X, Y);
            xterm.Write(">");
            cpuinfo = new CpuInfo();
        }
        string Line="";

        KeyCode keycode;
        public override void InputUpdate()
        {
            xterm.winX=X; xterm.winY=Y;
            //xterm.Write("> ");
            
            keycode=Console.ReadKey(false);
            if (keycode == KeyCode.Delete)
            {
                if (Line.Length != 0)
                {
                    xterm.Previous();
                    windowGraphics.DrawFilledRectangle(xterm.BackgroundColor, xterm.x, xterm.y, xterm.fontWidth, xterm.fontHeight);
                    Line = Line.Substring(0, Line.Length - 1);
                    windowGraphics.Update();
                }
            }
            else if (keycode == KeyCode.Enter)
            {
                xterm.WriteLine("");
                processInput(Line.ToLower());
                Line="";
                xterm.Write(">");
            }
            else if (keycode != 0)
            {
                if (PS2Keyboard.IsCapsLock || PS2Keyboard.IsShiftHeld)
                {
                    Line += keycode.KeyCodeToString().ToUpper();
                    xterm.Write(keycode.KeyCodeToString().ToUpper());
                }
                else
                {
                    Line += keycode.KeyCodeToString().ToLower();
                    xterm.Write(keycode.KeyCodeToString().ToLower());
                }
                
            }
        }
        
        private void processInput(string inp) {
            switch (inp) {
                case "help":
                    xterm.WriteLine("poweroff - Shutdown the system");
                    xterm.WriteLine("reboot - Reboots the system");
                    xterm.WriteLine("about - shows a message about the system");
                    xterm.WriteLine("neofetch - display a message with a logo");
                    break;
                case "poweroff":
                    Power.Shutdown();
                    break;
                case "reboot":
                    Power.Reboot();
                    break;
                case "about":
                    xterm.WriteLine("CSOS Is an OS written only in C#, with MOSA (Managed Open System Alliance)");
                    xterm.WriteLine("Credits: Elijah629 on github, asterd-og on github");
                    break;
                case "neofetch":
                    xterm.WriteLine(@"            ____   _____          Information about the system: ");
                    xterm.WriteLine($@"           / __ \ / ____|        Cpu Family: {cpuinfo.Family.ToString()}");
                    xterm.WriteLine($@"   ___ ___| |  | | (___          Cpu Model: {cpuinfo.Model.ToString()}");
                    xterm.WriteLine($@"  / __/ __| |  | |\___ \         Cpu Number of cores: {cpuinfo.NumberOfCores.ToString()}");
                    xterm.WriteLine($@" | (__\__ \ |__| |____) |        Cpu Stepping: {cpuinfo.Stepping.ToString()}");
                    xterm.WriteLine($@"  \___|___/\____/|_____/         Cpu Supports: Brand String - {cpuinfo.SupportsBrandString.ToString()} | Extended CpuID - {cpuinfo.SupportsExtendedCpuid.ToString()}");
                    break;
            }

        }
        
        public override void UIUpdate()
        {
            Program.graphics.DrawImageASM(windowGraphics.bitmap, X, Y);
        }
    }
}
