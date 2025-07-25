using System;
using System.Linq;

namespace DataAccessLayer
{
    public class EPC_TempeDAL
    {
        public static usp_GetTempeEPCDetails_Result Get_Tempe_EPCDetail(long RPO, long DetailNumber)
        {
            usp_GetTempeEPCDetails_Result objlist = new usp_GetTempeEPCDetails_Result();
            try
            {
                using (EPC_DBEntities db = new EPC_DBEntities())
                {
                    objlist = (from lst in db.usp_GetTempeEPCDetails(RPO, DetailNumber) select lst).FirstOrDefault();
                }
            }
            catch (Exception)
            {

            }
            return objlist;
        }
        public static void InsertJSON_EPCDeCode(string JsonRequest, string rfidRequestId, long RPO)
        {

            using (EPC_DBEntities db = new EPC_DBEntities())
            {
                _ = db.usp_Tempe_EPC_InsertEPCLog_Decode(JsonRequest, rfidRequestId, RPO);
            }

        }
        public static void InsertJSON_EPCLog(string JsonRequest, long RPO, string rfidRequestId)
        {

            using (EPC_DBEntities db = new EPC_DBEntities())
            {
                _ = db.usp_Tempe_EPC_InsertEPCLog(JsonRequest, RPO, rfidRequestId);
            }

        }
    }
}
