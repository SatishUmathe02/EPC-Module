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
                int i = db.usp_UpdateAssPassword_Moda(xml);
            }

            return 0;
        }
    }
}
