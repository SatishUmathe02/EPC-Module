using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.CommonDataModels;
using TagDataTranslation;
using DataAccessLayer;

namespace BussinessLayer
{
    public class EPCBLL
    {

        public static EPCResponse GTIN(EPCRequest EPC_Req)
        {
            EPCResponse EPC_Res = new EPCResponse();
            EPC_Res.Quantity = EPC_Req.Quantity;
            EPCLog ObjEPCLog = new EPCLog();

            // Check Schema 
            GTINDO Objgtin = GTINBLL.GetSchemaDetails(EPC_Req.Schema);
            if (Objgtin == null)
            {
                return GetError(101);
            }
            //
            // Check GTIN 
            Int64 result;
            if (!Int64.TryParse(EPC_Req.GTIN, out result))
            {
                return GetError(102);
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
                        _gtin = _gtin + "0";
                    }

                    EPC_Req.GTIN = _gtin + EPC_Req.GTIN;
                }
            }
            // Check Quantity 
            if (!Int64.TryParse(EPC_Req.Quantity.ToString(), out result))
            {

                return GetError(103);
            }
            if (EPC_Req.Quantity == 0)
            {
                return GetError(104);

            }


            ObjEPCLog.GTIN = EPC_Req.GTIN;

            //Get EPC Serial  
            EPCSerial ObjEPCSerial = GetSerialNoOfGIN(EPC_Req.GTIN, EPC_Req.UserId);
            long SerialStart = ObjEPCSerial.SerialLastNo + 1;
            long SerialEnd = (ObjEPCSerial.SerialLastNo + EPC_Req.Quantity);
            //*****
            //bool MaximumSerial = GTINBLL.GetGTIN(EPC_Req);
            // CHECK Maximum Serial
            if ((SerialEnd <= ObjEPCSerial.MaximumSerial) || (ObjEPCSerial.MaximumSerial == 0))
            {

                EPC_Res.EPCStart = GTINBLL.GetGTIN(EPC_Req.Schema, EPC_Req.GTIN, SerialStart);

                EPC_Res.SerialNumberStart = ObjEPCLog.SerialStart = SerialStart;
                ObjEPCLog.EPCStart = EPC_Res.EPCStart;

                ObjEPCSerial = GetSerialNoOfGIN(EPC_Req.GTIN, EPC_Req.Quantity, EPC_Req.UserId);


                EPC_Res.EPCEnd = GTINBLL.GetGTIN(EPC_Req.Schema, EPC_Req.GTIN, SerialEnd);

                EPC_Res.SerialNumberEnd = ObjEPCLog.SerialEnd = SerialEnd;
                ObjEPCLog.EPCEnd = EPC_Res.EPCEnd;
                ObjEPCLog.CustomerName = EPC_Req.CustomerName;
                ObjEPCLog.Schema = EPC_Req.Schema;

                if (!string.IsNullOrEmpty(ObjEPCLog.EPCStart))
                {
                    EPC_Res.Remark = "Success";
                }

                ObjEPCLog.Remark = EPC_Res.Remark;
                ObjEPCLog.UserId = EPC_Req.UserId;
                InsertEPCLog(ObjEPCLog);
            }
            else
            {
                return GetError(105);
            }

            return EPC_Res;
        }


        public static List<EPCLog> GetEPCLog()
        {
            List<EPCLog> Obj = new List<EPCLog>();
            var log = EPCDAL.GetEPCLog();

            if (log != null)
            {
                if (log.Count > 0)
                {
                    Obj = (from c in log
                           select new EPCLog()
                           {
                               CustomerName = c.varCustomerName,
                               EPCEnd = c.varEPCEnd,
                               EPCStart = c.varEPCStart,
                               GTIN = c.varGTIN,
                               Id = c.bigIntId,
                               Remark = c.varRemark,
                               Schema = c.varSchema,
                               SerialEnd = c.bigIntSerialEnd,
                               SerialStart = c.bigIntSerialStart


                           }).ToList();
                }
            }

            return Obj;
        }

        public static EPCSerial GetSerialNoOfGIN(string GTIN, long UserId)
        {
            var obj = EPCDAL.GetSerialNoOfGTIN(GTIN, UserId);

            EPCSerial Objgtin = new EPCSerial();

            Objgtin.GTIN = obj.VARGTIN;
            Objgtin.SerialLastNo = obj.bigIntSerialNoLastUsed;
            Objgtin.MaximumSerial = (long)obj.bigIntMaximumSerial;
            return Objgtin;

        }
        public static EPCSerial GetSerialNoOfGIN(string GTIN, long SerialNo, long UserId)
        {
            var obj = EPCDAL.GetSerialNoOfGTIN(GTIN, SerialNo, UserId);

            EPCSerial Objgtin = new EPCSerial();

            Objgtin.GTIN = obj.varGTIN.ToString();
            Objgtin.SerialLastNo = obj.bigIntSerialNoLastUsed;
            Objgtin.MaximumSerial = (long)obj.bigIntMaximumSerial;
            return Objgtin;

        }
        public static string InsertEPCLog(EPCLog Obj)
        {
            new EPCDAL().InsertEPCLog(Obj.GTIN, Obj.SerialStart, Obj.SerialEnd, Obj.EPCStart, Obj.EPCEnd, Obj.Schema, Obj.CustomerName, Obj.Remark, Obj.UserId);

            return "";
        }
        public static List<EPCLog> GetEPCSerial()
        {
            List<EPCLog> Obj = new List<EPCLog>();
            var log = EPCDAL.GetEPCSerial();

            if (log != null)
            {
                if (log.Count > 0)
                {
                    Obj = (from c in log
                           select new EPCLog()
                           {

                               GTIN = c.varGTIN.ToString(),
                               Id = c.bigIntId,
                               SerialStart = c.bigIntSerialNoLastUsed,
                               MaximumSerial = (long)c.bigIntMaximumSerial


                           }).ToList();
                }
            }

            return Obj;
        }
        private static EPCResponse GetError(int Code)
        {
            EPCResponse EPC_Res = new EPCResponse();
            EPC_Res.EPCStart = "";
            EPC_Res.EPCEnd = "";

            switch (Code)
            {
                case 101:
                    EPC_Res.Remark = "This schema is not found.";
                    break;
                case 102:
                    EPC_Res.Remark = "GTIN must be a numeric.";
                    break;
                case 103:
                    EPC_Res.Remark = "Quantity must be a numeric.";
                    break;
                case 104:
                    EPC_Res.Remark = "Quantity must be greater than 0.";
                    break;
                case 105:
                    EPC_Res.Remark = "The quantity exceed maximum Serial.";
                    break;
                default:
                    break;
            }

            return EPC_Res;
        }
    }
}
