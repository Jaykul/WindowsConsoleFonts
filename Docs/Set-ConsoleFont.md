---
external help file: WindowsConsoleFonts-Help.xml
Module Name: WindowsConsoleFonts
online version: https://github.com/Jaykul/WindowsConsoleFonts/blob/master/Docs/Set-ConsoleFont.md
schema: 2.0.0
---

# Set-ConsoleFont

## SYNOPSIS
Sets the font for the current console (including size and weight).

## SYNTAX

### Default (Default)
```
Set-ConsoleFont [-Name] <String> [-Size] <Int16> [-Bold] [-Passthru] [<CommonParameters>]
```

### ManualWeight
```
Set-ConsoleFont [-Name] <String> [-Size] <Int16> -Weight <Int32> [-Passthru] [<CommonParameters>]
```

## DESCRIPTION
Sets the font name, size and weight for the console, ignoring the rules! Allows specifying any TrueType font name, even if it's not monospaced.

## EXAMPLES

### Example 1
```powershell
PS C:\> Set-ConsoleFont "Consolas"
```

Sets the current font to Consolas without changing the font size.

### Example 2
```powershell
PS C:\> Set-ConsoleFont "Fura" -Size 14 -Passthru
```

Sets the current font to the first font which matches "Fura" -- probably "FuraCode NF," the Windows Compatible name for the Fira Code font altered by Nerd Fonts to add dozens of icons and shapes like those used in powerline etc.

Using the `-Passthru` parameter outputs the new font, which is particularly useful when you're using a partial name, because you tell whether it matched the font name you expected.

## PARAMETERS

### -Bold
Sets the font weight to 700 (on a scale from 100 to 1000).

```yaml
Type: SwitchParameter
Parameter Sets: Default
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Name
Specifies a partial font name. Note that Tab Completion here will loop through the fonts which would be available in the Windows Console property dialog.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByPropertyName, ByValue)
Accept wildcard characters: False
```

### -Passthru
Output the FontFamily object representing the font that was just set.

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Size
Specifies the font size for TrueType fonts (i.e. try even numbers between 10 and 14)

```yaml
Type: Int16
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Weight
Allows you to manually specify the font weight. Won't work well unless you have the actual weight represented for that font family.

```yaml
Type: Int32
Parameter Sets: ManualWeight
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable.
For more information, see about_CommonParameters (http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.String

## OUTPUTS

### PoshCode.Fonts.FontFamily

## NOTES

## RELATED LINKS
[Add-Font](Add-Font)
[Remove-Font](Remove-Font)
[Get-ConsoleFont](Get-ConsoleFont)
