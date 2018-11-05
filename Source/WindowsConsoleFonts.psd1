@{
    # The module version should be SemVer.org compatible
    ModuleVersion           = '1.0.0'

    # PrivateData is where all third-party metadata goes
    PrivateData = @{
        # PrivateData.PSData is the PowerShell Gallery data
        PSData = @{
            # Prerelease string of this module
            Prerelease      = '-beta04'

            # ReleaseNotes of this module
            ReleaseNotes    = '
            First Version for PowerShell Gallery
            '

            # Tags applied to this module. These help with module discovery in online galleries.
            Tags            = 'Console','Windows','Fonts'

            # A URL to the license for this module.
            LicenseUri      = 'https://github.com/Jaykul/WindowsConsoleFonts/blob/master/LICENSE'

            # A URL to the main website for this project.
            ProjectUri      = 'https://github.com/Jaykul/WindowsConsoleFonts'

            # A URL to an icon representing this module.
            IconUri         = 'https://github.com/Jaykul/WindowsConsoleFonts/blob/resources/logo.png?raw=true'
        } # End of PSData
    } # End of PrivateData
    # Script module or binary module file associated with this manifest.
    RootModule              = "lib\WindowsConsoleFonts.dll"
    FormatsToProcess        = "Format.ps1xml"
    # Name of the PowerShell host required by this module
    PowerShellHostName      = 'ConsoleHost'
    # Minimum version of the PowerShell host required by this module
    # PowerShellHostVersion = ''

    # Always define FunctionsToExport as an empty @() which will be replaced on build
    FunctionsToExport       = @()
    # Cmdlets to export from this module
    CmdletsToExport         = @('Set-ConsoleFont', 'Get-ConsoleFont', 'Add-Font', 'Remove-Font')
    AliasesToExport         = @()

    # ID used to uniquely identify this module
    GUID                    = 'c93f5818-220b-4b7f-866d-f0ddeea60eb6'
    Description             = 'A module for setting the console font in Windows'

    # Common stuff for all our modules:
    CompanyName             = 'PoshCode'
    Author                  = 'Joel Bennett'
    Copyright               = 'Copyright 2018 Joel Bennett'

    # Minimum version of the Windows PowerShell engine required by this module
    PowerShellVersion       = '5.1'
    CompatiblePSEditions    = @('Core','Desktop')
    # Minimum version of Microsoft .NET Framework required by this module. This prerequisite is valid for the PowerShell Desktop edition only.
    DotNetFrameworkVersion  = '4.7.2'
    # Minimum version of the common language runtime (CLR) required by this module. This prerequisite is valid for the PowerShell Desktop edition only.
    # CLRVersion = ''
}

