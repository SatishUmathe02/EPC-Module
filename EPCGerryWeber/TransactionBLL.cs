using BussinessLayer;
using DataAccessLayer;
using DataAccessLayer.CommonDataModels;
using System;
using System.Linq;

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

                // Check Quantity 
                if (!long.TryParse(EPC_Req.Quantity.ToString(), out long result))
                {
                    return GetError(103);
                }
                if (EPC_Req.Quantity == 0)
                {
                    return GetError(104);

                }
                else
                {
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
                        if (i != 5 && i != 6 && i != 8)
                        {
                            if (i != 1 && i != 3)
                            {
                                if (!long.TryParse(GWParamList[i].ToString(), out result))
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

                string ProdOrDev = Convert.ToString((from c in GWParamList
                                                     where c.ToString() == "Prod" || c.ToString() == "Dev"
                                                     select c).FirstOrDefault());

                if (string.IsNullOrEmpty(ProdOrDev))
                {
                    return GetError(118);
                }

                try
                {

                    RequestPrintOrderDO Obj = new RequestPrintOrderDO
                    {
                        //Obj.datref = 2645; // false
                        pos = 1, // false
                                 //Obj.pro_location = "";// "IS1"; // false
                                 //Obj.prr_id = "";// "1";// false

                        quantity = Convert.ToInt32(EPC_Req.Quantity),// true - and not accept 0


                        company = Convert.ToInt32(GWParamList[0]),// 1; // true
                        label_type = GWParamList[1],// "D2"; // true
                        po_pos = Convert.ToInt32(GWParamList[2]),// 1; // true
                        po_size = GWParamList[3],//  "34"; // true
                        po_number = Convert.ToInt32(GWParamList[4]),// 8776;// true
                        datref = GWParamList[5] == "" ? 0 : Convert.ToInt32(GWParamList[5]),// 2645; // false
                        pro_location = GWParamList[6],//  "IS1"; // false
                        season = Convert.ToInt32(GWParamList[7]),// 2181;// true

                        ProdOrDevEndPoint = ProdOrDev, // for endpoint & logging criteria

                        RPO = EPC_Req.RPO,
                        DetailNo = EPC_Req.DetailLineID,
                        CustomerId = EPC_Req.CustomerID,
                        UserId = EPC_Req.UserId,
                        GTIN = EPC_Req.GTIN
                    };

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
                    EPCDAL.SaveErrorFileResponse(Ex.ToString(), "usp_GTIN_GetEPC_GerryWeber_Result");
                    EPC_Res = GetError(122);
                    _ = InsertLog(Ex, "usp_GTIN_GetEPC_GerryWeber_Result");
                }

            }
            catch (Exception ex)
            {
                EPCDAL.SaveErrorFileResponse(ex.ToString(), "GerryWeber_GetEPC_New");
                EPC_Res = GetError(122);
                _ = InsertLog(ex, "GerryWeber_GetEPC_New");
            }
            return EPC_Res;
        }
    }
}
