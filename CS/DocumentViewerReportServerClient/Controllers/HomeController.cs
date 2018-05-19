using DocumentViewerReportServerClient.Models;
using System.Web.Mvc;

namespace DocumentViewerReportServerClient.Controllers {
    public class HomeController : Controller {
        public ActionResult Index() {
            ViewBag.Title = "Remotely Generated Documents";

            var documents = ReportServerHelper.GetReports();
            return View(documents);
        }

        public ActionResult Viewer(DocumentItem item) {
            ViewBag.Title = "Preview: " + item.name;

            var model = new ReportViewerModel() {
                ReportUrl = "report/" + item.id.ToString(),
                AuthToken = ReportServerHelper.GetToken(),
                ServerUri = ReportServerHelper.REPORT_SERVER_URI
            };            
            return View(model);
        }
    }
}