Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Net
Imports System.Net.Http
Imports System.Web
Imports System.Web.Http
Imports System.Security.Cryptography.X509Certificates
Imports System.Net.Security
Imports System.Web.Mvc
Imports System.Web.Routing
Imports DevExpress.Web.Mvc

Namespace DocumentViewerReportServerClient
    ' Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    ' visit http://go.microsoft.com/?LinkId=9394801

    Public Class MvcApplication
        Inherits System.Web.HttpApplication

        Private Shared _client As HttpClient
        Public Shared ReadOnly Property httpClient() As HttpClient
            Get
                Return _client
            End Get
        End Property

        Private Shared Function ServerCertificateValidationCallback(ByVal sender As Object, ByVal certificate As X509Certificate, ByVal chain As X509Chain, ByVal sslPolicyErrors As SslPolicyErrors) As Boolean
            Return True ' SSL certificate errors will be ignored for this example only.
        End Function

        Public Shared Sub RegisterGlobalFilters(ByVal filters As GlobalFilterCollection)
            filters.Add(New HandleErrorAttribute())
        End Sub

        Public Shared Sub RegisterRoutes(ByVal routes As RouteCollection)
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}")
            routes.IgnoreRoute("{resource}.ashx/{*pathInfo}")

            routes.MapRoute("Default", "{controller}/{action}/{id}", New With { _
                Key .controller = "Home", _
                Key .action = "Index", _
                Key .id = UrlParameter.Optional _
            })

        End Sub

        Protected Sub Application_Start()
            DevExpress.XtraReports.Web.WebDocumentViewer.Native.WebDocumentViewerBootstrapper.SessionState = System.Web.SessionState.SessionStateBehavior.Default
            MVCxWebDocumentViewer.StaticInitialize()
            AreaRegistration.RegisterAllAreas()

            RegisterGlobalFilters(GlobalFilters.Filters)
            RegisterRoutes(RouteTable.Routes)

            ModelBinders.Binders.DefaultBinder = New DevExpress.Web.Mvc.DevExpressEditorsBinder()

            AddHandler DevExpress.Web.ASPxWebControl.CallbackError, AddressOf Application_Error
            _client = New HttpClient()

            ' Uncomment this line if you need to customize SSL certificate policy on your Report Server.
            ' System.Net.ServicePointManager.ServerCertificateValidationCallback = AddressOf ServerCertificateValidationCallback
        End Sub

        Protected Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
            Dim exception As Exception = System.Web.HttpContext.Current.Server.GetLastError()
            'TODO: Log Exception ...
        End Sub
    End Class
End Namespace