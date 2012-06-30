properties {
  $rootDir = $pwd
  $solutionFile = (Resolve-Path $rootDir\*.sln)
  $buildDir = "$rootDir\build"
  $version = '1.0.0'

  if (Test-Path Env:\GO_PIPELINE_LABEL) {
    $version = $($Env:GO_PIPELINE_LABEL)
  }
}

task default -Depends Clean, Compile, UnitTest, Package

task Clean { 
  if (Test-Path $buildDir) {
    Remove-Item $buildDir -Recurse -Force
  }
  Exec { msbuild $solutionFile /t:clean } "Could not clean the project"
}

task SetVersion {
  Get-ChildItem $rootDir -Recurse | ? { $_.Name -eq "AssemblyInfo.cs" } | % {
    $tmpFile = "$($_.FullName).tmp"

    Get-Content $_.FullName |
      %{ $_ -Replace '\(Assembly\(File\)?Version\)\("[^"]*"\)', '$1("$version")' } |
      Out-File $tmpFile -Encoding UTF8

    Move-Item $tmpFile $_.FullName -Force
  }
}

task Compile -Depends SetVersion {
  Exec { msbuild $solutionFile /p:Configuration=Release } "Build Failed - Compilation"
}

task MakeBuildDir {
  if (-not (Test-Path $buildDir)) {
    New-Item $buildDir -ItemType Container
  }
}

task UnitTest -Depends Compile, MakeBuildDir {
  $targetDll = Resolve-Path $rootDir\RestMvc.UnitTests\bin\Release\RestMvc.UnitTests.dll
  $xmlFile = "$buildDir\UnitTest-Results.xml"
  Exec { Dependencies\NUnit\nunit-console.exe "$targetDll" /xml="$xmlFile" } "Unit Tests Failed"  
}

task Package -Depends MakeBuildDir, Compile {
  Exec { Dependencies\nuget\nuget.exe pack RestMvc.nuspec -Version $version -OutputDirectory $buildDir -NoPackageAnalysis }
  Exec { Dependencies\nuget\nuget.exe pack RestMvc.FunctionalTests.nuspec -Version $version -OutputDirectory $buildDir -NoPackageAnalysis }
  Exec { Dependencies\nuget\nuget.exe pack RestMvc.Example.nuspec -Version $version -OutputDirectory $buildDir -NoPackageAnalysis }
}

task TestPackage {
  if (Test-Path "shower") {
    Remove-Item "shower" -Recurse -Force
  }
  
  New-Item "shower" -ItemType Container
  Dependencies\nuget\nuget.exe Install -Source $buildDir -OutputDirectory "shower" RestMvc.Example
  shower\RestMvc.Example.1.0.0\Install.ps1
}

task ? -Description "Helper to display task info" {
  Write-Documentation
}
