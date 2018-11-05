using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Runtime.InteropServices;
using System.Text;
using PoshCode.Fonts;

using static PoshCode.ConsoleFonts.NativeMethods;

namespace PoshCode.ConsoleFonts.Commands
{
    [Cmdlet(VerbsCommon.Set, "ConsoleFont", DefaultParameterSetName = "Default")]
    public class SetConsoleFontCommand : Cmdlet
    {
        private static readonly IntPtr ConsoleOutputHandle = GetStdHandle(StandardOutputHandle);

        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
        [ArgumentCompleter(typeof(MonospaceFontNameCompleter))]
        public string Name { get; set; }

        [Parameter()]
        public short Size { get; set; } = 0;

        [Parameter(ParameterSetName = "Default")]
        public SwitchParameter Bold
        {
            get { return Weight >= 700; }
            set { Weight = 700; }
        }

        [Parameter(Mandatory = true, ParameterSetName = "ManualWeight")]
        public int Weight { get; set; } = 400;

        [Parameter]
        public SwitchParameter Passthru { get; set; }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            if (Name.Length > 32)
            {
                ThrowTerminatingError(new ErrorRecord(new ArgumentOutOfRangeException("font", Name, "Console font name has a maximum length of 32 characters"), "FontNameTooLong", ErrorCategory.InvalidData, Name));
            }

            // This requires you to have the font named according to it's font name?
            if (System.IO.File.Exists(Name))
            {
                WriteDebug($"Installing font {Name}");
                AddFontResource(Name);
                PostMessage((IntPtr)0xffff, 0x1D); // font change

                Name = System.IO.Path.GetFileNameWithoutExtension(Name).Split('-')[0];
                Name = FontFamily.All.FirstOrDefault(family => family.Name.Contains(Name)).Name ?? Name;
                WriteVerbose($"Attempting to use font {Name}");
            }
            else
            {
                Name = FontFamily.All.FirstOrDefault(family => family.Name.Contains(Name)).Name ?? Name;
                WriteVerbose($"Attempting to use font {Name}");
            }

            ConsoleFont before = new ConsoleFont
            {
                cbSize = Marshal.SizeOf<ConsoleFont>()
            };

            // Get the current font (just so we can keep the font size?)
            if (GetCurrentConsoleFontEx(ConsoleOutputHandle, false, ref before))
            {
                ConsoleFont set = new ConsoleFont
                {
                    cbSize = Marshal.SizeOf<ConsoleFont>(),
                    FontIndex = 0,
                    FontFamily = FixedWidthTrueType, // TMPF_TRUETYPE; 0 = FF_DONTCARE?
                    FontName = Name,
                    FontWeight = Weight,
                    FontSize = Size > 0 ? Size : before.FontSize
                };

                if (!SetCurrentConsoleFontEx(ConsoleOutputHandle, false, ref set))
                {
                    var ex = Marshal.GetLastWin32Error();
                    ThrowTerminatingError(new ErrorRecord(new System.ComponentModel.Win32Exception(ex, "Error setting the console font"), "SetFontFailure", ErrorCategory.WriteError, Name));
                }

                // Check to see if setting the font worked
                ConsoleFont after = new ConsoleFont
                {
                    cbSize = Marshal.SizeOf<ConsoleFont>()
                };
                GetCurrentConsoleFontEx(ConsoleOutputHandle, false, ref after);

                if (Name != after.FontName) {
                    WriteWarning($"Failed to change font to {Name} (it went to {Name}). Changing back...");
                    if (!SetCurrentConsoleFontEx(ConsoleOutputHandle, false, ref before))
                    {
                        var ex = Marshal.GetLastWin32Error();
                        ThrowTerminatingError(new ErrorRecord(new System.ComponentModel.Win32Exception(ex, "Error resetting the console font"), "ResetFontFailure", ErrorCategory.WriteError, Name));
                    }
                }

                if (Passthru.ToBool()) {
                    WriteObject(FontFamily.All.FirstOrDefault(family => family.Name.Equals(after.FontName)));
                }
            }
            else
            {
                var ex = Marshal.GetLastWin32Error();
                ThrowTerminatingError(new ErrorRecord(new System.ComponentModel.Win32Exception(ex, "Error getting the console font"), "GetFontFailure", ErrorCategory.ReadError, Name));
            }
        }
    }
}
