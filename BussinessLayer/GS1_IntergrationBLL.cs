using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.CommonDataModels;
using DataAccessLayer;
using System.Net.Http;
using System.Configuration;
using System.Web;
using System.Net;
using EPC_GS1;

namespace BussinessLayer
{
    public class GS1_IntergrationBLL
    {
        public static EPCRequest IsCustomerGS1(EPCRequest ObjRequest)
        {
            var list = GS1_IntergrationDAL.IsCustomerGS1(ObjRequest.GTIN, ObjRequest.CustomerID);

            ObjRequest.GS1Customer = Convert.ToBoolean(list.GS1Customer);
            ObjRequest.GS1apiRequired = Convert.ToBoolean(list.GS1apiRequired);

            return ObjRequest;

        }
        public static EPCRequest GS1_Details(EPCRequest ObjEPC, List<GS1> ObjGS1, string GS1JSON)
        {

            if (ObjGS1 != null)
            {

                for (int i = 0; i < ObjGS1.Count; i++)
                {
                    if (!string.IsNullOrEmpty(ObjGS1[0].Prefixes.GS1Prefix))
                    {
                        ObjEPC.GS1Prefix = ObjGS1[0].Prefixes.GS1Prefix;
                        break;
                    }
                }
            }

            if (!string.IsNullOrEmpty(ObjEPC.GS1Prefix))
            {
                //string GS1 = JsonConvert.SerializeObject(ObjGS1);
                var result = GS1_IntergrationDAL.InsertGS1Details_GetPartitionValue(ObjEPC.GTIN, ObjEPC.CustomerID, GS1JSON);

                if (result != null)
                {
                    ObjEPC.GS1Prefix = result.GS1Prefix;
                    ObjEPC.PartitionValue = Convert.ToInt32(result.Partiton);
                }
                else
                {
                    ObjEPC.GS1Prefix = null;
                }
            }

            return ObjEPC;
        }


        public static string GS1_apiResponse(EPCRequest ObjEPC)
        {
            return GS1_http.GS1_apiResponse(ObjEPC).Result;
        }

        public static string GS1_apiResponse_Restapi(EPCRequest ObjEPC)
        {
            return GS1_http.GS1_apiResponse_Rest(ObjEPC).Result;
        }

        public static void EmailNotification(string GS1JSON, List<GS1> listGs1, EPCRequest EPCReq)
        {
            try
            {
                if (listGs1.Count == 0)
                {
                    if (!string.IsNullOrEmpty(GS1JSON))
                    {
                        if (GS1JSON.Length <= 5)
                        {
                            GS1_http.SendEmail(3, GS1JSON, EPCReq.GTIN);
                        }
                    }
                }
                if (string.IsNullOrEmpty(EPCReq.GS1Prefix))
                {
                    if (listGs1.Count > 0)
                    {
                        GS1_http.SendEmail(2, GS1JSON, EPCReq.GTIN);

                        // No need to check because we already checked and set EPCReq.GS1Prefix 
                        //if (listGs1[0].Prefixes != null)
                        //{
                        //    int GSPre = Convert.ToString(listGs1[0].Prefixes.GS1Prefix).Length;
                        //    if (GSPre != 12 && GSPre != 11 && GSPre != 10 && GSPre != 9 && GSPre != 8 && GSPre != 7 && GSPre != 6)
                        //    {
                        //        GS1_http.SendEmail(2, GS1JSON, EPCReq.GTIN);
                        //    }
                        //}
                    }
                }
            }
            catch
            {

            }


        }
    }
}
