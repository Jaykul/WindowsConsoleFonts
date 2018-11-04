using System;
using System.Drawing;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;

namespace PoshCode.Fonts {

    public class FontFamily {
        public string Name { get; private set; }
        public string Style { get; private set; }
        public bool Fixed { get; private set; }
        public bool Modern { get; private set; }
        public FontType FontType { get; set; }

        internal bool Monospace { get; set; }

        public string FullName { get; private set; }
        public string Script { get; private set; }

        public readonly LogicalFont LogicalFont;

        public readonly FullTextMetric TextMetric;

        internal FontFamily(FontCallbackInfo fontInfo, FullTextMetric fontMetric, FontType fontType)
        {
            LogicalFont = fontInfo.LogicalFont;
            TextMetric = fontMetric;

            Name = fontInfo.LogicalFont.Name;
            FullName = fontInfo.FullName;
            Style = fontInfo.Style;
            Script = fontInfo.Script;
            Fixed = fontInfo.LogicalFont.PitchAndFamily.HasFlag(PitchAndFamily.FixedPitch);
            Modern = fontInfo.LogicalFont.PitchAndFamily.HasFlag(PitchAndFamily.ModernFamily);

            FontType = fontType;
        }


        private static List<FontFamily> _fonts = new List<FontFamily>();
        private static bool _testMonoSpaced;
        private static CharacterSet _characterSet = CharacterSet.ANSI;

        public static List<FontFamily> All => _fonts.Count > 0 ? _fonts : GetFonts();

        public static IEnumerable<FontFamily> Console => All.Where(f => f.Fixed && f.Modern && f.FontType == FontType.PostScript);

        public static IEnumerable<FontFamily> Monospaced => All.Where(f => f.Fixed || f.Monospace);

        public static bool PseudoMonoSpaced
        {
            get => _testMonoSpaced;
            set
            {
                _testMonoSpaced = value;
                _fonts.Clear();
            }
        }

        public static CharacterSet CharacterSet
        {
            get => _characterSet;
            set
            {
                _characterSet = value;
                _fonts.Clear();
            }
        }

        public static List<FontFamily> GetFonts()
        {
            _fonts.Clear();

            LogicalFont lf = new LogicalFont();
            {
                CharacterSet = CharacterSet;
            }

            IntPtr fontPointer = Marshal.AllocHGlobal(Marshal.SizeOf(lf));
            Marshal.StructureToPtr(lf, fontPointer, true);

            int result = 0;
            try
            {
                using (var bmp = new Bitmap(1, 1))
                {
                    using (var g = Graphics.FromImage(bmp))
                    {
                        var ptr = g.GetHdc();
                        try
                        {
                            var callback = new EnumFontExDelegate(EnumFontCallback);
                            result = EnumFontFamiliesEx(ptr, fontPointer, callback, IntPtr.Zero, 0);

                            _fonts = _fonts.OrderBy(f => f.Name).ToList();
                            if (_testMonoSpaced)
                            {
                                foreach (var family in _fonts)
                                {
                                    using (var font = new Font(family.Name, 10))
                                    {
                                        family.Monospace = g.MeasureString("iii", font).Width.Equals(g.MeasureString("WWW", font).Width);
                                    }
                                }
                            }
                        }
                        finally
                        {
                            g.ReleaseHdc(ptr);
                        }
                    }
                }
            }
            catch
            {
                System.Diagnostics.Trace.WriteLine("Error!");
            }
            finally
            {
                Marshal.DestroyStructure(fontPointer, typeof(LogicalFont));
            }

            return _fonts;
        }

        [DllImport("gdi32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        private static extern int EnumFontFamiliesEx(IntPtr hdc,
                    [In] IntPtr pLogfont,
                    EnumFontExDelegate lpEnumFontFamExProc,
                    IntPtr lParam,
                    uint dwFlags);


        private delegate int EnumFontExDelegate(ref FontCallbackInfo lpelfe, ref FullTextMetric lpntme, int FontType, int lParam);

        private static int EnumFontCallback(ref FontCallbackInfo lpelfe, ref FullTextMetric lpntme, int fontType, int lParam)
        {
            try
            {
                if (fontType != 2 && fontType != 4)
                {
                    fontType = (int)FontType.Unknown;
                }

                _fonts.Add(new FontFamily(lpelfe, lpntme, (FontType)fontType));
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(e.ToString());
            }
            return 1;
        }

        internal static void ClearCache()
        {
            _fonts.Clear();
        }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = System.Runtime.InteropServices.CharSet.Auto)]
    public class LogicalFont
    {
        public int Height = 0;
        public int Width = 0;
        public int Escapement = 0;
        public int Orientation = 0;

