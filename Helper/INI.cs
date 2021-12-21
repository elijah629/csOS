using Mosa.External.x86;
using System;
using System.Collections.Generic;

namespace csOS.Helper
{
    public static class INI
    {
        private static readonly Func<int, string, string, string, Exception> Error = (a, b, c, d) => new Exception($"Exception at line {a}: {b}, Expected '{c}'. Got '{d}'.");
        public static INIFile Parse(string code)
        {
            INIFile file = new INIFile();
            string currentSection = "";
            string[] lines = code.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i].Trim();
                if (line == "" || line.StartsWith(";")) continue;
                else if (line.StartsWith("["))
                {
                    if (line.Contains("]"))
                    {
                        string sectionName = line.Substring(1, line.IndexOf(']') - 1).Trim();
                        if (sectionName != "")
                        {
                            file.Sections.Add(sectionName, new INISection()
                            {
                                Name = sectionName,
                                Properties = new Dictionary<string, string>()
                            });
                            currentSection = sectionName;
                        }
                        else
                        {
                            throw Error(i, "Section has no name", "[{name}]", "[]");
                        }
                    }
                    else
                    {
                        throw Error(i, "Section defenition doesn't end", "[{name}]", "[...");
                    }
                }
                else if (line.Contains("="))
                {
                    if (currentSection != "")
                    {
                        string[] prop = line.Split('=');
                        for (int i2 = 0; i2 < prop.Length; i2++) prop[i2] = prop[i2].Trim();
                        if (prop.Length < 2)
                        {
                            throw Error(i, "Stray '=' in property defenition", "{key}={value}", "{key}=, =, ={value}");
                        }
                        else if (prop.Length == 2)
                        {
                            file.Sections[currentSection].Properties.Add(prop[0], prop[1]);
                        }
                        else
                        {
                            throw Error(i, "Unexpected char '='", "{key}={value}", line);
                        }
                    }
                    else
                    {
                        throw Error(i, "Property defenition is outside of a section defenition", "[section]\n{key}={value}", line);
                    }
                }
                else
                {
                    throw Error(i, "Syntax error", "Property defenition, Section defenition, or EOF", line);
                }

            }
            return file;
        }
        public class INIFile
        {
            public Dictionary<string, INISection> Sections = new Dictionary<string, INISection>();
        }
        public struct INISection
        {
            public string Name;
            public Dictionary<string, string> Properties;
        }
    }
}