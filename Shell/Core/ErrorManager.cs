using System.Collections.Generic;

namespace CSOS.Shell.Core
{
    public static class ErrorManager
    {
        public enum Error : uint
        {
            ERROR_SUCCESS = 0x0,
            ERROR_INVALID_ARGUMENTS = 0x1,
        }

        private static readonly Dictionary<uint, string> errors = new Dictionary<uint, string>();
        public static void Init()
        {
            errors.Add((uint)Error.ERROR_SUCCESS, "The operation completed successfully.");
            errors.Add((uint)Error.ERROR_INVALID_ARGUMENTS, "Invalid number of arguments.");
        }
        public static string GetError(uint error) => errors[error];
    }
}