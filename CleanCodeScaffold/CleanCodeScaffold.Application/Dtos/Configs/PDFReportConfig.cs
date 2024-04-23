using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanCodeScaffold.Application.Dtos.Configs
{
    internal class PDFReportConfig
    {
        public PDFReportConfig()
        {
            TemplateURL = string.Empty;
            CssURL = string.Empty;
            LogoUrl = string.Empty;
            ReportTitle = string.Empty;
        }

        public PDFReportConfig(string reportTitle, string templateUrl, string cssUrl, int recordPerPage)
        {
            ReportTitle = reportTitle;
            TemplateURL = templateUrl;
            CssURL = cssUrl;
            RecordPerPage = recordPerPage;
            LogoUrl= string.Empty;
        }
        public int RecordPerPage { get; set; }
        public string TemplateURL { get; set; }
        public string CssURL { get; set; }
        public string LogoUrl { get; set; }
        public string ReportTitle { get; set; }
    }
}
