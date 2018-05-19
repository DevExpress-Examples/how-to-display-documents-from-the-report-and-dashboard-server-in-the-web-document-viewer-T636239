Imports DocumentViewerReportServerClient.Models
Imports System.Web.Mvc

Namespace DocumentViewerReportServerClient.Controllers
    Public Class HomeController
        Inherits Controller

        Public Function Index() As ActionResult
            ViewBag.Title = "Remotely Generated Documents"

            Dim documents = ReportServerHelper.GetReports()
            Return View(documents)
        End Function

        Public Function Viewer(ByVal item As DocumentItem) As ActionResult
            ViewBag.Title = "Preview: " & item.name

            Dim model = New ReportViewerModel() With { _
                .ReportUrl = "report/" & item.id.ToString(), _
                .AuthToken = ReportServerHelper.GetToken(), _
                .ServerUri = ReportServerHelper.REPORT_SERVER_URI _
            }
            Return View(model)
        End Function
    End Class
End Namespace