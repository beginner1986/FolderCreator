@echo off
setlocal enabledelayedexpansion

powershell -Command "Write-Host '========================================' -ForegroundColor Green"
powershell -Command "Write-Host '           BUILD PROCESS STARTED        ' -ForegroundColor Green"
powershell -Command "Write-Host '========================================' -ForegroundColor Green"
echo.

powershell -Command "Write-Host '[1/4] Preparing to build application...' -ForegroundColor Cyan"
if not exist "App" (
    powershell -Command "Write-Host 'ERROR: App directory not found!' -ForegroundColor Red"
    exit /b 1
)

powershell -Command "Write-Host '[2/4] Building .NET application...' -ForegroundColor Cyan"
cd App
dotnet publish -c Release -o ../temp
if %ERRORLEVEL% neq 0 (
    powershell -Command "Write-Host 'ERROR: .NET publish failed!' -ForegroundColor Red"
    exit /b %ERRORLEVEL%
)
powershell -Command "Write-Host 'Application build completed successfully.' -ForegroundColor Green"
echo.

powershell -Command "Write-Host '[3/4] Building MSI installer...' -ForegroundColor Cyan"
cd ../Msi
msbuild Msi.wixproj /t:Build /p:Platform=x64 /p:Configuration=Release /p:BuildProjectReferences=false /p:OutputPath=../build
if %ERRORLEVEL% neq 0 (
    powershell -Command "Write-Host 'ERROR: MSI build failed!' -ForegroundColor Red"
    exit /b %ERRORLEVEL%
)
powershell -Command "Write-Host 'MSI build completed successfully.' -ForegroundColor Green"
echo.

powershell -Command "Write-Host '[4/4] Cleaning up temporary files...' -ForegroundColor Cyan"
cd ..
rmdir /s /q temp
if %ERRORLEVEL% neq 0 (
    powershell -Command "Write-Host 'WARNING: Could not clean up temp directory.' -ForegroundColor Yellow"
) else (
    powershell -Command "Write-Host 'Cleanup completed successfully.' -ForegroundColor Green"
)
echo.

powershell -Command "Write-Host '========================================' -ForegroundColor Green"
powershell -Command "Write-Host '       BUILD PROCESS COMPLETED          ' -ForegroundColor Green"
powershell -Command "Write-Host '========================================' -ForegroundColor Green"

endlocal
