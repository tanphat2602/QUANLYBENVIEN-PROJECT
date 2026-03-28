@echo off
setlocal enabledelayedexpansion
title QuanLyBenhVien - Hospital Management System
color 0A

echo ================================================
echo   QuanLyBenhVien - Hospital Management System
echo ================================================
echo.

:: Check if Java is installed
java -version >nul 2>&1
if %errorlevel% neq 0 (
    echo [ERROR] Java is not installed or not in PATH!
    echo Please install Java 17 or higher.
    pause
    exit /b 1
)

:: Kill any existing processes on ports 5000 and 5001
echo [PREPARE] Checking and cleaning ports...
for /f "tokens=5" %%a in ('netstat -aon ^| findstr :5000 ^| findstr LISTENING') do (
    echo Killing process PID %%a on port 5000...
    taskkill /F /PID %%a >nul 2>&1
)
for /f "tokens=5" %%a in ('netstat -aon ^| findstr :5001 ^| findstr LISTENING') do (
    echo Killing process PID %%a on port 5001...
    taskkill /F /PID %%a >nul 2>&1
)
timeout /t 3 /nobreak >nul
echo.

echo [1/3] Starting Backend (Spring Boot on port 5000)...
echo.
start "Backend - Spring Boot" cmd /k "cd /d "%~dp0SpringBoot" && mvn spring-boot:run -DskipTests"

:: Wait for backend to start using PowerShell
echo Waiting for backend to start (max 45 seconds)...
set backend_ok=0
for /L %%i in (1,1,45) do (
    timeout /t 1 /nobreak >nul
    powershell -Command "try { Invoke-WebRequest -Uri 'http://localhost:5000/api/auth/login' -Method POST -ContentType 'application/json' -Body '{\"username\":\"test\"}' -UseBasicParsing -TimeoutSec 2 | Out-Null; exit 0 } catch { exit 1 }"
    if !errorlevel! equ 0 (
        set backend_ok=1
        echo [OK] Backend started on port 5000!
        goto :backend_ready
    )
    if %%i equ 15 echo   Still waiting...
    if %%i equ 30 echo   Still waiting...
)
:backend_ready
if %backend_ok%==0 (
    echo.
    echo [ERROR] Backend FAILED to start after 45 seconds!
    echo Check the Backend window for errors.
    pause
    exit /b 1
)

echo.
echo [2/3] Starting Frontend (ASP.NET Core on port 5001)...
echo.
start "Frontend - ASP.NET Core" cmd /k "cd /d "%~dp0Frontend" && dotnet run"

:: Wait for frontend to start using PowerShell
echo Waiting for frontend to start (max 30 seconds)...
set frontend_ok=0
for /L %%i in (1,1,30) do (
    timeout /t 1 /nobreak >nul
    powershell -Command "try { Invoke-WebRequest -Uri 'http://localhost:5001' -UseBasicParsing -TimeoutSec 2 | Out-Null; exit 0 } catch { exit 1 }"
    if !errorlevel! equ 0 (
        set frontend_ok=1
        echo [OK] Frontend started on port 5001!
        goto :frontend_ready
    )
    if %%i equ 10 echo   Still waiting...
    if %%i equ 20 echo   Still waiting...
)
:frontend_ready
if %frontend_ok%==0 (
    echo.
    echo [ERROR] Frontend FAILED to start after 30 seconds!
    echo Check the Frontend window for errors.
    pause
    exit /b 1
)

echo.
echo [3/3] Opening web browser...
start http://localhost:5001

echo.
echo ================================================
echo   System is running successfully!
echo   - Backend: http://localhost:5000
echo   - Frontend: http://localhost:5001
echo ================================================
echo.
echo Demo accounts:
echo   Admin:        admin / 1
echo   Doctor:       dr.nguyenvana / 123456
echo   Patient:      patient1 / 123456
echo   Receptionist: receptionist1 / 123456
echo   Pharmacist:   pharmacist1 / 123456
echo.
echo Press any key to exit...
pause >nul
endlocal
