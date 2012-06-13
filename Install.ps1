param($installPath, $toolsPath, $package, $project)

if ( @(Get-Module | ? { $_.Name -eq "webadministration" }).count -eq 0 ) {
    Import-Module webadministration
}

function global:Install-IISWebApp {
    param($label, $path)

    # TODO figure out if -Force is the best way to handle the site existing
    New-Website -Name $label -PhysicalPath $path -ApplicationPool "ASP.NET v4.0" -Port 2112 -Force

    return $true
}

$here = Split-Path -Parent $MyInvocation.MyCommand.Path
Install-IISWebApp "RestMvc" "$here\content"
Start-Website "RestMvc"
