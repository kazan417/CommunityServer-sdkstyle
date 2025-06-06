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

using ASC.Web.Core;
using ASC.Web.Core.Utility;
using ASC.Web.Studio.Masters.MasterManagement;

namespace ASC.Web.Studio.UserControls.Feed
{
    public partial class FeedList : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(ModeThemeSettings.GetModeThemesSettings().ModeThemeName == ModeTheme.dark)
            {
                Page.RegisterStyle("~/UserControls/Feed/css/dark-feed.less");
            }
            else
            {
                Page.RegisterStyle("~/UserControls/Feed/css/feed.less");
            }
            Page.RegisterBodyScripts("~/UserControls/Feed/js/feed.js", "~/UserControls/Feed/js/feed.filter.js")
                .RegisterInlineScript(@"ASC.Feed.init('" + AccessRights() + "');");
        }

        public static string Location
        {
            get { return "~/UserControls/Feed/FeedList.ascx"; }
        }

        public static string AccessRights()
        {
            return string.Join(",",
                    WebItemSecurity.IsAvailableForMe(WebItemManager.CommunityProductID).ToString(),
                    WebItemSecurity.IsAvailableForMe(WebItemManager.PeopleProductID).ToString(),
                    WebItemSecurity.IsAvailableForMe(WebItemManager.CRMProductID).ToString(),
                    WebItemSecurity.IsAvailableForMe(WebItemManager.ProjectsProductID).ToString(),
                    WebItemSecurity.IsAvailableForMe(WebItemManager.DocumentsProductID).ToString()
                );
        }
    }
}