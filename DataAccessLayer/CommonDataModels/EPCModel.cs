using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.CommonDataModels
{
    public class EPCDO
    {
        public long UserId { get; set; }
        public string CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string EPC { get; set; }
        public long Serial { get; set; }
        public string TransactionType { get; set; }
        public string Event { get; set; }
        public long Quantity { get; set; }
        public long RPO { get; set; }
        public long DetailLineID { get; set; }
        public string CustomPara1 { get; set; }
        public string CustomPara2 { get; set; }




    }

    public class EPCModel
    {
        public string Request { get; set; }
        public string Response { get; set; }
        public EPCRequest EPCRequest { get; set; }
        public EPCResponse EPCResponse { get; set; }

    }

    public class EPCRequest : EPCDO
    {

        public string Schema { get; set; }
        public string GTIN { get; set; }
        //public long Quantity { get; set; }


    }
    public class EPCResponse
    {
        public string EPCStart { get; set; }
        public string EPCEnd { get; set; }
        public string SerialStart { get; set; }
        public string SerialEnd { get; set; }
        public string CustomerID { get; set; }
        public string GTIN { get; set; }
        public long Quantity { get; set; }
        public string Remark { get; set; }
       // public string TransactionType { get; set; }

    }

    public class GTINDO
    {
        public string GTIN { get; set; }
        //public long SerialStart { get; set; }
        //public long SerialEnd { get; set; }
        public long Filter { get; set; }
        public long Companyprefixlengt { get; set; }
        public long TagLength { get; set; }
        public string Serial { get; set; }
        public string EPC { get; set; }

    }
    public class EPCLog : EPCDO
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

    public class EPCCounter
    {
        public long Id { get; set; }
        public long  RPO { get; set; }
        public long DetailLineID { get; set; }
        public long LineNo { get; set; }
        public string GTIN { get; set; }
        public string EPC { get; set; }
        public string UserMemory { get; set; }
        public string Password { get; set; }
        public string LockID { get; set; }
        public long SerialNo { get; set; }
        public string Custom1 { get; set; }
        public string Custom2 { get; set; }
        public string Status { get; set; }
        public DateTime CreatedOn { get; set; }
    }

    public class EPCISDO
    {
        public string CustomerId { get; set; }
        public long rpcDetailNumber { get; set; }
        public long RPO { get; set; }
        public string EPC_urn { get; set; }
        public long SerailStart { get; set; }
        public long SerailEnd { get; set; }
        public long Qty { get; set; }
        public string EPC { get; set; }

    }
    public class EPCIS
    {
        public string EPCIS_Request { get; set; }
        public string EPCIS_Response { get; set; }
        public List<EPCISDO> EPCISList { get; set; }
        public string Remark { get; set; }
        public string eventId { get; set; }
        public string URL { get; set; }
    }
}
