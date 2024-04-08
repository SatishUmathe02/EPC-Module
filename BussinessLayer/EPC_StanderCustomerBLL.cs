using DataAccessLayer;
using DataAccessLayer.CommonDataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer
{
    public class EPC_StanderCustomerBLL : EPCBLL
    {
        public static EPCResponse GetEPC_SC(EPCRequest EPC_Req)
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

                    if (EPC_Req.TransactionType == "Test")
                    {
                        usp_GTIN_GetEPC_StanderCustomer_Result Obj = EPC_StanderCustomerDAL.GetEPC(EPC_Req.GTIN, EPC_Req.Quantity, EPC_Req.TransactionType, EPC_Req.Schema, EPC_Req.CustomerID, EPC_Req.CustomerName, EPC_Req.Event, EPC_Req.UserId, Convert.ToInt64(EPC_Req.Serial), EPC_Req.EPC, EPC_Req.RPO, EPC_Req.DetailLineID, EPC_Req.CustomPara1, EPC_Req.CustomPara2, EPC_Req.GS1Prefix, EPC_Req.PartitionValue, Convert.ToDateTime(EPC_Req.RequestStartTime), EPC_Req.FilterValue);
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
                    else
                    {
                        if (EPC_StanderCustomerDAL.CallEPCCustomerWise)
                        {
                            
                            usp_GTIN_GetEPC_StanderCustomerALL_Result Obj = EPC_StanderCustomerDAL.GetEPCAll(EPC_Req.GTIN, EPC_Req.Quantity, EPC_Req.TransactionType, EPC_Req.Schema, EPC_Req.CustomerID, EPC_Req.CustomerName, EPC_Req.Event, EPC_Req.UserId, Convert.ToInt64(EPC_Req.Serial), EPC_Req.EPC, EPC_Req.RPO, EPC_Req.DetailLineID, EPC_Req.CustomPara1, EPC_Req.CustomPara2, EPC_Req.GS1Prefix, EPC_Req.PartitionValue, Convert.ToDateTime(EPC_Req.RequestStartTime), EPC_Req.FilterValue);
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
                        else
                        {
                            usp_GTIN_GetEPC_StanderCustomer_Result Obj = EPC_StanderCustomerDAL.GetEPC(EPC_Req.GTIN, EPC_Req.Quantity, EPC_Req.TransactionType, EPC_Req.Schema, EPC_Req.CustomerID, EPC_Req.CustomerName, EPC_Req.Event, EPC_Req.UserId, Convert.ToInt64(EPC_Req.Serial), EPC_Req.EPC, EPC_Req.RPO, EPC_Req.DetailLineID, EPC_Req.CustomPara1, EPC_Req.CustomPara2, EPC_Req.GS1Prefix, EPC_Req.PartitionValue, Convert.ToDateTime(EPC_Req.RequestStartTime), EPC_Req.FilterValue);
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
                    }

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

        public static usp_GetStanderCustomerDetails_Result GetStanderCustomer_Details(EPCRequest EPC_Req)
        {

            usp_GetStanderCustomerDetails_Result ObjStanderCus = EPC_StanderCustomerDAL.GetStanderCustomerDetail(EPC_Req.CustomerID, EPC_Req.GTIN);


            return ObjStanderCus;

        }

        public static bool CheckCustomerSPInEPC(string CustomerId)
        {
            return EPC_StanderCustomerDAL.CheckSPForStatnderCustomer(CustomerId);
        }


    }
}
