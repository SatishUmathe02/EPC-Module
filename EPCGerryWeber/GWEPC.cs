using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace EPCGerryWeber
{
    public class GWEPC
    {
        public static ResponsePrintOrderDO GetEPC(RequestPrintOrderDO Obj)
        {


            string Req = string.Empty;
            string Res = string.Empty;
            Req = Request_requestPrintOrder(Obj);
            Res = HttpResponse(Req, "requestPrintOrder", Obj.ProdOrDevEndPoint);


            string endPoint = ConfigurationManager.AppSettings["" + Obj.ProdOrDevEndPoint + "_endPoint"].ToString();
            EPCDAL.InsertReqRes(Obj.CustomerId, Obj.RPO, Obj.DetailNo, Req, Res, endPoint, Obj.UserId, Obj.GTIN);

            ResponsePrintOrderDO ObjResponse = ReadResponse(Res);
            return ObjResponse;
        }
        private static string Request_requestPrintOrder(RequestPrintOrderDO Obj)
        {
            StringBuilder str = new StringBuilder();
            str.Append("<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">");
            str.Append("<soapenv:Body>");

            str.Append("<requestPrintOrder>");

            str.Append("<PrintRequestHead>");
            str.Append("<datref>" + Obj.datref + "</datref>");
            str.Append("<transmission_time>" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ") + "</transmission_time>");
            str.Append("<prr_id>" + Obj.prr_id + "</prr_id>");
            str.Append("<company>" + Obj.company + "</company>");
            str.Append("<season>" + Obj.season + "</season>");
            str.Append("<po_number>" + Obj.po_number + "</po_number>");
            str.Append("<pro_location>" + Obj.pro_location + "</pro_location>");
            str.Append("<prr_type>1</prr_type>");
            str.Append("<prr_prio>1</prr_prio>");

            str.Append("<positions>");
            //str.Append("<PrintRequestPosition>");

            str.Append("<pos>" + Obj.pos + "</pos>");
            str.Append("<label_type>" + Obj.label_type + "</label_type>");

            str.Append("<quantitys>");
            //str.Append("<PrintRequestQuantitys>");

            str.Append("<po_pos>" + Obj.po_pos + "</po_pos>");
            str.Append("<po_size>" + Obj.po_size + "</po_size>");
            str.Append("<quantity>" + Obj.quantity + "</quantity>");

            //str.Append("</PrintRequestQuantitys>");
            str.Append("</quantitys>");


            //str.Append("</PrintRequestPosition>");
            str.Append("</positions>");

            str.Append("</PrintRequestHead>");
            str.Append("</requestPrintOrder>");

            str.Append("</soapenv:Body>");
            str.Append("</soapenv:Envelope>");

            return str.ToString();
        }
        private static string HttpResponse(string Request, string Action, string ProdOrDev)
        {
            string response = string.Empty;

            string endPoint = Convert.ToString(ConfigurationManager.AppSettings["" + ProdOrDev + "_endPoint"]);
            string User = Convert.ToString(ConfigurationManager.AppSettings["" + ProdOrDev + "_User"]);
            string Password = Convert.ToString(ConfigurationManager.AppSettings["" + ProdOrDev + "_Password"]);


            WebRequest request = (HttpWebRequest)WebRequest.Create(endPoint);
            request.Method = "POST";

            byte[] byteArray = Encoding.UTF8.GetBytes(Request);
            request.ContentLength = byteArray.Length;

            request.ContentType = "application/soap+xml;charset=UTF-8";
            request.Headers.Add("SOAPAction", Action);

            NetworkCredential creds = new NetworkCredential(User, Password);
            request.Credentials = creds;

            Stream datastream = request.GetRequestStream();
            datastream.Write(byteArray, 0, byteArray.Length);
            datastream.Close();

            try
            {
                using (HttpWebResponse httpResponse = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream stream = httpResponse.GetResponseStream())
                    {
                        response = (new StreamReader(stream)).ReadToEnd();
                    }
                }
            }
            catch (WebException ex)
            {
                int StatusCode = Convert.ToInt32(((System.Net.HttpWebResponse)ex.Response).StatusCode);

                response = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                SendEmail(StatusCode, response);
            }


            return response;
        }
        private static ResponsePrintOrderDO ReadResponse(string httpResponse)
        {
            ResponsePrintOrderDO Obj = new ResponsePrintOrderDO();

            if (!string.IsNullOrEmpty(httpResponse))
            {

                try
                {


                    XmlDocument document = new XmlDocument();
                    document.LoadXml(httpResponse);  //loading soap message as string 
                    XmlNamespaceManager manager = new XmlNamespaceManager(document.NameTable);

                    manager.AddNamespace("d", "http://schemas.xmlsoap.org/soap/envelope/");
                    manager.AddNamespace("bhr", "http://www.w3.org/2001/XMLSchema");

                    XmlNodeList xnList = document.SelectNodes("//requestPrintOrderResponse/out", manager);
                    int nodes = xnList.Count;

                    if (nodes != 0)
                    {
                        foreach (XmlNode xn in xnList)
                        {
                            XmlNodeList xnListepc = xn.SelectNodes("//positions/idents", manager);
                            foreach (XmlNode item in xnListepc)
                            {
                                Obj.epc_start = item.SelectSingleNode("epc_start") == null ? "" : item.SelectSingleNode("epc_start").InnerText;
                                Obj.epc_end = item.SelectSingleNode("epc_end") == null ? "" : item.SelectSingleNode("epc_end").InnerText;
                                Obj.quantity = item.SelectSingleNode("quantity") == null ? 0 : Convert.ToInt32(item.SelectSingleNode("quantity").InnerText);
                                Obj.Remark = "Success";
                            }
                        }

                        if ((string.IsNullOrEmpty(Obj.epc_start)) || (string.IsNullOrEmpty(Obj.epc_end)))
                        {
                            Obj.Remark = "No EPC";
                        }
                    }
                    else
                    {
                        string Error = string.Empty;
                        try
                        {
                            foreach (XmlNode item in document.SelectNodes("//detail/faultData"))
                            {
                                Error = document.SelectSingleNode("//err_message") == null ? "" : document.SelectSingleNode("//err_message").InnerText;
                            }
                        }
                        catch (Exception ex)
                        {
                            Error = "";
                        }
                        if (string.IsNullOrEmpty(Error))
                        {
                            Error = document.SelectSingleNode("//faultcode") == null ? "" : document.SelectSingleNode("//faultcode").InnerText;
                        }
                        Obj.Remark = Error;


                    }
                }
                catch (Exception ex)
                {
                    Obj.Remark = ex.ToString();
                }
            }

            return Obj;
        }

        private static void SendEmail(int StatusCode, string Response)
        {
            int code = Convert.ToInt32(StatusCode.ToString().Substring(0, 1));
            string Recipient = ConfigurationManager.AppSettings["ITEmailID"].ToString();
            string Subject = string.Empty;
            string varCC = "";
            string varBCC = ""; string varReplyTo = "";
            //string varAttachmentName = "";

            StringBuilder Body = new StringBuilder();

            switch (code)
            {
                case 5:
                case 4:
                    varCC = "Satish.umathe@r-pac.com,Namrata.bhagat@r-pac.com,Jacco.Vandingstee@r-pac.com";
                    Subject = "r-pac: Gerry Weber web service not reachable " + StatusCode + "(" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ") + ")";
                    Body.Append(Response);
                    EPCDAL.InsertEmail(Recipient, varCC, varBCC, varReplyTo, Subject, Body.ToString(), 1, DateTime.Now);
                    break;
                default:
                    break;
            }
        }

    }

    public class RequestPrintOrderDO
    {
        public int company { get; set; }
        public int datref { get; set; }
        public string label_type { get; set; }
        public int pos { get; set; }
        public int po_pos { get; set; }
        public string po_size { get; set; }
        public int quantity { get; set; }
        public int po_number { get; set; }
        public string pro_location { get; set; }
        public string prr_id { get; set; }
        // public int prr_prio { get; set; }
        // public int prr_type { get; set; }
        public int season { get; set; }

        public long RPO { get; set; }
        public long DetailNo { get; set; }
        public string CustomerId { get; set; }
        public long UserId { get; set; }

        public string ProdOrDevEndPoint { get; set; }
        public string GTIN { get; set; }


    }
    public class ResponsePrintOrderDO
    {
        public string Remark { get; set; }
        public string epc_start { get; set; }
        public string epc_end { get; set; }
        public int quantity { get; set; }


    }
}
