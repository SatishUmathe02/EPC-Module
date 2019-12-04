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
                ObjRes = await Run_GetEPC(Request);

                return Ok(ObjRes);
            }
            catch (Exception Ex)
            {
                EPCBLL.InsertLog(Ex, "api/apiEPC/GetEPC");
                return Ok(Ex.ToString());
            }

        }

        private static async Task<EPCResponse> Run_GetEPC(EPCRequest Request)
        {
            //EPCRequest Obj = new EPCRequest();

            //Obj = JsonConvert.DeserializeObject<EPCRequest>(Request);

            if (Request != null)
            {
                EPCResponse ObjRes = Transaction_New.GetEPC_New(Request);

                if (Request.CustomerID == "EncuentroModa")
                {
                    await EPCPasswordBLL.UpdatePassword(Request.RPO, Request.DetailLineID);
                }

                return ObjRes;
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
