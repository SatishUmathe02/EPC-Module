using DataAccessLayer;
using DataAccessLayer.CommonDataModels;
using System;

namespace BussinessLayer
{

    public class Transaction_New : EPCBLL
    {
        public static EPCResponse GetEPC_New(EPCRequest EPC_Req)
        {
            EPCResponse EPC_Res = new EPCResponse();
            try
            {
                EPC_Res.Quantity = EPC_Req.Quantity;
                //EPCLog ObjEPCLog = new EPCLog();

                // Check GTIN 
                Int64 result;

                // Check Quantity 
                if (!Int64.TryParse(EPC_Req.Quantity.ToString(), out result))
                {
                    return GetError(103);
                }
                if (EPC_Req.Quantity == 0)
                {
                    return GetError(104);

                }

                try
                {
                    EPC_Req.Schema = EPC_Req.Schema == null ? "" : EPC_Req.Schema;
                    EPC_Req.CustomerID = EPC_Req.CustomerID == null ? "" : EPC_Req.CustomerID;

                    usp_GTIN_GetEPC_Result Obj = EPCDAL.GetEPC(EPC_Req.GTIN, EPC_Req.Quantity, EPC_Req.TransactionType, EPC_Req.Schema, EPC_Req.CustomerID, EPC_Req.CustomerName, EPC_Req.Event, EPC_Req.UserId, Convert.ToInt64(EPC_Req.Serial), EPC_Req.EPC, EPC_Req.RPO, EPC_Req.DetailLineID, EPC_Req.CustomPara1, EPC_Req.CustomPara2, EPC_Req.GS1Prefix, EPC_Req.PartitionValue);

                    EPC_Res.EPCStart = Obj.EpcStart;
                    EPC_Res.EPCEnd = Obj.EpcEnd;
                    EPC_Res.SerialStart = Convert.ToString(Obj.SerialStart);
                    EPC_Res.SerialEnd = Convert.ToString(Obj.SerailEnd);
                    EPC_Res.Remark = Convert.ToString(Obj.Remark);
                    EPC_Res.CustomerID = EPC_Req.CustomerID;
                    EPC_Res.Quantity = EPC_Req.Quantity;
                    EPC_Res.GTIN = EPC_Req.GTIN;
                    EPC_Res.AccessPWD = Obj.AccessPWD;
                    EPC_Res.KillPWD = Obj.KillPWD;

                }
                catch (Exception Ex)
                {
                    EPCDAL.SaveErrorFileResponse(Ex.ToString(), "GetEPC");
                    InsertLog(Ex, "GetEPC");
                    EPC_Res = GetError(122);
                }

            }
            catch (Exception ex)
            {
                EPCDAL.SaveErrorFileResponse(ex.ToString(), "GetEPC_New");
                InsertLog(ex, "GetEPC_New");
                EPC_Res = GetError(122);
            }

            return EPC_Res;
        }

    }

