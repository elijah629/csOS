using Mosa.External.x86;
using Mosa.Kernel.x86;
using System;

namespace CSOS.Helper
{
    public static class Tools
    {
        public static Func<Func<object>, object> Lambanda = (x) => x.Invoke();
        public static string GetTime(/*bool military = true*/) => /*military ? */$"{CMOS.Hour.ToString().PadLeft(2, '0')}:{CMOS.Minute.ToString().PadLeft(2, '0')}"/* : $"{(CMOS.Hour > 12 && CMOS.Minute > 0 ? (CMOS.Hour - 12).ToString().PadLeft(2, '0') : CMOS.Hour.ToString().PadLeft(2, '0'))}:{CMOS.Minute.ToString().PadLeft(2, '0')} {(CMOS.Hour > 12 && CMOS.Minute > 0 ? "PM" : "AM")}"*/;
    }
}