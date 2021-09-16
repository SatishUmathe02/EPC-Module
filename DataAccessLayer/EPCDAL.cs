using System;
using System.Collections.Generic;
using System.IO;
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
        public static usp_GTIN_GetEPC_Result GetEPC(string gtin14, long qty, string transaction, string schema, string customerId, string customerName, string Event, long UserId, long SerialStart, string EPC, long RPO, long DetailLineNo, string CustomPara1, string CustomPara2, string GS1Prefix, int PartionVal, DateTime EPCStartDateTime)
        {
            usp_GTIN_GetEPC_Result Obj = new usp_GTIN_GetEPC_Result();
            try
            {
                using (EPC_DBEntities db = new EPC_DBEntities())
                {
                    //if (qty >= 40000)
                    if (qty >= 8000)
                    {
                        db.Database.CommandTimeout = 0;

                    }

                    Obj = (from lst1 in db.usp_GTIN_GetEPC(gtin14, qty, transaction, schema, customerId, customerName, Event, UserId, SerialStart, EPC, RPO, DetailLineNo, CustomPara1, CustomPara2, GS1Prefix, PartionVal, EPCStartDateTime) select lst1).ToList().FirstOrDefault();

                }
            }
            catch (Exception ex)
            {

                //using (EPC_DBEntities db = new EPC_DBEntities())
                //{
                //    Obj = (from lst1 in db.usp_GTIN_GetEPC(gtin14, qty, transaction, schema, customerId, customerName, Event, UserId, SerialStart, EPC, RPO, DetailLineNo, CustomPara1, CustomPara2, GS1Prefix, PartionVal) select lst1).ToList().FirstOrDefault();
                //}
            }
            return Obj;
        }


        #endregion


        #region ADD ERROR LOG
        public static int InsertLog(System.Exception objException, string FileName)
        {

            using (EPC_DBEntities db = new EPC_DBEntities())
            {
                db.usp_AddErrorLog(Convert.ToString(objException.Source), Convert.ToString(objException.Message), Convert.ToString(System.Net.Dns.GetHostName()), Convert.ToString(objException.TargetSite), ("StackTrace : " + Convert.ToString(objException.StackTrace)), Convert.ToDateTime(System.DateTime.Now.ToString("yyyy/MM/dd HH:mm")), FileName);
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

        public static List<usp_GetEPCCounterForModaPWD_Result> GetEPCCounterFor_Moda_PWD(long RPO, long DetailNo)
        {
            List<usp_GetEPCCounterForModaPWD_Result> Obj = new List<usp_GetEPCCounterForModaPWD_Result>();
            using (EPC_DBEntities db = new EPC_DBEntities())
            {
                //db.Database.CommandTimeout = 0;
                Obj = (from lst1 in db.usp_GetEPCCounterForModaPWD(RPO, DetailNo) select lst1).ToList();
            }
            return Obj;
        }
        #endregion

        #region GET ECP COUNTER DATA FRO MANGO PASSWORD

        public static List<usp_GetEPCCounterForMANGO_PWD_Result> GetEPCCounterFor_MANGO_PWD(long RPO, long DetailNo)
        {
            List<usp_GetEPCCounterForMANGO_PWD_Result> Obj = new List<usp_GetEPCCounterForMANGO_PWD_Result>();
            using (EPC_DBEntities db = new EPC_DBEntities())
            {
                //db.Database.CommandTimeout = 0;
                Obj = (from lst1 in db.usp_GetEPCCounterForMANGO_PWD(RPO, DetailNo) select lst1).ToList();
            }
            return Obj;
        }
        #endregion

        #region GET ECP COUNTER DATA FRO AlvaroMoreno PASSWORD

        public static List<usp_GetEPCCounterForAlvaroMorenoPWD_Result> GetEPCCounterFor_AlvaroMoreno_PWD(long RPO, long DetailNo)
        {
            List<usp_GetEPCCounterForAlvaroMorenoPWD_Result> Obj = new List<usp_GetEPCCounterForAlvaroMorenoPWD_Result>();
            using (EPC_DBEntities db = new EPC_DBEntities())
            {
                //db.Database.CommandTimeout = 0;
                Obj = (from lst1 in db.usp_GetEPCCounterForAlvaroMorenoPWD(RPO, DetailNo) select lst1).ToList();
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

        #region INSERT FOR EMAIL GS1
        public static void InsertEmail_GS1(string varRecipient, string varCC, string varBCC, string varReplyTo, string varSubject, string varBody, long bigIntCreatedById, DateTime dtCreatedOn)
        {
            try
            {
                using (EPC_DBEntities db = new EPC_DBEntities())
                {
                    db.usp_InsertEmailTrigger_GS1("RT", varRecipient, varCC, varBCC, varReplyTo, varSubject, varBody, "", bigIntCreatedById, dtCreatedOn);
                }

            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region INSERT EPC REQUEST AND RESPONSE
        public static void InsertReqRes(string CustomerId, long RPO, long DetailLineNo, string Request, string Response, string URL, long UserId, string GTIN)
        {
            using (EPC_DBEntities db = new EPC_DBEntities())
            {
                db.USP_GTIN_InsertEPCReqRes(CustomerId, RPO, DetailLineNo, Request, Response, URL, UserId, GTIN);
            }
        }
        #endregion

        #region EPC CUSTOMER LIST
        public static List<usp_GetEPCCustomer_TestingWeb_Result> EPC_Customer()
        {
            List<usp_GetEPCCustomer_TestingWeb_Result> lst = null;
            using (EPC_DBEntities db = new EPC_DBEntities())
            {
                lst = (from lst1 in db.usp_GetEPCCustomer_TestingWeb() select lst1).ToList();
            }

            return lst;

        }
        #endregion

        #region GET ECP COUNTER DATA BY RPO AND SERIAL NUMBER

        public static List<usp_GetEPCCounter_RPO_SerialNum_Result> GetEPCCounter_RPO_SerialNum(string GTIN, long RPO, long DetailNo, long SerialStart, long SerialEnd)
        {
            List<usp_GetEPCCounter_RPO_SerialNum_Result> Obj = new List<usp_GetEPCCounter_RPO_SerialNum_Result>();
            using (EPC_DBEntities db = new EPC_DBEntities())
            {
                Obj = (from lst1 in db.usp_GetEPCCounter_RPO_SerialNum(GTIN, RPO, DetailNo, SerialStart, SerialEnd) select lst1).ToList();
            }
            return Obj;
        }
        #endregion

        public static void SaveErrorFileResponse(string Meg, string Filename)
        {

            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ErrorFileBox");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string file = DateTime.Now.ToString("dd-MMM-yyyy") + ".txt";
            path = Path.Combine(path, file);

            if (!File.Exists(path))
            {
                File.Create(path).Close();

            }
            using (StreamWriter w = File.AppendText(path))
            {
                string date = System.DateTime.Now.ToString("dd MMM yyyy hh:mm tt");
                w.WriteLine("Date: " + date);
                w.WriteLine(Filename + ":");
                w.WriteLine(Meg);
                w.WriteLine("");
                w.Flush();
                w.Close();

            }

        }

        #region EPC TEST SERVICE
        public static List<usp_GetCustomerWithGTIN_Parameters_Result> GetCustomerWithGTINParameter()
        {
            List<usp_GetCustomerWithGTIN_Parameters_Result> lst = null;
            using (EPC_DBEntities db = new EPC_DBEntities())
            {
                lst = (from lst1 in db.usp_GetCustomerWithGTIN_Parameters() select lst1).ToList();
            }

            return lst;
        }
        #endregion
    }
}
