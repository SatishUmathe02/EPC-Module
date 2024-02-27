using DataAccessLayer;
using DataAccessLayer.CommonDataModels;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EPC_CandA
{
    public class CandA_http
    {
        private static string CandA_EmailIds;
        private static string CandA_EmailId_CC;
        public static bool CandA_GSIM;



        static CandA_http()
        {
            CandA_http.CandA_EmailIds = ConfigurationManager.AppSettings["CandA_EmailIds"].ToString();
            CandA_http.CandA_EmailId_CC = ConfigurationManager.AppSettings["CandA_EmailId_CC"].ToString();
            CandA_http.CandA_GSIM =Convert.ToBoolean(ConfigurationManager.AppSettings["CandA_GSIM"].ToString());


        }

        public CandA_http()
        {

        }

        public static string GetSerialNumber(EPCRequest ObjEPC, long Threshold, string requestorId)
        {
            string Response = string.Empty;

            
            string url = ConfigurationManager.AppSettings["CandA_gsim_URL"].ToString();
            string Authorization = ConfigurationManager.AppSettings["CandA_Authorization"].ToString();
            string ServerEnvironment = ConfigurationManager.AppSettings["ServerEnvironment"].ToString();

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var clientepcLog = new RestClient(url);
            clientepcLog.Timeout = -1;
            var requestepcLog = new RestRequest(Method.POST);
            requestepcLog.AddHeader("Content-Type", "application/json");
            requestepcLog.AddHeader("Authorization", Authorization);
            string Request = "{\n  \"gtin\": \""+ ObjEPC.GTIN + "\",\n  \"requestorId\": \""+ requestorId + "\",\n  \"amount\": "+ Threshold + "\n}";
            requestepcLog.AddParameter("application/json", Request, ParameterType.RequestBody);
            IRestResponse response = clientepcLog.Execute(requestepcLog);

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
                SendEmail(StatusCode, Response, ObjEPC.GTIN, Request, ServerEnvironment);
            }
            catch (WebException ex)
            {
                int StatusCode = Convert.ToInt32(((System.Net.HttpWebResponse)ex.Response).StatusCode);
                SendEmail(StatusCode, Response, ObjEPC.GTIN, Request, ServerEnvironment);
            }

            
            EPCDAL.InsertReqRes(ObjEPC.CustomerID, ObjEPC.RPO, ObjEPC.DetailLineID, Request, Response, url, ObjEPC.UserId, ObjEPC.GTIN);

            return Response;

        }
        public static void SendEmail(int StatusCode, string Response, string GTIN, string Request, string ServerEnvironment)
        {
            int code = Convert.ToInt32(StatusCode.ToString().Substring(0, 1));
            string Recipient = CandA_EmailIds;
            string Subject = string.Empty;
            string varCC = CandA_EmailId_CC;
            string varBCC = ""; string varReplyTo = "";
            //string varAttachmentName = "";

            StringBuilder Body = new StringBuilder();

            switch (code)
            {
                case 5:
                case 4:
                case 0:
                    //varCC = "Satish.umathe@r-pac.com,Namrata.bhagat@r-pac.com";
                    Subject = "r-pac: Error requesting serials on "+ ServerEnvironment + " Server from GSIM " + StatusCode + " ";

                    Body.Append("Event Time: "+ DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ") +"");
                    Body.Append("<br/><br/>");
                    Body.Append("Error Code: " + StatusCode);
                    Body.Append("<br/><br/>");
                    Body.Append("Request: " + Request);
                                        
                    EPCDAL.InsertEmail_GS1(Recipient, varCC, varBCC, varReplyTo, Subject, Body.ToString(), 1, DateTime.Now);
                    break;

                default:
                    break;
            }
        }

        /*
        public static void SendEmail_AfterCheck_SerialNumber(int StatusCode, string Response, string GTIN)
        {

            string Recipient = CandA_EmailIds;
            string Subject = string.Empty;
            string varCC = CandA_EmailId_CC;
            string varBCC = ""; string varReplyTo = "";
            //string varAttachmentName = "";

            StringBuilder Body = new StringBuilder();

            switch (StatusCode)
            {
                case 1:
                    Subject = "r-pac: C&A sgtin/serial web service received an empty response for GTIN:" + GTIN + "";
                    Body.Append(Response);
                    EPCDAL.InsertEmail_GS1(Recipient, varCC, varBCC, varReplyTo, Subject, Body.ToString(), 1, DateTime.Now);
                    break;
                case 2:
                    Subject = "r-pac: C&A sgtin/serial web service received a wrong Prefix for GTIN:" + GTIN + "";
                    Body.Append(Response);
                    EPCDAL.InsertEmail_GS1(Recipient, varCC, varBCC, varReplyTo, Subject, Body.ToString(), 1, DateTime.Now);
                    break;
                default:
                    break;
            }
        }
        */
    }
}
