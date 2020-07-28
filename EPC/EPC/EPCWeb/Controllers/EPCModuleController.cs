using EPCWeb.CommonFunctions;
using EPCWeb.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace EPCWeb.Controllers
{
    public class EPCModuleController : Controller
    {

        public ActionResult EPCModule()
        {
            EPCRequest ObjReq = new EPCRequest();
            ObjReq.CustomerID = "";
            ObjReq.CustomerName = "";

            ObjReq.GTIN = "00123456789012";
            ObjReq.Schema = "SGTIN-96";
            ObjReq.Quantity = 1;
            ObjReq.TransactionType = "New";
            ObjReq.EPC = "";
            ObjReq.Serial = 0;
            ObjReq.RPO = 999999910;
            ObjReq.DetailLineID = 99999910;
            ObjReq.Serial = 0;

            ObjReq.Event = "";

            var jsonReq = new JavaScriptSerializer().Serialize(ObjReq);

            JToken parsedJson = JToken.Parse(jsonReq);
            var beautified = parsedJson.ToString(Formatting.Indented);

            EPCModel Obj = new Models.EPCModel();

            Obj.Request = beautified;
            Obj.EPCRequest = ObjReq;
            Obj.EPCResponse = new EPCResponse();
            Obj.CustomerList = GetCustomerList();
            Obj.EPCCounterList = new List<EPCCounter>();
            Obj.GS1_Info = new List<GS1>();
            //GetEPCCustomerCount();

            return View(Obj);
        }

        private List<SelectListItem> GetCustomerList()
        {
            List<SelectListItem> ObjList = new List<SelectListItem>();
            HttpResponseMessage Res = GlobalVariables.WebApiClient.GetAsync("api/apiEPC/GetEPCCustomer").Result;
            if (Res.IsSuccessStatusCode)
            {
                var result = Res.Content.ReadAsStringAsync().Result;
                List<EPCCustomer> resultList = JsonConvert.DeserializeObject<List<EPCCustomer>>(result);

                ObjList = (from c in resultList
                           select new SelectListItem()
                           {
                               Text = c.CustomerName,
                               Value = c.CustomerId
                           }).ToList();
                    
            }

            TempData["CustomerList"] = ObjList;
            return ObjList;
        }

        public void GetTest()
        {
            HttpResponseMessage Res = GlobalVariables.WebApiClient.GetAsync("api/apiEPC/GetPing").Result;
            if (Res.IsSuccessStatusCode)
            {
                var result = Res.Content.ReadAsStringAsync().Result;
                result = JsonConvert.DeserializeObject<string>(result);
            }
        }

        public ActionResult GetJSONResponse(EPCModel Obj)
        {
            try
            {
                
                GetEPC(Obj);
            }
            catch (Exception ex)
            {
                Obj.Response = ex.ToString();
                ViewBag.Response = ex.ToString();
            }
            Obj.CustomerList = GetCustomerList();
            Obj.EPCRequest = JsonConvert.DeserializeObject<EPCRequest>(Obj.Request);
            Obj.EPCResponse = JsonConvert.DeserializeObject<EPCResponse>(Obj.Response);
            if (Obj.EPCResponse.Remark == "Success")
            {
                Obj.EPCCounterList = GetEPCCounter_RPO_SerialNum(Obj.EPCResponse.GTIN, Obj.EPCRequest.RPO, Obj.EPCRequest.DetailLineID, Convert.ToInt64(Obj.EPCResponse.SerialStart), Convert.ToInt64(Obj.EPCResponse.SerialEnd));
            }else
            {
                Obj.EPCCounterList = new List<EPCCounter>();
                Obj.GS1_Info = new List<GS1>();
            }
            
            return View("EPCModule", Obj);
        }

        public ActionResult GetResponse(EPCModel Obj)
        {
            try
            {
                
                Obj.EPCRequest = FilObject(Obj.EPCRequest);
                var jsonReq = new JavaScriptSerializer().Serialize(Obj.EPCRequest);
                JToken parsedJson = JToken.Parse(jsonReq);
                Obj.Request = parsedJson.ToString(Formatting.Indented);

                GetEPC(Obj);
            }
            catch (Exception ex)
            {
                Obj.Response = ex.ToString();
                ViewBag.Response = ex.ToString();
            }
            Obj.CustomerList = GetCustomerList();
            Obj.EPCRequest = JsonConvert.DeserializeObject<EPCRequest>(Obj.Request);
            Obj.EPCResponse = JsonConvert.DeserializeObject<EPCResponse>(Obj.Response);

            if (Obj.EPCResponse.Remark == "Success")
            {
                Obj.EPCCounterList = GetEPCCounter_RPO_SerialNum(Obj.EPCRequest.GTIN, Obj.EPCRequest.RPO, Obj.EPCRequest.DetailLineID, Convert.ToInt64(Obj.EPCResponse.SerialStart), Convert.ToInt64(Obj.EPCResponse.SerialEnd));
                Obj.GS1_Info = GetGS1_Service(Obj.EPCRequest.GTIN, Obj.EPCRequest.CustomerID);
            }
            else
            {
                Obj.EPCCounterList = new List<EPCCounter>();
                Obj.GS1_Info = new List<GS1>();
            }

            return View("EPCModule", Obj);
        }

        
        public string GetJSON(string CustomerID, string GTIN, string Quantity, string RPO, string DetailLineID)
        {
            List<SelectListItem> ObjList = new List<SelectListItem>();

            

            EPCRequest ObjReq = new EPCRequest();
            

            var jsonReq = new JavaScriptSerializer().Serialize(ObjReq);

            JToken parsedJson = JToken.Parse(jsonReq);
            var beautified = parsedJson.ToString(Formatting.Indented);
            return beautified;
        }

        public EPCRequest FilObject(EPCRequest ObjReq)
        {
            
            ObjReq.CustomerName = (from c in GetCustomerList()
                                   where c.Value == ObjReq.CustomerID
                                   select c.Text).FirstOrDefault();

            
            ObjReq.Schema = "SGTIN-96";
            ObjReq.TransactionType = "New";
            ObjReq.EPC = "";
            ObjReq.Serial = 0;
            ObjReq.Serial = 0;
            ObjReq.Event = "";
            return ObjReq;
        }

        public void GetEPC(EPCModel Obj)
        {

            EPCRequest ReqObj = JsonConvert.DeserializeObject<EPCRequest>(Obj.Request);

            //for (int i = 1; i < 16; i++)
            //{


            //    EPCRequest ReqObj = new EPCRequest();
            //    ReqObj.CustomerID = "TEMPE";
            //    ReqObj.CustomerName = "TEMPE";

            //    ReqObj.GTIN = "00186107000507";
            //    ReqObj.Schema = "SGTIN-96";
            //    ReqObj.Quantity = 100;
            //    ReqObj.TransactionType = "New";
            //    ReqObj.EPC = "";
            //    ReqObj.Serial = 0;
            //    ReqObj.Event = "";
            //    ReqObj.RPO = 1000000857;
            //    ReqObj.DetailLineID = 6000051807;
            //    ReqObj.CustomPara1 = "NCDV#Z#" + i +"#1#New";




            HttpResponseMessage Res = new HttpResponseMessage();


            Res = GlobalVariables.WebApiClient.PostAsJsonAsync("api/apiEPC/GetEPC", ReqObj).Result;

            /*
            if (ReqObj.TransactionType == "New")
            {

                Res = GlobalVariables.WebApiClient.PostAsJsonAsync("api/apiEPC/GetEPC", ReqObj).Result;
            }
            else if (ReqObj.TransactionType == "Encode")
            {
                Res = GlobalVariables.WebApiClient.PostAsJsonAsync("api/apiEPC/GetEPCEncode", ReqObj).Result;
            }
            else if (ReqObj.TransactionType == "Decode")
            {
                Res = GlobalVariables.WebApiClient.PostAsJsonAsync("api/apiEPC/GetEPCDecode", ReqObj).Result;
            }
            else
            {
                Res = GlobalVariables.WebApiClient.PostAsJsonAsync("api/apiEPC/GetEPC", ReqObj).Result;
            }
            */
            if (Res.IsSuccessStatusCode)
            {
                var result = Res.Content.ReadAsStringAsync().Result;
                Obj.EPCResponse = JsonConvert.DeserializeObject<EPCResponse>(result);

                var jsonReq = new JavaScriptSerializer().Serialize(Obj.EPCResponse);
                JToken parsedJson = JToken.Parse(jsonReq);
                Obj.Response = parsedJson.ToString(Formatting.Indented);
                ViewBag.Response = Obj.Response;
            }
            //}
        }

        #region LOAD DATA
        public async Task<ActionResult> LoadData()
        {
            // string page;
            var draw = "";

            int recordsTotal = 0;

            List<EPCLog> Objepc = GetEpcLog();

            //total number of rows count     
            recordsTotal = Objepc.Count();


            //Paging     
            var data = Objepc;// parse.Skip(skip).Take(pageSize).ToList();

            //Returning Json Data    
            // return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
            return new JsonResult()
            {
                ContentEncoding = Encoding.Default,
                ContentType = "application/json",
                Data = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data },
                JsonRequestBehavior = 0,
                MaxJsonLength = int.MaxValue
            };

        }
        #endregion
        
        #region EPC LOG
        public List<EPCLog> GetEpcLog()
        {
            List<EPCLog> ObjList = new List<Models.EPCLog>();
            //HttpResponseMessage Res = GlobalVariables.WebApiClient.GetAsync("api/apiEPC/GetEPCLog").Result;
            //if (Res.IsSuccessStatusCode)
            //{
            //    var result = Res.Content.ReadAsStringAsync().Result;
            //    ObjList = JsonConvert.DeserializeObject<List<EPCLog>>(result);
            //}

            return ObjList;
        }
        #endregion
        
        #region LOAD EPC SERIAL DATA
        public async Task<ActionResult> LoadEPCSrialData()
        {
            // string page;
            var draw = "";

            int recordsTotal = 0;

            List<EPCLog> Objepc = GetEpcSerail();

            //total number of rows count     
            recordsTotal = Objepc.Count();


            //Paging     
            var data = Objepc;// parse.Skip(skip).Take(pageSize).ToList();

            //Returning Json Data    
            // return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
            return new JsonResult()
            {
                ContentEncoding = Encoding.Default,
                ContentType = "application/json",
                Data = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data },
                JsonRequestBehavior = 0,
                MaxJsonLength = int.MaxValue
            };

        }
        #endregion

        #region EPC SERAIL 
        public List<EPCLog> GetEpcSerail()
        {
            List<EPCLog> ObjList = new List<Models.EPCLog>();
            //HttpResponseMessage Res = GlobalVariables.WebApiClient.GetAsync("api/apiEPC/GetEPCSerialDetails").Result;
            //if (Res.IsSuccessStatusCode)
            //{
            //    var result = Res.Content.ReadAsStringAsync().Result;
            //    ObjList = JsonConvert.DeserializeObject<List<EPCLog>>(result);
            //}

            return ObjList;
        }
        #endregion

        #region EPC CUSTOMER COUNT
        public void GetEPCCustomerCount()
        {
            HttpResponseMessage Res = GlobalVariables.WebApiClient.GetAsync("api/apiEPC/GetCustomerCount").Result;
            if (Res.IsSuccessStatusCode)
            {
                var result = Res.Content.ReadAsStringAsync().Result;
                //result = JsonConvert.DeserializeObject<string>(result);
            }
        }
        #endregion

        #region EPC COUNTER VIA RPO WITH SERIAL NUMBER
        public List<EPCCounter> GetEPCCounter_RPO_SerialNum(string GTIN, long RPO, long DetailNo, long SerialStart, long SerialEnd)
        {
            List<EPCCounter> Objlist = new List<EPCCounter>();
            HttpResponseMessage Res = GlobalVariables.WebApiClient.GetAsync("api/apiEPC/GetEPCCounter_RPO_SerialNum?GTIN="+ GTIN + "&RPO=" + RPO + "&DetailNo="+ DetailNo + "&SerialStart="+ SerialStart + "&SerialEnd="+ SerialEnd + "").Result;
            if (Res.IsSuccessStatusCode)
            {
                var result = Res.Content.ReadAsStringAsync().Result;
                Objlist = JsonConvert.DeserializeObject<List<EPCCounter>>(result);
            }

            return Objlist;
        }
        #endregion

        #region GS1 SERVICE
        public List<GS1> GetGS1_Service(string GTIN, string CustomerId)
        {
            List<GS1> Objlist = new List<GS1>();
            HttpResponseMessage Res = GlobalVariables.WebApiClient.GetAsync("api/apiEPC/Get_GS1_Service?GTIN=" + GTIN + "&CustomerId="+ CustomerId + "").Result;
            if (Res.IsSuccessStatusCode)
            {
                var result = Res.Content.ReadAsStringAsync().Result;
                Objlist = JsonConvert.DeserializeObject<List<GS1>>(result);
            }

            return Objlist;
        }
        #endregion
    }
}