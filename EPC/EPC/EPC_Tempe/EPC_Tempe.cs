using DataAccessLayer;
using DataAccessLayer.CommonDataModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EPC_Tempe
{
    public class EPC_Tempe
    {
        public static string EPCGeneration(EPCRequest Request)
        {
            string Result = string.Empty;

            Tempe_EPC_Request ObjEPC = new Tempe_EPC_Request();

            usp_GetTempeEPCDetails_Result ObjTempeData = EPC_TempeDAL.Get_Tempe_EPCDetail(Request.RPO, Request.DetailLineID);

            if (Convert.ToString(ObjTempeData.purchaseOrder) != "")
            {

                #region GENERATE REQUEST


                ObjEPC.purchaseOrder = ObjTempeData.purchaseOrder;
                ObjEPC.model = ObjTempeData.model;
                ObjEPC.quality = ObjTempeData.quality;
                ObjEPC.colors = new List<Color>();
                Color Objc = new Color();

                Objc.colorCode = ObjTempeData.colorCode;

                Objc.quantityBySize = new List<QuantityBySize>();

                QuantityBySize Obj = new QuantityBySize();

                Obj.sizeCode = ObjTempeData.sizeCode;
                Obj.quantity = Convert.ToString(Request.Quantity);

                Objc.quantityBySize.Add(Obj);
                ObjEPC.colors.Add(Objc);

                ObjEPC.supplierId = ObjTempeData.supplierId;
                ObjEPC.inventoryTag = Convert.ToString(ObjTempeData.inventoryTag);
                ObjEPC.eas = ObjTempeData.eas;
                ObjEPC.tagSubType = Convert.ToString(ObjTempeData.tagSubType);
                ObjEPC.tagType = Convert.ToString(ObjTempeData.tagType);
                #endregion

                #region CALLED HTTP

                string Error = string.Empty;

                string Json_Request = JsonConvert.SerializeObject(ObjEPC);

                TempeResponse Response = Http_Tempe(Json_Request, ObjTempeData.rfidRequestId, Request.RPO);

                if (Response.Response != null)
                {

                    var ObjResponse = JObject.Parse(Response.Response);

                    #endregion

                    #region READ RESPONSE

                    try
                    {

                        if (Response.IsSucess)
                        {
                            string link = ObjResponse["_links"]["external"]["href"].ToString();

                            Root ObjRoot = Http_Tempe_EPC(link, Request.RPO, ObjTempeData.rfidRequestId, link, Request);

                            InsertEPC(ObjRoot, Request.RPO, ObjTempeData.rfidRequestId, Request);
                        }
                        else
                        {
                            if (ObjResponse["errors"].Count() > 0)
                            {
                                Result = ObjResponse["errors"][0]["message"].ToString();

                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        Result = "No Response from  Tempe API " + ex.ToString() + "";

                    }
                    #endregion
                }
                else
                {
                    Result = "No Response from  Tempe API " + Request + "";
                }
            }
            else
            {
                Result = "No data found for the RPO " + Request + "";
            }

            return Result;
        }

        private static void InsertEPC(Root EPCLog, long RPO, string rfidRequestId, EPCRequest Request)
        {
            if (EPCLog.results != null)
            {
                #region Http_Tempe called


                if (EPCLog.results.Count > 0)
                {
                    List<epcDecode> epcdecodelist = new List<epcDecode>();

                    epcdecodelist = (from c in EPCLog.results
                                     select new epcDecode()
                                     {

                                         epc = c.epcHex,
                                         accessPasswordHex = c.accessPasswordHex,
                                         killPasswordHex = c.killPasswordHex,
                                         serialNumber = c.SerialNumber,
                                         sizeCode = c.sizeCode,
                                         userMemoryHex = c.userMemoryHex,

                                     }).ToList();

                    //List<EPCList> results = epcdecodelist.GroupBy( p => p.sizeCode, (key, g) => new { SizeCode = key, epc = g.FirstOrDefault().epc });

                    List<EPCList> results = (from c in epcdecodelist
                                             group c by c.sizeCode into g
                                             select new EPCList()
                                             {
                                                 SizeCode = g.FirstOrDefault().sizeCode.ToString(),
                                                 EPC = g.FirstOrDefault().epc.ToString()

                                             }).ToList();

                    for (int ec = 0; ec < results.Count(); ec++)
                    {
                        epcDecode Objepcdecode = new epcDecode();

                        Objepcdecode = Http_Tempe_Decode(results[ec].EPC, rfidRequestId, RPO, Request);

                        if (Objepcdecode.IsError != 1)
                        {
                            List<epcDecode> EPClist = (from c in epcdecodelist
                                                       where c.sizeCode.ToString() == results[ec].SizeCode
                                                       select new epcDecode()
                                                       {
                                                           epc = c.epc,
                                                           accessPasswordHex = c.accessPasswordHex,
                                                           killPasswordHex = c.killPasswordHex,
                                                           serialNumber = c.serialNumber,
                                                           sizeCode = c.sizeCode,
                                                           userMemoryHex = c.userMemoryHex,
                                                           activeTag = Objepcdecode.activeTag,
                                                           barCode = Objepcdecode.barCode,
                                                           brandId = Objepcdecode.brandId,
                                                           color = Objepcdecode.color,
                                                           eas = Objepcdecode.eas,
                                                           encodeCheck = Objepcdecode.encodeCheck,
                                                           free = Objepcdecode.free,
                                                           inventoryTag = Objepcdecode.inventoryTag,
                                                           model = Objepcdecode.model,
                                                           Number = Objepcdecode.Number,
                                                           productCompositionId = Objepcdecode.productCompositionId,
                                                           productTypeCode = Objepcdecode.productTypeCode,
                                                           quality = Objepcdecode.quality,
                                                           sectionId = Objepcdecode.sectionId,
                                                           supplierId = Objepcdecode.supplierId,
                                                           tagSubType = Objepcdecode.tagSubType,
                                                           tagType = Objepcdecode.tagType,
                                                           version = Objepcdecode.version


                                                       }).ToList();


                            string xmlnewstring = JsonConvert.SerializeObject(EPClist);
                            EPC_TempeDAL.InsertJSON_EPCDeCode(xmlnewstring.ToString(), rfidRequestId, RPO);
                        }
                        else
                        {

                        }
                    }


                }
                #endregion
            }
        }

        private static TempeResponse Http_Tempe(string Request, string rfidRequestId, long RPO)
        {

            TempeResponse Response = new TempeResponse();

            string EPCpre_url = ConfigurationManager.AppSettings["Tempe_WebServiceURL_tags"].ToString();
            string itx_apiKey = ConfigurationManager.AppSettings["Tempe_WebServiceURL_tagsKey"].ToString();


            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var clientepcLog = new RestClient(EPCpre_url);
            clientepcLog.Timeout = -1;
            var requestepcLog = new RestRequest(Method.POST);
            requestepcLog.AddHeader("itx-apiKey", itx_apiKey);
            requestepcLog.AddHeader("Content-Type", "application/json");

            //var body = @"";
            requestepcLog.AddParameter("application/json", Request, ParameterType.RequestBody);
            IRestResponse responseepcLog = clientepcLog.Execute(requestepcLog);

            int StatusCode = Convert.ToInt32(((RestSharp.RestResponseBase)responseepcLog).StatusCode);

            if (StatusCode == 200 || StatusCode == 201)
            {

                Response.Response = responseepcLog.Content.Substring(1, responseepcLog.Content.Length - 2);
                Response.IsSucess = true;
            }
            else
            {
                Response.Response = responseepcLog.Content;
            }


            return Response;
        }

        private static Root Http_Tempe_EPC(string NewLink, long RPO, string rfidRequestId, string links_external_href, EPCRequest ObjEPCRequest)
        {

            Root myDeserializedClass = new Root();

            string itx_apiKey = ConfigurationManager.AppSettings["itx_apiKey_PRO"].ToString();

            int StatusCode = 0;

            #region epc called


            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var clientRfidexecution = new RestClient(NewLink);
            clientRfidexecution.Timeout = -1;
            var requestRfidexecution = new RestRequest(Method.GET);

            var body = @"";
            requestRfidexecution.AddHeader("itx-apiKey", itx_apiKey);
            requestRfidexecution.AddParameter("text/plain", body, ParameterType.RequestBody);
            IRestResponse responseRfidexecution = clientRfidexecution.Execute(requestRfidexecution);

            string respRfidexecution = "";

            StatusCode = Convert.ToInt32(((RestSharp.RestResponseBase)responseRfidexecution).StatusCode);

            if (StatusCode == 200 || StatusCode == 206)
            {
                respRfidexecution = responseRfidexecution.Content;

                //DAL.InsertReqRes(NewLink, respRfidexecution, RPO, rfidRequestId, links_external_href, "");
                EPCDAL.InsertReqRes(ObjEPCRequest.CustomerID, ObjEPCRequest.RPO, ObjEPCRequest.DetailLineID, NewLink, respRfidexecution, NewLink, ObjEPCRequest.UserId, ObjEPCRequest.GTIN);

                myDeserializedClass = JsonConvert.DeserializeObject<Root>(responseRfidexecution.Content);
                if (myDeserializedClass.results.Count > 0)
                {

                    myDeserializedClass.results = (from c in myDeserializedClass.results
                                                   select new Result()
                                                   {
                                                       epcHex = c.epcHex,
                                                       accessPasswordHex = c.accessPasswordHex,
                                                       killPasswordHex = c.killPasswordHex,
                                                       sizeCode = c.sizeCode,
                                                       userMemoryHex = c.userMemoryHex,
                                                       SerialNumber = GetHexToInt(c.epcHex)
                                                   }
                                                   ).ToList();

                    string JSON = JsonConvert.SerializeObject(myDeserializedClass);
                    EPC_TempeDAL.InsertJSON_EPCLog(JSON, RPO, rfidRequestId);

                }


            }
            else
            {
                respRfidexecution = "StatusCode: " + Convert.ToString(StatusCode) + ", Error: " + responseRfidexecution.Content;

                EPCDAL.InsertReqRes(ObjEPCRequest.CustomerID, ObjEPCRequest.RPO, ObjEPCRequest.DetailLineID, NewLink, respRfidexecution, NewLink, ObjEPCRequest.UserId, ObjEPCRequest.GTIN);

            }
            #endregion

            return myDeserializedClass;
        }

        private static epcDecode Http_Tempe_Decode(string epc, string rfidRequestId, long RPO, EPCRequest ObjEPCRequest)
        {

            epcDecode myDeserializedClass = new epcDecode();

            string EPCpre_url = ConfigurationManager.AppSettings["EPCpre_url"].ToString();
            string itx_apiKey = ConfigurationManager.AppSettings["itx_apiKey_PRE"].ToString();

            string Link = EPCpre_url + "/api/v1/product/epc/" + epc + "/decode";

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var clientepcLog = new RestClient(Link);
            clientepcLog.Timeout = -1;
            var requestepcLog = new RestRequest(Method.GET);
            requestepcLog.AddHeader("itx-apiKey", itx_apiKey);
            requestepcLog.AddHeader("Content-Type", "application/json");

            var body = @"";
            requestepcLog.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse responseepcLog = clientepcLog.Execute(requestepcLog);

            int StatusCode = Convert.ToInt32(((RestSharp.RestResponseBase)responseepcLog).StatusCode);

            EPCDAL.InsertReqRes(ObjEPCRequest.CustomerID, ObjEPCRequest.RPO, ObjEPCRequest.DetailLineID, Link, responseepcLog.Content, Link, ObjEPCRequest.UserId, ObjEPCRequest.GTIN);

            if (StatusCode == 200 || StatusCode == 201)
            {
                myDeserializedClass = JsonConvert.DeserializeObject<epcDecode>(responseepcLog.Content);
                //myDeserializedClass.Number = GetHexToInt(epc);

                myDeserializedClass.epc = myDeserializedClass.epc == null ? epc : Convert.ToString(myDeserializedClass.epc);

            }
            else
            {
                try
                {
                    EPCError ObjEPCError = JsonConvert.DeserializeObject<EPCError>(responseepcLog.Content);
                    if (ObjEPCError.errors != null)
                    {
                        myDeserializedClass.Error = ObjEPCError.errors[0].message.Replace("'", "''");
                    }
                    else
                    {
                        myDeserializedClass.Error = responseepcLog.Content;
                    }
                    myDeserializedClass.IsError = 1;
                    myDeserializedClass.epc = epc;
                }
                catch
                {
                    myDeserializedClass = null;
                }


            }


            return myDeserializedClass;
        }

        private static long GetHexToInt(string EPC)
        {
            /*
            string hex = EPC.Substring(15, 9);
            return Int64.Parse(hex, System.Globalization.NumberStyles.HexNumber);
            */

            string epcClean = EPC.Replace(" ", "").Replace("-", ""); // handleCopyPaste

            string binEpc = String.Join(String.Empty, epcClean.Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));

            if (epcClean.Length != 32)
                throw new Exception("Invaid EPC Len");

            int Version = Convert.ToInt32(binEpc.Substring(0, 5), 2);//Hardcoded.
            long Serial;

            switch (Version)
            {
                case 1:
                    Serial = Convert.ToInt64(binEpc.Substring(64, 32), 2);
                    break;
                case 2:
                    Serial = Convert.ToInt64(binEpc.Substring(96, 32), 2);
                    break;
                default:
                    throw new Exception("Invalid Version");
            }

            return Serial;
        }
    }

    public class TempeResponse
    {
        public bool IsSucess { get; set; }
        public string Response { get; set; }


    }

    public class Error
    {
        public string code { get; set; }
        public string description { get; set; }
        public string level { get; set; }
        public string message { get; set; }
    }

    public class EPCError
    {
        public List<Error> errors { get; set; }
    }
    public class EPCList
    {
        public long RPO { get; set; }
        public string rfidRequestId { get; set; }
        public string EPC { get; set; }
        public string SizeCode { get; set; }

    }

    public class Tempe_EPC_Request
    {
        public string purchaseOrder { get; set; }
        public string model { get; set; }
        public string quality { get; set; }
        public List<Color> colors { get; set; }
        public string supplierId { get; set; }
        public string inventoryTag { get; set; }
        public string eas { get; set; }
        public string tagSubType { get; set; }
        public string tagType { get; set; }
    }
    public class Color
    {
        public string colorCode { get; set; }
        public List<QuantityBySize> quantityBySize { get; set; }
    }
    public class QuantityBySize
    {
        public string sizeCode { get; set; }
        public string quantity { get; set; }
    }

    public class Root
    {
        public MetadataTagPage metadataTagPage { get; set; }
        public List<Result> results { get; set; }
    }
    public class MetadataTagPage
    {
        public ResultSet resultSet { get; set; }
        public string version { get; set; }
        public SecurityBits securityBits { get; set; }
    }
    public class Result
    {
        public int sizeCode { get; set; }
        public string epcHex { get; set; }
        public string accessPasswordHex { get; set; }
        public string killPasswordHex { get; set; }
        public string userMemoryHex { get; set; }

        public long SerialNumber { get; set; }
        public long RPO { get; set; }

    }
    public class ResultSet
    {
        public int count { get; set; }
        public int offset { get; set; }
        public string limit { get; set; }
        public int total { get; set; }
    }
    public class SecurityBits
    {
        public int epcLock { get; set; }
        public int epcPermaLock { get; set; }
        public int userMemoryLock { get; set; }
        public int userMemoryPermaLock { get; set; }
        public int killPasswordLock { get; set; }
        public int killPasswordPermaLock { get; set; }
        public int accessPasswordLock { get; set; }
        public int accessPasswordPermaLock { get; set; }
    }
    public class epcDecode
    {
        public string epc { get; set; }

        public string version { get; set; }
        public int brandId { get; set; }
        public int sectionId { get; set; }
        public int productTypeCode { get; set; }
        public int activeTag { get; set; }
        public int encodeCheck { get; set; }
        public object inventoryTag { get; set; }
        public int supplierId { get; set; }
        public int free { get; set; }
        public int sizeCode { get; set; }
        public int color { get; set; }
        public int quality { get; set; }
        public int model { get; set; }
        public object productCompositionId { get; set; }
        public int tagType { get; set; }
        public object tagSubType { get; set; }
        public long serialNumber { get; set; }
        public long barCode { get; set; }
        public string eas { get; set; }
        public string accessPasswordHex { get; set; }
        public string killPasswordHex { get; set; }
        public string userMemoryHex { get; set; }
        public string Error { get; set; }
        public int IsError { get; set; }
        public long Number { get; set; }
    }
}
