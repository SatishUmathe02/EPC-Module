using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPCWeb.Models
{
    public class EPCSetting
    {
        public string WebApi { get; set; }
        public string ClientId { get; set; }
        public string Tenant { get; set; }
        public string AADInstance { get; set; }
        public string PostLogoutRedirectUri { get; set; }
        public string Path { get; set; }
        public string rtracLogo { get; set; }
        public string loginIcon { get; set; }
        public string multiLang { get; set; }
        public string xlsIcon { get; set; }
        public string languagePath { get; set; }
    }
}