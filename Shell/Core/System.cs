using CSOS.Shell.Commands;
using Tools = CSOS.Helper.Tools;
using System;

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
/*                case "ls":
                    //FAT32 fs = Program.fs;
                    //Encoding.ASCII.GetString();
                    //GetBytes
                    break;*/
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