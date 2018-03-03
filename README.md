# IceChat

## IceChat IRC Client v9

IceChat is an open source IRC Client written in C# using WinForms.

It has been fully released, but always undergoing development and improvements.

It is RFC 1459 Compliant.

Main website is http://www.icechat.net and wiki page setup at http://wiki.icechat.net 

IceChat uses .Net Framework v3.5 by default.

If you are experiencing connection problems to certain SSL Servers, run the BuildIceChat-4.5.cmd file in the build folder, to rebuild IceChat 9 with .Net 4.5
There are also just binaries of IceChat built with .NET 4.5 sitting in the Releases section.

## Building from Source Code

http://wiki.icechat.net/index.php?title=Build_from_source_code

IceChat is coded using Visual Studio Express 2008. There are solutions for Visual Studio 2010 and Visual Studio 2015 included in the source code.
You do not need Visual Studio to build IceChat though, you can just use the Batch Files supplied in the build folder.

Unzip the file into any folder. It is best to use My Documents and not the Program Files folder, because of permissions issues.

There is an BuildIceChat.cmd file in the build folder which will build IceChat 9.

If you have a problem with BuildIceChat.cmd erroring about msbuild, try BuildIceChat.4.0.cmd

If you want to build IceChat using .Net 4 Framework, use BuildIceChat.4.5.cmd

You can double click the file, and it will open a command window.

A new folder called IceChatBuild will be created, and once the batch file has completed, 4 files will be placed in this folder.

Copy these 4 files into your IceChat installation/Program Files folder, or place them anywhere else you want.

To see all the changes from build to build, read the changelog.txt file in the src\IceChat folder.
