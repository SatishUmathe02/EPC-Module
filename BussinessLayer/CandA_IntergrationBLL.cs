using DataAccessLayer;
using DataAccessLayer.CommonDataModels;
using EPC_CandA;
using Newtonsoft.Json;
using System;

namespace BussinessLayer
{
    public class CandA_IntergrationBLL : EPCBLL
    {
        public static bool CandA_GSIM;
        static CandA_IntergrationBLL()
        {
            CandA_IntergrationBLL.CandA_GSIM = CandA_http.CandA_GSIM;


        }

        public static EPCResponse GetCA_SGTIN_Serial(EPCRequest EPC_Req)
        {
            EPCResponse EPC_Res = new EPCResponse();
            try
            {

                // Check GTIN 
                int GTINLen = EPC_Req.GTIN.Length;
                if (GTINLen < 14)
                {
                    int atuallen = 14 - GTINLen;
                    string _gtin = "";
                    for (int i = 0; i < atuallen; i++)
                    {
                        _gtin += "0";
                    }

                    EPC_Req.GTIN = _gtin + EPC_Req.GTIN;
                }

                EPC_Res.EPCStart = "";
                EPC_Res.EPCEnd = "";
                EPC_Res.Quantity = EPC_Req.Quantity;
                EPC_Res.GTIN = EPC_Req.GTIN;
                EPC_Res.CustomerID = EPC_Req.CustomerID;

                // Check Serial number available on not

                usp_GetCheckSerialNoLastUsed_CandA_Result CheckSerailNo = EPC_CandADAL.Get_CandA_LastSerialNoUsed(EPC_Req.GTIN);

                if (CheckSerailNo.SerialStart != 0)
                {

                    EPC_Res.SerialStart = CheckSerailNo.SerialStart.ToString();
                    EPC_Res.SerialEnd = CheckSerailNo.SerialEnd.ToString();
                    EPC_Res.Remark = CheckSerailNo.Remark;

                }
                else
                {
                    EPC_Res = CA_SGTIN_Serial(EPC_Req, Convert.ToInt64(CheckSerailNo.Threshold), CheckSerailNo.RequestorId);
                    EPC_Res.Remark = EPC_Res.Remark;

                    usp_GetCheckSerialNoLastUsed_CandA_Result ObjSerailNo = new usp_GetCheckSerialNoLastUsed_CandA_Result();

                    if (EPC_Res.SerialStart != "")
                    {
                        ObjSerailNo = EPC_CandADAL.Get_CandA_LastSerialNoUsed(EPC_Req.GTIN);
                        EPC_Res.Remark = ObjSerailNo.Remark;
                    }

                    EPC_Res.SerialStart = ObjSerailNo.SerialStart.ToString();
                    EPC_Res.SerialEnd = ObjSerailNo.SerialEnd.ToString();

                }

            }
            catch (Exception Ex)
            {
                _ = InsertLog(Ex, "GetEPC_Decode");
                return GetError(107);
            }

            return EPC_Res;
        }
        private static EPCResponse CA_SGTIN_Serial(EPCRequest ObjRequest, long Threshold, string requestorId)
        {
            EPCResponse ObjResponse = new EPCResponse();

            string Response = CandA_http.GetSerialNumber(ObjRequest, Threshold, requestorId);
            try
            {
                dynamic obj = JsonConvert.DeserializeObject(Response);

                string gtin = (string)obj["gtin"];
                long serialStart = (long)obj["serialStart"];
                int amount = (int)obj["amount"];

                if (serialStart > 0)
                {
                    EPC_CandADAL.Update_CandA_MaxserialNo(amount, gtin, serialStart);
                }

                ObjResponse.SerialStart = Convert.ToString(serialStart);


            }
            catch (Exception)
            {
                dynamic obj = JsonConvert.DeserializeObject(Response);
                ObjResponse.Remark = (string)obj["message"];
                ObjResponse.SerialStart = "";
            }

            return ObjResponse;
        }
    }
}
