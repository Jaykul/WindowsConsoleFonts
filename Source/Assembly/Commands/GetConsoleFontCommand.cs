using System.Linq;
using System.Management.Automation;
using System.Runtime.InteropServices;
using PoshCode.Fonts;
using static PoshCode.ConsoleFonts.NativeMethods;

namespace PoshCode.ConsoleFonts.Commands
{
    [Cmdlet(VerbsCommon.Get, "ConsoleFont", DefaultParameterSetName = "Current")]
    public class GetConsoleFontCommand : Cmdlet
    {
        private static readonly System.IntPtr ConsoleOutputHandle = GetStdHandle(StandardOutputHandle);

        [SupportsWildcards]
        [ValidateNotNullOrEmpty]
        [Parameter(ParameterSetName = "ByName", Mandatory = true, Position = 0)]
        public string Name { get; set; }

        [Parameter(ParameterSetName = "ConsoleFonts", Mandatory = true)]
        public SwitchParameter ListAvailable { get; set; }

        [Parameter(ParameterSetName = "ConsoleFonts")]
        public SwitchParameter Extended { get; set; }

        protected override void EndProcessing()
        {
            base.EndProcessing();
            
            if (ListAvailable.ToBool())
            {
                if (Extended.ToBool())
                {
                    FontFamily.PseudoMonoSpaced = true;
                    WriteObject(FontFamily.Monospaced.Where(f => f.FontType != FontType.PostScript && f.Name.Length <= 32), true);
                }
                else
                {
                    WriteObject(FontFamily.Console, true);
                }
            }
            else if (string.IsNullOrEmpty(Name))
            {
                ConsoleFont info = new ConsoleFont
                {
                    cbSize = Marshal.SizeOf<ConsoleFont>()
                };

                // First determine whether there's already a TrueType font.
                if (GetCurrentConsoleFontEx(ConsoleOutputHandle, true, ref info))
                {
                    var ff = FontFamily.All.FirstOrDefault(f => f.Name == info.FontName);
                    if (null != ff)
                    {
                        WriteObject(ff);
                    }
                    else
                    {
                        WriteObject(info);
                    }
                }
                else
                {
                    var er = Marshal.GetLastWin32Error();
                    throw new System.ComponentModel.Win32Exception(er, "Error getting the console font");
                }
            }
            else
            {
                var pattern = WildcardPattern.Get(Name, WildcardOptions.Compiled & WildcardOptions.CultureInvariant & WildcardOptions.IgnoreCase);
                WriteObject(FontFamily.All.Where(f => pattern.IsMatch(f.Name)), true);
            }
        }
    }
}
