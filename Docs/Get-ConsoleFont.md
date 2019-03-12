---
external help file: WindowsConsoleFonts-Help.xml
Module Name: WindowsConsoleFonts
online version: https://github.com/Jaykul/WindowsConsoleFonts/blob/master/Docs/Get-ConsoleFont.md
schema: 2.0.0
---

# Get-ConsoleFont

## SYNOPSIS

Get the current console font or a list of available console fonts.

## SYNTAX

### Current (Default)
```
Get-ConsoleFont [<CommonParameters>]
```

### ByName
```
Get-ConsoleFont [-Name] <String> [<CommonParameters>]
```

### ConsoleFonts
```
Get-ConsoleFont [-ListAvailable] [-Extended] [<CommonParameters>]
```

## DESCRIPTION

Get the current console font or a list of available console fonts, optionally including TrueType fonts which _could_ be used, even if they wouldn't show up in the console's property dialog.

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-ConsoleFont
```
Returns the current console font

### Example 2
```powershell
PS C:\> Get-ConsoleFont -ListAvailable -Extended
```

Lists all of the available fonts which might work as console fonts. That is, if they seem to be monospaced fonts, regardless of having the `Fixed` or `Modern` properties set to true -- which is required to show up in the console property dialog.

### Example 3
```powershell
PS C:\> Get-ConsoleFont -ListAvailable
```

Lists the fonts which would be available in the font dialog (or in tab-completion for Set-ConsoleFont)

## PARAMETERS

### -Extended
Includes fonts where "iii" is the same width as "WWW" -- regardless of whether they are marked FixedPitch or VariablePitch (and even if they are not "Modern" fonts).

```yaml
Type: SwitchParameter
Parameter Sets: ConsoleFonts
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ListAvailable
List all available fonts. By default this includes monospace fonts which are marked `FixedPitch` and `Modern` (which are the ones which show up in the font dialog).

```yaml
Type: SwitchParameter
Parameter Sets: ConsoleFonts
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Name
Returns available fonts which match the specified name, regardless of whether or not they're monospaced, etc.

```yaml
Type: String
Parameter Sets: ByName
Aliases:

Required: True
Position: 0
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
[Set-ConsoleFont](Set-ConsoleFont)
