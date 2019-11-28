using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.CommonDataModels;

namespace DataAccessLayer
{
    public class EPCISDAL
    {
        #region GET CUSTOMER LIST FOR EPCIS
        public static List<USP_EPCIS_Customer_Result> GetEPCISCustomer()
        {
            List<USP_EPCIS_Customer_Result> objlist = new List<USP_EPCIS_Customer_Result>();
            using (EPC_DBEntities db = new EPC_DBEntities())
            {
                objlist = (from lst in db.USP_EPCIS_Customer() select lst).ToList();
            }
            return objlist;
        }
        #endregion

        #region GET EPCIS 

        public static List<usp_EPCIS_GetEP_URN_Result> GetEPCIS_URN(string CustomerId)
        {
            List<usp_EPCIS_GetEP_URN_Result> Obj = new List<usp_EPCIS_GetEP_URN_Result>();
            using (EPC_DBEntities db = new EPC_DBEntities())
            {
                Obj = (from lst in db.usp_EPCIS_GetEP_URN(CustomerId) select lst).ToList();
            }
            return Obj;
        }
        #endregion

        #region INSERT EPCIS LOG

        public static string InsertEPCISLog(EPCIS ObjEPCIS, string xml)
        {
            //string _xml = "";
            using (EPC_DBEntities db = new EPC_DBEntities())
            {
              int i = db.usp_EPCIS_InsertLog(xml, ObjEPCIS.EPCIS_Request, ObjEPCIS.EPCIS_Response, ObjEPCIS.URL);
            }
            return "";
        }
        #endregion
    }
}
