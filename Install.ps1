if ( @(Get-Module | ? { $_.Name -eq "webadministration" }).count -eq 0 ) {
    Import-Module webadministration
}

function Install-IISWebApp {
    param($label, $path)

    # TODO figure out if -Force is the best way to handle the site existing
    New-Website -Name $label -PhysicalPath $path -Port 2112 -Force

    return $true
}

$here = Split-Path -Parent $MyInvocation.MyCommand.Path
Install-IISWebApp "RestMvc" "$here\content"
Start-Website "RestMvc"
