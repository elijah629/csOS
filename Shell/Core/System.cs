using CSOS.Shell.Commands;
using System.Collections.Generic;
using static CSOS.Shell.Commands.CommandParser;
using Tools = CSOS.Helper.Tools;

namespace CSOS.Shell.Core
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
            //ErrorManager.Init();
        }

        //public static string GetCMDError(string cmd, string error) => $"[{cmd}] {ErrorManager.GetError(error)}";

        public static void Update()
        {
            GUITerminal.Write("> ");
            CommandParserOutput command = CommandParser.Parse(GUITerminal.ReadLine());
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
                    else
                    {
                        //GUITerminal.WriteLine(ErrorManager.errors[0], 0xFF0000);
                    }
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