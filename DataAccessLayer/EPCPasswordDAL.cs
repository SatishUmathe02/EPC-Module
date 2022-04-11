using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class EPCPasswordDAL
    {
        public static int UpdatePassword(string xml)
        {

            using (EPC_DBEntities db = new EPC_DBEntities())
            {
                //db.Database.CommandTimeout = 0;
                db.usp_UpdateAssPassword_Moda(xml);
            }

            /*
            try
            {
                using (EPC_DBEntities db = new EPC_DBEntities())
                {
                    db.usp_UpdateAssPassword_Moda(xml);
                }
            }
            catch(Exception ex)
            {
                using (EPC_DBEntities db = new EPC_DBEntities())
                {
                    db.usp_UpdateAssPassword_Moda(xml);
                }
            }
            */

            return 0;
        }

        public static int UpdatePassword_MANGO(string xml)
        {

            using (EPC_DBEntities db = new EPC_DBEntities())
            {
                //db.Database.CommandTimeout = 0;
                db.usp_UpdateAssPassword_MANGO(xml);
            }
            

            return 0;
        }

    }
}
