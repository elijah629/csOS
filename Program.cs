using CSOS.Helper;
using Mosa.External.x86;
using Mosa.External.x86.Drawing;
using Mosa.External.x86.Drawing.Fonts;
using Mosa.External.x86.Driver;
//using Mosa.External.x86.FileSystem
using Mosa.Kernel.x86;
using Mosa.Runtime.Plug;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;

namespace CSOS
{
    public static unsafe class Program
    {
        public static int ScreenWidth = 1024;
        public static int ScreenHeight = 768;
        private static bool Panicked = false;
        public static Graphics graphics;
        //public static FAT32 fs;
        public static void Main() { }

        [VBERequire(1024, 768, 32)]
        [Resource(
            "terminal_16.bmp",
            "terminal_48.bmp"
        )]
        [Plug("Mosa.Runtime.StartUp::KMain")]
        [UnmanagedCallersOnly(EntryPoint = "KMain", CallingConvention = CallingConvention.StdCall)]
        public static void KMain()
        {
            IDT.OnInterrupt += IDT_OnInterrupt;
            Panic.OnPanic += Panic_OnPanic;

            /*IDisk disk = new IDEDisk(0);
            MBR mBR = new MBR();
            mBR.Initialize(disk);
            fs = new FAT32(disk, mBR.PartitionInfos[0]);*/

            if (VBE.IsVBEAvailable)
            {
                ScreenWidth = VBE.VBEModeInfo->ScreenWidth;
                ScreenHeight = VBE.VBEModeInfo->ScreenHeight;
            }
            graphics = GraphicsSelector.GetGraphics(ScreenWidth, ScreenHeight);
            new Thread(SelectBoot).Start();
        }
        public static void SelectBoot()
        {
            VBEConsole.setup(graphics, 0x0);
            Menu menu = new Menu("Select OS mode", new string[] { "GUI", "SHELL" }, (ScreenWidth / 8) / 2, ScreenHeight / 16 / 2);
            int mode = menu.Start();
            menu.Stop();
            graphics.Clear(0x0);
            graphics.Update();
            if (mode == 0)
            {
                BootGUI();
            }
            else
            {
                BootShell();
            }
        }
        public static void BootGUI()
        {
            GUI.Core.System.Boot();
            new Thread(() =>
            {
                for (; ; )
                {
                    if (!Panicked)
                    {
                        GUI.Core.System.Update();
                    }
                }
            }).Start();
        }
        private static void BootShell()
        {
            Shell.Core.System.Boot();
            new Thread(() =>
            {
                for (; ; )
                {
                    if (!Panicked)
                    {
                        Shell.Core.System.Update();
                    }
                }
            }).Start();
        }
        private static void IDT_OnInterrupt(uint irq, uint error)
        {
            switch (irq)
            {
                case 0x2C:
                    PS2Mouse.OnInterrupt();
                    break;
                case 0x21:
                    PS2Keyboard.OnInterrupt();
                    break;
            }
        }
        public static void Panic_OnPanic(string msg)
        {
            Panicked = true;
            for (; ; )
            {
                graphics.Clear(0x0000FF);
                graphics.DrawACS16String(Color.White.ToArgb(), "Kernel Panic!", 10, 10);
                graphics.DrawACS16String(Color.White.ToArgb(), msg, 10, 30);
                graphics.Update();
            }
        }
    }
}
