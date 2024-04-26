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
        public static int UpdatePassword_TENDAM(string xml)
        {

            using (EPC_DBEntities db = new EPC_DBEntities())
            {
                //db.Database.CommandTimeout = 0;
                db.usp_UpdateAssPassword_TENDAM(xml);
            }


            return 0;
        }

        #region GET ECP COUNTER DATA FRO KIABI PASSWORD

        public static List<usp_GetEPCCounterForKiabi_PWD_Result> GetEPCCounterFor_Kiabi_PWD(long RPO, long DetailNo)
        {
            List<usp_GetEPCCounterForKiabi_PWD_Result> Obj = new List<usp_GetEPCCounterForKiabi_PWD_Result>();
            using (EPC_DBEntities db = new EPC_DBEntities())
            {
                //db.Database.CommandTimeout = 0;
                Obj = (from lst1 in db.usp_GetEPCCounterForKiabi_PWD(RPO, DetailNo) select lst1).ToList();
            }
            return Obj;
        }

        public static int UpdatePassword_Kiabi(string xml)
        {

            using (EPC_DBEntities db = new EPC_DBEntities())
            {
                //db.Database.CommandTimeout = 0;
                db.usp_UpdateAssPassword_Kiabi(xml);
            }


            return 0;
        }


        #endregion

    }
}
