using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class EPCDAL
    {
        public static List<usp_GetEPCLog_Result> GetEPCLog()
        {
            List<usp_GetEPCLog_Result> lst = null;
            using (EPC_DBEntities db = new EPC_DBEntities())
            {
                lst = (from lst1 in db.usp_GetEPCLog() select lst1).ToList();
            }

            return lst;

        }

        public static List<usp_GetEPCSerial_Result> GetEPCSerial()
        {
            List<usp_GetEPCSerial_Result> lst = null;
            using (EPC_DBEntities db = new EPC_DBEntities())
            {
                lst = (from lst1 in db.usp_GetEPCSerial() select lst1).ToList();
            }

            return lst;

        }


        #region GET EPC VIA STORE PROCEDURE
        public static usp_GTIN_GetEPC_Result GetEPC(string gtin14, long qty, string transaction, string schema, string customerId, string customerName, string Event, long UserId, long SerialStart, string EPC, long RPO, long DetailLineNo, string CustomPara1, string CustomPara2)
        {
            usp_GTIN_GetEPC_Result Obj = new usp_GTIN_GetEPC_Result();
            using (EPC_DBEntities db = new EPC_DBEntities())
            {
                Obj = (from lst1 in db.usp_GTIN_GetEPC(gtin14, qty, transaction, schema, customerId, customerName, Event, UserId, SerialStart, EPC, RPO, DetailLineNo, CustomPara1, CustomPara2) select lst1).ToList().FirstOrDefault();
            }
            return Obj;
        }


        #endregion


        #region ADD ERROR LOG
        public static int InsertLog(System.Exception objException, string FileName)
        {
            using (EPC_DBEntities db = new EPC_DBEntities())
            {
                int i = Convert.ToInt32(db.usp_AddErrorLog(Convert.ToString(objException.Source), Convert.ToString(objException.Message), Convert.ToString(System.Net.Dns.GetHostName()), Convert.ToString(objException.TargetSite), ("StackTrace : " + Convert.ToString(objException.StackTrace)), Convert.ToDateTime(System.DateTime.Now.ToString("yyyy/MM/dd HH:mm")), FileName));
            }

            return 0;
        }
        #endregion

        #region GET ECP COUNTER DATA

        public static List<usp_GetEPCCounter_Result> GetEPCCounter(long RPO)
        {
            List<usp_GetEPCCounter_Result> Obj = new List<usp_GetEPCCounter_Result>();
            using (EPC_DBEntities db = new EPC_DBEntities())
            {
                Obj = (from lst1 in db.usp_GetEPCCounter(RPO) select lst1).ToList();
            }
            return Obj;
        }
        #endregion

        #region GET ECP CUSTOMER COUNT

        public static string GetEPCCustomerCount()
        {
            string xml = "";
            using (EPC_DBEntities db = new EPC_DBEntities())
            {
                xml = (from lst1 in db.usp_GetCustomerCount() select lst1).FirstOrDefault();
            }
            return xml;
        }
        #endregion

        #region GET ECP COUNTER DATA FRO MODA PASSWORD

        public static List<usp_GetEPCCounterForModaPWD_Result> GetEPCCounterForModaPWD(long RPO, long DetailNo)
        {
            List<usp_GetEPCCounterForModaPWD_Result> Obj = new List<usp_GetEPCCounterForModaPWD_Result>();
            using (EPC_DBEntities db = new EPC_DBEntities())
            {
                Obj = (from lst1 in db.usp_GetEPCCounterForModaPWD(RPO, DetailNo) select lst1).ToList();
            }
            return Obj;
        }
        #endregion

        #region INSERT FOR EMAIL
        public static void InsertEmail(string varRecipient, string varCC, string varBCC, string varReplyTo, string varSubject, string varBody, long bigIntCreatedById, DateTime dtCreatedOn)
        {
            try
            {
                using (EPC_DBEntities db = new EPC_DBEntities())
                {
                    db.usp_InsertEmailTrigger("RT", varRecipient, varCC, varBCC, varReplyTo, varSubject, varBody, "", bigIntCreatedById, dtCreatedOn);
                }
                
            }
            catch (Exception ex)
            {
                
            }
        }
        #endregion
    }
}
