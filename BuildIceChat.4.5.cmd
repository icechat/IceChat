@echo off
echo Building IceChat using .Net Framework v4.5

rd .\IceChatBuild /S /Q
md .\IceChatBuild

set msBuildDir=%WINDIR%\Microsoft.NET\Framework\v4.0.30319
call %msBuildDir%\msbuild.exe IceChat9-45-VS2015.sln /p:Configuration=Release /p:Platform="Any CPU" /l:FileLogger,Microsoft.Build.Engine;logfile=Manual_MSBuild_ReleaseVersion_LOG.log

set msBuildDir=

XCOPY .\bin\Debug\IceChat2009.exe .\IceChatBuild\ 
XCOPY .\bin\Debug\IPluginIceChat.dll .\IceChatBuild\ 
XCOPY .\bin\Debug\IRCConnection.dll .\IceChatBuild\ 
XCOPY .\bin\Debug\IThemeIceChat.dll .\IceChatBuild\ 


echo IceChat files copied to the IceChatBuild folder

pause