//
// DotNetCoreDebuggerEngine.cs
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
using Mono.Debugging.Client;
using MonoDevelop.Core.Execution;
using MonoDevelop.DotNetCore;

namespace MonoDevelop.Debugger.DotNetCore
{
    class DotNetCoreDebuggerEngine : DebuggerEngineBackend
    {
        #region IDebuggerEngine Members

        public override bool CanDebugCommand(ExecutionCommand cmd)
        {
            if (cmd is DotNetCoreExecutionCommand dotnet)
            {
                return true;
            }

            return cmd.GetType().Name == "AspNetCoreExecutionCommand";
        }

        public override DebuggerStartInfo CreateDebuggerStartInfo(ExecutionCommand cmd)
        {
            if (cmd is DotNetCoreExecutionCommand command)
            {
                DebuggerStartInfo startInfo = new DebuggerStartInfo
                {
                    Command = command.OutputPath,
                    Arguments = command.DotNetArguments,
                    WorkingDirectory = command.WorkingDirectory
                };
                if (command.EnvironmentVariables.Count > 0)
                {
                    foreach (KeyValuePair<string, string> val in command.EnvironmentVariables)
                        startInfo.EnvironmentVariables[val.Key] = val.Value;
                }

                return startInfo;
            }
            else if (cmd.GetType().Name == "AspNetCoreExecutionCommand")
            {
                DebuggerStartInfo startInfo = new DebuggerStartInfo
                {
                    Command = (string)GetPropValue(cmd, "OutputPath"),
                    Arguments = (string)GetPropValue(cmd, "DotNetArguments"),
                    WorkingDirectory = (string)GetPropValue(cmd, "WorkingDirectory")
                };
                var variables = (IDictionary<string, string>)GetPropValue(cmd, "EnvironmentVariables");
                if (variables.Count > 0)
                {
                    foreach (KeyValuePair<string, string> val in variables)
                        startInfo.EnvironmentVariables[val.Key] = val.Value;
                }

                return startInfo;
            }

            throw new NotSupportedException("Only .NET Core projects are supported.");
        }

        private static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }

        public override DebuggerSession CreateSession()
        {
            return new DotNetCoreDebuggerSession();
        }

        #endregion
    }
}
