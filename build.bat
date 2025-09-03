@echo off
setlocal enabledelayedexpansion

echo [92m========================================[0m
echo [92m           BUILD PROCESS STARTED        [0m
echo [92m========================================[0m
echo.

echo [96m[1/4][0m Preparing to build application...
if not exist "App" (
    echo [91mERROR: App directory not found![0m
    exit /b 1
)

echo [96m[2/4][0m Building .NET application...
cd App
dotnet publish -c Release -o ../temp
if %ERRORLEVEL% neq 0 (
    echo [91mERROR: .NET publish failed![0m
    exit /b %ERRORLEVEL%
)
echo [92mApplication build completed successfully.[0m
echo.

echo [96m[3/4][0m Building MSI installer...
cd ../Msi
msbuild Msi.wixproj /t:Build /p:Platform=x64 /p:Configuration=Release /p:BuildProjectReferences=false /p:OutputPath=../build
if %ERRORLEVEL% neq 0 (
    echo [91mERROR: MSI build failed![0m
    exit /b %ERRORLEVEL%
)
echo [92mMSI build completed successfully.[0m
echo.

echo [96m[4/4][0m Cleaning up temporary files...
cd ..
rmdir /s /q temp
if %ERRORLEVEL% neq 0 (
    echo [93mWARNING: Could not clean up temp directory.[0m
) else (
    echo [92mCleanup completed successfully.[0m
)

cd ..
echo.
echo [92m========================================[0m
echo [92m       BUILD PROCESS COMPLETED          [0m
echo [92m========================================[0m

endlocal
