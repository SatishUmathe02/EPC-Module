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

    public class apiEPCController : ApiController
    {

        #region PING

        [Route("api/apiEPC/GetPing")]
        public IHttpActionResult GetPing()
        {
            string Meg = "Successfully Connected The EPC Service";
            var jsonResult = JsonConvert.SerializeObject(Meg);

            if (jsonResult != null)
            {
                return Ok(jsonResult);
            }
            return NotFound();
        }

        #endregion



        [HttpPost]
        [Route("api/apiEPC/GetEPC")]
        public async Task<IHttpActionResult> GetEPC([FromBody] EPCRequest Request)
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

                return Ok(ObjRes);
            }
            catch (Exception Ex)
            {
                //EPCBLL.InsertLog(Ex, "api/apiEPC/GetEPC");
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
                //if (Request.CustomerID == "GerryWeber")
                //Here we are checking the item code for GW which they will get from our EPC (r-trac EPC)
                if ((Request.CustomerID == "GerryWeber") && (Request.CustomPara1 != "Catalog"))
                {
                    EPCResponse ObjRes = Transaction_New_GWEPC.GetEPC_New(Request);
                    return ObjRes;
                }
                else
                {
                    //**
                    Request = GS1_IntergrationBLL.IsCustomerGS1(Request);

                    if (Request.GS1Customer) //the customer is GS1
                    {
                        if (Request.GS1apiRequired)
                        {
                            try
                            {
                                string GS1_Response = GS1_IntergrationBLL.GS1_apiResponse_Restapi(Request);
                                List<GS1> ObjGS1 = JsonConvert.DeserializeObject<List<GS1>>(GS1_Response);
                                string GS1JSON = JsonConvert.SerializeObject(ObjGS1);
                                Request = GS1_IntergrationBLL.GS1_Details(Request, ObjGS1, GS1JSON);

                                if (string.IsNullOrEmpty(Request.GS1Prefix))
                                {
                                    GS1_IntergrationBLL.EmailNotification(GS1_Response, ObjGS1, Request);
                                }

                            }
                            catch
                            {
                                Request.GS1Prefix = "";
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
                        if (Request.CustomerID == "StoneIsland")
                        {
                            ObjRes = Transaction_New.GetEPCDetail(Request);
                        }
                        if (ObjRes == null)
                        {
                            ObjRes = Transaction_New.GetEPC_New(Request);
                        }

                        //EPCResponse ObjRes = Transaction_New.GetEPC_New(Request);

                        bool flag = false;
                        switch (Request.CustomerID)
                        {

                            case "EncuentroModa":
                                flag = await EPCPasswordBLL.UpdatePassword_MODA(Request.RPO, Request.DetailLineID);
                                if (!flag)
                                {
                                    ObjRes = EPCBLL.GetError(122);
                                }
                                break;
                            case "MANGO":
                                flag = await EPCPasswordBLL.UpdatePassword_MANGO(Request.RPO, Request.DetailLineID);
                                if (!flag)
                                {
                                    ObjRes = EPCBLL.GetError(122);
                                }
                                break;
                            case "AlvaroMoreno":
                                flag = await EPCPasswordBLL.UpdatePassword_AlvaroMoreno(Request.RPO, Request.DetailLineID);
                                if (!flag)
                                {
                                    ObjRes = EPCBLL.GetError(122);
                                }
                                break;
                            default:
                                break;
                        }


                        return ObjRes;
                    }
                }


            }
            else
            {
                return EPCBLL.GetError(107);
            }
        }

        [HttpPost]
        [Route("api/apiEPC/GetEPCEncode")]
        public async Task<IHttpActionResult> GetEPCEncode([FromBody] EPCRequest Request)
        {
            EPCResponse ObjRes = new EPCResponse();

            try
            {
                Request.RequestStartTime = System.DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss.fff");
                ObjRes = await Run_GetEPCEncode(Request);

                return Ok(ObjRes);
            }
            catch (Exception ex)
            {
                EPCBLL.InsertLog(ex, "api/apiEPC/GetEPCEncode");
                return Ok(ex.ToString());
            }

        }

        private static async Task<EPCResponse> Run_GetEPCEncode(EPCRequest Request)
        {
            //EPCRequest Obj = new EPCRequest();

            //Obj = JsonConvert.DeserializeObject<EPCRequest>(Request);

            if (Request != null)
            {
                EPCResponse ObjRes = Transaction_Encode.GetEPC_Encode(Request);

                return ObjRes;
            }
            else
            {

                return EPCBLL.GetError(107);
            }
        }


        [HttpPost]
        [Route("api/apiEPC/GetEPCDecode")]
        public async Task<IHttpActionResult> GetEPCDecode([FromBody] EPCRequest Request)
        {
            EPCResponse ObjRes = new EPCResponse();

            try
            {
                Request.RequestStartTime = System.DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss.fff");
                ObjRes = await Run_GetEPCDecode(Request);

                return Ok(ObjRes);
            }
            catch (Exception ex)
            {
                EPCBLL.InsertLog(ex, "api/apiEPC/GetEPCDecode");
                return Ok(ex.ToString());
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
