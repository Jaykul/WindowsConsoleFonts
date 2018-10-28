Push-Location $PSScriptRoot
## Cross-compile the assemblies
dotnet publish -f netstandard2.0 -o lib\netstandard2.0
dotnet publish -f net472 -o lib\net472
### The problem is that the old framework ships extra junk:
# dotnet publish -f net461
# Get-ChildItem .\Source\bin\Debug\*\publish\*.dll | Group-Object Directory | Out-Default

## Build a module out of them
Build-Module .\Source