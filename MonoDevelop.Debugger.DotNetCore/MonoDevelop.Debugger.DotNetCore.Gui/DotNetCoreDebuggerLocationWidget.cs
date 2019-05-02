//
// DotNetCoreSdkLocationWidget.cs
//
// Author:
//       Matt Ward <matt.ward@microsoft.com>
//       Lex Li <support@lextm.com>
//
// Copyright (c) 2018 Microsoft
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
using System.Linq;
using System.Text;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using Xwt.Drawing;

namespace MonoDevelop.Debugger.DotNetCore.Gui
{
    partial class DotNetCoreDebuggerLocationWidget
    {
        DotNetCoreDebuggerLocationPanel panel;

        public DotNetCoreDebuggerLocationWidget(DotNetCoreDebuggerLocationPanel panel)
        {
            this.panel = panel;

            Build();

            string location = panel.LoadDebuggerLocationSetting();
            locationFileSelector.FileName = location ?? string.Empty;

            locationFileSelector.FileChanged += LocationChanged;

            LocationChanged(null, null);
        }

        void LocationChanged(object sender, EventArgs e)
        {
            Validate();
            UpdateFileSelectorDirectory();
        }

        void Validate()
        {
            FilePath location = CleanPath(locationFileSelector.FileName);
            panel.ValidateDebuggerLocation(location);

            ShowDotNetCoreInformation();
        }

        void UpdateFileSelectorDirectory()
        {
            FilePath location = CleanPath(locationFileSelector.FileName);
            if (location.IsNull)
            {
                locationFileSelector.CurrentFolder = null;
            }
            else
            {
                locationFileSelector.CurrentFolder = location.ParentDirectory;
            }
        }

        FilePath CleanPath(FilePath path)
        {
            if (path.IsNullOrEmpty)
            {
                return null;
            }

            try
            {
                return path.FullPath;
            }
            catch
            {
                return null;
            }
        }

        public void ApplyChanges()
        {
            panel.SaveDebuggerLocationSetting(CleanPath(locationFileSelector.FileName));
        }

        void ShowDotNetCoreInformation()
        {
            if (panel.DotNetCoreDebuggerPath?.Exists == true)
            {
                debuggerFoundLabel.Text = GettextCatalog.GetString("Found");
                debuggerFoundIcon.Image = GetIcon(Gtk.Stock.Apply);
                UpdateDebuggerIconAccessibility(true);
            }
            else
            {
                debuggerFoundLabel.Text = GettextCatalog.GetString("Not found");
                debuggerFoundIcon.Image = GetIcon(Gtk.Stock.Cancel);
                UpdateDebuggerIconAccessibility(false);
            }
        }

        static Image GetIcon(string name)
        {
            return ImageService.GetIcon(name, Gtk.IconSize.Menu);
        }
    }
}
