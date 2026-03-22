@echo off
echo ========================================
echo 清除 Windows 图标缓存
echo ========================================
echo.

REM 停止 Windows 资源管理器
echo [1/4] 停止 Windows 资源管理器...
taskkill /f /im explorer.exe
timeout /t 2 /nobreak >nul

REM 删除图标缓存数据库
echo [2/4] 删除图标缓存数据库...
if exist "%userprofile%\AppData\Local\IconCache.db" del /f /q "%userprofile%\AppData\Local\IconCache.db"
if exist "%userprofile%\AppData\Local\IconCache.db" del /f /q "%userprofile%\AppData\Local\IconCache.db"
if exist "%localappdata%\IconCache.db" del /f /q "%localappdata%\IconCache.db"

REM 删除缩略图缓存
echo [3/4] 删除缩略图缓存...
if exist "%userprofile%\AppData\Local\Microsoft\Windows\Explorer\*.db" (
    del /f /q "%userprofile%\AppData\Local\Microsoft\Windows\Explorer\iconcache*" >nul 2>&1
    del /f /q "%userprofile%\AppData\Local\Microsoft\Windows\Explorer\thumbcache*" >nul 2>&1
)

REM 重启 Windows 资源管理器
echo [4/4] 重启 Windows 资源管理器...
start explorer.exe

echo.
echo ========================================
echo 图标缓存已清除！
echo 现在应该能看到正确的安装包图标了
echo ========================================
echo.
pause
