using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
   public class EPC_CandADAL
    {
        public static usp_GetCheckSerialNoLastUsed_CandA_Result Get_CandA_LastSerialNoUsed(string GTIN)
        {
            usp_GetCheckSerialNoLastUsed_CandA_Result Objresult = new usp_GetCheckSerialNoLastUsed_CandA_Result();
            try
            {
                using (EPC_DBEntities db = new EPC_DBEntities())
                {
                    Objresult = (from lst in db.usp_GetCheckSerialNoLastUsed_CandA(GTIN) select lst).FirstOrDefault();
                }
            }
            catch (Exception EX)
            {

            }
            return Objresult;
        }

        public static void  Update_CandA_MaxserialNo(int Qty, string GTIN, long SerialStart)
        {
            
            try
            {
                using (EPC_DBEntities db = new EPC_DBEntities())
                {
                    db.usp_UpdateSerialNumber_CandA(Qty, GTIN, SerialStart);
                }
            }
            catch (Exception EX)
            {

            }
          
        }
    }
}
