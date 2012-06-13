

properties {
  $solutionFile = (Resolve-Path $rootDir\*.sln)
  $rootDir = $pwd
}

task default -depends Test

task Test -depends Compile, Clean { 
  
}

task Compile -depends Clean {
  Write-Host $solutionFile 
  Exec { msbuild $solutionFile /p:Configuration=Release } "Build Failed - Compilation"
}

task Clean { 
  Exec { msbuild $solutionFile /t:clean } "Could not clean the project"
}

task ? -Description "Helper to display task info" {
	Write-Documentation
}