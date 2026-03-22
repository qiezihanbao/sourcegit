# SourceGit 编译和打包指南

## 1. 发布纯净版 Release 包

```bash
dotnet publish -c Release -r win-x64 -o ./publish/win-x64 -p:DisableAOT=true -p:DebugType=None -p:DebugSymbols=false src/SourceGit.csproj
```

**说明：**
- `DisableAot=true` - 禁用AOT编译（推荐，性能影响微小）
- `DebugType=None` - 不生成PDB调试文件
- `DebugSymbols=false` - 不包含调试符号

**输出位置：** `publish/win-x64\`
**包大小：** 约 62MB（纯净版）

---

## 2. AOT 编译修复（可选，不推荐）

### 问题原因
Git Bash 的 `link.exe` 干扰了 Visual Studio 的 MSVC `link.exe`

### 修复方法

**方法一：临时从PATH移除Git（推荐）**
```powershell
# 在发布前临时修改PATH
$env:Path = ($env:Path -split ';' | Where-Object { $_ -notlike '*Git\usr\bin*' }) -join ';'
dotnet publish -c Release -r win-x64 -o ./publish/win-x64-aot src/SourceGit.csproj
```

**方法二：使用Developer Command Prompt**
```bash
# 打开 "Developer Command Prompt for VS 2022"
# 然后运行发布命令
```

**方法三：设置环境变量**
```bash
set VCINSTALLDIR=C:\Program Files\Microsoft Visual Studio\18\Community\VC\Tools\MSVC\14.44.35207
set PATH=C:\Program Files\Microsoft Visual Studio\18\Community\VC\Tools\MSVC\14.44.35207\bin\Hostx64\x64;%PATH%
dotnet publish -c Release -r win-x64 -o ./publish/win-x64-aot src/SourceGit.csproj
```

### AOT 优缺点
✅ 优点：
- 启动速度快 10-20%
- 包体积减少 5-10MB

❌ 缺点：
- 编译时间长 3-5 分钟
- 某些反射功能可能失效
- 兼容性问题

**建议：** 发布正式版本使用 DisableAOT，用户体验更好。

---

## 3. 创建 exe 安装包

### 步骤 1: 安装 Inno Setup

下载地址：https://jrsoftware.org/isdl.php

或使用 Chocolatey：
```bash
choco install innosetup
```

### 步骤 2: 编译安装包

**方法一：使用批处理脚本（推荐）**
```bash
BUILD_INSTALLER.bat
```

**方法二：手动编译**
```bash
"C:\Program Files (x86)\Inno Setup 6\ISCC.exe" installer.iss
```

### 安装包功能

✅ **安装功能：**
- 自动安装到 `C:\Program Files\SourceGit`
- 创建开始菜单快捷方式
- 创建桌面快捷方式（可选）
- 写入注册表关联（.git 文件和文件夹右键菜单）
- 创建卸载程序

✅ **注册表项：**
- 文件夹右键菜单："在 SourceGit 中打开"
- .git 文件关联
- 驱动器右键菜单

✅ **卸载功能：**
- 完整卸载所有文件
- 清除注册表项
- 清除用户数据（可选）

### 输出位置
`install\SourceGit-Setup-2026.06-win-x64.exe`

---

## 4. 字体说明

只包含 4 个必要字体（约 600KB）：
- ✅ MiSans-Regular.ttf - UI 默认字体
- ✅ MiSans-Bold.ttf - UI 粗体
- ✅ JetBrainsMono-Regular.ttf - 代码等宽字体
- ✅ JetBrainsMono-Italic.ttf - 代码斜体

删除了 12 个未使用的字体，节省约 2MB

---

## 5. 版本更新

修改 `VERSION` 文件中的版本号，然后重新编译：

```bash
echo 2026.07 > VERSION
dotnet publish -c Release -r win-x64 -o ./publish/win-x64 -p:DisableAOT=true -p:DebugType=None -p:DebugSymbols=false src/SourceGit.csproj
```

---

## 6. 完整打包流程

```bash
# 1. 清理旧文件
rm -rf ./publish ./install

# 2. 发布 Release 版本
dotnet publish -c Release -r win-x64 -o ./publish/win-x64 -p:DisableAOT=true -p:DebugType=None -p:DebugSymbols=false src/SourceGit.csproj

# 3. 编译安装包
BUILD_INSTALLER.bat

# 完成！
# - 绿色版：publish/win-x64/
# - 安装版：install/SourceGit-Setup-2026.06-win-x64.exe
```

---

## 文件结构

```
D:\Git\sourcegit\
├── installer.iss          # Inno Setup 安装脚本
├── BUILD_INSTALLER.bat    # 安装包编译脚本
├── VERSION                # 版本号文件
├── publish/
│   └── win-x64/           # 绿色版（62MB）
└── install/
    └── SourceGit-Setup-2026.06-win-x64.exe  # 安装包（约25MB）
```

---

## 常见问题

### Q: AOT 编译失败怎么办？
A: 使用 `-p:DisableAOT=true` 参数，性能影响可忽略。

### Q: 如何修改安装目录？
A: 编辑 `installer.iss` 中的 `DefaultDirName`。

### Q: 如何添加更多语言支持？
A: 在 `installer.iss` 的 `[Languages]` 段添加更多语言。

### Q: 如何自定义安装包图标？
A: 在 `installer.iss` 中添加 `[Setup] Icon=youricon.ico`。

### Q: 安装包太大怎么办？
A: 已经使用 LZMA2 最大压缩，62MB 是合理大小。

---

## 技术支持

- GitHub: https://github.com/sourcegit-scm/sourcegit
- 问题反馈: https://github.com/sourcegit-scm/sourcegit/issues
