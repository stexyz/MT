using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace MT_REST_API.HttpStack {
    public static partial class HttpConnector {
        private static HttpWebRequest CreatePostRequest(string loginUri, string payLoad, out byte[] bytes) {
            HttpWebRequest req = WebRequest.Create(loginUri) as HttpWebRequest;
            req.ContentType = "application/x-www-form-urlencoded";
            req.Method = "POST";
            req.AllowAutoRedirect = false;

            bytes = Encoding.ASCII.GetBytes(payLoad);
            req.ContentLength = bytes.Length;
            return req;
        }

        private static WebResponse GetResponse(HttpWebRequest req, byte[] bytes) {
            System.IO.Stream os = req.GetRequestStream();
            os.Write(bytes, 0, bytes.Length);
            os.Close();
            WebResponse resp = req.GetResponse();
            return resp;
        }

        private static string GetPasswordCookie(string loginUri, string connectionString) {
            byte[] bytes;
            HttpWebRequest req = CreatePostRequest(loginUri, connectionString, out bytes);
            WebResponse resp = GetResponse(req, bytes);

            System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
            sr.ReadToEnd().Trim();

            return resp.Headers.Get("Set-Cookie").Split(new[] { ';' }).Single(s => s.Contains("_user_admin")).Substring(8);
        }

        public static string GetPostResponseString(string payLoad, string targetUri) {
            string pass = GetPasswordCookie(loginUri, connectionString);
            byte[] bytes;
            HttpWebRequest req = CreatePostRequest(targetUri, payLoad, out bytes);

            req.CookieContainer = new CookieContainer();
            string name = pass.Split(new[] { '=' })[0];
            string hash = pass.Split(new[] { '=' })[1];
            req.CookieContainer.Add(baseUri, new Cookie(name, hash));

            WebResponse resp = GetResponse(req, bytes);

            System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
            return sr.ReadToEnd().Trim();
        }

        public static string GetOrderListCsv(DateTime from, DateTime to, ReportCategory category) {
            return GetPostResponseString(GetPayloadForOrdersCsv(from, to, category), CsvUri);
        }

        public static string GetPurchPriceCsv() {
            return GetPostResponseString(purchasePricePayload, PurchUri);
        }

        public static string GetAllOrdersCsv(DateTime from, DateTime to) {
            throw new NotImplementedException("TODO.");
        }

        public static string GetStockCategoriesCsv() {
            throw new NotImplementedException("TODO.");
        }

        public static string GetStockCsv() {
            throw new NotImplementedException("TODO.");
        }
    }
}