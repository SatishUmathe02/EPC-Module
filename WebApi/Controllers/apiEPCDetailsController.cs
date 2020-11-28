using BussinessLayer;
using DataAccessLayer.CommonDataModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApi.Controllers
{
    public class apiEPCDetailsController : ApiController
    {


        #region EPC LOG
        [Route("api/apiEPC/GetEPCLog")]
        public List<EPCLog> GetEPCLog()
        {

            return EPCGTIN.GetEPCLog();

        }
        #endregion

        #region EPC SERIAL details
        [Route("api/apiEPC/GetEPCSerialDetails")]
        public List<EPCLog> GetEPCSerialDetails()
        {

            return EPCGTIN.GetEPCSerialDetails();

        }
        #endregion

        #region EPC COUNTER DETAILS
        [HttpGet]
        [Route("api/apiEPC/GetEPCCounter")]
        public List<EPCCounter> GetEPCCounter(long RPO)
        {

            return EPCGTIN.GetEPCCounter(RPO);

        }
        #endregion

        #region EPC CUSTOMER COUNT
        [HttpGet]
        [Route("api/apiEPC/GetCustomerCount")]
        public string GetCustomerCount()
        {

            return EPCGTIN.GetCustomerCount();

        }
        #endregion

        #region GET EPC CUSTOMER

        [HttpGet]
        [Route("api/apiEPC/GetEPCCustomer")]
        public List<EPCCustomer> GetEPCCustomer()
        {
            return EPCBLL.GetEPCCustomer();

        }

        #endregion

        #region GET ECP COUNTER DATA BY RPO AND SERIAL NUMBER
        [HttpGet]
        [Route("api/apiEPC/GetEPCCounter_RPO_SerialNum")]
        public List<EPCCounter> GetEPCCounter_RPO_SerialNum(string GTIN, long RPO, long DetailNo, long SerialStart, long SerialEnd)
        {

            return EPCGTIN.GetEPCCounter_RPO_SerialNum(GTIN, RPO, DetailNo, SerialStart, SerialEnd);

        }
        #endregion


        #region GS1 SERVICE
        [HttpGet]
        [Route("api/apiEPC/Get_GS1_Service")]
        public GS1Response Get_GS1_Service(string GTIN,string CustomerId)
        {
            GS1Response ObjGS1 = new GS1Response();
            EPCRequest ObjEPC = new EPCRequest();
            ObjEPC.GTIN = GTIN;
            ObjEPC.CustomerID = CustomerId;
            var GS1_Response = GS1_IntergrationBLL.GS1_apiResponse_Restapi(ObjEPC);
            
            try
            {
                
                ObjGS1.GS1List = JsonConvert.DeserializeObject<List<GS1>>(GS1_Response);
            }
            catch (Exception ex)
            {
                ObjGS1.Error = GS1_Response;
            }
            return ObjGS1;
        }
        #endregion
    }
}
