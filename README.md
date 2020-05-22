# IceChat

## IceChat IRC Client v9

IceChat is an open source IRC Client written in C# using WinForms.

It has been fully released, but always undergoing development and improvements.

It is [RFC 1459](https://tools.ietf.org/html/rfc1459.html) Compliant.

The primary website for IceChat is <https://www.icechat.net> and has a wiki page set up at <https://wiki.icechat.net>

IceChat 9.5 uses .NET Framework v4.5.2 by default.

## Building from Source Code

Download the latest source code from the page, by clicking on the Download Zip button.

Unzip the file into any folder, for example a dedicated "C:\IceChatBuild". It is best not to use the [%CommonProgramFiles%](https://en.wikipedia.org/wiki/Environment_variable#Default_values), because of permissions possible limitations.

The source tree has several text files in its root folder, a build folder and a src, where the actual code and resources are located in several sub-folders.
There are several build options in the extracted root folder.

* `BuildIceChat.4.5.cmd`
  * which uses .NET 4.5.
* `BuildIceChat.4.5-x86.cmd`
  * which uses .NET 4.5 and a specific project for x86 - 32bit computers.
* `BuildIceChat.latest.cmd`
  * which uses .NET 4.7.1
* `BuildIceChatLinux.sh`
  * A script to build IceChat 9 under Linux. See IceChat on Linux for details.

You will get at the end of the process the following lines:

```cmd
    0 Error(s)

Time Elapsed 00:00:00.32
..\src\IceChat\bin\Debug\IceChat2009.exe
1 File(s) copied
..\src\IceChat\\bin\Debug\IPluginIceChat.dll
1 File(s) copied
..\src\IceChat\\bin\Debug\IRCConnection.dll
1 File(s) copied
..\src\IceChat\\bin\Debug\IThemeIceChat.dll
1 File(s) copied
IceChat files copied to the IceChatBuild folder
Press any key to continue . . .
```

A new folder is created called IceChatBuild, and once the batch file has completed, 4 files will be placed in this folder.
A log file named Manual_MSBuild_ReleaseVersion_LOG.log is also created.

Copy these 4 files into your IceChat installation folder, or place them anywhere else you want.

To see all the changes from build to build, read the changelog.txt file in the root folder.
