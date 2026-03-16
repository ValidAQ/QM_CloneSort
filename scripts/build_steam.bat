@echo off
REM Build the mod and deploy it to the Steam Workshop.
REM Reads the Workshop item ID from SteamWorkshopId.txt and passes it to the build.
setlocal

REM Change to the project root (this script lives in the scripts/ subfolder).
cd /d "%~dp0.."

REM Ensure the Workshop ID file exists.
if not exist SteamWorkshopId.txt (
    echo ERROR: SteamWorkshopId.txt not found.
    exit /b 1
)

REM Read the Workshop item ID from the file.
set /p STEAM_ID=<SteamWorkshopId.txt
if "%STEAM_ID%"=="" (
    echo ERROR: SteamWorkshopId.txt is empty.
    exit /b 1
)

REM Find the .csproj file in the src directory.
for %%f in (src\*.csproj) do set CSPROJ=%%f

REM Build the project and pass the Steam Workshop ID for deployment.
dotnet build %CSPROJ% /p:SteamId=%STEAM_ID%
if %errorlevel% neq 0 (
    echo Build and deploy failed.
    exit /b %errorlevel%
)
echo Build and deploy to Steam workshop %STEAM_ID% succeeded.
