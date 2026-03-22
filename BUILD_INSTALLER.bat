@echo off
echo ========================================
echo SourceGit 安装包编译脚本
echo ========================================
echo.

REM 检查 Inno Setup 是否安装
if not exist "C:\Program Files (x86)\Inno Setup 6\ISCC.exe" (
    if not exist "C:\Program Files\Inno Setup 6\ISCC.exe" (
        echo [错误] 未找到 Inno Setup
        echo 请先安装: https://jrsoftware.org/isdl.php
        pause
        exit /b 1
    )
)

REM 编译安装包
echo [1/2] 正在编译安装包...
"C:\Program Files (x86)\Inno Setup 6\ISCC.exe" installer.iss
if errorlevel 1 (
    echo [错误] 编译失败！
    pause
    exit /b 1
)

echo [2/2] 编译完成！
echo.
echo 安装包位置: install\SourceGit-Setup-2026.06-win-x64.exe
echo.
pause
