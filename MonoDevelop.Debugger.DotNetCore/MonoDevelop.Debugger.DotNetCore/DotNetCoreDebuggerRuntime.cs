//
// DotNetCoreDebuggerRuntime.cs
//
// Author:
//       Matt Ward <matt.ward@xamarin.com>
//       Lex Li <support@lextm.com>
//
// Copyright (c) 2017 Xamarin Inc. (http://xamarin.com)
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
using MonoDevelop.Core;

namespace MonoDevelop.Debugger.DotNetCore
{
    public static class DotNetCoreDebuggerRuntime
    {
        internal static readonly string DotNetCoreDebuggerRuntimeFileNameProperty = "DotNetCoreDebuggerRuntimeFileName";

        static DotNetCoreDebuggerRuntime()
        {
            Init(GetDotNetCoreDebuggerPath());

            if (!IsInstalled)
                LoggingService.LogInfo(".NET Core debugger not found.");
        }

        static DotNetCoreDebuggerPath GetDotNetCoreDebuggerPath()
        {
            string fileName = PropertyService.Get<string>(DotNetCoreDebuggerRuntimeFileNameProperty);

            if (!string.IsNullOrEmpty(fileName))
                return new DotNetCoreDebuggerPath(fileName);

            return new DotNetCoreDebuggerPath();
        }

        static void Init(DotNetCoreDebuggerPath path)
        {
            IsInstalled = !path.IsMissing;
            FileName = path.FileName;
        }

        internal static void Update(DotNetCoreDebuggerPath path)
        {
            if (path.FileName == FileName)
                return;

            Init(path);

            if (path.IsDefault())
            {
                PropertyService.Set(DotNetCoreDebuggerRuntimeFileNameProperty, null);
            }
            else
            {
                PropertyService.Set(DotNetCoreDebuggerRuntimeFileNameProperty, path.FileName);
            }

            OnChanged();
        }

        static internal event EventHandler Changed;

        static void OnChanged()
        {
            Changed?.Invoke(null, EventArgs.Empty);
        }

        public static string FileName { get; private set; }

        public static bool IsInstalled { get; private set; }

        public static bool IsMissing
        {
            get { return !IsInstalled; }
        }

        /// <summary>
        /// Used by unit tests to fake having the sdk installed.
        /// </summary>
        internal static void SetInstalled(bool installed)
        {
            IsInstalled = installed;
        }
    }
}
