using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BussinessLayer;
using DataAccessLayer.CommonDataModels;

namespace WebApi.Controllers
{
    public class apiEPCISController : ApiController
    {
        #region GET EPCIS CUSTOMER LIST
        [HttpGet]
        [Route("api/apiEPCIS/GetEPCISCustomer")]
        public List<string> GetEPCISCustomer()
        {

            return EPCISBLL.GetEPCISCustomer();

        }
        #endregion

        #region GET EPCIS URN NUMBER
        //[HttpPost]
        [Route("api/apiEPCIS/GetEPCIS_URN")]
        public List<EPCISDO> GetEPCIS_URN(string CustomerId)
        {
            return EPCISBLL.GetEPCIS_URN(CustomerId);

        }
        #endregion

        #region INSERT EPCIS LOG 
        [HttpPost]
        [Route("api/apiEPCIS/InsertEPCISLog")]
        public string InsertEPCISLog([FromBody] EPCIS ObjEPCIS)
        {
            return EPCISBLL.InsertEPCISLog(ObjEPCIS);

        }
        #endregion

        #region GET EPCIS URN NUMBER
        //[HttpPost]
        [Route("api/apiEPCIS/GetEPCIS_URN_RPO")]
        public List<EPCISDO> GetEPCIS_URN_RPO(string CustomerId, long RPO)
        {
            return EPCISBLL.GetEPCIS_URN_RPO(CustomerId,RPO);

        }
        #endregion

    }
}
