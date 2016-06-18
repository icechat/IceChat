Welcome to the VB Script Plugin

This plugin was created so that the VB Scripts from IceChat 7 could be run in IceChat 9.

This is far from complete, as a lot of the custom commands still need to be added.

There is one major thing to make it work. IceChat has to be compiled in 32 bit.
The plugin will not work in 64bit. This is because the Microsoft Script control will not work in 64bit.

The error for the line ' module.Run(method, ref param);' can just be ignored, as it works perfectly like that.

There is an IceChat-x86.sln solution file created in the IceChat source code, so you can quickly compile IceChat 9 in 32bit mode.
The resulting EXE and DLL's will be placed in the bin/x86/debug folder.