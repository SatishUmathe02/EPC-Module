using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class EPCGerryWeberDAL
    {
        #region GET EPC VIA STORE PROCEDURE
        public static usp_GTIN_GetEPC_GerryWeber_Result GetEPC(string gtin14, long qty, string transaction, string schema, string customerId, string customerName, string Event, long UserId, long SerialStart, string EPC, long RPO, long DetailLineNo, string CustomPara1, string CustomPara2, string epcStart, string epcend, string Remark)
        {
            usp_GTIN_GetEPC_GerryWeber_Result Obj = new usp_GTIN_GetEPC_GerryWeber_Result();
            using (EPC_DBEntities db = new EPC_DBEntities())
            {
                Obj = (from lst1 in db.usp_GTIN_GetEPC_GerryWeber(gtin14, qty, transaction, schema, customerId, customerName, Event, UserId, SerialStart, EPC, RPO, DetailLineNo, CustomPara1, CustomPara2, epcStart, epcend, Remark) select lst1).ToList().FirstOrDefault();
            }
            return Obj;
        }


        #endregion

        

        
    }
}
