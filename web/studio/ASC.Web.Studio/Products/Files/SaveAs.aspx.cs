/*
 *
 * (c) Copyright Ascensio System Limited 2010-2023
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * http://www.apache.org/licenses/LICENSE-2.0
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
*/


using System;
using System.Text;

using ASC.Web.Core.Client.Bundling;
using ASC.Web.Core.Files;
using ASC.Web.Core.Utility;
using ASC.Web.Files.Classes;
using ASC.Web.Files.Controls;
using ASC.Web.Files.Resources;
using ASC.Web.Studio.Masters;
using ASC.Web.Studio.Masters.MasterManagement;
using Global = ASC.Web.Files.Classes.Global;

namespace ASC.Web.Files
{
    public partial class SaveAs : MainPage, IStaticBundle
    {
        public static string Location
        {
            get { return FilesLinkUtility.FilesBaseAbsolutePath + "SaveAs.aspx"; }
        }

        public static string GetUrl
        {
            get { return Location + string.Format("?{0}={{{0}}}&{1}={{{1}}}", FilesLinkUtility.FileTitle, FilesLinkUtility.FileUri); }
        }

        public static string GetUrlToFolder(object folderId)
        {
            return string.Format("{0}&{1}={2}", GetUrl, FilesLinkUtility.FolderId, folderId);
        }

        public string RequestFileTitle
        {
            get { return Global.ReplaceInvalidCharsAndTruncate(Request[FilesLinkUtility.FileTitle]); }
        }

        public string RequestUrl
        {
            get { return Request[FilesLinkUtility.FileUri]; }
        }

        private bool DisplayPrivacy
        {
            get { return !string.IsNullOrEmpty(Request["displayPrivacy"]); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.Master.DisabledSidePanel = true;
            Master.Master.DisabledTopStudioPanel = true;
            Master.Master
                  .AddStaticStyles(ModeThemeSettings.GetModeThemesSettings().ModeThemeName == ModeTheme.dark ? GetStaticDarkStyleSheet() : GetStaticStyleSheet())
                  .AddStaticBodyScripts(GetStaticJavaScript());

            var fileSelector = (FileSelector)LoadControl(FileSelector.Location);
            fileSelector.IsFlat = true;
            fileSelector.OnlyFolder = true;
            fileSelector.SuccessButton = FilesCommonResource.ButtonSave;
            CommonContainerHolder.Controls.Add(fileSelector);

            InitScript();
        }

        private void InitScript()
        {
            var originForPost = "*";
            if (!string.IsNullOrEmpty(FilesLinkUtility.DocServiceApiUrl) && !FilesLinkUtility.DocServiceApiUrl.StartsWith("/"))
            {
                var origin = new Uri(FilesLinkUtility.DocServiceApiUrl);
                originForPost = origin.Scheme + "://" + origin.Host + ":" + origin.Port;
            }

            var script = new StringBuilder();
            script.AppendFormat("ASC.Files.FileChoice.init(\"{0}\", \"{1}\", ({2} == true));",
                                originForPost,
                                (Request[FilesLinkUtility.FolderId] ?? "").Replace("\"", "\\\""),
                                DisplayPrivacy.ToString().ToLower());
            Page.RegisterInlineScript(script.ToString());
        }


        public ScriptBundleData GetStaticJavaScript()
        {
            return (ScriptBundleData)
                   new ScriptBundleData("filessaveas", "files")
                       .AddSource(PathProvider.GetFileStaticRelativePath,
                                  "common.js",
                                  "templatemanager.js",
                                  "servicemanager.js",
                                  "ui.js",
                                  "eventhandler.js",
                                  "saveas.js"
                       )
                       .AddSource(r => FilesLinkUtility.FilesBaseAbsolutePath + r,
                                  "Controls/EmptyFolder/emptyfolder.js",
                                  "Controls/FileSelector/fileselector.js",
                                  "Controls/Tree/tree.js"
                       );
        }

        public StyleBundleData GetStaticStyleSheet()
        {
            return (StyleBundleData)
                   new StyleBundleData("filessaveas", "files")
                       .AddSource(PathProvider.GetFileStaticRelativePath, "saveas.less")
                       .AddSource(r => FilesLinkUtility.FilesBaseAbsolutePath + r,
                                  "Controls/FileSelector/fileselector.less",
                                  "Controls/ThirdParty/thirdparty.less",
                                  "Controls/ContentList/contentlist.less",
                                  "Controls/EmptyFolder/emptyfolder.less",
                                  "Controls/Tree/tree.less"
                       );
        }
        public StyleBundleData GetStaticDarkStyleSheet()
        {
            return (StyleBundleData)
                   new StyleBundleData("dark-filessaveas", "files")
                       .AddSource(PathProvider.GetFileStaticRelativePath, "saveas.less")
                       .AddSource(r => FilesLinkUtility.FilesBaseAbsolutePath + r,
                                  "Controls/FileSelector/fileselector.less",
                                  "Controls/ThirdParty/dark-thirdparty.less",
                                  "Controls/ContentList/dark-contentlist.less",
                                  "Controls/EmptyFolder/emptyfolder.less",
                                  "Controls/Tree/dark-tree.less"
                       );
        }
    }
}