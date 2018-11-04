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
            return PoshCode.Fonts.FontFamily.Monospaced.Where(family => family.Name.StartsWith(wordToComplete.Trim('"', ' ', '\t'), StringComparison.OrdinalIgnoreCase)).Select(family => new CompletionResult(family.Name.Contains(' ') ? $"\"{family.Name}\"": family.Name));
        }
    }
}
