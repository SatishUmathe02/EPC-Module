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

        public static EPCResponse GetError(int Code)
        {
            EPCResponse EPC_Res = new EPCResponse();
            EPC_Res.EPCStart = string.Empty;
            EPC_Res.EPCEnd = string.Empty;
            EPC_Res.SerialStart = string.Empty;
            EPC_Res.SerialEnd = string.Empty;
            EPC_Res.GTIN = string.Empty;
            EPC_Res.CustomerID = string.Empty;

            switch (Code)
            {
                //case 101:
                //    EPC_Res.Remark = "Error: This schema is not found.";
                //    break;
                //case 102:
                //    EPC_Res.Remark = "Error: GTIN must be a numeric.";
                //    break;
                case 103:
                    EPC_Res.Remark = "EPC Error Occurred: Quantity must be a numeric.";
                    break;
                case 104:
                    EPC_Res.Remark = "EPC Error Occurred: Quantity must be greater than 0";
                    break;
                //case 105:
                //    EPC_Res.Remark = "Error: The quantity exceed maximum Serial.";
                //    break;
                //case 106:
                //    EPC_Res.Remark = "Error: Log data did not insert";
                //    break;
                case 107:
                    EPC_Res.Remark = "EPC Error Occurred: Invalid request.";
                    break;
                case 108:
                    EPC_Res.Remark = "EPC Error Occurred: Serial number must be numeric.";
                    break;
                case 109:
                    EPC_Res.Remark = "EPC Error Occurred: EPC field cannot be empty.";
                    break;

                //case 110:
                //    EPC_Res.Remark = "Error: Transaction not found";
                //    break;

                //case 112:
                //    EPC_Res.Remark = "Error: Cust/Encoding Schema doesn’t exist";
                //    break;
                //case 113:
                //    EPC_Res.Remark = "Error: Schema not found";
                //    break;
                //case 114:
                //    EPC_Res.Remark = "Error: Customer not found";
                //    break;
                //case 115:
                //    EPC_Res.Remark = "Error: GTIN must be a 14 digit.";
                //    break;
                //case 116:
                //    EPC_Res.Remark = "Error: GTIN must be start with 0 digit.";
                //    break;
                case 117:
                    EPC_Res.Remark = "EPC Error Occurred: CustomPara1 field cannot be empty.";
                    break;
                case 118:
                    EPC_Res.Remark = "EPC Error Occurred: CustomPara1 field values have missing.";
                    break;
                case 119:
                    EPC_Res.Remark = "EPC Error Occurred: CustomPara1 field value must be a numeric.";
                    break;
                case 120:
                    EPC_Res.Remark = "EPC Error Occurred: CustomPara1 field value cannot be empty.";
                    break;
                case 121:
                    EPC_Res.Remark = "EPC Error Occurred: RPO or DetailLineID field value cannot be empty or zero.";
                    break;
                case 122:
                    EPC_Res.Remark = "Failed";
                    break;
                case 123:
                    EPC_Res.Remark = "EPC Error Occurred: Quantity must be greater than 0";
                    break;

                default:
                    EPC_Res.Remark = "Error: Invalid request";
                    break;
            }


            return EPC_Res;
        }

        public static int InsertLog(Exception ex, string FileName)
        {
            try
            {
                return EPCDAL.InsertLog(ex, FileName);
            }
            catch
            {

            }

            return 0;
        }

        public static List<EPCCustomer> GetEPCCustomer()
        {
            var q = EPCDAL.EPC_Customer();

            List<EPCCustomer> Objlist = (from c in q
                                         select new EPCCustomer()
                                         {
                                             CustomerId = c.CustomerId,
                                             CustomerName = c.CustomerName,
                                             IsGS1 = c.IsGS1
                                         }).ToList();

            return Objlist;

        }
        public static List<string> GetReprintEvent()
        { 

            return new List<string>()
            {
                "Preview",
                "RePrint",
                "Reprint_CP",
                "Reprint_TH",
                "PrintTest_CP",
                "PrintTest_TH",
                "PrintTestTH"
            };


        }
    }

    public class EPCGTIN
    {
        public static List<EPCLog> GetEPCSerialDetails()
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

        public static List<EPCCounter> GetEPCCounter(long RPO)
        {
            List<EPCCounter> Obj = new List<EPCCounter>();

            try
            {
                var epclist = EPCDAL.GetEPCCounter(RPO);

                if (epclist != null)
                {
                    if (epclist.Count > 0)
                    {
                        Obj = (from c in epclist
                               select new EPCCounter()
                               {
                                   Id = c.bigintId,
                                   RPO = c.bigIntRPO,
                                   DetailLineID = c.bigIntDetailLineID,
                                   LineNo = c.bigIntLineNo,
                                   GTIN = c.varGTIN,
                                   EPC = c.EPC,
                                   UserMemory = c.varUserMemory,
                                   Password = c.varPassword,
                                   LockID = c.varLockID,
                                   SerialNo = Convert.ToInt64(c.bigIntSerialNo),
                                   Custom1 = c.varCustom1,
                                   Custom2 = c.varCustom2,
                                   Status = c.varStatus,
                                   CreatedOn = Convert.ToDateTime(c.dtCreatedOn),
                                   KillPassword = c.varKillPassword,


                               }).ToList();
                    }
                }
            }
            catch (Exception eX)
            {
                EPCDAL.InsertLog(eX, "GetEPCCounter");
            }
            return Obj;
        }

        public static string GetCustomerCount()
        {
            return EPCDAL.GetEPCCustomerCount();
        }

        public static List<EPCCounter> GetEPCCounter_RPO_SerialNum(string GTIN, long RPO, long DetailNo, long SerialStart, long SerialEnd)
        {
            List<EPCCounter> Obj = new List<EPCCounter>();

            try
            {
                var epclist = EPCDAL.GetEPCCounter_RPO_SerialNum(GTIN, RPO, DetailNo, SerialStart, SerialEnd);

                if (epclist != null)
                {
                    if (epclist.Count > 0)
                    {
                        Obj = (from c in epclist
                               select new EPCCounter()
                               {
                                   Id = c.bigintId,
                                   RPO = c.bigIntRPO,
                                   DetailLineID = c.bigIntDetailLineID,
                                   LineNo = c.bigIntLineNo,
                                   GTIN = c.varGTIN,
                                   EPC = c.EPC,
                                   UserMemory = c.varUserMemory,
                                   Password = c.varPassword,
                                   LockID = c.varLockID,
                                   SerialNo = Convert.ToInt64(c.bigIntSerialNo),
                                   Custom1 = c.varCustom1,
                                   Custom2 = c.varCustom2,
                                   Status = c.varStatus,
                                   CreatedOn = Convert.ToDateTime(c.dtCreatedOn),
                                   KillPassword = c.varKillPassword,


                               }).ToList();
                    }
                }
            }
            catch (Exception eX)
            {
                EPCDAL.InsertLog(eX, "GetEPCCounter");
            }
            return Obj;
        }
    }
}
