using DataAccessLayer;
using DataAccessLayer.CommonDataModels;
using EPC_Kaibi;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace BussinessLayer
{
    public class Kiabi_IntergrationBLL
    {
        public Kiabi_IntergrationBLL()
        {
        }

        public static usp_GTIN_GetEPC_Kiabi_Range_Result Kaibi_apiResponse_Restapi(EPCRequest EPC_Req)
        {
            usp_GTIN_GetEPC_Kiabi_Range_Result uspGTINGetEPCKiabiRangeResult = new usp_GTIN_GetEPC_Kiabi_Range_Result();
            try
            {

                if(string.IsNullOrEmpty(EPC_Req.CustomPara1))
                {
                    uspGTINGetEPCKiabiRangeResult.Remark = "Customer Para is empty";
                    return uspGTINGetEPCKiabiRangeResult;
                }

                string[] strArrays = EPC_Req.CustomPara1.Split("#".ToCharArray());
                if (strArrays.Count<string>() <= 0)
                {
                    uspGTINGetEPCKiabiRangeResult.Remark = "Customer Para is empty";
                }
                else
                {
                    string empty = string.Empty;
                    string empty1 = string.Empty;

                    if (strArrays.Count() > 3)
                    {
                        string str = string.Concat(string.Concat(new object[] { "{\n  \"skuId\": ", strArrays[3], ",\n  \"poId\": ", strArrays[2], ",\n  \"skuQuantity\": ", EPC_Req.Quantity }), "\n}");

                        empty = Kiabi_http.Kiabi_apiResponse_Rest(EPC_Req, str).Result;
                    }
                    if (string.IsNullOrEmpty(empty))
                    {
                        uspGTINGetEPCKiabiRangeResult.Remark = "Error from Kiabi API: No response from Kiabi api";
                    }
                    else
                    {
                        try
                        {
                            dynamic obj = JsonConvert.DeserializeObject(empty);
                            if (obj["code"] != (dynamic)null)
                            {
                                if (Convert.ToInt32(obj["code"].ToString().Substring(0, 1)) == 4)
                                {
                                    uspGTINGetEPCKiabiRangeResult.Remark = "Error from Kiabi API:" + (string)obj["message"] +
                                         "<br/>" + "This order may have been cancelled. Please contact the vendor and global CS for further instructions."; //BR 65 

                                }
                            }
                            else if (obj["epcRangeId"] != (dynamic)null)
                            {
                                EPC_Req.CustomPara2 = (string)(obj["firstSerialNumber"] + "#" + obj["lastSerialNumber"] + "#" + obj["poId"] + "#" + obj["skuId"]);
                                uspGTINGetEPCKiabiRangeResult = EPCDAL.GetEPC_Kiabi_Range(EPC_Req.GTIN, EPC_Req.Quantity, EPC_Req.TransactionType, EPC_Req.Schema, EPC_Req.CustomerID, EPC_Req.CustomerName, EPC_Req.Event, EPC_Req.UserId, Convert.ToInt64(EPC_Req.Serial), EPC_Req.EPC, EPC_Req.RPO, EPC_Req.DetailLineID, EPC_Req.CustomPara1, EPC_Req.CustomPara2, EPC_Req.GS1Prefix, EPC_Req.PartitionValue, Convert.ToDateTime(EPC_Req.RequestStartTime));
                            }
                        }
                        catch (Exception exception)
                        {
                            // uspGTINGetEPCKiabiRangeResult.Remark = "Error from Kiabi API:" + exception.ToString();
                            uspGTINGetEPCKiabiRangeResult.Remark = "Error from Kiabi API:" + empty;
                        }
                    }
                }
            }
            catch (Exception exception1)
            {
                uspGTINGetEPCKiabiRangeResult.Remark = exception1.ToString();
            }
            return uspGTINGetEPCKiabiRangeResult;
        }
    }
}