    public class Transaction_Encode : EPCBLL
    {
        public static EPCResponse GetEPC_Encode(EPCRequest EPC_Req)
        {
            EPCResponse EPC_Res = new EPCResponse();
            try
            {
                EPC_Res.Quantity = EPC_Req.Quantity;
                //EPCLog ObjEPCLog = new EPCLog();

                // Check GTIN 
                Int64 result;
                //if (!Int64.TryParse(EPC_Req.GTIN, out result))
                //{
                //    return GetError(102);
                //}
                //else
                //{
                //    int GTINLen = EPC_Req.GTIN.Length;
                //    if (GTINLen < 14)
                //    {
                //        int atuallen = 14 - GTINLen;
                //        string _gtin = "";
                //        for (int i = 0; i < atuallen; i++)
                //        {
                //            _gtin = _gtin + "0";
                //        }

                //        EPC_Req.GTIN = _gtin + EPC_Req.GTIN;
                //    }
                //}
                // Check Serial Number

                //if (!Int64.TryParse(EPC_Req.Serial, out result))
                if (!(EPC_Req.Serial > 0))
                {
                    return GetError(108);
                }


                //ObjEPCLog.GTIN = EPC_Req.GTIN;


                try
                {
                    EPC_Req.Schema = EPC_Req.Schema == null ? "" : EPC_Req.Schema;
                    EPC_Req.CustomerID = EPC_Req.CustomerID == null ? "" : EPC_Req.CustomerID;

                    usp_GTIN_GetEPC_Result Obj = EPCDAL.GetEPC(EPC_Req.GTIN, EPC_Req.Quantity, EPC_Req.TransactionType, EPC_Req.Schema, EPC_Req.CustomerID, EPC_Req.CustomerName, EPC_Req.Event, EPC_Req.UserId, Convert.ToInt64(EPC_Req.Serial), EPC_Req.EPC, EPC_Req.RPO, EPC_Req.DetailLineID, EPC_Req.CustomPara1, EPC_Req.CustomPara2, EPC_Req.GS1Prefix, EPC_Req.PartitionValue);

                    EPC_Res.EPCStart = Obj.EpcStart;
                    EPC_Res.EPCEnd = Obj.EpcEnd;
                    EPC_Res.SerialStart = Convert.ToString(Obj.SerialStart);
                    EPC_Res.SerialEnd = Convert.ToString(Obj.SerailEnd);
                    EPC_Res.Remark = Convert.ToString(Obj.Remark);
                    EPC_Res.CustomerID = EPC_Req.CustomerID;
                    EPC_Res.Quantity = EPC_Req.Quantity;
                    EPC_Res.GTIN = EPC_Req.GTIN;
                }
                catch (Exception Ex)
                {
                    EPCDAL.SaveErrorFileResponse(Ex.ToString(), "usp_GTIN_GetEPC_Result");
                    //InsertLog(Ex, "usp_GTIN_GetEPC_Result");
                    EPC_Res = GetError(122);
                }
                    
            }
            catch (Exception Ex)
            {
                EPCDAL.SaveErrorFileResponse(Ex.ToString(), "GetEPC_Encode");
                //InsertLog(Ex, "GetEPC_Encode");
                EPC_Res = GetError(122);
            }

            return EPC_Res;
        }
    }

    public class Transaction_Decode : EPCBLL
    {
        public static EPCResponse GetEPC_Decode(EPCRequest EPC_Req)
        {
            EPCResponse EPC_Res = new EPCResponse();
            try
            {

                if (String.IsNullOrEmpty(EPC_Req.EPC))
                {
                    return GetError(109);
                }

                EPC_Req.Schema = EPC_Req.Schema == null ? "" : EPC_Req.Schema;
                EPC_Req.CustomerID = EPC_Req.CustomerID == null ? "" : EPC_Req.CustomerID;

                usp_GTIN_GetEPC_Result Obj = EPCDAL.GetEPC(EPC_Req.GTIN, EPC_Req.Quantity, EPC_Req.TransactionType, EPC_Req.Schema, EPC_Req.CustomerID, EPC_Req.CustomerName, EPC_Req.Event, EPC_Req.UserId, Convert.ToInt64(EPC_Req.Serial), EPC_Req.EPC, EPC_Req.RPO, EPC_Req.DetailLineID, EPC_Req.CustomPara1, EPC_Req.CustomPara2, EPC_Req.GS1Prefix, EPC_Req.PartitionValue);

                EPC_Res.EPCStart = Obj.EpcStart;
                EPC_Res.EPCEnd = Obj.EpcEnd;
                EPC_Res.SerialStart = Convert.ToString(Obj.SerialStart);
                EPC_Res.SerialEnd = Convert.ToString(Obj.SerailEnd);
                EPC_Res.Remark = Convert.ToString(Obj.Remark);
                EPC_Res.CustomerID = EPC_Req.CustomerID;
                EPC_Res.Quantity = 0;
                EPC_Res.GTIN = Obj.GTIN;

            }
            catch (Exception Ex)
            {
                InsertLog(Ex, "GetEPC_Decode");
                return GetError(107);
            }

            return EPC_Res;
        }
    }
}
