using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using DataAccessLayer.CommonDataModels;

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
            var list = EPCISDAL.GetEPCIS_URN(CustomerId);

            ObjList = (from c in list
                       select new EPCISDO
                       {
                           CustomerId = c.CustomerId,
                           RPO = (long)c.RPO,
                           EPC_urn = c.EPCIS,
                           rpcDetailNumber = (long)c.DetailNumber,
                           SerailStart = (long)c.SerailStart,
                           SerailEnd =(long)c.SerailEnd,
                           Qty=c.Qty,
                           EPC=c.EPC
                           
                       }).ToList();


            return ObjList;
        }
        public static string InsertEPCISLog(EPCIS ObjEPCIS)
        {
            StringBuilder Objxml = new StringBuilder();

            Objxml.Append("<EPCIS>");
            Objxml.Append("<data>");
            foreach (var item in ObjEPCIS.EPCISList)
            {
                Objxml.Append("<Order>");
                Objxml.Append("<CustomerId>" + item.CustomerId + "</CustomerId>");
                Objxml.Append("<RPO>" + item.RPO + "</RPO>");
                Objxml.Append("<DetailNumber>" + item.rpcDetailNumber + "</DetailNumber>");
                Objxml.Append("<EPC_urn>" + item.EPC_urn + "</EPC_urn>");
                Objxml.Append("<Remark>" + ObjEPCIS.Remark + "</Remark>");
                Objxml.Append("<SerailStart>" + item.SerailStart + "</SerailStart>");
                Objxml.Append("<SerailEnd>" + item.SerailEnd + "</SerailEnd>");
                Objxml.Append("</Order>");
            }
            Objxml.Append("</data>");
            Objxml.Append("</EPCIS>");


            return EPCISDAL.InsertEPCISLog(ObjEPCIS, Objxml.ToString());
        }

        public static List<EPCISDO> GetEPCIS_URN_RPO(string CustomerId, long RPO)
        {
            List<EPCISDO> ObjList = new List<EPCISDO>();
            var list = EPCISDAL.GetEPCIS_URN_RPO(CustomerId, RPO);

            ObjList = (from c in list
                       select new EPCISDO
                       {
                           CustomerId = c.CustomerId,
                           RPO = (long)c.RPO,
                           EPC_urn = c.EPCIS,
                           rpcDetailNumber = (long)c.DetailNumber,
                           SerailStart = (long)c.SerailStart,
                           SerailEnd = (long)c.SerailEnd,
                           Qty = c.Qty,
                           EPC = c.EPC

                       }).ToList();


            return ObjList;
        }
    }
}
