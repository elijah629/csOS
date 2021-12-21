using csOS.Helper;
using csOS.Shell.Commands;
using System.Collections.Generic;
using Tools = csOS.Helper.Tools;

namespace csOS.Shell.Core
{
    public static class System
    {
        public static void Boot()
        {
            GUITerminal.Init(Program.graphics, 0x0);
            GUITerminal.Defaultcolour = 0xFFFFFF;
            GUITerminal.SetCursorPosition(0, 0);
            GUITerminal.WriteLine(@"            ____   _____ ");
            GUITerminal.WriteLine(@"           / __ \ / ____|");
            GUITerminal.WriteLine(@"   ___ ___| |  | | (___  ");
            GUITerminal.WriteLine(@"  / __/ __| |  | |\___ \ ");
            GUITerminal.WriteLine(@" | (__\__ \ |__| |____) |");
            GUITerminal.WriteLine(@"  \___|___/\____/|_____/ ");
            GUITerminal.WriteLine("Welcome to csOS!", 0x00FF00);
            GUITerminal.WriteLine("The current time is " + Tools.GetTime());
        }
        /*public static byte[] GetBytes(string value)
        {
            byte[] data = new byte[value.Length];

            for (int i = 0; i < value.Length; i++)
            {
                data[i] = BitConverter.GetBytes(value[i])[0];
            }
            return data;
        }*/
        public static void Update()
        {
            GUITerminal.Write("> ");
            CommandParser.CommandParserOutput command = CommandParser.Parse(GUITerminal.ReadLine());
            switch (command.Command.ToLower())
            {
                case "echo":
                    if (command.Arguments.Count > 0)
                    {
                        string estr = "";
                        foreach (string arg in command.Arguments)
                        {
                            estr += arg + (command.Arguments.IndexOf(arg) != command.Arguments.Count - 1 ? " " : "");
                        }

                        GUITerminal.WriteLine(estr);
                    }
                    break;
                case "stack":
                    Stack<int> myStack = new Stack<int>(new int[]
                    {
                        1,2,3,4,5,6
                    });
                    // Displays the Stack.
                    GUITerminal.WriteLine("Stack values:");
                    GUITerminal.WriteLine(myStack.Pop().ToString());
                    GUITerminal.WriteLine(myStack.Pop().ToString());
                    GUITerminal.WriteLine(myStack.Peek().ToString());
                    myStack.Push(999);
                    GUITerminal.WriteLine(myStack.Peek().ToString());
                    break;
                case "ini":
                    /*INI.INIFile parsed = INI.Parse(@"[I]
n=i");
                    GUITerminal.WriteLine(parsed.Sections["I"].Name);*/
                    break;
                case "clear":
                    GUITerminal.Clear();
                    break;
                default:
                    GUITerminal.WriteLine("[csOS] Invalid Command: " + command.Command, 0xFF0000);
                    break;
            }
        }
    }
}