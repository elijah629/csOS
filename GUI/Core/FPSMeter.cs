using Mosa.Kernel.x86;

namespace csOS.GUI.Core
{
    internal static class FPSMeter
    {
        public static int FPS = 0;

        public static int LastS = -1;
        public static int Ticken = 0;

        public static void Update()
        {
            if (LastS == -1)
            {
                LastS = CMOS.Second;
            }
            if (CMOS.Second - LastS != 0)
            {
                if (CMOS.Second > LastS)
                {
                    FPS = Ticken / (CMOS.Second - LastS);
                }
                LastS = CMOS.Second;
                Ticken = 0;
            }
            Ticken++;
        }
    }
}
