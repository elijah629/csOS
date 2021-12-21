using Mosa.External.x86;
using System;
using System.Collections.Generic;
namespace csOS.Shell.Commands
{
    public static class CommandParser
    {
        public struct CommandParserOutput {
            public List<string> Arguments;
            public List<char> Flags;
            public string Command;
        };
        public static CommandParserOutput Parse(string commandLine)
        {
            List<string> Arguments = new List<string>();
            List<char> Flags = new List<char>();
            string[] sections = commandLine.Split(' ');
            string Command = sections[0];
            for (int i = 1; i < sections.Length; i++)
                if (sections[i].StartsWith("-"))
                {
                    foreach (var flag in sections[i].ToCharArray())
                        if (flag != '-') Flags.Add(flag);
                }
                else Arguments.Add(sections[i]);
            return new CommandParserOutput()
            {
                Arguments = Arguments,
                Flags = Flags,
                Command = Command
            };
        }
    }
}