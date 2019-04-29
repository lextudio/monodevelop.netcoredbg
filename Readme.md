Samsung .NET Core Debugger Extension for MonoDevelop
----------------------------------------------------
MonoDevelop does not yet have .NET Core debugging support, due to the fact
that Microsoft does not license its own .NET Core debugger (vsdbg) to
anything other than Visual Studio/Visual Studio Code.

This extension uses the open source .NET Core debugger provided by Samsung to
fill the gaps.

**It is still experimental, so please use with caution and report issues here.**

Installation
============
1. Download the ZIP package from https://github.com/lextudio/monodevelop.netcoredbg/releases/tag/0.1, extract the .mpack file, and install in MonoDevelop 7/8. (No plan to support older releases).
1. Clone netcoredbg source code from my personal fork, https://github.com/lextm/netcoredbg/tree/test and compile from "test" branch so that all patches are included. (Note that you have to use my fork right now, while Samsung is fixing their debugger.)
1. Copy the compiled files from `bin` folder to your user folder `~/netcoredbg/bin`. Run `~/netcoredbg/bin/netcoredbg --help` to confirm everything works. (Note that on Windows you should copy to My Documents).

Then you should be able to debug .NET Core projects in MonoDevelop.

Known Issues
============
* Some breakpoints do not break.
* Locals/watches might not work.

The code is released under MIT/X11 license.
Copyright (C) 2019 LeXtudio Inc. (http://www.lextudio.com)
