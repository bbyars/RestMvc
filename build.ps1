properties {
  $solutionFile = (Resolve-Path $rootDir\*.sln)
  $rootDir = $pwd
  $buildDir = "$rootDir\build"
}

task default -Depends Clean, Compile

task Clean { 
  if (Test-Path $buildDir) {
  	Remove-Item $buildDir -Recurse -Force
  }
  Exec { msbuild $solutionFile /t:clean } "Could not clean the project"
}

task Compile {
  Exec { msbuild $solutionFile /p:Configuration=Release } "Build Failed - Compilation"
}

task Package {
  if (-not (Test-Path $buildDir)) {
    New-Item $buildDir -ItemType Container
  }

  $version = '1.0.0'
  if (Test-Path Env:\GO_PIPELINE_LABEL) {
    $version = $($Env:GO_PIPELINE_LABEL)
  }
  Dependencies\nuget\nuget.exe pack RestMvc.Example.nuspec -Version $version -OutputDirectory $buildDir -NoPackageAnalysis
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