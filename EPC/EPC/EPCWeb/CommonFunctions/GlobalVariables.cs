using EPCWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace EPCWeb.CommonFunctions
{
    public class GlobalVariables
    {
        public static HttpClient WebApiClient = new HttpClient();

        static GlobalVariables()
        {
            EPCSetting Objsetting = GetEPCSetting.GetSetting();
            string publishWebApi = Objsetting.WebApi;

            //"http://192.168.136.185:85/";-- Test
            //http://52.172.135.0:136/ -- Dev
            //http://52.172.187.213/ -- Prod

            WebApiClient.BaseAddress = new Uri(publishWebApi);
            WebApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}