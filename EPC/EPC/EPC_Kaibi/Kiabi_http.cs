using DataAccessLayer;
using DataAccessLayer.CommonDataModels;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EPC_Kaibi
{
    public class Kiabi_http
    {
        private static string Kiabi_EmailIds;

        static Kiabi_http()
        {
            Kiabi_http.Kiabi_EmailIds = ConfigurationManager.AppSettings["Kiabi_EmailIds"].ToString();
        }

        public Kiabi_http()
        {
        }

        public static string GetAuthToken(EPCRequest ObjEPC)
        {
            string str = "";
            string str1 = "https://auth.kiabi.com/uaa/oauth/token";
            try
            {
                RestClient restClient = new RestClient(str1);
                RestRequest restRequest = new RestRequest(Method.POST);
                restRequest.AddHeader("content-type", "application/x-www-form-urlencoded");
                NetworkCredential networkCredential = new NetworkCredential(ConfigurationManager.AppSettings["Kiabi_UserName"].ToString(), ConfigurationManager.AppSettings["Kiabi_Password"].ToString());
                restRequest.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["Kiabi_UserName"].ToString(), ConfigurationManager.AppSettings["Kiabi_Password"].ToString());
                restRequest.AddParameter("application/x-www-form-urlencoded", "grant_type=client_credentials&client_id=PrintProvider_PAC", ParameterType.RequestBody);
                IRestResponse restResponse = restClient.Execute(restRequest);
                if (restResponse.StatusCode.ToString() == "OK")
                {
                    str = restResponse.Content.Split(new char[] { ',' })[0].Split(new char[] { ':' })[1].Replace("\"", "");
                }
            }
            catch (WebException webException1)
            {
                WebException webException = webException1;
                int num = Convert.ToInt32(((HttpWebResponse)webException.Response).StatusCode);
                EPCDAL.InsertReqRes(ObjEPC.CustomerID, ObjEPC.RPO, ObjEPC.DetailLineID, "token", webException.ToString(), str1, ObjEPC.UserId, ObjEPC.GTIN);
                Kiabi_http.SendEmail(num, webException.ToString(), ObjEPC.GTIN);
            }
            return str;
        }

        public static async Task<string> Kiabi_apiResponse_Rest(EPCRequest ObjEPC, string RequestBody)
        {
            string str;
            string empty = string.Empty;
            List<GS1> gS1s = new List<GS1>();
            string str1 = ConfigurationManager.AppSettings["Kiabi_api"].ToString();
            string str2 = ConfigurationManager.AppSettings["Kiabi_apikey"].ToString();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            string authToken = Kiabi_http.GetAuthToken(ObjEPC);
            if (!string.IsNullOrEmpty(authToken))
            {
                RestClient restClient = new RestClient(str1)
                {
                    Timeout = -1
                };
                RestRequest restRequest = new RestRequest(Method.POST);
                restRequest.AddHeader("Content-Type", "application/json");
                restRequest.AddHeader("Accept", "application/json;charset=UTF-8");
                restRequest.AddHeader("Authorization", string.Concat("Bearer ", authToken));
                restRequest.AddHeader("x-apikey", str2);
                restRequest.AddParameter("application/json", RequestBody, ParameterType.RequestBody);
                IRestResponse restResponse = restClient.Execute(restRequest);
                try
                {
                    if (restResponse.StatusCode.ToString() != "OK")
                    {
                        str = (restResponse.Content == "" ? restResponse.ErrorMessage : restResponse.Content);
                        empty = str;
                    }
                    else
                    {
                        empty = restResponse.Content;
                    }
                    Kiabi_http.SendEmail(Convert.ToInt32(restResponse.StatusCode), empty, ObjEPC.GTIN);
                }
                catch (WebException webException)
                {
                    Kiabi_http.SendEmail(Convert.ToInt32(((HttpWebResponse)webException.Response).StatusCode), empty, ObjEPC.GTIN);
                }
                EPCDAL.InsertReqRes(ObjEPC.CustomerID, ObjEPC.RPO, ObjEPC.DetailLineID, RequestBody, empty, str1, ObjEPC.UserId, ObjEPC.GTIN);
            }
            return empty;
        }

        public static void SendEmail(int StatusCode, string Response, string GTIN)
        {
            int num = Convert.ToInt32(StatusCode.ToString().Substring(0, 1));
            string kiabiEmailIds = Kiabi_http.Kiabi_EmailIds;
            string empty = string.Empty;
            string str = "";
            string str1 = "";
            string str2 = "";
            StringBuilder stringBuilder = new StringBuilder();
            if (num == 4 || num == 5)
            {
                empty = string.Concat(new object[] { "r-pac: Kiabi web service not reachable ", StatusCode, " for GTIN:", GTIN }) ?? "";
                stringBuilder.Append(Response);
                EPCDAL.InsertEmail_GS1(kiabiEmailIds, str, str1, str2, empty, stringBuilder.ToString(), (long)1, DateTime.Now);
            }
        }
    }
}