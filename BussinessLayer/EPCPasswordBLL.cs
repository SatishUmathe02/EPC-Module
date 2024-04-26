using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer
{
    public class EPCPasswordBLL
    {

        public static async Task<bool> UpdatePassword_MODA(long RPO, long DetailNo)
        {
            bool flag = false;
            try
            {
                var epclist = EPCDAL.GetEPCCounterFor_Moda_PWD(RPO, DetailNo);

                StringBuilder xml = new StringBuilder();
                if (epclist.Count == 0)
                {
                    flag = true;
                }

                if (epclist.Count > 0)
                {
                    xml.Append("<EPC>");
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
                    foreach (var item in epclist)
                    {
                        xml.Append("<Password>");
                        xml.Append("<Id>" + Convert.ToString(item.bigintId) + "</Id>");
                        xml.Append("<RPO>" + Convert.ToString(item.bigIntRPO) + "</RPO>");
                        xml.Append("<EPC>" + item.EPC + "</EPC>");
                        xml.Append("<AccesPwd>" + HMACSHA256ToHexStringL8(item.EPC, item.AccHexKey) + "</AccesPwd>");
                        xml.Append("<KillPwd>" + HMACSHA256ToHexStringL8(item.EPC, item.KillHexKey) + "</KillPwd>");
                        xml.Append("</Password>");
                    }
                    xml.Append("</EPC>");
                    EPCPasswordDAL.UpdatePassword(xml.ToString());
                    flag = true;

                }
            }
            catch (Exception Ex)
            {
                EPCDAL.SaveErrorFileResponse(Ex.ToString(), "UpdatePassword_Moda");
                flag = false;
                EPCBLL.InsertLog(Ex, "UpdatePassword_Moda");
            }

            return flag;

        }
        public static async Task<bool> UpdatePassword_MANGO(long RPO, long DetailNo)
        {
            bool flag = false;

            try
            {
                var epclist = EPCDAL.GetEPCCounterFor_MANGO_PWD(RPO, DetailNo);

                StringBuilder xml = new StringBuilder();

                if (epclist.Count == 0)
                {
                    flag = true;
                }
                if (epclist.Count > 0)
                {
                    xml.Append("<EPC>");
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
                    foreach (var item in epclist)
                    {
                        xml.Append("<Password>");
                        xml.Append("<Id>" + Convert.ToString(item.bigintId) + "</Id>");
                        xml.Append("<RPO>" + Convert.ToString(item.bigIntRPO) + "</RPO>");
                        xml.Append("<EPC>" + item.EPC + "</EPC>");
                        xml.Append("<AccesPwd>" + HMACSHA256ToHexStringL8(item.EPC, item.AccHexKey) + "</AccesPwd>");
                        xml.Append("<KillPwd>" + HMACSHA256ToHexStringL8(item.EPC, item.KillHexKey) + "</KillPwd>");
                        xml.Append("</Password>");
                    }
                    xml.Append("</EPC>");
                    EPCPasswordDAL.UpdatePassword_MANGO(xml.ToString());
                    flag = true;

                }
            }
            catch (Exception Ex)
            {
                flag = false;
                EPCBLL.InsertLog(Ex, "UpdatePassword_MANGO");
            }

            return flag;

        }
        public static async Task<bool> UpdatePassword_AlvaroMoreno(long RPO, long DetailNo)
        {
            bool flag = false;
            try
            {
                var epclist = EPCDAL.GetEPCCounterFor_AlvaroMoreno_PWD(RPO, DetailNo);

                StringBuilder xml = new StringBuilder();
                if (epclist.Count == 0)
                {
                    flag = true;
                }

                if (epclist.Count > 0)
                {
                    xml.Append("<EPC>");
                    foreach (var item in epclist)
                    {
                        xml.Append("<Password>");
                        xml.Append("<Id>" + Convert.ToString(item.bigintId) + "</Id>");
                        xml.Append("<RPO>" + Convert.ToString(item.bigIntRPO) + "</RPO>");
                        xml.Append("<EPC>" + item.EPC + "</EPC>");
                        xml.Append("<AccesPwd>" + HMACSHA256ToHexStringL8(item.EPC, item.AccHexKey) + "</AccesPwd>");
                        xml.Append("<KillPwd>" + HMACSHA256ToHexStringL8(item.EPC, item.KillHexKey) + "</KillPwd>");
                        xml.Append("</Password>");
                    }
                    xml.Append("</EPC>");
                    EPCPasswordDAL.UpdatePassword(xml.ToString());
                    flag = true;

                }
            }
            catch (Exception Ex)
            {
                EPCDAL.SaveErrorFileResponse(Ex.ToString(), "UpdatePassword_Moda");
                flag = false;
                EPCBLL.InsertLog(Ex, "UpdatePassword_Moda");
            }

            return flag;

        }
        public static async Task<bool> UpdatePassword_Charanga(long RPO, long DetailNo)
        {
            bool flag = false;
            try
            {
                var epclist = EPCDAL.GetEPCCounterFor_Charanga_PWD(RPO, DetailNo);

                StringBuilder xml = new StringBuilder();
                if (epclist.Count == 0)
                {
                    flag = true;
                }

                if (epclist.Count > 0)
                {
                    xml.Append("<EPC>");
                    foreach (var item in epclist)
                    {
                        xml.Append("<Password>");
                        xml.Append("<Id>" + Convert.ToString(item.bigintId) + "</Id>");
                        xml.Append("<RPO>" + Convert.ToString(item.bigIntRPO) + "</RPO>");
                        xml.Append("<EPC>" + item.EPC + "</EPC>");
                        xml.Append("<AccesPwd>" + HMACSHA256ToHexStringL8(item.EPC, item.AccHexKey) + "</AccesPwd>");
                        xml.Append("<KillPwd>" + HMACSHA256ToHexStringL8(item.EPC, item.KillHexKey) + "</KillPwd>");
                        xml.Append("</Password>");
                    }
                    xml.Append("</EPC>");
                    EPCPasswordDAL.UpdatePassword(xml.ToString());
                    flag = true;

                }
            }
            catch (Exception Ex)
            {
                EPCDAL.SaveErrorFileResponse(Ex.ToString(), "UpdatePassword_Charanga");
                flag = false;
                EPCBLL.InsertLog(Ex, "UpdatePassword_Charanga");
            }

            return flag;

        }
        public static async Task<bool> UpdatePassword_TENDAM(long RPO, long DetailNo)
        {
            bool flag = false;

            try
            {
                var epclist = EPCDAL.GetEPCCounterFor_TENDAM_PWD(RPO, DetailNo);

                StringBuilder xml = new StringBuilder();

                if (epclist.Count == 0)
                {
                    flag = true;
                }
                if (epclist.Count > 0)
                {
                    xml.Append("<EPC>");
                    foreach (var item in epclist)
                    {
                        xml.Append("<Password>");
                        xml.Append("<Id>" + Convert.ToString(item.bigintId) + "</Id>");
                        xml.Append("<RPO>" + Convert.ToString(item.bigIntRPO) + "</RPO>");
                        xml.Append("<EPC>" + item.EPC + "</EPC>");
                        xml.Append("<AccesPwd>" + HMACSHA256ToHexStringL8(item.EPC, item.AccHexKey) + "</AccesPwd>");
                        xml.Append("<KillPwd>" + HMACSHA256ToHexStringL8(item.EPC, item.KillHexKey) + "</KillPwd>");
                        xml.Append("</Password>");
                    }
                    xml.Append("</EPC>");
                    EPCPasswordDAL.UpdatePassword_TENDAM(xml.ToString());
                    flag = true;

                }
            }
            catch (Exception Ex)
            {
                flag = false;
                EPCBLL.InsertLog(Ex, "UpdatePassword_TENDAM");
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

                using (var hmacsha256 = new HMACSHA256(key))
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
                if (stHex.Length % 2 != 0)
                    throw new Exception("The length of the hex needs to be pair.");
                return Enumerable.Range(0, stHex.Length).Where(x => x % 2 == 0).Select(x => Convert.ToByte(stHex.Substring(x, 2), 16)).ToArray();
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
                string fmt;

                //formato en mayúsculas o minúsculas
                if (bUpper)
                    fmt = "{0:X2}";
                else
                    fmt = "{0:x2}";

                StringBuilder hex = new StringBuilder(ba.Length * 2);
                foreach (byte b in ba)
                    hex.AppendFormat(fmt, b);
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
                var epclist = EPCPasswordDAL.GetEPCCounterFor_Kiabi_PWD(RPO, DetailNo);

                StringBuilder xml = new StringBuilder();

                if (epclist.Count == 0)
                {
                    flag = true;
                }
                if (epclist.Count > 0)
                {
                    xml.Append("<EPC>");
                    foreach (var item in epclist)
                    {
                        xml.Append("<Password>");
                        xml.Append("<Id>" + Convert.ToString(item.bigintId) + "</Id>");
                        xml.Append("<RPO>" + Convert.ToString(item.bigIntRPO) + "</RPO>");
                        xml.Append("<EPC>" + item.EPC + "</EPC>");
                        string Pwd = PasswordGenerator_Kiabi(item.EPC);
                        xml.Append("<AccesPwd>" + Pwd + "</AccesPwd>");
                        xml.Append("<KillPwd>" + Pwd + "</KillPwd>");
                        xml.Append("</Password>");
                    }
                    xml.Append("</EPC>");
                    EPCPasswordDAL.UpdatePassword_Kiabi(xml.ToString());
                    flag = true;

                }
            }
            catch (Exception Ex)
            {
                flag = false;
                EPCBLL.InsertLog(Ex, "UpdatePassword_KIBAI");
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
        
    }


}