        public FontWeight Weight = FontWeight.Normal;

        [MarshalAs(UnmanagedType.U1)]
        public bool Italic = false;

        [MarshalAs(UnmanagedType.U1)]
        public bool Underline = false;

        [MarshalAs(UnmanagedType.U1)]
        public bool StrikeOut = false;

        public CharacterSet CharacterSet = CharacterSet.ANSI;

        public Precision Precision = Precision.Default;

        public ClipPrecision ClipPrecision = ClipPrecision.Default;

        public Quality Quality = Quality.Default;

        public PitchAndFamily PitchAndFamily = PitchAndFamily.DefaultPitch;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string Name = string.Empty;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = System.Runtime.InteropServices.CharSet.Auto)]
    public struct FontCallbackInfo
    {
        public LogicalFont LogicalFont;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string FullName;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string Style;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string Script;
    }

    public enum FontWeight : int
    {
        DontCare = 0,
        Thin = 100,
        ExtraLight = 200,
        Light = 300,
        Normal = 400,
        Medium = 500,
        SemiBold = 600,
        Bold = 700,
        ExtraBold = 800,
        Heavy = 900,
    }
    public enum CharacterSet : byte
    {
        ANSI = 0,
        Default = 1,
        Symbol = 2,
        ShiftJIS = 128,
        HangeUL = 129,
        HangUL = 129,
        GB2312 = 134,
        ChinesBig5 = 136,
        OEM = 255,
        Johab = 130,
        Hebrew = 177,
        Arabic = 178,
        Greek = 161,
        Turkish = 162,
        Vietnamese = 163,
        Thai = 222,
        EasternEurope = 238,
        Russian = 204,
        Mac = 77,
        Baltic = 186,
    }
    public enum Precision : byte
    {
        Default = 0,
        String = 1,
        Character = 2,
        Stroke = 3,
        TrueType = 4,
        Device = 5,
        Raster = 6,
        TrueTypeOnly = 7,
        Outline = 8,
        ScreenOutline = 9,
        PostScriptOnly = 10,
    }
    public enum ClipPrecision : byte
    {
        Default = 0,
        Character = 1,
        Stroke = 2,
        Mask = 0xf,
        Angles = (1 << 4),
        Always = (2 << 4),
        Disable = (4 << 4),
        Embedded = (8 << 4),
    }
    public enum Quality : byte
    {
        Default = 0,
        Draft = 1,
        Proof = 2,
        Unalised = 3,
        AntiAliased = 4,
        ClearType = 5,
        ClearTypeNatural = 6,
    }

    [Flags]
    public enum PitchAndFamily : byte
    {
        DefaultPitch = 0,
        FixedPitch = 1,
        VariablePitch = 2,
        DontCare = (0 << 4),
        RomanFamily = (1 << 4),
        SwissFamily = (2 << 4),
        ModernFamily = (3 << 4),
        ScriptFamily = (4 << 4),
        DecorativeFamily = (5 << 4),
    }

    [StructLayout(LayoutKind.Sequential, CharSet = System.Runtime.InteropServices.CharSet.Auto)]
    public struct TextMetric
    {
        public int Height;
        public int Ascent;
        public int Descent;
        public int InternalLeading;
        public int ExternalLeading;
        public int AverageCharWidth;
        public int MaximumCharWidth;
        public int Weight;
        public int Overhang;
        public int DigitizedAspectX;
        public int DigitizedAspectY;
        public char FirstChar;
        public char LastChar;
        public char DefaultChar;
        public char BreakChar;
        public byte Italic;
        public byte Underlined;
        public byte StruckOut;
        public byte PitchAndFamily;
        public byte CharSet;
        public int Flags;
        public int SizeEm;
        public int CellHeight;
        public int AverageWidth;
    }

    public struct FontSignature
    {
        [MarshalAs(UnmanagedType.ByValArray)]
        public int[] UnicodeSubranges;

        [MarshalAs(UnmanagedType.ByValArray)]
        public int[] CodePageSubranges;
    }
    public struct FullTextMetric
    {
        public TextMetric TextMetric;
        public FontSignature FontSignature;
    }

    public enum FontType {
        Unknown = 1,
        PostScript = 2,
        TrueType = 4
    }
}