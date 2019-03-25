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

namespace WebApi.Controllers
{
    public class apiEPCController : ApiController
    {

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

        [HttpPost]
        [Route("api/apiEPC/GTIN")]
        public IHttpActionResult GTIN([FromBody] string Request)
        {
            EPCRequest Obj = new EPCRequest();

            try
            {
                Obj = JsonConvert.DeserializeObject<EPCRequest>(Request);

                if (Obj != null)
                {
                    EPCResponse ObjRes = EPCBLL.GTIN(Obj);
                   // var jsonRes = new JavaScriptSerializer().Serialize(ObjRes);
                    return Ok(ObjRes);
                }

                
            }catch(Exception ex)
            {
                //var jsonRes = new JavaScriptSerializer().Serialize(ex.Message);
                return Ok(ex.Message);
            }
            return NotFound();
        }

        #region EPC LOG
        [Route("api/apiEPC/GetEPCLog")]
        public List<EPCLog> GetEPCLog()
        {

           return EPCBLL.GetEPCLog();

        }
        #endregion

        #region EPC LOG
        [Route("api/apiEPC/GetEPCSerial")]
        public List<EPCLog> GetEPCSerial()
        {

            return EPCBLL.GetEPCSerial();

        }
        #endregion
    }
}
