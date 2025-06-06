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

using AjaxPro;

using ASC.Web.Core.Sms;
using ASC.Web.Studio.Core;
using ASC.Web.Studio.Core.SMS;
using ASC.Web.Studio.Core.TFA;
using ASC.Web.Studio.Masters.MasterManagement;
using ASC.Web.Studio.Utility;

namespace ASC.Web.Studio.UserControls.Management
{
    [ManagementControl(ManagementType.PortalSecurity, Location, SortOrder = 50)]
    [AjaxNamespace("AjaxPro.SmsValidationSettingsController")]
    public partial class SmsValidationSettings : UserControl
    {
        public const string Location = "~/UserControls/Management/SmsControls/SmsValidationSettings.ascx";

        protected bool SmsVisible;
        protected bool SmsAvailable;
        protected bool SmsEnable;
        protected bool TfaAppEnable;
        protected bool ThirdPartyVisible;

        protected string HelpLink { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(GetType());
            Page.RegisterBodyScripts("~/UserControls/Management/SmsControls/js/smsvalidation.js")
                .RegisterStyle("~/UserControls/Management/SmsControls/css/smsvalidationsettings.less");

            SmsVisible = StudioSmsNotificationSettings.IsVisibleSettings;
            SmsAvailable = StudioSmsNotificationSettings.IsAvailableSettings;
            SmsEnable = SmsAvailable && SmsProviderManager.Enabled();
            TfaAppEnable = TfaAppAuthSettings.IsVisibleSettings;
            ThirdPartyVisible = SetupInfo.IsVisibleSettings(ManagementType.ThirdPartyAuthorization.ToString());

            HelpLink = CommonLinkUtility.GetHelpLink();
        }
    }
}