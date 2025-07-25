using EPCWeb.Models;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

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
            //https://devepc.r-pac.com/ -- Dev
            //https://epc.r-pac.com/ -- Prod

            WebApiClient.BaseAddress = new Uri(publishWebApi);
            WebApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}