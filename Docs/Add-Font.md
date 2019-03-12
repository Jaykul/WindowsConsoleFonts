---
external help file: WindowsConsoleFonts-Help.xml
Module Name: WindowsConsoleFonts
online version: https://github.com/Jaykul/WindowsConsoleFonts/blob/master/Docs/Add-Font.md
schema: 2.0.0
---

# Add-Font

## SYNOPSIS

Install a font from a font file (.ttf, .otf, etc) for the current session, even without elevation

## SYNTAX

```
Add-Font [-Path] <String> [<CommonParameters>]
```

## DESCRIPTION
Add-Font allows non-administrator users to install fonts for use within a session. Note that when the system restarts, the font will not be available. To have the font stay installed even after restarting the system, the font must be copied to `${Env:windir}\fonts\` and listed in the registry in `HKLM:\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Fonts` -- which requires elevation.

## EXAMPLES

### Example 1
```powershell
PS C:\> Add-Font .\Fonts\*
```

Adds all of the fonts in the Fonts folder to the system temporarily. Note that the WindowsConsoleFonts ships a few of my favorite console fonts in it's Fonts folder!

## PARAMETERS

### -Path
The path to the font(s)

```yaml
Type: String
Parameter Sets: (All)
Aliases: PSPath

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: True
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable.
For more information, see about_CommonParameters (http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.String

## OUTPUTS

### System.Object
## NOTES

## RELATED LINKS
[Remove-Font](Remove-Font)
[Set-ConsoleFont](Set-ConsoleFont)
[Get-ConsoleFont](Get-ConsoleFont)