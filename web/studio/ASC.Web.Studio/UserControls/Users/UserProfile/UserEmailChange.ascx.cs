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
using System.Web.UI;

using ASC.Core.Users;
using ASC.Web.Core.Utility;
using ASC.Web.Studio.Core;
using ASC.Web.Studio.Masters.MasterManagement;

namespace ASC.Web.Studio.UserControls.Users.UserProfile
{
    public partial class UserEmailChange : UserControl
    {
        public static string Location
        {
            get { return "~/UserControls/Users/UserProfile/UserEmailChange.ascx"; }
        }

        public UserInfo UserInfo { get; set; }

        public bool RegisterStylesAndScripts { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            _emailChangerContainer.Options.IsPopup = true;
            AjaxPro.Utility.RegisterTypeForAjax(typeof(EmailOperationService));

            if (RegisterStylesAndScripts)
            {
                if(ModeThemeSettings.GetModeThemesSettings().ModeThemeName == ModeTheme.dark)
                {
                    Page.RegisterStyle("~/UserControls/Users/UserProfile/css/dark-userprofilecontrol_style.less");
                }
                else
                {
                    Page.RegisterStyle("~/UserControls/Users/UserProfile/css/userprofilecontrol_style.less");
                }
                Page.RegisterBodyScripts("~/UserControls/Users/UserProfile/js/userprofilecontrol.js");
            }
        }
    }
}