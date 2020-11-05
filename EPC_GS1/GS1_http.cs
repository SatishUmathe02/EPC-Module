using DataAccessLayer;
using DataAccessLayer.CommonDataModels;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace EPC_GS1
{
    public class GS1_http
    {
               
        public static async Task<string> GS1_apiResponse_Rest(EPCRequest ObjEPC)
        {
            string Response = string.Empty;
            List<GS1> ObjGS1 = new List<GS1>();


            string GS1_api = ConfigurationManager.AppSettings["GS1_api"].ToString();
            string GS1_apiKey = ConfigurationManager.AppSettings["GS1_apiKey"].ToString();



            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            var url = GS1_api + "/" + ObjEPC.GTIN;
            
            var client = new RestClient(url);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("APIKey", GS1_apiKey);
            IRestResponse response = client.Execute(request);

            try
            {
                
                if (response.StatusCode.ToString() == "OK")
                {
                    Response = response.Content;

                }
                else
                {
                    Response = response.Content == "" ? response.ErrorMessage : response.Content;
                    
                }
                int StatusCode = Convert.ToInt32(response.StatusCode);
                SendEmail(StatusCode, Response, ObjEPC.GTIN);
            }
            catch (WebException ex)
            {
                int StatusCode = Convert.ToInt32(((System.Net.HttpWebResponse)ex.Response).StatusCode);
                SendEmail(StatusCode, Response, ObjEPC.GTIN);
            }
            
            //Response =System.IO.File.ReadAllText(@"E:\DEMO Project\DemoApplication\GS1_Int\bin\Debug\GS2.json");
            EPCDAL.InsertReqRes(ObjEPC.CustomerID, ObjEPC.RPO, ObjEPC.DetailLineID, url, Response, GS1_api, ObjEPC.UserId, ObjEPC.GTIN);
            
            return Response;

        }

        public static void SendEmail(int StatusCode, string Response, string GTIN)
        {
            int code = Convert.ToInt32(StatusCode.ToString().Substring(0, 1));
            string Recipient = "ali.mehdi@r-pac.com,ahmad.kabakebi@r-pac.com,Amit.Kadam@r-pac.com,Nayana.Krishnamurthy@r-pac.com,satish.umathe@r-pac.com";
            string Subject = string.Empty;
            string varCC = "";
            string varBCC = ""; string varReplyTo = "";
            //string varAttachmentName = "";

            StringBuilder Body = new StringBuilder();

            switch (code)
            {
                case 5:
                case 4:
                    //varCC = "Satish.umathe@r-pac.com,Namrata.bhagat@r-pac.com";
                    Subject = "r-pac: GS1 hub web service not reachable " + StatusCode + " for GTIN:" + GTIN + "";
                    Body.Append(Response);
                    EPCDAL.InsertEmail_GS1(Recipient, varCC, varBCC, varReplyTo, Subject, Body.ToString(), 1, DateTime.Now);
                    break;
               
                default:
                    break;
            }
        }

        public static void SendEmail_AfterCheck_GS(int StatusCode, string Response, string GTIN)
        {

            string Recipient = "ali.mehdi@r-pac.com,ahmad.kabakebi@r-pac.com,Amit.Kadam@r-pac.com,Nayana.Krishnamurthy@r-pac.com";
            string Subject = string.Empty;
            string varCC = "Satish.umathe@r-pac.com";
            string varBCC = ""; string varReplyTo = "";
            //string varAttachmentName = "";

            StringBuilder Body = new StringBuilder();

            switch (StatusCode)
            {
                case 1:
                    Subject = "r-pac: GS1 Hub web service received an empty response for GTIN:" + GTIN + "";
                    Body.Append(Response);
                    EPCDAL.InsertEmail_GS1(Recipient, varCC, varBCC, varReplyTo, Subject, Body.ToString(), 1, DateTime.Now);
                    break;
                case 2:
                    Subject = "r-pac: GS1 Hub web service received a wrong Prefix for GTIN:" + GTIN + "";
                    Body.Append(Response);
                    EPCDAL.InsertEmail_GS1(Recipient, varCC, varBCC, varReplyTo, Subject, Body.ToString(), 1, DateTime.Now);
                    break;
                default:
                    break;
            }
        }

        /*
        public static async Task<string> GS1_apiResponse(EPCRequest ObjEPC)
        {
            string Response = string.Empty;
            List<GS1> ObjGS1 = new List<GS1>();


            string GS1_api = ConfigurationManager.AppSettings["GS1_api"].ToString();
            string GS1_apiKey = ConfigurationManager.AppSettings["GS1_apiKey"].ToString();



            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            HttpResponseMessage response;



            // Request headers
            client.DefaultRequestHeaders.Add("APIKey", GS1_apiKey);
            //var uri = "https://api.gs1us.org/company/v3/company/GTIN/8887891878916?" + queryString;
            var uri = GS1_api + ObjEPC.GTIN + "?" + queryString;
            try
            {
                response = client.GetAsync(uri).Result;
                if (response.StatusCode.ToString() == "OK")
                {
                    Response = response.Content.ReadAsStringAsync().Result;

                }
                else
                {
                    //Response = response.Content == "" ? response.ErrorMessage : response.Content;
                    Response = response.ReasonPhrase;

                }
            }
            catch (WebException ex)
            {
                int StatusCode = Convert.ToInt32(((System.Net.HttpWebResponse)ex.Response).StatusCode);
                SendEmail(StatusCode, Response, ObjEPC.GTIN);
            }
                        
            EPCDAL.InsertReqRes(ObjEPC.CustomerID, ObjEPC.RPO, ObjEPC.DetailLineID, uri, Response, GS1_api, ObjEPC.UserId, ObjEPC.GTIN);
            
            return Response;

        }
        */
    }
}
