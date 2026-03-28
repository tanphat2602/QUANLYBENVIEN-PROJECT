@echo off
title Quan Ly Benh Vien
color 0A

echo ========================================
echo   HE THONG QUAN LY BENH VIEN
echo ========================================
echo.

set SCRIPT_DIR=%~dp0

:: Khoi dong Backend
echo [*] Khoi dong Backend API (Port 5000)...
start "" /B cmd /c "cd /d %SCRIPT_DIR%Backend && dotnet run > nul 2>&1"

:: Cho Backend khoi dong
timeout /t 5 /nobreak > nul

:: Khoi dong Frontend
echo [*] Khoi dong Frontend (Port 5001)...
start "" /B cmd /c "cd /d %SCRIPT_DIR%Frontend && dotnet run --urls=http://localhost:5001 > nul 2>&1"

:: Mo trinh duyet
timeout /t 5 /nobreak > nul
start http://localhost:5001

echo.
echo ========================================
echo   DA KHOI DONG THANH CONG!
echo   Backend: http://localhost:5000
echo   Frontend: http://localhost:5001
echo ========================================
echo.
echo Dong cua so nay va mo trinh duyet de su dung...
pause
