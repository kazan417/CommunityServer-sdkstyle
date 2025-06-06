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
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;

using ASC.Web.Core.Utility;
using ASC.Web.Studio.Masters.MasterManagement;
using ASC.Web.Studio.Utility;

using ICSharpCode.SharpZipLib.Zip;

namespace ASC.Web.Studio.UserControls.Management
{
    [ManagementControl(ManagementType.Monitoring, Location, SortOrder = 100)]
    public partial class LogHelper : UserControl
    {
        public const string Location = "~/UserControls/Management/Monitoring/LogHelper.ascx";

        protected string HelpLink { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.RegisterBodyScripts("~/UserControls/Management/Monitoring/js/loghelper.js");
            if(ModeThemeSettings.GetModeThemesSettings().ModeThemeName == ModeTheme.dark)
            {
                Page.RegisterStyle("~/uUserControls/Management/Monitoring/css/dark-monitoring.less");
            }
            else
            {
                Page.RegisterStyle("~/uUserControls/Management/Monitoring/css/monitoring.less");
            }
            if (IsDownloadRequest())
                DownloadArchive();

            HelpLink = CommonLinkUtility.GetHelpLink();
        }

        private void DownloadArchive()
        {
            var archiveName = GetArchiveName();
            Response.ContentType = MimeMapping.GetMimeMapping(archiveName);
            Response.AddHeader("Content-Disposition", "attachment; filename=" + archiveName);
            CreateArchive(Response.OutputStream);
            Response.End();
        }

        private void CreateArchive(Stream outputStream)
        {
            using (var zipOutputStream = new ZipOutputStream(outputStream))
            {
                zipOutputStream.IsStreamOwner = false;

                var logFiles = EnumerateLogFiles(GetStartDate(), GetEndDate());
                foreach (var file in logFiles)
                {
                    zipOutputStream.PutNextEntry(new ZipEntry(file));
                    using (FileStream fs = File.OpenRead(file))
                    {
                        fs.CopyTo(zipOutputStream);
                    }
                }
                zipOutputStream.Finish();
            }
        }

        private string GetArchiveName()
        {
            return string.Format("teamlab_office_logs_{0:yyyy-MM-dd}_{1:yyyy-MM-dd}.zip", GetStartDate(), GetEndDate());
        }

        private bool IsDownloadRequest()
        {
            return HttpContext.Current.Request["download"] == "true";
        }

        private DateTime GetStartDate()
        {
            string startDate = HttpContext.Current.Request["start"];
            return !string.IsNullOrEmpty(startDate) ? Convert.ToDateTime(startDate) : DateTime.MinValue;
        }

        private DateTime GetEndDate()
        {
            string endDate = HttpContext.Current.Request["end"];
            return !string.IsNullOrEmpty(endDate) ? Convert.ToDateTime(endDate) : DateTime.MaxValue;
        }

        private IEnumerable<string> EnumerateLogFiles(DateTime startDate, DateTime endDate)
        {
            return GetLogFolders()
                .SelectMany(logFolder => Directory.EnumerateFiles(logFolder, "*.log", SearchOption.AllDirectories)
                                                  .Where(logFile => IsLogMatchingDate(logFile, startDate, endDate)));
        }

        private IEnumerable<string> GetLogFolders()
        {
            var paths = ConfigurationManagerExtension.AppSettings["monitoring.log-folder"] ?? @"..\Logs\";
            return paths.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(path =>
                            {
                                if (!Path.IsPathRooted(path))
                                    path = Path.Combine(Request.PhysicalApplicationPath, path);
                                return path;
                            })
                        .Where(Directory.Exists);
        }

        private bool IsLogMatchingDate(string logFile, DateTime startDate, DateTime endDate)
        {
            var fileInfo = new FileInfo(logFile);
            return fileInfo.LastWriteTimeUtc.Date >= startDate && fileInfo.LastWriteTimeUtc.Date <= endDate;
        }
    }
}