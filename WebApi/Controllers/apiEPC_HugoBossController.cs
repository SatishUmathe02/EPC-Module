using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DataAccessLayer.CommonDataModels;
using BussinessLayer;
using System.Web.Script.Serialization;
using System.Web.Http.Cors;
using System.Threading.Tasks;
using EPCGerryWeber;

namespace WebApi.Controllers
{
    public class apiEPC_HugoBossController : apiEPCController
    {
        // GET: apiEPC_HugoBoss
        [HttpPost]
        [Route("api/apiEPC/GetEPC_HugoBoss")]
        public async Task<IHttpActionResult> GetEPC_HugoBoss([FromBody] EPCRequest Request)
        {
            EPCResponse ObjRes = new EPCResponse();

            try
            {

                Request.RequestStartTime = System.DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff");


                if (Request.TransactionType == "New" || Request.TransactionType == "Encode")
                {
                    ObjRes = await Run_GetEPC(Request);
                }
                else if (Request.TransactionType == "Decode")
                {
                    ObjRes = await Run_GetEPCDecode(Request);
                }

                string epc_res = JsonConvert.SerializeObject(ObjRes);
                EPCBLL.rtrac_EPCReqRes(Request, epc_res);
                return Ok(ObjRes);
            }
            catch (Exception Ex)
            {
                //EPCBLL.InsertLog(Ex, "api/apiEPC/GetEPC");
                EPCBLL.rtrac_EPCReqRes(Request, Ex.ToString());
                return Ok(Ex.ToString());
            }


        }

        private static async Task<EPCResponse> Run_GetEPC(EPCRequest Request)
        {
            //EPCRequest Obj = new EPCRequest();

            //Obj = JsonConvert.DeserializeObject<EPCRequest>(Request);

            if (Request.Quantity <= 0)
            {
                EPCResponse ObjRes = new EPCResponse();
                return ObjRes = EPCBLL.GetError(123);
            }

            if (Request != null)
            {

                //**
                Request = GS1_IntergrationBLL.IsCustomerGS1(Request);

                if (Request.GS1Customer) //the customer is GS1
                {
                    if (Request.GS1apiRequired)
                    {
                        List<GS1> ObjGS1 = new List<GS1>();
                        string GS1_Response = string.Empty;

                        try
                        {
                            GS1_Response = GS1_IntergrationBLL.GS1_apiResponse_Restapi(Request);
                            ObjGS1 = JsonConvert.DeserializeObject<List<GS1>>(GS1_Response);

                        }
                        catch (Exception ex)
                        {
                            Request.GS1Prefix = "";
                        }

                        if (ObjGS1.Count == 0 && Request.CustomerID.ToUpper() == "CABOT")
                        {
                            Request.GS1Prefix = "Defualt";
                            Request.PartitionValue = 5;
                        }
                        else
                        {
                            string GS1JSON = JsonConvert.SerializeObject(ObjGS1);
                            Request = GS1_IntergrationBLL.GS1_Details(Request, ObjGS1, GS1JSON);
                        }

                        if (string.IsNullOrEmpty(Request.GS1Prefix))
                        {
                            GS1_IntergrationBLL.EmailNotification(GS1_Response, ObjGS1, Request);
                        }
                    }

                }
                //***
                if ((Request.GS1Customer && Request.GS1apiRequired) && (string.IsNullOrEmpty(Request.GS1Prefix)))
                {
                    // show the error
                    EPCResponse EPC_Res = new EPCResponse();
                    EPC_Res.EPCStart = "";
                    EPC_Res.EPCEnd = "";
                    EPC_Res.SerialStart = "";
                    EPC_Res.SerialEnd = "";
                    EPC_Res.GTIN = "";
                    EPC_Res.CustomerID = Request.CustomerID;
                    EPC_Res.Remark = "The GTIN " + Request.GTIN + " included in this order does not exist in GS1 database. Please contact Customer Service to confirm GTIN:" + Request.GTIN;

                    return EPC_Res;
                }
                else
                {

                    EPCResponse ObjRes = null;

                    ObjRes = Transaction_HugoBossBLL.GetEPC_New_HugoBoss(Request);



                    return ObjRes;
                }



            }
            else
            {
                return EPCBLL.GetError(107);
            }
        }

        private static async Task<EPCResponse> Run_GetEPCDecode(EPCRequest Request)
        {
            EPCRequest Obj = new EPCRequest();

            //Obj = JsonConvert.DeserializeObject<EPCRequest>(Request);

            if (Request != null)
            {
                EPCResponse ObjRes = Transaction_Decode.GetEPC_Decode(Request);

                return ObjRes;
            }
            else
            {
                return EPCBLL.GetError(107);
            }
        }
    }
}