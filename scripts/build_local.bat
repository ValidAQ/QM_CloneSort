@echo off
REM Build the mod locally without deploying.
REM DeployPath is left empty so the output stays in the default bin/Debug folder.

REM Change to the project root (this script lives in the scripts/ subfolder).
cd /d "%~dp0.."

REM Find the .csproj file in the src directory.
for %%f in (src\*.csproj) do set CSPROJ=%%f

REM Build the project.
dotnet build %CSPROJ% /p:DeployPath=
if %errorlevel% neq 0 (
    echo Build failed.
    exit /b %errorlevel%
)
echo Build succeeded.
