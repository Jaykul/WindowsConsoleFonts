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

        public static ConsoleFont GetCurrentFont()
        {
            ConsoleFont info = new ConsoleFont
            {
                cbSize = Marshal.SizeOf<ConsoleFont>()
            };

            // First determine whether there's already a TrueType font.
            if (GetCurrentConsoleFontEx(ConsoleOutputHandle, true, ref info))
            {
                return info;
            }
            else
            {
                var er = Marshal.GetLastWin32Error();
                throw new System.ComponentModel.Win32Exception(er, "Error getting the console font");
            }
        }

        public static ConsoleFont[] SetCurrentFont(string font, short fontSize = 0, int fontWeight = 400)
        {
            if(font.Length > 32) {
                throw new ArgumentOutOfRangeException("font", font, "Console font name has a maximum length of 32 characters");
            }

            // This requires you to have the font named according to it's font name?
            if (System.IO.File.Exists(font))
            {
                AddFontResource(font);
                PostMessage((IntPtr)0xffff, 0x1D); // font change

                font = System.IO.Path.GetFileNameWithoutExtension(font).Split('-')[0];
                font = Fonts.FontFamily.Console.FirstOrDefault(family => family.Name.Contains(font)).Name ?? font;
            }
            else
            {
                font = Fonts.FontFamily.Console.FirstOrDefault(family => family.Name.Contains(font)).Name ?? font;
            }

            ConsoleFont before = new ConsoleFont
            {
                cbSize = Marshal.SizeOf<ConsoleFont>()
            };

            if (GetCurrentConsoleFontEx(ConsoleOutputHandle, false, ref before))
            {
                ConsoleFont set = new ConsoleFont
                {
                    cbSize = Marshal.SizeOf<ConsoleFont>(),
                    FontIndex = 0,
                    FontFamily = FixedWidthTrueType, // TMPF_TRUETYPE; 0 = FF_DONTCARE?
                    FontName = font,
                    FontWeight = fontWeight,
                    FontSize = fontSize > 0 ? fontSize : before.FontSize
                };

                // Get some settings from current font.
                if (!SetCurrentConsoleFontEx(ConsoleOutputHandle, false, ref set))
                {
                    var ex = Marshal.GetLastWin32Error();
                    throw new System.ComponentModel.Win32Exception(ex, "Error setting the console font");
                }

                ConsoleFont after = new ConsoleFont
                {
                    cbSize = Marshal.SizeOf<ConsoleFont>()
                };
                GetCurrentConsoleFontEx(ConsoleOutputHandle, false, ref after);

                return new[] { before, set, after };
            }
            else
            {
                var er = Marshal.GetLastWin32Error();
                throw new System.ComponentModel.Win32Exception(er, "Error getting the console font");
            }
        }
    }

}
