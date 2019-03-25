using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class EPCDAL
    {
        public static List<usp_GetEPCLog_Result> GetEPCLog()
        {
            List<usp_GetEPCLog_Result> lst = null;
            using (EPC_DBEntities db = new EPC_DBEntities())
            {
                lst = (from lst1 in db.usp_GetEPCLog() select lst1).ToList();
            }

            return lst;

        }
        public static usp_GetSerialNoGTIN_Result  GetSerialNoOfGTIN(string GTIN, long UserId)
        {
            usp_GetSerialNoGTIN_Result Obj = new usp_GetSerialNoGTIN_Result();
            using (EPC_DBEntities db = new EPC_DBEntities())
            {
                Obj = (from lst1 in db.usp_GetSerialNoGTIN(GTIN, UserId) select lst1).ToList().FirstOrDefault();
            }

            return Obj;

        }
        public static usp_GetInsertGTIN_Result GetSerialNoOfGTIN(string GTIN, long SerialNo, long UserId)
        {
            usp_GetInsertGTIN_Result Obj = new usp_GetInsertGTIN_Result();
            using (EPC_DBEntities db = new EPC_DBEntities())
            {
                Obj = (from lst1 in db.usp_GetInsertGTIN(GTIN, SerialNo, UserId) select lst1).ToList().FirstOrDefault();
            }

            return Obj;

        }

        public void InsertEPCLog(string GTIN, long SerialStart, long SerialEnd, string EPCStart, string EPCEnd,string Schema, string CustomerName,string Remark,long UserId)
        {
            using (EPC_DBEntities db = new EPC_DBEntities())
            {
                db.usp_InsertEPCLog(GTIN, SerialStart, SerialEnd, EPCStart, EPCEnd, Schema, CustomerName, Remark, UserId);
            }
        }

        public static List<usp_GetEPCSerial_Result> GetEPCSerial()
        {
            List<usp_GetEPCSerial_Result> lst = null;
            using (EPC_DBEntities db = new EPC_DBEntities())
            {
                lst = (from lst1 in db.usp_GetEPCSerial() select lst1).ToList();
            }

            return lst;

        }
    }
}
