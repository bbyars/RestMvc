properties {
  $solutionFile = (Resolve-Path $rootDir\*.sln)
  $rootDir = $pwd
  $buildDir = "$rootDir\build"
}

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
	Exec { msbuild RestMvc.Example\RestMvc.Example.csproj /t:Rebuild /t:ResolveReferences /p:Configuration=Release /p:WebProjectOutputDir=build\web\ /p:OutDir=build\web\ }
}

task ? -Description "Helper to display task info" {
	Write-Documentation
}