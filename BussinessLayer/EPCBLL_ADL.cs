using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.CommonDataModels;
using TagDataTranslation;
using DataAccessLayer;
using System.Numerics;
using System.Globalization;

namespace BussinessLayer
{
    public class EPCBLL_ADL
    {
        public static EPCResponse GetEPC_ADL(EPCRequest ObjReq)
        {
            EPCResponse ObjRes = new EPCResponse();

            ups_GetEPC_Customer_ADL_Result ObjEPC = EPCDAL.GetEPC_Customer_ADL(ObjReq.GTIN, ObjReq.RPO, ObjReq.DetailLineID, ObjReq.Quantity, ObjReq.CustomerID, ObjReq.CustomerName, ObjReq.Event, ObjReq.UserId, ObjReq.CustomPara1, ObjReq.CustomPara2);
            if (ObjEPC != null)
            {
                string StartEPC = ObjEPC.varNextEPC;
                string EndEPC = GetEPCNext(StartEPC, Convert.ToInt32(ObjReq.Quantity - 1));
                string NextEndEPC = GetEPCNext(EndEPC, 1);
                // Check EndEPC 
                if (CheckEPC(EndEPC, ObjEPC.varEndEPC))
                {
                    ObjRes.EPCStart = StartEPC;
                    ObjRes.EPCEnd = EndEPC;
                    ObjRes.GTIN = ObjEPC.varGTIN;
                    ObjRes.SerialStart = ObjEPC.intNextEPCSerial.ToString();
                    ObjRes.SerialEnd = (ObjEPC.intNextEPCSerial + ObjReq.Quantity - 1).ToString();
                    ObjRes.Remark = "Success";

                    ObjEPC.varNextEPC = NextEndEPC;
                    ObjEPC.intNextEPCSerial = (ObjEPC.intNextEPCSerial + Convert.ToInt32(ObjReq.Quantity));

                    usp_EPC_Cust_ADL_Result ObjEPC_ADL = EPCDAL.usp_EPC_Cust_ADL(ObjRes.GTIN, Convert.ToInt64(ObjRes.SerialStart),Convert.ToInt64(ObjRes.SerialEnd), ObjReq.Schema, ObjReq.TransactionType, ObjRes.EPCStart, ObjRes.EPCEnd, ObjReq.Quantity, ObjReq.CustomerID, ObjReq.CustomerName, ObjReq.Event, ObjReq.UserId, ObjReq.EPC, ObjReq.RPO, ObjReq.DetailLineID, ObjReq.CustomPara1, ObjReq.CustomPara2,Convert.ToDateTime(ObjReq.RequestStartTime), ObjEPC.bigintPtrId, ObjEPC.varNextEPC,Convert.ToInt32(ObjEPC.intNextEPCSerial));

                    ObjRes.EPCStart = ObjEPC_ADL.EpcStart;
                    ObjRes.EPCEnd = ObjEPC_ADL.EpcEnd;
                    ObjRes.SerialStart = ObjEPC_ADL.SerialStart.ToString();
                    ObjRes.SerialEnd = ObjEPC_ADL.SerailEnd.ToString();
                    ObjRes.CustomerID = ObjEPC_ADL.CustomerId;
                    ObjRes.GTIN = ObjEPC_ADL.GTIN;
                    ObjRes.Remark = ObjEPC_ADL.Remark;

                }
                else
                {
                    ObjRes.EPCStart = "";
                    ObjRes.EPCEnd = "";
                    ObjRes.Remark = "No EPC Available for this Key";
                    EPCDAL.EPC_InsertEPCLog(ObjRes.GTIN, Convert.ToInt64(ObjRes.SerialStart), Convert.ToInt64(ObjRes.SerialEnd), ObjReq.Schema, ObjReq.TransactionType,ObjRes.EPCStart, ObjRes.EPCEnd, ObjReq.Quantity, ObjReq.CustomerID, ObjReq.CustomerName, ObjReq.Event, ObjReq.UserId, ObjReq.EPC, ObjReq.RPO, ObjReq.DetailLineID, ObjReq.CustomPara1, ObjReq.CustomPara2, Convert.ToDateTime(ObjReq.RequestStartTime), ObjRes.Remark);
                }
            }
            else
            {
                ObjRes.Remark = "No EPC Available for this Key";
                EPCDAL.EPC_InsertEPCLog(ObjRes.GTIN, Convert.ToInt64(ObjRes.SerialStart), Convert.ToInt64(ObjRes.SerialEnd), ObjReq.Schema, ObjReq.TransactionType, ObjRes.EPCStart, ObjRes.EPCEnd, ObjReq.Quantity, ObjReq.CustomerID, ObjReq.CustomerName, ObjReq.Event, ObjReq.UserId, ObjReq.EPC, ObjReq.RPO, ObjReq.DetailLineID, ObjReq.CustomPara1, ObjReq.CustomPara2, Convert.ToDateTime(ObjReq.RequestStartTime), ObjRes.Remark);
            }
        


            return ObjRes;
        }

        private static string GetEPCNext(string EPCStart, int PrintQty)
        {
            BigInteger intEPCStart = BigInteger.Parse(EPCStart, NumberStyles.HexNumber);
            BigInteger intPrintQty = BigInteger.Parse(PrintQty.ToString());
            BigInteger intEPCNext = BigInteger.Add(intEPCStart, intPrintQty);
            string EPCNext = intEPCNext.ToString("X");
            return EPCNext;
        }
        private static bool CheckEPC(string EPC, string EndEpc)
        {
            BigInteger EPCStart = BigInteger.Parse(EPC, NumberStyles.HexNumber);
            BigInteger EPCEnd = BigInteger.Parse(EndEpc, NumberStyles.HexNumber);
            if (EPCStart <= EPCEnd)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
