@echo off
setlocal enabledelayedexpansion

powershell -Command "Write-Host '========================================' -ForegroundColor Green"
powershell -Command "Write-Host '           BUILD PROCESS STARTED        ' -ForegroundColor Green"
powershell -Command "Write-Host '========================================' -ForegroundColor Green"
echo.

powershell -Command "Write-Host '[1/3] Building solution...' -ForegroundColor Cyan"
if not exist "FolderCreator.sln" (
    powershell -Command "Write-Host 'ERROR: Solution file not found!' -ForegroundColor Red"
    exit /b 1
)

rem Build the solution
dotnet build "FolderCreator.sln" -c Release
if %ERRORLEVEL% neq 0 (
    powershell -Command "Write-Host 'ERROR: Solution build failed!' -ForegroundColor Red"
    exit /b %ERRORLEVEL%
)
powershell -Command "Write-Host 'Solution build completed successfully.' -ForegroundColor Green"
echo.

powershell -Command "Write-Host '[2/3] Preparing publish folder...' -ForegroundColor Cyan"
set "ROOT_DIR=%CD%"
set "PUBLISH_DIR=%ROOT_DIR%\build"

if not exist "!PUBLISH_DIR!" (
    mkdir "!PUBLISH_DIR!"
)

rem Locate the MSI file
set "MSI_SOURCE=%ROOT_DIR%\Msi\bin\x86\Release\Msi.msi"
set "MSI_DESTINATION=%PUBLISH_DIR%\Aplikacja Folderowa.msi"

if exist "!MSI_SOURCE!" (
    copy /y "!MSI_SOURCE!" "!MSI_DESTINATION!"
    if %ERRORLEVEL% neq 0 (
        powershell -Command "Write-Host 'ERROR: Failed to copy MSI file to publish folder!' -ForegroundColor Red"
        exit /b %ERRORLEVEL%
    )
    powershell -Command "Write-Host 'MSI file copied and renamed successfully.' -ForegroundColor Green"
) else (
    powershell -Command "Write-Host 'ERROR: MSI file not found!' -ForegroundColor Red"
    exit /b 1
)
echo.

powershell -Command "Write-Host '[3/3] Cleaning up build artifacts...' -ForegroundColor Cyan"
for /d %%d in ("%ROOT_DIR%\*\bin") do (
    rmdir /s /q "%%d"
)
for /d %%d in ("%ROOT_DIR%\*\obj") do (
    rmdir /s /q "%%d"
)
powershell -Command "Write-Host 'Build artifacts cleaned up successfully.' -ForegroundColor Green"
echo.

powershell -Command "Write-Host '========================================' -ForegroundColor Green"
powershell -Command "Write-Host '       BUILD PROCESS COMPLETED          ' -ForegroundColor Green"
powershell -Command "Write-Host '========================================' -ForegroundColor Green"

endlocal