[![Build Status](https://dev.azure.com/lextudio/monodevelop.netcoredbg/_apis/build/status/lextudio.monodevelop.netcoredbg?branchName=master)](https://dev.azure.com/lextudio/monodevelop.netcoredbg/_build/latest?definitionId=7&branchName=master)

Samsung .NET Core Debugger Extension for MonoDevelop
====================================================
MonoDevelop does not have built-in .NET Core debugging support, due to the
fact that Microsoft does not license its own .NET Core debugger (vsdbg) to
anything other than Visual Studio/Visual Studio Code.

This extension uses the open source .NET Core debugger provided by Samsung to
fill the gaps.

The code is released under MIT/X11 license.

Copyright (C) 2019 LeXtudio Inc. (http://www.lextudio.com)

**Please use with caution and report issues here.**

Installation
------------
1. Open `Extension Manager`, and switch to `Gallery` tab.
1. Choose `.NET Core support for Mono.Debugging` under `Debugging` category, and click `Install...` to install it.
1. Download precompiled netcoredbg binaries from https://github.com/lextudio/monodevelop.netcoredbg/releases.
1. Extract the files from the ZIP package to your user folder `~/netcoredbg/bin`. Run `~/netcoredbg/bin/netcoredbg --help` to confirm everything works. (Note that on Windows you should copy to My Documents).

Then you should be able to debug .NET Core projects in MonoDevelop.

Note that you can also use `Preferences` to select `Samsung .NET Core debugger` location, if you don't want to use the default path,

![Preferences](preferences.png)

Known Issues
------------
* Some breakpoints might not break.

Debugging
---------
If you like, you can debug the code on your own.

1. Open `Extension Manager`, and switch to `Gallery` tab.
1. Choose `AddinMaker` under `Extension Development` category, and click `Install...` to install it.
1. Clone this repo to your local drive, and open `MonoDevelop.Debugger.DotNetCore.sln` in MonoDevelop (or Visual Studio for Mac).
1. Debug the project, so a second instance of MonoDevelop is launched. You can set break points to follow the workflow.
