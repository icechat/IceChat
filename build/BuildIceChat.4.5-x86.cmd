@echo off
rd .\IceChatBuild /S /Q
md .\IceChatBuild

set msBuildDir=%WINDIR%\Microsoft.NET\Framework\v4.0.30319
call %msBuildDir%\msbuild.exe  ..\src\IceChat\IceChat9-VS2015-x86.sln /p:Configuration=Release /p:Platform="Any CPU" /l:FileLogger,Microsoft.Build.Engine;logfile=Manual_MSBuild_ReleaseVersion_LOG.log
set msBuildDir=

XCOPY ..\src\IceChat\bin\Debug\IceChat2009.exe .\IceChatBuild\ 
XCOPY ..\src\IceChat\bin\Debug\IPluginIceChat.dll .\IceChatBuild\ 
XCOPY ..\src\IceChat\bin\Debug\IRCConnection.dll .\IceChatBuild\ 
XCOPY ..\src\IceChat\bin\Debug\IThemeIceChat.dll .\IceChatBuild\ 


echo IceChat files copied to the IceChatBuild folder

pause