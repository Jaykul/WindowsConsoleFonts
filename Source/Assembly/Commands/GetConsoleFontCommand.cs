using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Text;
using PoshCode.ConsoleFonts;

namespace PoshCode.ConsoleFonts.Commands
{
    [Cmdlet(VerbsCommon.Get, "ConsoleFont", DefaultParameterSetName = "Default")]
    public class GetConsoleFontCommand : Cmdlet
    {
        [Parameter(ParameterSetName = "Default")]
        public SwitchParameter ListAvailable { get; set; }

        protected override void EndProcessing()
        {
            base.EndProcessing();
            if (ListAvailable.ToBool())
            {
                WriteObject(PoshCode.Fonts.FontFamily.Console, true);
            }
            else
            {
                WriteObject(ConsoleHelper.GetCurrentFont());
            }
        }
    }
}
