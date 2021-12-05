using Mosa.External.x86;
using Mosa.External.x86.Drawing;
using Mosa.External.x86.Drawing.Fonts;
using Mosa.External.x86.Driver;
using Mosa.Kernel.x86;
using Mosa.Runtime.x86;
using CSOS.Shell.Commands;
using System;
using System.Collections.Generic;

namespace CSOS.Shell.Core
{
    
    public class System {
        public System()
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
            GUITerminal.WriteLine("Welcome to CSOS, This message is shown each boot.",0x00FF00);
        }
        public string command = "";

        public void Update()
        {
            GUITerminal.Write("> ");
            command = GUITerminal.ReadLine();
            GUITerminal.WriteLine("[csOS] This OS is not ready!",0xFF0000);
        }
    }
}