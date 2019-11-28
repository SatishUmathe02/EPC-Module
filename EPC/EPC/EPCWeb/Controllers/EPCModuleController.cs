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
            ObjReq.CustomerID = "CENT";
            ObjReq.CustomerName = "Lacoste";

            ObjReq.GTIN = "00123456789012";
            ObjReq.Schema = "SGTIN-96";
            ObjReq.Quantity = 100;
            ObjReq.TransactionType = "New";
            ObjReq.EPC = "";
            ObjReq.Serial = 0;
            ObjReq.Event = "";

            var jsonReq = new JavaScriptSerializer().Serialize(ObjReq);

            JToken parsedJson = JToken.Parse(jsonReq);
            var beautified = parsedJson.ToString(Formatting.Indented);

            EPCModel Obj = new Models.EPCModel();

            Obj.Request = beautified;
            ///Obj.Response = "test";

            GetEPCCustomerCount();

            return View(Obj);
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

        public ActionResult GetResponse(EPCModel Obj)
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

            return View("EPCModule", Obj);
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


            // Res = GlobalVariables.WebApiClient.PostAsJsonAsync("api/apiEPC/GetEPC", ObjReq).Result;

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
    }
}