using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocumentViewerReportServerClient.Models {
    public class ReportViewerModel {
        public string AuthToken { get; set; }
        public string ReportUrl { get; set; }
        public string ServerUri { get; set; }
    }
}