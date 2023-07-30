# Windows Console Fonts module

Lets you install fonts temporarily for use, and configure the current console font. You can also list available (monospaced) fonts.

**NOTE:** This is for the legacy Windows Console Host (conhost.exe). See also my [EzTheme](/jaykul/EzTheme) module `Theme.WindowsConsole` for controlling the colors.

## To Install the Module

```PowerShell
Install-Module WindowsConsoleFonts
```

You can then `Import-Module WindowsConsoleFonts` and `Get-Command -Module WindowsConsoleFonts` and there are examples in `Get-Help -Examples ` ... for each command.

## To Register Fonts for Use

```PowerShell
Add-Font .\FuraCode.ttf
```

`Add-Font` allows non-administrator users to install fonts temporarily. This lasts until the user logs out or reboots.
To have the font stay installed even after restarting the system, the font must be copied to `${Env:windir}\fonts` **and** listed in the registry in `HKLM:\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Fonts` -- which requires elevation.

## To Set the Font

```PowerShell
Set-ConsoleFont FiraCode -Size 18
```

`Set-ConsoleFont` automatically supports partial matches, so you can say "Lucida" for "Lucida Console" or "Fira" for "FiraCode Nerd Font Mono" etc. Because of this, it has a `-Passthru` parameter so you can see which font it selected.




## Problems:

`Add-Font` doesn't make the font show up in `Get-ConsoleFont` (or Tab completion) until you open a new window. You can use it in `Set-ConsoleFont`, but only if you know the name...

This module should serve as an example of the pain of building modules that are cross-platform. I'm having to manually write `if/else` statements in the `RootModule` to load the right assembly -- and this is _just on Windows_. If I had to do x64/x32/ARM or Windows/Linux, I don't know if that's even possible.
