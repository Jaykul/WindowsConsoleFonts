using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;

using static PoshCode.ConsoleFonts.NativeMethods;
namespace PoshCode.ConsoleFonts
{
    public static class ConsoleHelper
    {
        private static readonly IntPtr ConsoleOutputHandle = GetStdHandle(StandardOutputHandle);

        public static FontInfo GetCurrentFont()
        {
            FontInfo info = new FontInfo();
            info.cbSize = Marshal.SizeOf<FontInfo>();
            // First determine whether there's already a TrueType font.
            if (GetCurrentConsoleFontEx(ConsoleOutputHandle, true, ref info))
            {
                return info;
            }
            else
            {
                var er = Marshal.GetLastWin32Error();
                Console.WriteLine("Read error " + er);
                throw new System.ComponentModel.Win32Exception(er);
            }
        }

        public static FontInfo[] SetCurrentFont(string font, short fontSize = 0, int fontWeight = 400)
        {
            Console.WriteLine("Set Current Font: " + font + ", " + fontSize + ", "  + fontWeight);
            // This requires you to have the font named according to it's font name?
            if (System.IO.File.Exists(font))
            {
                AddFontResource(font);
                PostMessage((IntPtr)0xffff, 0x1D); // font change

                font = System.IO.Path.GetFileNameWithoutExtension(font).Split('-')[0];
                font = MonospaceFontNameCompleter.GetMonospaceFonts(true).FirstOrDefault(name => name.Contains(font)) ?? font;
            }
            else
            {
                font = MonospaceFontNameCompleter.GetMonospaceFonts().FirstOrDefault(name => name.Contains(font)) ?? font;
            }

            FontInfo before = new FontInfo
            {
                cbSize = Marshal.SizeOf<FontInfo>()
            };

            if (GetCurrentConsoleFontEx(ConsoleOutputHandle, false, ref before))
            {
                if (font.Length > 32)
                {
                    Console.WriteLine("Font name must be less than 32 characters. Attempting to set Font: " + font);
                }

                FontInfo set = new FontInfo
                {
                    cbSize = Marshal.SizeOf<FontInfo>(),
                    FontIndex = 0,
                    FontFamily = FixedWidthTrueType, // TMPF_TRUETYPE; 0 = FF_DONTCARE?
                    FontName = font,
                    FontWeight = fontWeight,
                    FontSize = fontSize > 0 ? fontSize : before.FontSize
                };
                //IntPtr ptr = new IntPtr(info.FontName);
                //Marshal.Copy(font.ToCharArray(), 0, ptr, font.Length);

                // Get some settings from current font.
                if (!SetCurrentConsoleFontEx(ConsoleOutputHandle, false, ref set))
                {
                    var ex = Marshal.GetLastWin32Error();
                    Console.WriteLine("Set error " + ex);
                    throw new System.ComponentModel.Win32Exception(ex);
                }

                FontInfo after = new FontInfo
                {
                    cbSize = Marshal.SizeOf<FontInfo>()
                };
                GetCurrentConsoleFontEx(ConsoleOutputHandle, false, ref after);

                return new[] { before, set, after };
            }
            else
            {
                var er = Marshal.GetLastWin32Error();
                Console.WriteLine("Get error " + er);
                throw new System.ComponentModel.Win32Exception(er);
            }
        }
    }

}
