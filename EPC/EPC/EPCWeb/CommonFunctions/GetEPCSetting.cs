namespace EPCWeb.CommonFunctions
{
    using System;
    using System.Linq;
    using System.Web;
    using Models;
    using System.Xml.Linq;

    public class GetEPCSetting
    {
        public static EPCSetting GetSetting()
        {
            EPCSetting ObjSetting = new EPCSetting();
            try
            {
                string xmlPath = HttpContext.Current.Server.MapPath("~/App_Data/EPCSetting.xml");
                XDocument xdoc = XDocument.Load(xmlPath);
                ObjSetting = (from p in xdoc.Descendants("Client")
                              select new EPCSetting()
                              {
                                  WebApi = p.Element("WebApi").Value,
                                  AADInstance = p.Element("AADInstance").Value,
                                  ClientId = p.Element("ClientId").Value,
                                  PostLogoutRedirectUri = p.Element("PostLogoutRedirectUri").Value,
                                  Tenant = p.Element("Tenant").Value,
                                  Path = p.Element("Path").Value

                              }).LastOrDefault();



            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return ObjSetting;
        }
    }
}