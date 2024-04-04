using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Http;
using DataAccessLayer.CommonDataModels;
using BussinessLayer;
using System.Threading.Tasks;
using DataAccessLayer;

namespace WebApi.Controllers
{
    public class apiEPC_StanderCustomerController : ApiController
    {
        #region GET EPC FOR STANDER CUSTOMER
        [HttpPost]
        [Route("api/apiEPC/GetEPC_StanderCustomer")]
        public async Task<IHttpActionResult> GetEPC_StanderCustomer([FromBody] EPCRequest Request)
        {
            EPCResponse ObjRes = new EPCResponse();

            try
            {

                Request.RequestStartTime = System.DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff");


                if (Request.TransactionType == "New")
                {
                    ObjRes = await Run_GetEPC_StanderCustomer(Request);
                }
                else if (Request.TransactionType == "Test")
                {
                    ObjRes = await Run_GetEPC_StanderCustomer_Test(Request);
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

        private static async Task<EPCResponse> Run_GetEPC_StanderCustomer(EPCRequest Request)
        {

            if (Request.Quantity <= 0)
            {
                EPCResponse ObjRes = new EPCResponse();
                return ObjRes = EPCBLL.GetError(123);
            }

            if (Request != null)
            {

                usp_GetStanderCustomerDetails_Result ObjEC = EPC_StanderCustomerBLL.GetStanderCustomer_Details(Request);

                if (ObjEC == null)
                {
                    return EPCBLL.GetError(125);
                }
                else
                {
                    if (ObjEC.CustmerId == null)
                    {
                        return EPCBLL.GetError(125);
                    }
                }


                Request.GS1Customer = ObjEC.bitGS1;
                Request.GS1apiRequired = Convert.ToBoolean(ObjEC.GS1apiRequired);


                if (Request.GS1Customer) //the customer is GS1
                {
                    #region GS1 CALL

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

                        string GS1JSON = JsonConvert.SerializeObject(ObjGS1);
                        Request = GS1_IntergrationBLL.GS1_Details(Request, ObjGS1, GS1JSON);

                        if (string.IsNullOrEmpty(Request.GS1Prefix))
                        {
                            GS1_IntergrationBLL.EmailNotification(GS1_Response, ObjGS1, Request);
                        }
                    }


                    #endregion
                }
                else
                {
                    if (ObjEC.GS1Prefix == null || ObjEC.GS1Prefix == "")
                    {
                        return EPCBLL.GetError(124);
                    }
                    Request.GS1Prefix = Convert.ToString(ObjEC.GS1Prefix);
                    //Request.PartitionValue = Convert.ToString(ObjEC.GS1Prefix).Length;
                }


                #region EPC CALL


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

                    EPCResponse ObjRes = EPC_StanderCustomerBLL.GetEPC_SC(Request);


                    return ObjRes;
                }

                #endregion

            }
            else
            {
                return EPCBLL.GetError(107);
            }
        }
        private static async Task<EPCResponse> Run_GetEPC_StanderCustomer_Test(EPCRequest Request)
        {

            if (Request.Quantity <= 0)
            {
                EPCResponse ObjRes = new EPCResponse();
                return ObjRes = EPCBLL.GetError(123);
            }

            if (Request != null)
            {

                //usp_GetStanderCustomerDetails_Result ObjEC = EPC_StanderCustomerBLL.GetStanderCustomer_Details(Request);
                usp_GetStanderCustomerDetails_Result ObjEC = new usp_GetStanderCustomerDetails_Result();


                ObjEC.CustmerId = Request.CustomerID;
                ObjEC.SerialRange_Start = Request.Serial;
                ObjEC.bitGS1 = Request.GS1Customer;
                ObjEC.intFilterValue = Request.FilterValue;
                ObjEC.bitUniqueCodeSerialization = Request.UniqueCodeSerialization;
                ObjEC.GS1Prefix = Request.GS1Prefix;
                //ObjEC.GS1apiRequired = Convert.ToBoolean(Request.GS1Customer);
                

                if (ObjEC == null)
                {
                    return EPCBLL.GetError(125);
                }

                Request.GS1Customer = ObjEC.bitGS1;
                Request.GS1apiRequired = Convert.ToBoolean(Request.GS1Customer);


                if (Request.GS1Customer) //the customer is GS1
                {
                    #region GS1 CALL

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

                        string GS1JSON = JsonConvert.SerializeObject(ObjGS1);
                        Request = GS1_IntergrationBLL.GS1_Details(Request, ObjGS1, GS1JSON);

                        if (string.IsNullOrEmpty(Request.GS1Prefix))
                        {
                            GS1_IntergrationBLL.EmailNotification(GS1_Response, ObjGS1, Request);
                        }
                    }


                    #endregion
                }
                else
                {
                    if (ObjEC.GS1Prefix == null || ObjEC.GS1Prefix == "")
                    {
                        return EPCBLL.GetError(124);
                    }
                    Request.GS1Prefix = Convert.ToString(ObjEC.GS1Prefix);
                   // Request.PartitionValue = Convert.ToString(ObjEC.GS1Prefix).Length;
                }


                #region EPC CALL


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

                    EPCResponse ObjRes = EPC_StanderCustomerBLL.GetEPC_SC(Request);


                    return ObjRes;
                }

                #endregion

            }
            else
            {
                return EPCBLL.GetError(107);
            }
        }
        #endregion
    }
}
