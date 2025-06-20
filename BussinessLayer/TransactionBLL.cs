using DataAccessLayer;
using DataAccessLayer.CommonDataModels;
using System;
using System.Linq;

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

                    switch (EPC_Req.CustomerID)
                    {
                        case "CandA":
                        case "CAApps":
                        case "CAInts":
                        case "CATrai":

                            if (CandA_IntergrationBLL.CandA_GSIM)
                            {
                                #region C&A GSIM 
                                                              
                                EPC_Res = CandA_IntergrationBLL.GetCA_SGTIN_Serial(EPC_Req);

                                if (EPC_Res.Remark == "Quantity is available")
                                {
                                    usp_GTIN_GetEPC_CA_Result Obj_CA = EPCDAL.GetEPC_CA(EPC_Req.GTIN, EPC_Req.Quantity, EPC_Req.TransactionType, EPC_Req.Schema, EPC_Req.CustomerID, EPC_Req.CustomerName, EPC_Req.Event, EPC_Req.UserId, Convert.ToInt64(EPC_Req.Serial), EPC_Req.EPC, EPC_Req.RPO, EPC_Req.DetailLineID, EPC_Req.CustomPara1, EPC_Req.CustomPara2, EPC_Req.GS1Prefix, EPC_Req.PartitionValue, Convert.ToDateTime(EPC_Req.RequestStartTime));

                                    EPC_Res.EPCStart = Obj_CA.EpcStart;
                                    EPC_Res.EPCEnd = Obj_CA.EpcEnd;
                                    EPC_Res.SerialStart = Convert.ToString(Obj_CA.SerialStart);
                                    EPC_Res.SerialEnd = Convert.ToString(Obj_CA.SerailEnd);
                                    EPC_Res.Remark = Convert.ToString(Obj_CA.Remark);
                                    EPC_Res.CustomerID = EPC_Req.CustomerID;
                                    EPC_Res.Quantity = EPC_Req.Quantity;
                                    EPC_Res.GTIN = EPC_Req.GTIN;
                                    EPC_Res.AccessPWD = Obj_CA.AccessPWD;
                                    EPC_Res.KillPWD = Obj_CA.KillPWD;
                                }
                                else
                                {
                                   // EPC_Res.Remark = "";
                                }

                                #endregion
                            }
                            else
                            {
                                #region C&A 
                                                                
                                usp_GTIN_GetEPC_Result Obj_CA = EPCDAL.GetEPC(EPC_Req.GTIN, EPC_Req.Quantity, EPC_Req.TransactionType, EPC_Req.Schema, EPC_Req.CustomerID, EPC_Req.CustomerName, EPC_Req.Event, EPC_Req.UserId, Convert.ToInt64(EPC_Req.Serial), EPC_Req.EPC, EPC_Req.RPO, EPC_Req.DetailLineID, EPC_Req.CustomPara1, EPC_Req.CustomPara2, EPC_Req.GS1Prefix, EPC_Req.PartitionValue, Convert.ToDateTime(EPC_Req.RequestStartTime));
                                EPC_Res.EPCStart = Obj_CA.EpcStart;
                                EPC_Res.EPCEnd = Obj_CA.EpcEnd;
                                EPC_Res.SerialStart = Convert.ToString(Obj_CA.SerialStart);
                                EPC_Res.SerialEnd = Convert.ToString(Obj_CA.SerailEnd);
                                EPC_Res.Remark = Convert.ToString(Obj_CA.Remark);
                                EPC_Res.CustomerID = EPC_Req.CustomerID;
                                EPC_Res.Quantity = EPC_Req.Quantity;
                                EPC_Res.GTIN = EPC_Req.GTIN;
                                EPC_Res.AccessPWD = Obj_CA.AccessPWD;
                                EPC_Res.KillPWD = Obj_CA.KillPWD;

                                #endregion
                            }

                            break;


                        case "Kiabi":

                            int KiabiCount = (from c in EPCBLL.GetReprintEvent()
                                              where c.ToUpper() == EPC_Req.Event.ToUpper()
                                              select c).Count();
                            if (KiabiCount == 0)
                            {
                                usp_GTIN_GetEPC_Kiabi_Result ePCKiabi = EPCDAL.GetEPC_Kiabi(EPC_Req.GTIN, EPC_Req.Quantity, EPC_Req.TransactionType, EPC_Req.Schema, EPC_Req.CustomerID, EPC_Req.CustomerName, EPC_Req.Event, EPC_Req.UserId, Convert.ToInt64(EPC_Req.Serial), EPC_Req.EPC, EPC_Req.RPO, EPC_Req.DetailLineID, EPC_Req.CustomPara1, EPC_Req.CustomPara2, EPC_Req.GS1Prefix, EPC_Req.PartitionValue, Convert.ToDateTime(EPC_Req.RequestStartTime));
                                
                                EPC_Res.EPCStart = ePCKiabi.EpcStart;
                                EPC_Res.EPCEnd = ePCKiabi.EpcEnd;
                                EPC_Res.SerialStart = Convert.ToString(ePCKiabi.SerialStart);
                                EPC_Res.SerialEnd = Convert.ToString(ePCKiabi.SerailEnd);
                                EPC_Res.Remark = Convert.ToString(ePCKiabi.Remark);
                                EPC_Res.CustomerID = EPC_Req.CustomerID;
                                EPC_Res.Quantity = EPC_Req.Quantity;
                                EPC_Res.GTIN = EPC_Req.GTIN;
                                EPC_Res.AccessPWD = ePCKiabi.AccessPWD;
                                EPC_Res.KillPWD = ePCKiabi.KillPWD;

                                if (Convert.ToString(ePCKiabi.Remark).Contains("Serial number is exceeding Maximum serial number assigned for this customer"))
                                {
                                    EPC_Res = HandleEPCGeneration_KIABI(EPC_Req, EPC_Res);
                                }
                            }
                            else
                            {

                                EPC_Res = HandleEPCGeneration_KIABI(EPC_Req, EPC_Res);
                                /*
                                usp_GTIN_GetEPC_Kiabi_Range_Result uspGTINGetEPCKiabiRangeResult = Kiabi_IntergrationBLL.Kaibi_apiResponse_Restapi(EPC_Req);
                                EPC_Res.EPCStart = uspGTINGetEPCKiabiRangeResult.EpcStart;
                                EPC_Res.EPCEnd = uspGTINGetEPCKiabiRangeResult.EpcEnd;
                                EPC_Res.SerialStart = Convert.ToString(uspGTINGetEPCKiabiRangeResult.SerialStart);
                                EPC_Res.SerialEnd = Convert.ToString(uspGTINGetEPCKiabiRangeResult.SerailEnd);
                                EPC_Res.Remark = Convert.ToString(uspGTINGetEPCKiabiRangeResult.Remark);
                                EPC_Res.CustomerID = EPC_Req.CustomerID;
                                EPC_Res.Quantity = EPC_Req.Quantity;
                                EPC_Res.GTIN = EPC_Req.GTIN;
                                EPC_Res.AccessPWD = uspGTINGetEPCKiabiRangeResult.AccessPWD;
                                EPC_Res.KillPWD = uspGTINGetEPCKiabiRangeResult.KillPWD;
                                */
                            }
                            break;
                        case "Hugoboss":
                        case "HugoBoss":

                            var Obj_HB = EPCDAL.GetEPC_HugoBoss(EPC_Req.GTIN, EPC_Req.Quantity, EPC_Req.TransactionType, EPC_Req.Schema, EPC_Req.CustomerID, EPC_Req.CustomerName, EPC_Req.Event, EPC_Req.UserId, Convert.ToInt64(EPC_Req.Serial), EPC_Req.EPC, EPC_Req.RPO, EPC_Req.DetailLineID, EPC_Req.CustomPara1, EPC_Req.CustomPara2, EPC_Req.GS1Prefix, EPC_Req.PartitionValue, Convert.ToDateTime(EPC_Req.RequestStartTime));

                            EPC_Res.EPCStart = Obj_HB.EpcStart;
                            EPC_Res.EPCEnd = Obj_HB.EpcEnd;
                            EPC_Res.SerialStart = Convert.ToString(Obj_HB.SerialStart);
                            EPC_Res.SerialEnd = Convert.ToString(Obj_HB.SerailEnd);
                            EPC_Res.Remark = Convert.ToString(Obj_HB.Remark);
                            EPC_Res.CustomerID = EPC_Req.CustomerID;
                            EPC_Res.Quantity = EPC_Req.Quantity;
                            EPC_Res.GTIN = EPC_Req.GTIN;
                            EPC_Res.AccessPWD = Obj_HB.AccessPWD;
                            EPC_Res.KillPWD = Obj_HB.KillPWD;
                            break;
                        case "Diesel":
                            usp_GTIN_GetEPC_Diesel_Result Obj_Diesel = EPCDAL.GetEPC_Diesel(EPC_Req.GTIN, EPC_Req.Quantity, EPC_Req.TransactionType, EPC_Req.Schema, EPC_Req.CustomerID, EPC_Req.CustomerName, EPC_Req.Event, EPC_Req.UserId, Convert.ToInt64(EPC_Req.Serial), EPC_Req.EPC, EPC_Req.RPO, EPC_Req.DetailLineID, EPC_Req.CustomPara1, EPC_Req.CustomPara2, EPC_Req.GS1Prefix, EPC_Req.PartitionValue, Convert.ToDateTime(EPC_Req.RequestStartTime));

                            EPC_Res.EPCStart = Obj_Diesel.EpcStart;
                            EPC_Res.EPCEnd = Obj_Diesel.EpcEnd;
                            EPC_Res.SerialStart = Convert.ToString(Obj_Diesel.SerialStart);
                            EPC_Res.SerialEnd = Convert.ToString(Obj_Diesel.SerailEnd);
                            EPC_Res.Remark = Convert.ToString(Obj_Diesel.Remark);
                            EPC_Res.CustomerID = EPC_Req.CustomerID;
                            EPC_Res.Quantity = EPC_Req.Quantity;
                            EPC_Res.GTIN = EPC_Req.GTIN;
                            EPC_Res.AccessPWD = Obj_Diesel.AccessPWD;
                            EPC_Res.KillPWD = Obj_Diesel.KillPWD;
                            break;
                        default:
                            usp_GTIN_GetEPC_Result Obj_ = EPCDAL.GetEPC(EPC_Req.GTIN, EPC_Req.Quantity, EPC_Req.TransactionType, EPC_Req.Schema, EPC_Req.CustomerID, EPC_Req.CustomerName, EPC_Req.Event, EPC_Req.UserId, Convert.ToInt64(EPC_Req.Serial), EPC_Req.EPC, EPC_Req.RPO, EPC_Req.DetailLineID, EPC_Req.CustomPara1, EPC_Req.CustomPara2, EPC_Req.GS1Prefix, EPC_Req.PartitionValue, Convert.ToDateTime(EPC_Req.RequestStartTime));

                            EPC_Res.EPCStart = Obj_.EpcStart;
                            EPC_Res.EPCEnd = Obj_.EpcEnd;
                            EPC_Res.SerialStart = Convert.ToString(Obj_.SerialStart);
                            EPC_Res.SerialEnd = Convert.ToString(Obj_.SerailEnd);
                            EPC_Res.Remark = Convert.ToString(Obj_.Remark);
                            EPC_Res.CustomerID = EPC_Req.CustomerID;
                            EPC_Res.Quantity = EPC_Req.Quantity;
                            EPC_Res.GTIN = EPC_Req.GTIN;
                            EPC_Res.AccessPWD = Obj_.AccessPWD;
                            EPC_Res.KillPWD = Obj_.KillPWD;
                            break;
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

        public static EPCResponse GetEPCDetail(EPCRequest EPC_Req)
        {
            EPCResponse EPC_Res = new EPCResponse();
            try
            {

                try
                {

                    usp_GetEPCDetails_Result Obj = EPCDAL.GetEPCDetail(EPC_Req.GTIN, (int)EPC_Req.Quantity, EPC_Req.CustomerID, EPC_Req.RPO, EPC_Req.DetailLineID);

                    if (Obj != null)
                    {
                        EPC_Res.EPCStart = Obj.EPCStart;
                        EPC_Res.EPCEnd = Obj.EPCEnd;
                        EPC_Res.SerialStart = Convert.ToString(Obj.SerialStart);
                        EPC_Res.SerialEnd = Convert.ToString(Obj.SerialEnd);
                        EPC_Res.Remark = Convert.ToString(Obj.varRemark);
                        EPC_Res.CustomerID = Obj.varCustomerId;
                        EPC_Res.Quantity = Obj.bigIntQuantity;
                        EPC_Res.GTIN = Obj.GTIN;
                        EPC_Res.AccessPWD = "";
                        EPC_Res.KillPWD = "";
                    }
                    else
                    {
                        EPC_Res = null;
                    }

                }
                catch (Exception Ex)
                {
                    EPCDAL.SaveErrorFileResponse(Ex.ToString(), "GetEPCDetail");
                    InsertLog(Ex, "GetEPCDetail");
                    EPC_Res = null;
                }

            }
            catch (Exception ex)
            {
                EPCDAL.SaveErrorFileResponse(ex.ToString(), "GetEPCDetail");
                InsertLog(ex, "GetEPCDetail");
                EPC_Res = null;
            }

            return EPC_Res;
        }

        public static EPCResponse GetEPC_Customer(EPCRequest EPC_Req)
        {
            EPCResponse EPC_Res = new EPCResponse();
            try
            {


                try
                {
                    EPC_Req.Schema = EPC_Req.Schema == null ? "" : EPC_Req.Schema;
                    EPC_Req.CustomerID = EPC_Req.CustomerID == null ? "" : EPC_Req.CustomerID;

                    usp_GTIN_GetEPC_Customer_Result Obj = EPCDAL.GetEPC_Customer(EPC_Req.GTIN, EPC_Req.Quantity, EPC_Req.TransactionType, EPC_Req.Schema, EPC_Req.CustomerID, EPC_Req.CustomerName, EPC_Req.Event, EPC_Req.UserId, Convert.ToInt64(EPC_Req.Serial), EPC_Req.EPC, EPC_Req.RPO, EPC_Req.DetailLineID, EPC_Req.CustomPara1, EPC_Req.CustomPara2, EPC_Req.GS1Prefix, EPC_Req.PartitionValue, Convert.ToDateTime(EPC_Req.RequestStartTime));

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

        #region TEMPE EPC
        public static EPCResponse GetEPC_Customer_Tempe(EPCRequest EPC_Req)
        {
            EPCResponse ePCResponse = new EPCResponse();
            try
            {
                try
                {
                    EPC_Req.Schema = (EPC_Req.Schema == null ? "" : EPC_Req.Schema);
                    EPC_Req.CustomerID = (EPC_Req.CustomerID == null ? "" : EPC_Req.CustomerID);
                    usp_GTIN_GetEPC_Customer_Tempe_Result ePCCustomerTempe = EPCDAL.GetEPC_Customer_Tempe(EPC_Req.GTIN, EPC_Req.Quantity, EPC_Req.TransactionType, EPC_Req.Schema, EPC_Req.CustomerID, EPC_Req.CustomerName, EPC_Req.Event, EPC_Req.UserId, Convert.ToInt64(EPC_Req.Serial), EPC_Req.EPC, EPC_Req.RPO, EPC_Req.DetailLineID, EPC_Req.CustomPara1, EPC_Req.CustomPara2, EPC_Req.GS1Prefix, EPC_Req.PartitionValue, Convert.ToDateTime(EPC_Req.RequestStartTime));
                    ePCResponse.EPCStart = ePCCustomerTempe.EpcStart;
                    ePCResponse.EPCEnd = ePCCustomerTempe.EpcEnd;
                    ePCResponse.SerialStart = Convert.ToString(ePCCustomerTempe.SerialStart);
                    ePCResponse.SerialEnd = Convert.ToString(ePCCustomerTempe.SerailEnd);
                    ePCResponse.Remark = Convert.ToString(ePCCustomerTempe.Remark);
                    ePCResponse.CustomerID = EPC_Req.CustomerID;
                    ePCResponse.Quantity = EPC_Req.Quantity;
                    ePCResponse.GTIN = EPC_Req.GTIN;
                    ePCResponse.AccessPWD = ePCCustomerTempe.AccessPWD;
                    ePCResponse.KillPWD = ePCCustomerTempe.KillPWD;
                }
                catch (Exception exception)
                {
                    EPCDAL.SaveErrorFileResponse(exception.ToString(), "GetEPC");
                    EPCBLL.InsertLog(exception, "GetEPC");
                    ePCResponse = EPCBLL.GetError(122);
                }
            }
            catch (Exception exception1)
            {
                EPCDAL.SaveErrorFileResponse(exception1.ToString(), "GetEPC_New");
                EPCBLL.InsertLog(exception1, "GetEPC_New");
                ePCResponse = EPCBLL.GetError(122);
            }
            return ePCResponse;
        }

        #endregion

        #region KIBAI CALL EPC
        private static EPCResponse HandleEPCGeneration_KIABI(EPCRequest EPC_Req, EPCResponse EPC_Res)
        {
            usp_GTIN_GetEPC_Kiabi_Range_Result uspGTINGetEPCKiabiRangeResult = Kiabi_IntergrationBLL.Kaibi_apiResponse_Restapi(EPC_Req);
            EPC_Res.EPCStart = uspGTINGetEPCKiabiRangeResult.EpcStart;
            EPC_Res.EPCEnd = uspGTINGetEPCKiabiRangeResult.EpcEnd;
            EPC_Res.SerialStart = Convert.ToString(uspGTINGetEPCKiabiRangeResult.SerialStart);
            EPC_Res.SerialEnd = Convert.ToString(uspGTINGetEPCKiabiRangeResult.SerailEnd);
            EPC_Res.Remark = Convert.ToString(uspGTINGetEPCKiabiRangeResult.Remark);
            EPC_Res.CustomerID = EPC_Req.CustomerID;
            EPC_Res.Quantity = EPC_Req.Quantity;
            EPC_Res.GTIN = EPC_Req.GTIN;
            EPC_Res.AccessPWD = uspGTINGetEPCKiabiRangeResult.AccessPWD;
            EPC_Res.KillPWD = uspGTINGetEPCKiabiRangeResult.KillPWD;

            return EPC_Res;
        }
        #endregion

        #region MorellatoGroup EPC
        public static EPCResponse GetEPC_Customer_MorellatoGroup(EPCRequest EPC_Req)
        {
            EPCResponse ePCResponse = new EPCResponse();
            try
            {
                try
                {
                    EPC_Req.Schema = (EPC_Req.Schema == null ? "" : EPC_Req.Schema);
                    EPC_Req.CustomerID = (EPC_Req.CustomerID == null ? "" : EPC_Req.CustomerID);
                    usp_GTIN_GetEPC_Customer_MorellatoGroup_Result ePCCustomerTempe = EPCDAL.GetEPC_Customer_MorellatoGroup(EPC_Req.GTIN, EPC_Req.Quantity, EPC_Req.TransactionType, EPC_Req.Schema, EPC_Req.CustomerID, EPC_Req.CustomerName, EPC_Req.Event, EPC_Req.UserId, Convert.ToInt64(EPC_Req.Serial), EPC_Req.EPC, EPC_Req.RPO, EPC_Req.DetailLineID, EPC_Req.CustomPara1, EPC_Req.CustomPara2, EPC_Req.GS1Prefix, EPC_Req.PartitionValue, Convert.ToDateTime(EPC_Req.RequestStartTime));
                    ePCResponse.EPCStart = ePCCustomerTempe.EpcStart;
                    ePCResponse.EPCEnd = ePCCustomerTempe.EpcEnd;
                    ePCResponse.SerialStart = Convert.ToString(ePCCustomerTempe.SerialStart);
                    ePCResponse.SerialEnd = Convert.ToString(ePCCustomerTempe.SerailEnd);
                    ePCResponse.Remark = Convert.ToString(ePCCustomerTempe.Remark);
                    ePCResponse.CustomerID = EPC_Req.CustomerID;
                    ePCResponse.Quantity = EPC_Req.Quantity;
                    ePCResponse.GTIN = EPC_Req.GTIN;
                    ePCResponse.AccessPWD = ePCCustomerTempe.AccessPWD;
                    ePCResponse.KillPWD = ePCCustomerTempe.KillPWD;
                }
                catch (Exception exception)
                {
                    EPCDAL.SaveErrorFileResponse(exception.ToString(), "GetEPC");
                    EPCBLL.InsertLog(exception, "GetEPC");
                    ePCResponse = EPCBLL.GetError(122);
                }
            }
            catch (Exception exception1)
            {
                EPCDAL.SaveErrorFileResponse(exception1.ToString(), "GetEPC_New");
                EPCBLL.InsertLog(exception1, "GetEPC_New");
                ePCResponse = EPCBLL.GetError(122);
            }
            return ePCResponse;
        }

        #endregion

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

                    usp_GTIN_GetEPC_Result Obj = EPCDAL.GetEPC(EPC_Req.GTIN, EPC_Req.Quantity, EPC_Req.TransactionType, EPC_Req.Schema, EPC_Req.CustomerID, EPC_Req.CustomerName, EPC_Req.Event, EPC_Req.UserId, Convert.ToInt64(EPC_Req.Serial), EPC_Req.EPC, EPC_Req.RPO, EPC_Req.DetailLineID, EPC_Req.CustomPara1, EPC_Req.CustomPara2, EPC_Req.GS1Prefix, EPC_Req.PartitionValue, Convert.ToDateTime(EPC_Req.RequestStartTime));

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

                usp_GTIN_GetEPC_Result Obj = EPCDAL.GetEPC(EPC_Req.GTIN, EPC_Req.Quantity, EPC_Req.TransactionType, EPC_Req.Schema, EPC_Req.CustomerID, EPC_Req.CustomerName, EPC_Req.Event, EPC_Req.UserId, Convert.ToInt64(EPC_Req.Serial), EPC_Req.EPC, EPC_Req.RPO, EPC_Req.DetailLineID, EPC_Req.CustomPara1, EPC_Req.CustomPara2, EPC_Req.GS1Prefix, EPC_Req.PartitionValue, Convert.ToDateTime(EPC_Req.RequestStartTime));

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
