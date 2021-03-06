[CmdletBinding()]
param(
    # A specific folder to build into
    $OutputDirectory,

    # The version of the output module
    [Alias("ModuleVersion")]
    [string]$SemVer
)
Push-Location $PSScriptRoot -StackName BuildWindowsConsoleFont
try {
    if(!$SemVer -and (Get-Command gitversion -ErrorAction SilentlyContinue)) {
        $gitversion = gitversion | ConvertFrom-Json
        $SemVer = $PSBoundParameters["SemVer"] = $gitversion.InformationalVersion
    }

    foreach($proj in Get-ChildItem -Recurse -Filter *.csproj) {
        [xml]$doc = Get-Content $proj.FullName
        $doc.Project.SelectSingleNode("//VersionPrefix").InnerText = $SemVer.Split("+")[0].Split("-")[0]
        $doc.Project.SelectSingleNode("//VersionSuffix").InnerText = $SemVer.Split("+")[0].Split("-")[1]
        $doc.Project.SelectSingleNode("//InformationalVersion").InnerText = $SemVer
        $doc.Save($proj.FullName)
    }

    # build the assembly (note, the resulting assembly works in PS 6 on .NET Core
    dotnet publish -f net472 -o lib
    ### Using the old framework would require us to ship 96 extra shim files
    # dotnet publish -f net461 -o lib

    ## Build the actual module
    $ModuleInfo = Build-Module .\Source @PSBoundParameters -Passthru

    ## Build the help files
    New-ExternalHelp -OutputPath (Join-Path $ModuleInfo.ModuleBase "en") -Path .\Docs
} finally {
    Pop-Location -StackName BuildWindowsConsoleFont
}