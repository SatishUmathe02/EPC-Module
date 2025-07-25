using DataAccessLayer;
using DataAccessLayer.CommonDataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace BussinessLayer
{
    public class EPCPasswordBLL
    {

        public static async Task<bool> UpdatePassword_MODA(long RPO, long DetailNo)
        {
            bool flag = false;
            try
            {
                List<usp_GetEPCCounterForModaPWD_Result> epclist = EPCDAL.GetEPCCounterFor_Moda_PWD(RPO, DetailNo);

                StringBuilder xml = new StringBuilder();
                if (epclist.Count == 0)
                {
                    flag = true;
                }

                if (epclist.Count > 0)
                {
                    _ = xml.Append("<EPC>");
                    /*
                    for (int i = 0; i < epclist.Count(); i++)
                    {
                        xml.Append("<Password>");
                        xml.Append("<RPO>" + Convert.ToString(epclist[i].bigIntRPO) + "</RPO>");
                        xml.Append("<EPC>" + epclist[i].EPC + "</EPC>");
                        xml.Append("<AccesPwd>" + HMACSHA256ToHexStringL8(epclist[i].EPC, epclist[i].AccHexKey) + "</AccesPwd>");
                        xml.Append("<KillPwd>" + HMACSHA256ToHexStringL8(epclist[i].EPC, epclist[i].KillHexKey) + "</KillPwd>");
                        xml.Append("</Password>");
                    }
                    */
                    foreach (usp_GetEPCCounterForModaPWD_Result item in epclist)
                    {
                        _ = xml.Append("<Password>");
                        _ = xml.Append("<Id>" + Convert.ToString(item.bigintId) + "</Id>");
                        _ = xml.Append("<RPO>" + Convert.ToString(item.bigIntRPO) + "</RPO>");
                        _ = xml.Append("<EPC>" + item.EPC + "</EPC>");
                        _ = xml.Append("<AccesPwd>" + HMACSHA256ToHexStringL8(item.EPC, item.AccHexKey) + "</AccesPwd>");
                        _ = xml.Append("<KillPwd>" + HMACSHA256ToHexStringL8(item.EPC, item.KillHexKey) + "</KillPwd>");
                        _ = xml.Append("</Password>");
                    }
                    _ = xml.Append("</EPC>");
                    _ = EPCPasswordDAL.UpdatePassword(xml.ToString());
                    flag = true;

                }
            }
            catch (Exception Ex)
            {
                EPCDAL.SaveErrorFileResponse(Ex.ToString(), "UpdatePassword_Moda");
                flag = false;
                _ = EPCBLL.InsertLog(Ex, "UpdatePassword_Moda");
            }

            return flag;

        }
        public static async Task<bool> UpdatePassword_MANGO(long RPO, long DetailNo)
        {
            bool flag = false;

            try
            {
                List<usp_GetEPCCounterForMANGO_PWD_Result> epclist = EPCDAL.GetEPCCounterFor_MANGO_PWD(RPO, DetailNo);

                StringBuilder xml = new StringBuilder();

                if (epclist.Count == 0)
                {
                    flag = true;
                }
                if (epclist.Count > 0)
                {
                    _ = xml.Append("<EPC>");
                    /*
                    for (int i = 0; i < epclist.Count(); i++)
                    {
                        xml.Append("<Password>");
                        xml.Append("<RPO>" + epclist[i].bigIntRPO + "</RPO>");
                        xml.Append("<EPC>" + epclist[i].EPC + "</EPC>");
                        xml.Append("<AccesPwd>" + HMACSHA256ToHexStringL8(epclist[i].EPC, epclist[i].AccHexKey) + "</AccesPwd>");
                        xml.Append("<KillPwd>" + HMACSHA256ToHexStringL8(epclist[i].EPC, epclist[i].KillHexKey) + "</KillPwd>");
                        xml.Append("</Password>");
                    }*/
                    foreach (usp_GetEPCCounterForMANGO_PWD_Result item in epclist)
                    {
                        _ = xml.Append("<Password>");
                        _ = xml.Append("<Id>" + Convert.ToString(item.bigintId) + "</Id>");
                        _ = xml.Append("<RPO>" + Convert.ToString(item.bigIntRPO) + "</RPO>");
                        _ = xml.Append("<EPC>" + item.EPC + "</EPC>");
                        _ = xml.Append("<AccesPwd>" + HMACSHA256ToHexStringL8(item.EPC, item.AccHexKey) + "</AccesPwd>");
                        _ = xml.Append("<KillPwd>" + HMACSHA256ToHexStringL8(item.EPC, item.KillHexKey) + "</KillPwd>");
                        _ = xml.Append("</Password>");
                    }
                    _ = xml.Append("</EPC>");
                    _ = EPCPasswordDAL.UpdatePassword_MANGO(xml.ToString());
                    flag = true;

                }
            }
            catch (Exception Ex)
            {
                flag = false;
                _ = EPCBLL.InsertLog(Ex, "UpdatePassword_MANGO");
            }

            return flag;

        }
        public static async Task<bool> UpdatePassword_AlvaroMoreno(long RPO, long DetailNo)
        {
            bool flag = false;
            try
            {
                List<usp_GetEPCCounterForAlvaroMorenoPWD_Result> epclist = EPCDAL.GetEPCCounterFor_AlvaroMoreno_PWD(RPO, DetailNo);

                StringBuilder xml = new StringBuilder();
                if (epclist.Count == 0)
                {
                    flag = true;
                }

                if (epclist.Count > 0)
                {
                    _ = xml.Append("<EPC>");
                    foreach (usp_GetEPCCounterForAlvaroMorenoPWD_Result item in epclist)
                    {
                        _ = xml.Append("<Password>");
                        _ = xml.Append("<Id>" + Convert.ToString(item.bigintId) + "</Id>");
                        _ = xml.Append("<RPO>" + Convert.ToString(item.bigIntRPO) + "</RPO>");
                        _ = xml.Append("<EPC>" + item.EPC + "</EPC>");
                        _ = xml.Append("<AccesPwd>" + HMACSHA256ToHexStringL8(item.EPC, item.AccHexKey) + "</AccesPwd>");
                        _ = xml.Append("<KillPwd>" + HMACSHA256ToHexStringL8(item.EPC, item.KillHexKey) + "</KillPwd>");
                        _ = xml.Append("</Password>");
                    }
                    _ = xml.Append("</EPC>");
                    _ = EPCPasswordDAL.UpdatePassword(xml.ToString());
                    flag = true;

                }
            }
            catch (Exception Ex)
            {
                EPCDAL.SaveErrorFileResponse(Ex.ToString(), "UpdatePassword_Moda");
                flag = false;
                _ = EPCBLL.InsertLog(Ex, "UpdatePassword_Moda");
            }

            return flag;

        }
        public static async Task<bool> UpdatePassword_Charanga(long RPO, long DetailNo)
        {
            bool flag = false;
            try
            {
                List<usp_GetEPCCounterForCharangaPWD_Result> epclist = EPCDAL.GetEPCCounterFor_Charanga_PWD(RPO, DetailNo);

                StringBuilder xml = new StringBuilder();
                if (epclist.Count == 0)
                {
                    flag = true;
                }

                if (epclist.Count > 0)
                {
                    _ = xml.Append("<EPC>");
                    foreach (usp_GetEPCCounterForCharangaPWD_Result item in epclist)
                    {
                        _ = xml.Append("<Password>");
                        _ = xml.Append("<Id>" + Convert.ToString(item.bigintId) + "</Id>");
                        _ = xml.Append("<RPO>" + Convert.ToString(item.bigIntRPO) + "</RPO>");
                        _ = xml.Append("<EPC>" + item.EPC + "</EPC>");
                        _ = xml.Append("<AccesPwd>" + HMACSHA256ToHexStringL8(item.EPC, item.AccHexKey) + "</AccesPwd>");
                        _ = xml.Append("<KillPwd>" + HMACSHA256ToHexStringL8(item.EPC, item.KillHexKey) + "</KillPwd>");
                        _ = xml.Append("</Password>");
                    }
                    _ = xml.Append("</EPC>");
                    _ = EPCPasswordDAL.UpdatePassword(xml.ToString());
                    flag = true;

                }
            }
            catch (Exception Ex)
            {
                EPCDAL.SaveErrorFileResponse(Ex.ToString(), "UpdatePassword_Charanga");
                flag = false;
                _ = EPCBLL.InsertLog(Ex, "UpdatePassword_Charanga");
            }

            return flag;

        }
        public static async Task<bool> UpdatePassword_TENDAM(long RPO, long DetailNo)
        {
            bool flag = false;

            try
            {
                List<usp_GetEPCCounterForTENDAM_PWD_Result> epclist = EPCDAL.GetEPCCounterFor_TENDAM_PWD(RPO, DetailNo);

                StringBuilder xml = new StringBuilder();

                if (epclist.Count == 0)
                {
                    flag = true;
                }
                if (epclist.Count > 0)
                {
                    _ = xml.Append("<EPC>");
                    foreach (usp_GetEPCCounterForTENDAM_PWD_Result item in epclist)
                    {
                        _ = xml.Append("<Password>");
                        _ = xml.Append("<Id>" + Convert.ToString(item.bigintId) + "</Id>");
                        _ = xml.Append("<RPO>" + Convert.ToString(item.bigIntRPO) + "</RPO>");
                        _ = xml.Append("<EPC>" + item.EPC + "</EPC>");
                        _ = xml.Append("<AccesPwd>" + HMACSHA256ToHexStringL8(item.EPC, item.AccHexKey) + "</AccesPwd>");
                        _ = xml.Append("<KillPwd>" + HMACSHA256ToHexStringL8(item.EPC, item.KillHexKey) + "</KillPwd>");
                        _ = xml.Append("</Password>");
                    }
                    _ = xml.Append("</EPC>");
                    _ = EPCPasswordDAL.UpdatePassword_TENDAM(xml.ToString());
                    flag = true;

                }
            }
            catch (Exception Ex)
            {
                flag = false;
                _ = EPCBLL.InsertLog(Ex, "UpdatePassword_TENDAM");
            }

            return flag;

        }

        private static string HMACSHA256ToHexStringL8(string stEpc, string stHexKey)
        {
            // password is (hex) F6B5013C f6b5013c for stEpc "303400000400004000000001" and stHexKey "000102030405060708090a0b0c0d0e0f"
            try
            {
                byte[] epc = HexStringToByteArray(stEpc);
                byte[] key = HexStringToByteArray(stHexKey);

                using (HMACSHA256 hmacsha256 = new HMACSHA256(key))
                {
                    byte[] hash = hmacsha256.ComputeHash(epc);
                    byte[] passwordBytes = new byte[4];

                    // tag password is the last 32 bits of the digest
                    Array.Copy(hash, hash.Length - 4, passwordBytes, 0, 4);
                    return ByteArrayToString(passwordBytes).ToUpper();
                }
            }
            //catch (ControledException)
            //{
            //    throw;
            //}
            catch (Exception ex)
            {
                //throw new ControledException(string.Format(Resources.StFormatException3Param, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex.Message));
                throw ex;
            }
        }
        private static byte[] HexStringToByteArray(string stHex)
        {
            try
            {
                return stHex.Length % 2 != 0
                    ? throw new Exception("The length of the hex needs to be pair.")
                    : Enumerable.Range(0, stHex.Length).Where(x => x % 2 == 0).Select(x => Convert.ToByte(stHex.Substring(x, 2), 16)).ToArray();
            }
            catch (Exception ex)
            {
                //throw new ControledException(string.Format(Resources.StFormatException3Param, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex.Message));
                throw ex;
            }
        }
        private static string ByteArrayToString(byte[] ba, bool bUpper = false)
        {
            try
            {
                string fmt = bUpper ? "{0:X2}" : "{0:x2}";

                //formato en mayúsculas o minúsculas

                StringBuilder hex = new StringBuilder(ba.Length * 2);
                foreach (byte b in ba)
                {
                    _ = hex.AppendFormat(fmt, b);
                }

                return hex.ToString();
            }
            //catch (ControledException)
            //{
            //    throw;
            //}
            catch (Exception ex)
            {
                //throw new ControledException(string.Format(Resources.StFormatException3Param, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex.Message));
                throw ex;
            }
        }



        #region Kiabi Passwords

        public static async Task<bool> UpdatePassword_KIABI(long RPO, long DetailNo)
        {
            bool flag = false;
            try
            {
                List<usp_GetEPCCounterForKiabi_PWD_Result> epclist = EPCPasswordDAL.GetEPCCounterFor_Kiabi_PWD(RPO, DetailNo);

                StringBuilder xml = new StringBuilder();

                if (epclist.Count == 0)
                {
                    flag = true;
                }
                if (epclist.Count > 0)
                {
                    _ = xml.Append("<EPC>");
                    foreach (usp_GetEPCCounterForKiabi_PWD_Result item in epclist)
                    {
                        _ = xml.Append("<Password>");
                        _ = xml.Append("<Id>" + Convert.ToString(item.bigintId) + "</Id>");
                        _ = xml.Append("<RPO>" + Convert.ToString(item.bigIntRPO) + "</RPO>");
                        _ = xml.Append("<EPC>" + item.EPC + "</EPC>");
                        string Pwd = PasswordGenerator_Kiabi(item.EPC);
                        _ = xml.Append("<AccesPwd>" + Pwd + "</AccesPwd>");
                        _ = xml.Append("<KillPwd>" + Pwd + "</KillPwd>");
                        _ = xml.Append("</Password>");
                    }
                    _ = xml.Append("</EPC>");
                    _ = EPCPasswordDAL.UpdatePassword_Kiabi(xml.ToString());
                    flag = true;

                }
            }
            catch (Exception Ex)
            {
                flag = false;
                _ = EPCBLL.InsertLog(Ex, "UpdatePassword_KIBAI");
            }

            return flag;

        }

        internal static string PasswordGenerator_Kiabi(string EPCHexaValue)
        {

            char[] charArray = EPCHexaValue.ToCharArray();
            List<int> EPCDEcValue = new List<int>();

            foreach (char a in charArray)
            {
                EPCDEcValue.Add(int.Parse(a.ToString(), System.Globalization.NumberStyles.HexNumber));
            }

            //Calculation Stage 1
            int[] PasswordStageOne = new int[8];
            PasswordStageOne[0] = EPCDEcValue[0] + EPCDEcValue[23];
            PasswordStageOne[1] = EPCDEcValue[5] + EPCDEcValue[10] + EPCDEcValue[15] + EPCDEcValue[20];
            PasswordStageOne[2] = EPCDEcValue[2] + EPCDEcValue[6] + EPCDEcValue[11] + EPCDEcValue[16];
            PasswordStageOne[3] = EPCDEcValue[7] + EPCDEcValue[13] + EPCDEcValue[18] + EPCDEcValue[21];
            PasswordStageOne[4] = EPCDEcValue[3] + EPCDEcValue[17] + EPCDEcValue[22] + EPCDEcValue[23];
            PasswordStageOne[5] = EPCDEcValue[8] + EPCDEcValue[9] + EPCDEcValue[19];
            PasswordStageOne[6] = EPCDEcValue[1] + EPCDEcValue[4] + EPCDEcValue[12] + EPCDEcValue[14] + EPCDEcValue[22];
            PasswordStageOne[7] = EPCDEcValue[1] + EPCDEcValue[22] + EPCDEcValue[23];

            //Calculation Coefficient Stage 2
            int[] PasswordStageTwo = new int[8];
            PasswordStageTwo[0] = PasswordStageOne[5];
            PasswordStageTwo[1] = 2;
            PasswordStageTwo[2] = PasswordStageTwo[0] + PasswordStageTwo[1];
            PasswordStageTwo[3] = PasswordStageTwo[1] + PasswordStageTwo[2];
            PasswordStageTwo[4] = PasswordStageTwo[2] + PasswordStageTwo[3];
            PasswordStageTwo[5] = PasswordStageTwo[3] + PasswordStageTwo[4];
            PasswordStageTwo[6] = PasswordStageTwo[4] + PasswordStageTwo[5];
            PasswordStageTwo[7] = PasswordStageTwo[5] + PasswordStageTwo[6];

            //Calculation Stage 3
            int[] PasswordStageThree = new int[8];
            PasswordStageThree[0] = PasswordStageOne[0] * PasswordStageTwo[0];
            PasswordStageThree[1] = PasswordStageOne[1] * PasswordStageTwo[1];
            PasswordStageThree[2] = PasswordStageOne[2] * PasswordStageTwo[2];
            PasswordStageThree[3] = PasswordStageOne[3] * PasswordStageTwo[3];
            PasswordStageThree[4] = PasswordStageOne[4] * PasswordStageTwo[4];
            PasswordStageThree[5] = PasswordStageOne[5] * PasswordStageTwo[5];
            PasswordStageThree[6] = PasswordStageOne[6] * PasswordStageTwo[6];
            PasswordStageThree[7] = PasswordStageOne[7] * PasswordStageTwo[7];

            //Calculation Final Password Stage 4 (dec)
            int[] PasswordStageFour = new int[8];
            PasswordStageFour[0] = PasswordStageThree[0] % 15;
            PasswordStageFour[1] = PasswordStageThree[1] % 15;
            PasswordStageFour[2] = PasswordStageThree[2] % 15;
            PasswordStageFour[3] = PasswordStageThree[3] % 15;
            PasswordStageFour[4] = PasswordStageThree[4] % 15;
            PasswordStageFour[5] = PasswordStageThree[5] % 15;
            PasswordStageFour[6] = PasswordStageThree[6] % 15;
            PasswordStageFour[7] = PasswordStageThree[7] % 15;

            //Conversion of Final Password from dec to hex
            char[] FinalPassword = new char[8];
            for (int i = 0; i < PasswordStageFour.Length; i++)
            {
                FinalPassword[i] = Convert.ToChar(PasswordStageFour[i].ToString("X"));
            }

            return new string(FinalPassword);
        }

        #endregion


        #region ACCESS PASSWORD

        public static async Task<EPCResponse> UpdateAccessPassword(EPCRequest Request, EPCResponse Response)
        {
            if (EPCPasswordBLL.CheckCustomerForAccessPassword(Request))
            {
                bool flag = await EPCPasswordBLL.UpdatePassword(Request.RPO, Request.DetailLineID, Request.CustomerID);
                if (!flag)
                {
                    Response = EPCBLL.GetError(122);
                }
            }

            return Response;
        }

        private static bool CheckCustomerForAccessPassword(EPCRequest request)
        {
            List<Customers> ObjList = (from c in GetCustomerAccessPassord()
                                       where c.CustomerId == request.CustomerID
                                       select c).ToList();


            return ObjList.Count() != 0;
        }

        private static List<Customers> GetCustomerAccessPassord()
        {

            string xmlContent = HttpContext.Current.Server.MapPath("~/App_Data/CustomerSetting.xml");// ConfigurationManager.AppSettings["CustomerSetting"].ToString();

            List<Customers> ObjList = new List<Customers>();
            try
            {

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlContent);

                XmlNodeList customerNodes = xmlDoc.SelectNodes("Customers/AccessPassword/Customer");
                foreach (XmlNode customer in customerNodes)
                {

                    Customers Obj = new Customers
                    {
                        CustomerId = customer.Attributes["Id"].Value
                    };

                    ObjList.Add(Obj);
                }
            }
            catch (Exception)
            {

            }


            return ObjList;
        }

        private static async Task<bool> UpdatePassword(long RPO, long DetailNo, string CustomerId)
        {
            bool flag = false;
            try
            {
                List<usp_GetEPCCounterForAccessKillPassword_Result> epclist = EPCDAL.GetEPCCounterForAccessKillPassword(RPO, DetailNo, CustomerId);

                StringBuilder xml = new StringBuilder();
                if (epclist.Count == 0)
                {
                    flag = true;
                }

                if (epclist.Count > 0)
                {
                    _ = xml.Append("<EPC>");
                    foreach (usp_GetEPCCounterForAccessKillPassword_Result item in epclist)
                    {
                        _ = xml.Append("<Password>");
                        _ = xml.Append("<Id>" + Convert.ToString(item.bigintId) + "</Id>");
                        _ = xml.Append("<RPO>" + Convert.ToString(item.bigIntRPO) + "</RPO>");
                        _ = xml.Append("<EPC>" + item.EPC + "</EPC>");
                        _ = xml.Append("<AccesPwd>" + HMACSHA256ToHexStringL8(item.EPC, item.AccHexKey) + "</AccesPwd>");
                        _ = xml.Append("<KillPwd>" + HMACSHA256ToHexStringL8(item.EPC, item.KillHexKey) + "</KillPwd>");
                        _ = xml.Append("</Password>");
                    }
                    _ = xml.Append("</EPC>");
                    _ = EPCDAL.UpdatePassword(xml.ToString(), CustomerId);
                    flag = true;

                }
            }
            catch (Exception Ex)
            {
                EPCDAL.SaveErrorFileResponse(Ex.ToString(), "UpdatePassword");
                flag = false;
                _ = EPCBLL.InsertLog(Ex, "UpdatePassword");
            }

            return flag;

        }
        #endregion


        #region HEX TO BASE65

        public static async Task<EPCResponse> UpdateHexToBase64(EPCRequest Request, EPCResponse Response)
        {
            if (EPCPasswordBLL.CheckCustomerForHexToBase64(Request))
            {
                bool flag = await EPCPasswordBLL.UpdateHexToBase64(Request.RPO, Request.DetailLineID, Request.CustomerID);
                if (!flag)
                {
                    Response = EPCBLL.GetError(122);
                }
            }

            return Response;
        }

        private static bool CheckCustomerForHexToBase64(EPCRequest request)
        {
            List<Customers> ObjList = (from c in GetCustomerHexToBase64()
                                       where c.CustomerId == request.CustomerID
                                       select c).ToList();


            return ObjList.Count() != 0;
        }

        private static List<Customers> GetCustomerHexToBase64()
        {

            string xmlContent = HttpContext.Current.Server.MapPath("~/App_Data/CustomerSetting.xml");// ConfigurationManager.AppSettings["CustomerSetting"].ToString();

            List<Customers> ObjList = new List<Customers>();
            try
            {

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlContent);

                XmlNodeList customerNodes = xmlDoc.SelectNodes("Customers/HexToBase64/Customer");
                foreach (XmlNode customer in customerNodes)
                {

                    Customers Obj = new Customers
                    {
                        CustomerId = customer.Attributes["Id"].Value
                    };

                    ObjList.Add(Obj);
                }
            }
            catch (Exception)
            {

            }


            return ObjList;
        }

        private static async Task<bool> UpdateHexToBase64(long RPO, long DetailNo, string CustomerId)
        {
            bool flag = false;
            try
            {
                List<usp_GetEPCCounterForHexToBase64_Result> epclist = EPCDAL.GetEPCCounterForHexToBase64(RPO, DetailNo, CustomerId);

                StringBuilder xml = new StringBuilder();
                if (epclist.Count == 0)
                {
                    flag = true;
                }

                if (epclist.Count > 0)
                {
                    _ = xml.Append("<EPC>");
                    foreach (usp_GetEPCCounterForHexToBase64_Result item in epclist)
                    {
                        _ = xml.Append("<Password>");
                        _ = xml.Append("<Id>" + Convert.ToString(item.bigintId) + "</Id>");
                        _ = xml.Append("<RPO>" + Convert.ToString(item.bigIntRPO) + "</RPO>");
                        _ = xml.Append("<EPC>" + item.EPC + "</EPC>");
                        _ = xml.Append("<AccesPwd>" + HexStringToBase64(item.EPC) + "</AccesPwd>");
                        _ = xml.Append("<KillPwd></KillPwd>");
                        _ = xml.Append("</Password>");
                    }
                    _ = xml.Append("</EPC>");
                    _ = EPCDAL.UpdateHexToBase64(xml.ToString(), CustomerId);
                    flag = true;

                }
            }
            catch (Exception Ex)
            {
                EPCDAL.SaveErrorFileResponse(Ex.ToString(), "UpdatePassword");
                flag = false;
                _ = EPCBLL.InsertLog(Ex, "UpdatePassword");
            }

            return flag;

        }

        private static string HexStringToBase64(string input)
        {
            byte[] text = HexStringToHex(input);
            string _str = System.Convert.ToBase64String(text);

            return _str;
        }

        private static byte[] HexStringToHex(string inputhex)
        {
            byte[] resultantArray = new byte[inputhex.Length / 2];
            for (int i = 0; i < resultantArray.Length; i++)
            {
                resultantArray[i] = System.Convert.ToByte(inputhex.Substring(i * 2, 2), 16);
            }

            return resultantArray;
        }

        #endregion

    }


}
