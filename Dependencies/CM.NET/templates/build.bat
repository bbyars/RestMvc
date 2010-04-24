
if "%1" == "/?" goto Usage
if "%1" == "-?" goto Usage
if "%1" == "--help" goto Usage

set msbuild=%windir%\Microsoft.NET\Framework\v3.5\MSBuild.exe
set command=%msbuild% %project%

rem The first argument, if given acts as the target; all others will be parameters
set nextFlag=/t

:ArgsLoop
if not "%~1" == "" (
    set command=%command% %nextFlag%:"%1"
    set nextFlag=/p
    shift
    goto ArgsLoop
)

echo %command%
call %command%
goto End

:Usage
echo %0 [target ["paramKey=paramValue" [...]]]
echo Make sure to quote all "key=value" parameters, or the shell will interpret the equals sign!
echo Examples:
echo %0                                                  ;; runs the default target
echo %0 test                                             ;; runs the test target
echo %0 test "PackageDir=package" "DBServer=localhost"   ;; runs test with parameters

:End