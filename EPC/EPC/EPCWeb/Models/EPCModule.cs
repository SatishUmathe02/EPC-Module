
namespace EPCWeb.Models
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
        //public string TransactionType { get; set; }


    }

    public class GTINDO
    {
        public string GTIN { get; set; }
        public long Serial { get; set; }
        public long Filter { get; set; }
        public long Companyprefixlengt { get; set; }
        public long TagLength { get; set; }
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
}