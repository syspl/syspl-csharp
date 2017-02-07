@echo Off
set config=%1
if "%config%" == "" (
   set config=Release
)

set version=
if not "%BuildCounter%" == "" (
   set version=--version-suffix ci-%BuildCounter%
)

REM Restore
call dotnet restore
if not "%errorlevel%"=="0" goto failure

REM Build
call dotnet build src\ --configuration %config%
if not "%errorlevel%"=="0" goto failure

REM Unit tests
call dotnet test test\ --configuration %config%
if not "%errorlevel%"=="0" goto failure

REM Package
mkdir %cd%\Artifacts
call dotnet pack src\ --configuration %config% %version% --output Artifacts
if not "%errorlevel%"=="0" goto failure

:success
exit 0

:failure
exit -1