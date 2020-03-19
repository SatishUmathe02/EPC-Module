using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.CommonDataModels;
using BussinessLayer;
using DataAccessLayer;

namespace EPCGerryWeber
{
    public class Transaction_New_GWEPC : EPCBLL
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
                if (string.IsNullOrEmpty(EPC_Req.CustomPara1))
                {
                    return GetError(117);
                }
                if (EPC_Req.RPO == 0 || EPC_Req.DetailLineID == 0)
                {
                    return GetError(121);
                }

                string[] GWParamList = EPC_Req.CustomPara1.Split("#".ToArray());

                if (GWParamList.Count() < 8)
                {
                    return GetError(118);
                }
                else
                {
                    for (int i = 0; i < GWParamList.Count(); i++)
                    {
                        if (i != 5 && i != 6)
                        {
                            if (i != 1 && i != 3)
                            {
                                if (!Int64.TryParse(GWParamList[i].ToString(), out result))
                                {
                                    return GetError(119);
                                }
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(GWParamList[i]))
                                {
                                    return GetError(117);
                                }
                            }
                        }
                    }
                }

                try
                {

                    RequestPrintOrderDO Obj = new RequestPrintOrderDO();

                    //Obj.datref = 2645; // false
                    //Obj.pos = 1; // false
                    //Obj.pro_location = "";// "IS1"; // false
                    //Obj.prr_id = "";// "1";// false

                    Obj.quantity = Convert.ToInt32(EPC_Req.Quantity);// true - and not accept 0


                    Obj.company = Convert.ToInt32(GWParamList[0]);// 1; // true
                    Obj.label_type = GWParamList[1];// "D2"; // true
                    Obj.po_pos = Convert.ToInt32(GWParamList[2]);// 1; // true
                    Obj.po_size = GWParamList[3];//  "34"; // true
                    Obj.po_number = Convert.ToInt32(GWParamList[4]);// 8776;// true
                    Obj.datref = GWParamList[5] == "" ? 0 : Convert.ToInt32(GWParamList[5]);// 2645; // false
                    Obj.pro_location = GWParamList[6];//  "IS1"; // false
                    Obj.season = Convert.ToInt32(GWParamList[7]);// 2181;// true


                    Obj.RPO = EPC_Req.RPO;
                    Obj.DetailNo = EPC_Req.DetailLineID;
                    Obj.CustomerId = EPC_Req.CustomerID;
                    Obj.UserId = EPC_Req.UserId;

                    ResponsePrintOrderDO Objres = GWEPC.GetEPC(Obj);


                    usp_GTIN_GetEPC_GerryWeber_Result ObjResult = EPCGerryWeberDAL.GetEPC(EPC_Req.GTIN, EPC_Req.Quantity, EPC_Req.TransactionType, EPC_Req.Schema, EPC_Req.CustomerID,
                       EPC_Req.CustomerName, EPC_Req.Event, EPC_Req.UserId, Convert.ToInt64(EPC_Req.Serial), EPC_Req.EPC, EPC_Req.RPO, EPC_Req.DetailLineID, EPC_Req.CustomPara1, EPC_Req.CustomPara2, Objres.epc_start, Objres.epc_end, Objres.Remark);

                    EPC_Res.EPCStart = ObjResult.EpcStart;
                    EPC_Res.EPCEnd = ObjResult.EpcEnd;
                    EPC_Res.SerialStart = Convert.ToString(ObjResult.SerialStart);
                    EPC_Res.SerialEnd = Convert.ToString(ObjResult.SerailEnd);
                    EPC_Res.Remark = Convert.ToString(ObjResult.Remark);
                    EPC_Res.CustomerID = EPC_Req.CustomerID;
                    EPC_Res.Quantity = EPC_Req.Quantity;
                    EPC_Res.GTIN = ObjResult.GTIN;
                }
                catch (Exception Ex)
                {
                    EPC_Res = GetError(107);
                    InsertLog(Ex, "usp_GTIN_GetEPC_Result");
                }

            }
            catch (Exception ex)
            {
                EPC_Res = GetError(107);
                InsertLog(ex, "GetEPC_New");
            }
            return EPC_Res;
        }
    }
}
