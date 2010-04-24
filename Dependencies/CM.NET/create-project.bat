@echo off

if "%2" == "" goto Usage
if "%1" == "/?" goto Usage
if "%1" == "-?" goto Usage
if "%1" == "--help" goto Usage

echo preparing file for svn:ignores...
echo build> .svnignore
echo *.sln.cache>> .svnignore
echo _ReSharper*>> .svnignore
echo *.resharper>> .svnignore
echo *.resharper.user>> .svnignore
echo *.suo>> .svnignore
echo *.user>> .svnignore
echo TestResult.xml>> .svnignore
echo *.Cache>> .svnignore
svn propset "svn:ignore" -F.svnignore .
rm .svnignore

echo -
echo linking svn and mingle...
svn propset "bugtraq:message" "Card #%%BUGID%%" .
svn propset "bugtraq:label" "Card" .
svn propset "bugtraq:number" "yes" .
svn propset "bugtraq:url" "http://%1/projects/%2/cards/%%BUGID%%" .
svn propset "tsvn:logminsize" "3" .

echo -
echo creating standard project files...
REM %~dp0 expands to the drive & directory of this script
set commonBuildDir=%~dp0
set templatesDir=%commonBuildDir%templates
echo ...your project file is called %1.proj
if not exist %2.proj copy %templatesDir%\template.proj %2.proj
if not exist environments (
    echo ...creating environment property files in the environments directory
    mkdir environments
    copy %templatesDir%\env.properties environments\dev.properties
    copy %templatesDir%\env.properties environments\qa.properties
    copy %templatesDir%\env.properties environments\staging.properties
    copy %templatesDir%\env.properties environments\prod.properties
)

echo ...creating build.bat, run build.bat with the /? parameter for help
echo @echo off> build.bat
echo set project=%2.proj>> build.bat
type %templatesDir%\build.bat>> build.bat

echo -
echo All done!
echo Try running "build ci" from the command line to make sure it all worked and checkin your changes 
goto End

:Usage
echo Usage: %0  mingle-server-url mingle-project-name
echo     Adds standard svn properties to your project directory
echo     Also sets up your basic .proj and build.bat files
echo BE SURE TO RUN FROM THE PROJECT DIRECTORY!
echo     e.g. References/common-build/create-project.cmd
echo     instead of cd'ing into that directory before running.

:End
