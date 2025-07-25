using DataAccessLayer;
using DataAccessLayer.CommonDataModels;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BussinessLayer
{
    public class EPCISBLL
    {
        public static List<string> GetEPCISCustomer()
        {
            List<string> ObjList = new List<string>();
            List<USP_EPCIS_Customer_Result> objlistcus = EPCISDAL.GetEPCISCustomer();

            for (int i = 0; i < objlistcus.Count(); i++)
            {
                ObjList.Add(objlistcus[i].CustomerId);
            }

            return ObjList;

        }
        public static List<EPCISDO> GetEPCIS_URN(string CustomerId)
        {
            List<EPCISDO> ObjList = new List<EPCISDO>();
            List<usp_EPCIS_GetEP_URN_Result> list = EPCISDAL.GetEPCIS_URN(CustomerId);

            ObjList = (from c in list
                       select new EPCISDO
                       {
                           CustomerId = c.CustomerId,
                           RPO = (long)c.RPO,
                           EPC_urn = c.EPCIS,
                           rpcDetailNumber = (long)c.DetailNumber,
                           SerailStart = (long)c.SerailStart,
                           SerailEnd = c.SerailEnd,
                           Qty = c.Qty,
                           EPC = c.EPC

                       }).ToList();


            return ObjList;
        }
        public static string InsertEPCISLog(EPCIS ObjEPCIS)
        {
            StringBuilder Objxml = new StringBuilder();

            _ = Objxml.Append("<EPCIS>");
            _ = Objxml.Append("<data>");
            foreach (EPCISDO item in ObjEPCIS.EPCISList)
            {
                _ = Objxml.Append("<Order>");
                _ = Objxml.Append("<CustomerId>" + item.CustomerId + "</CustomerId>");
                _ = Objxml.Append("<RPO>" + item.RPO + "</RPO>");
                _ = Objxml.Append("<DetailNumber>" + item.rpcDetailNumber + "</DetailNumber>");
                _ = Objxml.Append("<EPC_urn>" + item.EPC_urn + "</EPC_urn>");
                _ = Objxml.Append("<Remark>" + ObjEPCIS.Remark + "</Remark>");
                _ = Objxml.Append("<SerailStart>" + item.SerailStart + "</SerailStart>");
                _ = Objxml.Append("<SerailEnd>" + item.SerailEnd + "</SerailEnd>");
                _ = Objxml.Append("</Order>");
            }
            _ = Objxml.Append("</data>");
            _ = Objxml.Append("</EPCIS>");


            return EPCISDAL.InsertEPCISLog(ObjEPCIS, Objxml.ToString());
        }

        public static List<EPCISDO> GetEPCIS_URN_RPO(string CustomerId, long RPO)
        {
            List<EPCISDO> ObjList = new List<EPCISDO>();
            List<usp_EPCIS_GetEPC_URN_RPO_Result> list = EPCISDAL.GetEPCIS_URN_RPO(CustomerId, RPO);

            ObjList = (from c in list
                       select new EPCISDO
                       {
                           CustomerId = c.CustomerId,
                           RPO = (long)c.RPO,
                           EPC_urn = c.EPCIS,
                           rpcDetailNumber = (long)c.DetailNumber,
                           SerailStart = (long)c.SerailStart,
                           SerailEnd = c.SerailEnd,
                           Qty = c.Qty,
                           EPC = c.EPC

                       }).ToList();


            return ObjList;
        }
    }
}
