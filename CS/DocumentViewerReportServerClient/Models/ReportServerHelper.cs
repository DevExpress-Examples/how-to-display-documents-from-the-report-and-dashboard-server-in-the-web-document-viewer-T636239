using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace DocumentViewerReportServerClient.Models {
    public class ReportServerHelper {
        public const string REPORT_SERVER_URI = "https://my-report-server.com";  //Your Report Server URL
        private const string DOCUMENT_VIEWER_ACCOUNT_LOGIN = "DocumentViewer";
        private const string REPORT_SERVER_PASSWORD = "MyPassword";

        public static string GetToken() {
            HttpContent httpContent = new FormUrlEncodedContent(new Dictionary<string, string>() {
                { "grant_type", "password"},
                { "username", DOCUMENT_VIEWER_ACCOUNT_LOGIN },
                { "password", REPORT_SERVER_PASSWORD }
            });
            HttpResponseMessage result = MvcApplication.httpClient.PostAsync(new Uri(REPORT_SERVER_URI + "/oauth/token"), httpContent).Result;
            return (result.Content.ReadAsAsync<Token>().Result).AuthToken;
        }

        public static List<DocumentItem> GetReports()
        {
            var message = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(REPORT_SERVER_URI + "/api/documents")
            };
            var client = MvcApplication.httpClient;
            client.DefaultRequestHeaders.Add(HttpRequestHeader.Authorization.ToString(), "Bearer " + GetToken());
            HttpResponseMessage result = client.SendAsync(message).Result;
            List<DocumentItem> documents = JsonConvert.DeserializeObject<List<DocumentItem>>(result.Content.ReadAsStringAsync().Result);
            return documents.Where(document => document.documentType == "Report").ToList();
        }
    }

    [DataContract]
    public class Token {
        [DataMember(Name = "access_token")]
        public string AuthToken { get; set; }
    }

    public class DocumentItem {
        public int id { get; set; }
        public string name { get; set; }
        public string documentType { get; set; }
    }   
}