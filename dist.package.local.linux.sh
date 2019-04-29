msbuild MonoDevelop.Debugger.DotNetCore.sln /t:clean
msbuild MonoDevelop.Debugger.DotNetCore.sln
cd MonoDevelop.Debugger.DotNetCore/bin/Debug/net471
mdtool setup pack MonoDevelop.Debugger.DotNetCore.dll
