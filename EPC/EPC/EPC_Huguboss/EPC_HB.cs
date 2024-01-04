using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPC_Huguboss
{
    public class EPC_HB
    {
        #region GET EPC VIA STORE PROCEDURE HUGO BOSS
        public static usp_GTIN_GetEPC_HugoBoss_Result GetEPC_HugoBoss(string gtin14, long qty, string transaction, string schema, string customerId, string customerName, string Event, long UserId, long SerialStart, string EPC, long RPO, long DetailLineNo, string CustomPara1, string CustomPara2, string GS1Prefix, int PartionVal, DateTime EPCStartDateTime)
        {
            usp_GTIN_GetEPC_HugoBoss_Result Obj = new usp_GTIN_GetEPC_HugoBoss_Result();
            try
            {
                using (EPC_HBEntities db = new EPC_HBEntities())
                {

                    Obj = (from lst1 in db.usp_GTIN_GetEPC_HugoBoss(gtin14, qty, transaction, schema, customerId, customerName, Event, UserId, SerialStart, EPC, RPO, DetailLineNo, CustomPara1, CustomPara2, GS1Prefix, PartionVal, EPCStartDateTime) select lst1).ToList().FirstOrDefault();

                }
            }
            catch (Exception ex)
            {


            }
            return Obj;
        }


        #endregion
    }
}
