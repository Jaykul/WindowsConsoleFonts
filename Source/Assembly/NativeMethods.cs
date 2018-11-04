using System;
using System.Runtime.InteropServices;
namespace PoshCode.ConsoleFonts
{
    public static class NativeMethods
    {

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern IntPtr GetStdHandle(int nStdHandle);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern bool SetCurrentConsoleFontEx(IntPtr hConsoleOutput, bool MaximumWindow, ref ConsoleFont ConsoleCurrentFontEx);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern bool GetCurrentConsoleFontEx(IntPtr hConsoleOutput, bool MaximumWindow, ref ConsoleFont ConsoleCurrentFontEx);

        // internal static extern bool SetConsoleIcon(IntPtr hIcon);


        [DllImport("gdi32.dll")]
        internal static extern bool AddFontResource(string filePath);

        [DllImport("gdi32.dll")]
        internal static extern bool RemoveFontResource(string filePath);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern bool PostMessage(IntPtr hWnd, int Msg, int wParam = 0, int lParam = 0);

        internal const int FixedWidthTrueType = 54;
        internal const int StandardOutputHandle = -11;

        internal static readonly IntPtr EVERYONE = new IntPtr(0xffff);
        internal static readonly int FONTCHANGE = 0x1D;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct ConsoleFont
        {
            internal int cbSize;
            public int FontIndex;
            internal short FontWidth;
            public short FontSize;
            public int FontFamily;
            public int FontWeight;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            //[MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.wc, SizeConst = 32)]
            public string FontName;
        }
    }

}
