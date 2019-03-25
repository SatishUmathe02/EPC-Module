using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.CommonDataModels
{
    public class EPC
    {
        public long UserId { get; set; }
        public long CustomerID { get; set; }
        public string CustomerName { get; set; }
    }
        
    public class EPCModel: EPC
    {
        public string Request { get; set; }
        public string Response { get; set; }
        public EPCRequest EPCRequest { get; set; }
        public EPCResponse EPCResponse { get; set; }

    }

    public class EPCRequest : EPC
    {
       
        public string Schema { get; set; }
        public string GTIN { get; set; }
        public long Quantity { get; set; }
       
    }
    public class EPCResponse: EPC
    {
        public string EPCStart { get; set; }
        public string EPCEnd { get; set; }
        public string Remark { get; set; }
       
        public long SerialNumberStart { get; set; }
        public long SerialNumberEnd { get; set; }
        public long Quantity { get; set; }
       
    }

    public class GTINDO
    {
        public string GTIN { get; set; }
        //public long SerialStart { get; set; }
        //public long SerialEnd { get; set; }
        public long Filter { get; set; }
        public long Companyprefixlengt { get; set; }
        public long TagLength { get; set; }
        public long Serial { get; set; }
    }
    public class EPCLog:EPC
    {
        public long Id { get; set; }
        public string GTIN { get; set; }
        public long SerialStart { get; set; }
        public long SerialEnd { get; set; }
        public string EPCStart { get; set; }
        public string EPCEnd { get; set; }
        public string Schema { get; set; }
        public string Remark { get; set; }
        public long MaximumSerial { get; set; }


    }
    public class EPCSerial
    {
        public long Id { get; set; }
        public string GTIN { get; set; }
        public long SerialLastNo { get; set; }
        public long MaximumSerial { get; set; }


    }
}
