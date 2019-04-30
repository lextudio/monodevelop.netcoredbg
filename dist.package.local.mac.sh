msbuild MonoDevelop.Debugger.DotNetCore.sln /t:clean
msbuild MonoDevelop.Debugger.DotNetCore.sln
mono /Applications/Visual\ Studio.app/Contents/Resources/lib/monodevelop/bin/vstool.exe setup pack MonoDevelop.Debugger.DotNetCore/bin/Debug/net471/MonoDevelop.Debugger.DotNetCore.dll
