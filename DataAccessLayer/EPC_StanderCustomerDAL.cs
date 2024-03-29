using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class EPC_StanderCustomerDAL
    {
        public static usp_GetStanderCustomerDetails_Result GetStanderCustomerDetail(string CustomerId, string gtin)
        {
            usp_GetStanderCustomerDetails_Result objlist = new usp_GetStanderCustomerDetails_Result();
            try
            {
                using (EPC_DBEntities db = new EPC_DBEntities())
                {
                    objlist = (from lst in db.usp_GetStanderCustomerDetails(CustomerId, gtin) select lst).FirstOrDefault();
                }
            }
            catch (Exception EX)
            {

            }
            return objlist;
        }

        #region GET EPC VIA STORE PROCEDURE
        public static usp_GTIN_GetEPC_StanderCustomer_Result GetEPC(string gtin14, long qty, string transaction, string schema, string customerId, string customerName, string Event, long UserId, long SerialStart, string EPC, long RPO, long DetailLineNo, string CustomPara1, string CustomPara2, string GS1Prefix, int PartionVal, DateTime EPCStartDateTime, int FilterValue)
        {
            usp_GTIN_GetEPC_StanderCustomer_Result Obj = new usp_GTIN_GetEPC_StanderCustomer_Result();
            try
            {
                using (EPC_DBEntities db = new EPC_DBEntities())
                {
                    //if (qty >= 40000)
                    {
                        db.Database.CommandTimeout = 0;

                    }

                    Obj = (from lst1 in db.usp_GTIN_GetEPC_StanderCustomer(gtin14, qty, transaction, schema, customerId, customerName, Event, UserId, SerialStart, EPC, RPO, DetailLineNo, CustomPara1, CustomPara2, GS1Prefix, PartionVal, EPCStartDateTime, FilterValue) select lst1).ToList().FirstOrDefault();

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
