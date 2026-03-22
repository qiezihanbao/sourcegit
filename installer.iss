; SourceGit 安装程序脚本
; 需要先安装 Inno Setup: https://jrsoftware.org/isdl.php

#define AppName "SourceGit"
#define AppVersion "2026.06"
#define AppPublisher "sourcegit-scm"
#define AppURL "https://github.com/sourcegit-scm/sourcegit"
#define AppExeName "SourceGit.exe"

[Setup]
AppId={{A1B2C3D4-E5F6-7890-ABCD-EF1234567890}
AppName={#AppName}
AppVersion={#AppVersion}
AppPublisher={#AppPublisher}
AppPublisherURL={#AppURL}
AppSupportURL={#AppURL}
AppUpdatesURL={#AppURL}
DefaultDirName={autopf}\{#AppName}
DefaultGroupName={#AppName}
AllowNoIcons=yes
OutputDir=.\install
OutputBaseFilename=SourceGit-Setup-{#AppVersion}-win-x64
Compression=lzma2/max
SolidCompression=yes
WizardStyle=modern
PrivilegesRequired=admin
ArchitecturesAllowed=x64
ArchitecturesInstallIn64BitMode=x64
UninstallDisplayIcon={app}\{#AppExeName}
DisableDirPage=no
DisableProgramGroupPage=yes
; Setup icons
SetupIconFile=src\App.ico
WizardImageFile=build/resources/_common/icons/sourcegit.png
WizardSmallImageFile=build/resources/_common/icons/sourcegit.png

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked
Name: "quicklaunchicon"; Description: "{cm:CreateQuickLaunchIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked; OnlyBelowVersion: 6.1

[Files]
Source: ".\publish\win-x64\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{group}\{#AppName}"; Filename: "{app}\{#AppExeName}"
Name: "{group}\{cm:UninstallProgram,{#AppName}}"; Filename: "{uninstallexe}"
Name: "{autodesktop}\{#AppName}"; Filename: "{app}\{#AppExeName}"; Tasks: desktopicon
Name: "{userappdata}\Microsoft\Internet Explorer\Quick Launch\{#AppName}"; Filename: "{app}\{#AppExeName}"; Tasks: quicklaunchicon

[Run]
Filename: "{app}\{#AppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(AppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

[Registry]
; 文件关联
Root: HKCR; Subkey: ".git\OpenWithProgids"; ValueType: string; ValueName: "SourceGit.git"; ValueData: ""; Flags: uninsdeletevalue
Root: HKCR; Subkey: "SourceGit.git"; ValueType: string; ValueName: ""; ValueData: "Git Repository"; Flags: uninsdeletekey
Root: HKCR; Subkey: "SourceGit.git\DefaultIcon"; ValueType: string; ValueData: "{app}\{#AppExeName},0"
Root: HKCR; Subkey: "SourceGit.git\shell\open\command"; ValueType: string; ValueData: """{app}\{#AppExeName}"" ""%1"""
Root: HKCR; Subkey: "Directory\Background\shell\{#AppName}"; ValueType: string; ValueName: ""; ValueData: "Open with {#AppName}"; Flags: uninsdeletekey
Root: HKCR; Subkey: "Directory\Background\shell\{#AppName}\command"; ValueType: string; ValueData: """{app}\{#AppExeName}"" ""%V"""
Root: HKCR; Subkey: "Directory\shell\{#AppName}"; ValueType: string; ValueName: ""; ValueData: "Open with {#AppName}"; Flags: uninsdeletekey
Root: HKCR; Subkey: "Directory\shell\{#AppName}\command"; ValueType: string; ValueData: """{app}\{#AppExeName}"" ""%1"""
Root: HKCR; Subkey: "Drive\shell\{#AppName}"; ValueType: string; ValueName: ""; ValueData: "Open with {#AppName}"; Flags: uninsdeletekey
Root: HKCR; Subkey: "Drive\shell\{#AppName}\command"; ValueType: string; ValueData: """{app}\{#AppExeName}"" ""%1"""

[UninstallDelete]
Type: filesandordirs; Name: "{userappdata}\{#AppName}"
