//
// DotNetCoreDebuggerSession.cs
//
// Author:
//       Lex Li <support@lextm.com>
//
// Copyright (c) 2019 LeXtudio Inc. (http://www.lextudio.com)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.Shared.VSCodeDebugProtocol;
using Microsoft.VisualStudio.Shared.VSCodeDebugProtocol.Messages;
using Mono.Debugging.Client;
using MonoDevelop.Core;
using MonoDevelop.Debugger.VsCodeDebugProtocol;
using Newtonsoft.Json.Linq;

namespace MonoDevelop.Debugger.DotNetCore
{
    public class DotNetCoreDebuggerSession : VSCodeDebuggerSession
    {
        protected override AttachRequest CreateAttachRequest(long processId)
        {
            throw new NotImplementedException();
        }

        protected override InitializeRequest CreateInitRequest()
        {
            var result = new InitializeRequest();
            result.Args.ClientID = "monodevelop";
            result.Args.AdapterID = "dotnetcore";
            return result;
        }

        protected override LaunchRequest CreateLaunchRequest(DebuggerStartInfo startInfo)
        {
            var properties = new Dictionary<string, JToken>();
            properties.Add("args", JToken.FromObject(startInfo.Arguments.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)));
            properties.Add("program", JToken.FromObject(startInfo.Command));
            var result = new LaunchRequest();
            result.Args.ConfigurationProperties = properties;
            return result;
        }

        protected override string GetDebugAdapterPath()
        {
            var path = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                "netcoredbg",
                "bin",
                Platform.IsWindows ? "netcoredbg.exe" : "netcoredbg");
            if (!File.Exists(path))
            {
                throw new ProtocolException($"Samsung .NET Core debugger is not at {path}. Please download and configure it before debugging a .NET Core project.");
            }

            return path;
        }

        protected override string GetDebugAdapterArguments()
        {
            return "--interpreter=vscode";
        }
    }
}
