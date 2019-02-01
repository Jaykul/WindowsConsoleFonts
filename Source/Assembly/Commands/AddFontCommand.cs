using PoshCode.ConsoleFonts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;

namespace PoshCode.Fonts.Commands
{
    [Cmdlet(VerbsCommon.Add, "Font", DefaultParameterSetName = "Default")]
    public class AddFontCommand : PSCmdlet
    {
        private static readonly string[] fonts = new [] { ".fon", ".fnt", ".ttf", ".ttc", ".fot", ".otf", ".mmm", ".pfb", ".pfm" };
        private static readonly string validFonts = string.Join(", ", fonts);


        [Parameter(Position = 0, Mandatory = true, ValueFromPipelineByPropertyName = true)]
        [Alias("PSPath")]
        public string Path { get; set; }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            var path = SessionState.Path.GetResolvedProviderPathFromPSPath(Path, out ProviderInfo provider);
            if(provider.Name != "FileSystem")
            {
                ThrowTerminatingError(new ErrorRecord(new PSInvalidOperationException("You can only add fonts from the FileSystem"), "FontFileRequired", ErrorCategory.ResourceUnavailable, Path));
            }

            foreach(var font in path)
            {
                if (fonts.Any(s => s.Equals(System.IO.Path.GetExtension(font), StringComparison.OrdinalIgnoreCase)))
                {
                    NativeMethods.AddFontResource(font);
                }
                else
                {
                    WriteWarning($"'{font}' is not a supported font file type. Please try again with a valid font file ({validFonts})");
                }
            }
        }

        protected override void EndProcessing()
        {
            NativeMethods.PostMessage(NativeMethods.EVERYONE, NativeMethods.FONTCHANGE, 0, 0);
            base.EndProcessing();
            FontFamily.ClearCache();
        }
    }
}
