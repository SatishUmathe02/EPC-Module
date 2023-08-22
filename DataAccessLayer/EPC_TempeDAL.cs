using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class EPC_TempeDAL
    {
        public static usp_GetTempeEPCDetails_Result Get_Tempe_EPCDetail(long RPO, long DetailNumber)
        {
            usp_GetTempeEPCDetails_Result objlist = new usp_GetTempeEPCDetails_Result();
            using (EPC_DBEntities db = new EPC_DBEntities())
            {
                objlist = (from lst in db.usp_GetTempeEPCDetails(RPO, DetailNumber) select lst).FirstOrDefault();
            }
            return objlist;
        }
        public static void InsertJSON_EPCDeCode(string JsonRequest, string rfidRequestId, long RPO)
        {
            
            using (EPC_DBEntities db = new EPC_DBEntities())
            {
                db.usp_Tempe_EPC_InsertEPCLog_Decode(JsonRequest, rfidRequestId, RPO);
            }
            
        }
        public static void InsertJSON_EPCLog(string JsonRequest, long RPO, string rfidRequestId)
        {

            using (EPC_DBEntities db = new EPC_DBEntities())
            {
                db.usp_Tempe_EPC_InsertEPCLog(JsonRequest,RPO, rfidRequestId);
            }

        }
    }
}
