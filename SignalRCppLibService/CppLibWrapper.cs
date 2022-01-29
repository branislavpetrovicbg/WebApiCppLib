using System.Runtime.InteropServices;

namespace SignalRCppLibService
{
    public static class CppLibWrapper
    {
        [DllImport("CppLib.dll")]
        public static extern long calculate(long a);
    }
}
