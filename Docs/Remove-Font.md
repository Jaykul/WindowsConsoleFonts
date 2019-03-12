---
external help file: WindowsConsoleFonts-Help.xml
Module Name: WindowsConsoleFonts
online version: https://github.com/Jaykul/WindowsConsoleFonts/blob/master/Docs/Remove-Font.md
schema: 2.0.0
---

# Remove-Font

## SYNOPSIS
Removes fonts added by Add-Font

## SYNTAX

```
Remove-Font [-Path] <String> [<CommonParameters>]
```

## DESCRIPTION
Removes fonts added by Add-Font. The documentation for [AddFontResource](https://docs.microsoft.com/en-us/windows/desktop/api/wingdi/nf-wingdi-addfontresourcea) suggests that any application which adds a font and no longer needs it should remove it -- however, it's not actually necessary unless you want to remove the font file, as the fonts will be removed when your Windows session ends.

## EXAMPLES

### Example 1
```powershell
PS C:\> Remove-Font .\Fonts\*
```

Removes all of the fonts in the Fonts folder.

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
[Add-Font](Add-Font)
[Set-ConsoleFont](Set-ConsoleFont)
[Get-ConsoleFont](Get-ConsoleFont)

