using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Language;
using System.Text;

namespace PoshCode.ConsoleFonts
{
    public class MonospaceFontNameCompleter : IArgumentCompleter
    {
        public IEnumerable<CompletionResult> CompleteArgument(string commandName, string parameterName, string wordToComplete, CommandAst commandAst, IDictionary fakeBoundParameters)
        {
            return GetMonospaceFonts().Where(name => name.StartsWith(wordToComplete.Trim('"', ' ', '\t'), StringComparison.OrdinalIgnoreCase)).Select(name => new CompletionResult(name.Contains(' ') ? $"\"{name}\"": name));
        }

        private static IEnumerable<string> GetAvailableFonts(bool monospace)
        {
            using (var bmp = new Bitmap(1, 1))
            {
                using (var g = Graphics.FromImage(bmp))
                {
                    foreach (var family in FontFamily.Families)
                    {
                        if (!monospace)
                        {
                            yield return family.Name;
                        }
                        else
                        {
                            // this is a clumsy test for monospace ...
                            using (var font = new Font(family.Name, 10))
                            {
                                if (g.MeasureString("i", font).Width.Equals(g.MeasureString("W", font).Width))
                                {
                                    yield return font.Name;
                                }
                            }
                        }
                    }
                }
            }
        }

        private static string[] _fontCache = new string[0];

        public static string[] GetMonospaceFonts(bool refreshCache = false)
        {
            if (refreshCache || _fontCache.Length == 0)
            {
                _fontCache = GetAvailableFonts(true).ToArray();
            }
            return _fontCache;
        }

        public static void RefreshFonts()
        {
            NativeMethods.PostMessage(NativeMethods.EVERYONE, NativeMethods.FONTCHANGE);
            _fontCache = new string[0];
        }
    }
}
