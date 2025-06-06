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
using System.Web;
using System.Web.UI;

using ASC.Web.Core.Utility;
using ASC.Web.Files.Controls;
using ASC.Web.Mail.Resources;
using ASC.Web.Studio.Core;
using ASC.Web.Studio.Masters.MasterManagement;
using ASC.Web.Studio.UserControls.Common.MediaViewer;

namespace ASC.Web.Mail.Controls
{
    public partial class MailBox : UserControl
    {
        public static string Location { get { return "~/addons/mail/Controls/MailBox/MailBox.ascx"; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.RegisterBodyScripts("~/js/uploader/jquery.fileupload.js",
                                     "~/UserControls/Common/ckeditor/ckeditor-connector.js",
                                     "~/Products/Files/js/common.js",
                                     "~/Products/Files/js/templatemanager.js",
                                     "~/Products/Files/js/servicemanager.js",
                                     "~/Products/Files/js/ui.js",
                                     "~/Products/Files/js/eventhandler.js",
                                     "~/Products/Files/Controls/EmptyFolder/emptyfolder.js",
                                     "~/Products/Files/Controls/FileSelector/fileselector.js",
                                     "~/Products/Files/Controls/Tree/tree.js",
                                     "~/Products/Files/Controls/ConvertFile/confirmconvert.js"
                )
                .RegisterStyle("~/Products/Files/Controls/FileSelector/fileselector.less");
                
            if(ModeThemeSettings.GetModeThemesSettings().ModeThemeName == ModeTheme.dark)
            {
                Page.RegisterStyle("~/Products/Files/Controls/ThirdParty/dark-thirdparty.less",
                    "~/Products/Files/Controls/ContentList/dark-contentlist.less",
                    "~/Products/Files/Controls/EmptyFolder/emptyfolder.less",
                    "~/Products/Files/Controls/Tree/dark-tree.less",
                    "~/Products/Files/Controls/ConvertFile/confirmconvert.less");
            }
            else
            {
                Page.RegisterStyle("~/Products/Files/Controls/ThirdParty/thirdparty.less",
                    "~/Products/Files/Controls/ContentList/contentlist.less",
                    "~/Products/Files/Controls/EmptyFolder/emptyfolder.less",
                    "~/Products/Files/Controls/Tree/tree.less",
                    "~/Products/Files/Controls/ConvertFile/confirmconvert.less");
            }
            ControlPlaceHolder.Controls.Add(LoadControl(MediaPlayer.Location));

            TagsPageHolder.Controls.Add(LoadControl(TagsPage.Location) as TagsPage);

            OutContentPlaceHolder.Controls.Add(LoadControl(UserFoldersPage.Location) as UserFoldersPage);

            TagsPageHolder.Controls.Add(LoadControl(FiltersPage.Location) as FiltersPage);
            TagsPageHolder.Controls.Add(LoadControl(AccountsPage.Location) as AccountsPage);
            TagsPageHolder.Controls.Add(LoadControl(ContactsPage.Location) as ContactsPage);
            TagsPageHolder.Controls.Add(LoadControl(CommonSettingsPage.Location) as CommonSettingsPage);

            if (Configuration.Settings.IsAdministrationPageAvailable())
                TagsPageHolder.Controls.Add(LoadControl(AdministrationPage.Location) as AdministrationPage);

            var documentsPopup = (DocumentsPopup)LoadControl(DocumentsPopup.Location);
            _phDocUploader.Controls.Add(documentsPopup);

            QuestionPopup.Options.IsPopup = true;

            var fileSelector = (FileSelector)LoadControl(FileSelector.Location);
            fileSelector.DialogTitle = MailResource.SelectFolderDialogTitle;
            fileSelector.OnlyFolder = true;
            fileholder.Controls.Add(fileSelector);

            fileholder.Controls.Add(LoadControl(ConfirmConvert.Location));
        }

        protected String RenderRedirectUpload()
        {
            return string.Format("{0}://{1}:{2}{3}", Request.GetUrlRewriter().Scheme, Request.GetUrlRewriter().Host, Request.GetUrlRewriter().Port, VirtualPathUtility.ToAbsolute("~/") + "fckuploader.ashx?esid=mail");
        }

        public bool IsMailPrintAvailable()
        {
            return SetupInfo.IsVisibleSettings("MailPrint");
        }
    }
}
