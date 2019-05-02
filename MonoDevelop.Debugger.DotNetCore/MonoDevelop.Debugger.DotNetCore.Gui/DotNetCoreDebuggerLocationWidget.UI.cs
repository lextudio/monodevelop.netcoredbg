//
// DotNetCoreSdkLocationWidget.UI.cs
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
using MonoDevelop.Components.AtkCocoaHelper;
using MonoDevelop.Components.Extensions;
using MonoDevelop.Core;
using Xwt;

namespace MonoDevelop.Debugger.DotNetCore.Gui
{
    partial class DotNetCoreDebuggerLocationWidget : Widget
    {
        CustomFileSelector locationFileSelector;
        Label debuggerFoundLabel;
        ImageView debuggerFoundIcon;
        Label sdkFoundLabel;
        ImageView sdkFoundIcon;
        ListBox sdkVersionsListBox;
        Label runtimeFoundLabel;
        ImageView runtimeFoundIcon;
        ListBox runtimeVersionsListBox;

        void Build()
        {
            var mainVBox = new VBox();
            mainVBox.Spacing = 12;

            // .NET Core command line section.
            var titleLabel = new Label();
            titleLabel.Markup = GetBoldMarkup(GettextCatalog.GetString("Samsung .NET Core Debugger"));
            mainVBox.PackStart(titleLabel);

            var debuggerVBox = new VBox();
            debuggerVBox.Spacing = 6;
            debuggerVBox.MarginLeft = 24;
            mainVBox.PackStart(debuggerVBox);

            var debuggerFoundHBox = new HBox();
            debuggerFoundHBox.Spacing = 6;
            debuggerVBox.PackStart(debuggerFoundHBox, false, false);

            debuggerFoundIcon = new ImageView();
            debuggerFoundHBox.PackStart(debuggerFoundIcon, false, false);

            debuggerFoundLabel = new Label();
            debuggerFoundHBox.PackStart(debuggerFoundLabel, true, true);

            var locationBox = new HBox();
            locationBox.Spacing = 6;
            debuggerVBox.PackStart(locationBox, false, false);

            var locationLabel = new Label();
            locationLabel.Text = GettextCatalog.GetString("Location:");
            locationBox.PackStart(locationLabel, false, false);

            locationFileSelector = new CustomFileSelector();
            locationBox.PackStart(locationFileSelector, true, true);

            Content = mainVBox;
        }

        static string GetBoldMarkup(string text)
        {
            return "<b>" + GLib.Markup.EscapeText(text) + "</b>";
        }

        void UpdateDebuggerIconAccessibility(bool found)
        {
            debuggerFoundIcon.SetCommonAccessibilityAttributes(
                "DotNetCoreDebuggerFoundImage",
                found ? GettextCatalog.GetString("A Tick") : GettextCatalog.GetString("A Cross"),
                found ? GettextCatalog.GetString("The Samsung .NET Core debugger was found") : GettextCatalog.GetString("The Samsung .NET Core debugger was not found"));
        }

        /// <summary>
        /// This is slightly different from the standard FileSelector. When the select file
        /// dialog is cancelled the current directory is not remembered. This keeps the
        /// behaviour consistent with the other SDK pages that use the FileSelector from
        /// MonoDevelop.Components.
        /// </summary>
        class CustomFileSelector : Widget
        {
            TextEntry entry;
            FileDialog dialog;
            string currentFolder;
            string title;

            public CustomFileSelector()
            {
                var box = new HBox();
                entry = new TextEntry();
                entry.Changed += EntryChanged;
                box.PackStart(entry, true);

                var browseButton = new Button("…");
                box.PackStart(browseButton);
                browseButton.Clicked += BrowseButtonClicked;

                Content = box;
            }

            public string CurrentFolder
            {
                get
                {
                    return dialog != null ? dialog.CurrentFolder : currentFolder;
                }
                set
                {
                    if (dialog != null)
                        dialog.CurrentFolder = value;

                    currentFolder = value;
                }
            }

            public string FileName
            {
                get { return dialog != null ? dialog.FileName : entry.Text; }
                set { entry.Text = value; }
            }

            public string Title
            {
                get
                {
                    return title;
                }

                set
                {
                    title = value;
                    if (dialog != null)
                        dialog.Title = value;
                }
            }

            public event EventHandler FileChanged;

            void EntryChanged(object sender, EventArgs e)
            {
                FileChanged?.Invoke(sender, e);
            }

            void BrowseButtonClicked(object sender, EventArgs e)
            {
                dialog = new OpenFileDialog();

                try
                {
                    if (!string.IsNullOrEmpty(currentFolder))
                        dialog.CurrentFolder = currentFolder;
                    if (!string.IsNullOrEmpty(title))
                        dialog.Title = title;
                    if (dialog.Run(ParentWindow))
                        FileName = dialog.FileName;
                }
                finally
                {
                    dialog.Dispose();
                    dialog = null;
                }
            }
        }
    }
}
