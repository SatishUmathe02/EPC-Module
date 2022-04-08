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
    }


}
