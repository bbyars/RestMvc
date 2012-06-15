@echo off

@powershell -ExecutionPolicy RemoteSigned -NoProfile -NonInteractive -Command "& {.\deploy.ps1; exit $LastExitCode }" 

exit /B %errorlevel%