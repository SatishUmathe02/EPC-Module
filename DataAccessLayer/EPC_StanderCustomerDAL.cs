using System;
using System.Configuration;
using System.Linq;

namespace DataAccessLayer
{
    public class EPC_StanderCustomerDAL
    {
        public static bool CallEPCCustomerWise;



        static EPC_StanderCustomerDAL()
        {
            EPC_StanderCustomerDAL.CallEPCCustomerWise = Convert.ToBoolean(ConfigurationManager.AppSettings["CallEPCCustomerWise"].ToString());

        }

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
            catch (Exception)
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
            catch (Exception)
            {

            }
            return Obj;
        }


        #endregion

        #region GET EPC VIA STORE PROCEDURE FOR ALL CUSTOMER
        public static usp_GTIN_GetEPC_StanderCustomerALL_Result GetEPCAll(string gtin14, long qty, string transaction, string schema, string customerId, string customerName, string Event, long UserId, long SerialStart, string EPC, long RPO, long DetailLineNo, string CustomPara1, string CustomPara2, string GS1Prefix, int PartionVal, DateTime EPCStartDateTime, int FilterValue)
        {
            usp_GTIN_GetEPC_StanderCustomerALL_Result Obj = new usp_GTIN_GetEPC_StanderCustomerALL_Result();
            try
            {
                using (EPC_DBEntities db = new EPC_DBEntities())
                {
                    //if (qty >= 40000)
                    {
                        db.Database.CommandTimeout = 0;

                    }

                    Obj = (from lst1 in db.usp_GTIN_GetEPC_StanderCustomerALL(gtin14, qty, transaction, schema, customerId, customerName, Event, UserId, SerialStart, EPC, RPO, DetailLineNo, CustomPara1, CustomPara2, GS1Prefix, PartionVal, EPCStartDateTime, FilterValue) select lst1).ToList().FirstOrDefault();

                }
            }
            catch (Exception)
            {

            }
            return Obj;
        }


        #endregion

        public static bool CheckSPForStatnderCustomer(string CustomerId)
        {
            bool SPExit = false;
            try
            {
                using (EPC_DBEntities db = new EPC_DBEntities())
                {

                    int k = (int)(from lst1 in db.usp_CheckSPForStatnderCustomer(CustomerId) select lst1).ToList().FirstOrDefault();

                    if (k > 0)
                    {
                        SPExit = true;
                    }
                }
            }
            catch (Exception)
            {
                SPExit = false;
            }
            return SPExit;
        }
    }
}
