using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Text;
using PoshCode.ConsoleFonts;

namespace PoshCode.ConsoleFonts.Commands
{
    [Cmdlet(VerbsCommon.Set, "ConsoleFont", DefaultParameterSetName = "Default")]
    public class SetConsoleFontCommand : Cmdlet
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
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

        protected override void ProcessRecord()
        {
            base.ProcessRecord();
            ConsoleHelper.SetCurrentFont(Name, Size, Weight);
        }
    }
}
