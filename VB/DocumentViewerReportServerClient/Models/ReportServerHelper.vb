Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Net
Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Runtime.Serialization
Imports Newtonsoft.Json

Namespace DocumentViewerReportServerClient.Models
    Public Class ReportServerHelper
        Public Const REPORT_SERVER_URI As String = "https://my-report-server.com" 'Your Report Server URL
        Private Const DOCUMENT_VIEWER_ACCOUNT_LOGIN As String = "DocumentViewer"
        Private Const REPORT_SERVER_PASSWORD As String = "MyPassword"

        Public Shared Function GetToken() As String
            Dim httpContent As HttpContent = New FormUrlEncodedContent(New Dictionary(Of String, String)() From { _
                { "grant_type", "password"}, _
                { "username", DOCUMENT_VIEWER_ACCOUNT_LOGIN }, _
                { "password", REPORT_SERVER_PASSWORD } _
            })
            Dim result As HttpResponseMessage = MvcApplication.httpClient.PostAsync(New Uri(REPORT_SERVER_URI & "/oauth/token"), httpContent).Result
            Return (result.Content.ReadAsAsync(Of Token)().Result).AuthToken
        End Function

        Public Shared Function GetReports() As List(Of DocumentItem)
            Dim message = New HttpRequestMessage With { _
                .Method = HttpMethod.Get, _
                .RequestUri = New Uri(REPORT_SERVER_URI & "/api/documents") _
            }
            Dim client = MvcApplication.httpClient
            client.DefaultRequestHeaders.Add(HttpRequestHeader.Authorization.ToString(), "Bearer " & GetToken())
            Dim result As HttpResponseMessage = client.SendAsync(message).Result
            Dim documents As List(Of DocumentItem) = JsonConvert.DeserializeObject(Of List(Of DocumentItem))(result.Content.ReadAsStringAsync().Result)
            Return documents.Where(Function(document) document.documentType = "Report").ToList()
        End Function
    End Class

    <DataContract> _
    Public Class Token
        <DataMember(Name := "access_token")> _
        Public Property AuthToken() As String
    End Class

    Public Class DocumentItem
        Public Property id() As Integer
        Public Property name() As String
        Public Property documentType() As String
    End Class
End Namespace