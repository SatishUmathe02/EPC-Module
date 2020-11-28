
using System;
using System.Collections.Generic;
using System.Web.Mvc;

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

        public string Request_New { get; set; }
        public List<SelectListItem> CustomerList { get; set; }
        public List<EPCCounter> EPCCounterList { get; set; }

        public List<GS1> GS1_Info { get; set; }
        public string GS1_Error { get; set; }

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

        public string AccessPWD { get; set; }
        public string KillPWD { get; set; }


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
    public class EPCCustomer
    {
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string IsGS1 { get; set; }
    }

    public class EPCCounter
    {
        public long Id { get; set; }
        public long RPO { get; set; }
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
        public string KillPassword { get; set; }
    }

    public class GS1Response
    {
        public List<GS1> GS1List { get; set; }
        public string Error { get; set; }
    }
    public class GS1
    {

        public string Source { get; set; }
        public string EntityGLN { get; set; }
        public string CompanyName { get; set; }
        public string StreetAddress1 { get; set; }
        public string StreetAddress2 { get; set; }
        public string StreetAddress3 { get; set; }
        public string City { get; set; }
        public string StateProvince { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string GSRN { get; set; }
        public string ModifiedDate { get; set; }
        public Prefixes Prefixes { get; set; }
    }
    public class Prefixes
    {
        public string UPCPrefix { get; set; }
        public string GS1Prefix { get; set; }
        public string PrefixStatus { get; set; }
        public string ModifiedDate { get; set; }
    }
}