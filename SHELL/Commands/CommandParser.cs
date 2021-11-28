using Mosa.External.x86;
using System.Collections.Generic;

namespace CSOS.Shell.Commands
{
    public class CommandParserOutput
    {
        public List<string> Arguments = new List<string>();
        public List<char> Flags = new List<char>();
        public string Command;
    }
    public class CommandParser
    {
        public CommandParserOutput Parse(string commandLine)
        {
            List<string> Arguments = new List<string>();
            List<char> Flags = new List<char>();
            string[] sections = commandLine.Split(' ');
            string Command = sections[0];
            for (int i = 1; i < sections.Length; i++)
            {
                if (sections[i].StartsWith("-"))
                {
                    foreach (var flag in sections[i].ToCharArray())
                    {
                        if (flag != '-') Flags.Add(flag);
                    }
                }
                else
                {
                    Arguments.Add(sections[i]);
                }
            }
            return new CommandParserOutput()
            {
                Arguments = Arguments,
                Flags = Flags,
                Command = Command
            };
        }
    }
}