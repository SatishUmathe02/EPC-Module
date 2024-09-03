using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class GS1_IntergrationDAL
    {
        public static usp_GS1_Customer_Result IsCustomerGS1(string gtin14, string CustomerId)
        {
            usp_GS1_Customer_Result objlist = new usp_GS1_Customer_Result();
            try
            {
                using (EPC_DBEntities db = new EPC_DBEntities())
                {
                    objlist = db.usp_GS1_Customer(gtin14, CustomerId).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

            }

            return objlist;
        }
        public static usp_GS1_Insert_Result InsertGS1Details_GetPartitionValue(string gtin14, string CustomerId, string GS1Details)
        {
            usp_GS1_Insert_Result objlist = new usp_GS1_Insert_Result();
            try
            {
                using (EPC_DBEntities db = new EPC_DBEntities())
                {
                    objlist = db.usp_GS1_Insert(gtin14, CustomerId, GS1Details).FirstOrDefault();
                }
            }
            catch(Exception ex)
            {
               // throw new Exception(ex.Message);
            }
            return objlist;
        }
    }
}
