# Windows Console Fonts module

Lets you `Set` and `Get` the current console font, as well as list available (monospaced) fonts, and install fonts temporarily for use.

```PowerShell
Add-Font .\FuraCode.ttf
Set-ConsoleFont "FuraCode NF"
```

## Problems:

`Add-Font` doesn't make the font show up in `Get-ConsoleFont` (or Tab completion) until you open a new window. You can use it in `Set-ConsoleFont`, but only if you know the name...

This module should serve as an example of the pain of building modules that are cross-platform. I'm having to manually write `if/else` statements in the `RootModule` to load the right assembly -- and this is _just on Windows_. If I had to do x64/x32/ARM or Windows/Linux, I don't know if that's even possible.