@echo off
title Stopping QuanLyBenhVien
color 0C

echo ================================================
echo   Stopping QuanLyBenhVien System
echo ================================================
echo.

:: Kill Backend (Spring Boot - Java)
echo Stopping Backend (Java)...
taskkill /F /IM java.exe >nul 2>&1
taskkill /F /IM javaw.exe >nul 2>&1

:: Kill Frontend (.NET)
echo Stopping Frontend (.NET)...
taskkill /F /IM dotnet.exe >nul 2>&1

:: Kill windows by title
taskkill /F /FI "WINDOWTITLE eq Backend*" >nul 2>&1
taskkill /F /FI "WINDOWTITLE eq Frontend*" >nul 2>&1

echo.
echo [OK] All processes stopped.
timeout /t 1 /nobreak >nul
