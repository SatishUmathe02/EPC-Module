using DataAccessLayer;
using DataAccessLayer.CommonDataModels;
using EPC_GS1;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BussinessLayer
{
    public class GS1_IntergrationBLL
    {

        public static EPCRequest IsCustomerGS1(EPCRequest ObjRequest)
        {
            usp_GS1_Customer_Result list = GS1_IntergrationDAL.IsCustomerGS1(ObjRequest.GTIN, ObjRequest.CustomerID);

            ObjRequest.GS1Customer = Convert.ToBoolean(list.GS1Customer);
            ObjRequest.GS1apiRequired = Convert.ToBoolean(list.GS1apiRequired);

            return ObjRequest;

        }
        public static EPCRequest GS1_Details(EPCRequest ObjEPC, List<GS1> ObjGS1, string GS1JSON)
        {

            if (ObjGS1 != null)
            {

                for (int i = 0; i < ObjGS1.Count; i++)
                {
                    if (ObjGS1[0].Prefixes != null)
                    {
                        if (!string.IsNullOrEmpty(ObjGS1[0].Prefixes.GS1Prefix))
                        {
                            ObjEPC.GS1Prefix = ObjGS1[0].Prefixes.GS1Prefix;
                            break;
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(ObjEPC.GS1Prefix))
            {
                bool chkPrefix = ChkPrefixCorrect(ObjEPC.GTIN, ObjEPC.GS1Prefix);

                ObjEPC.GS1Prefix = null;
                if (chkPrefix)
                {
                    usp_GS1_Insert_Result result = GS1_IntergrationDAL.InsertGS1Details_GetPartitionValue(ObjEPC.GTIN, ObjEPC.CustomerID, GS1JSON);

                    if (result != null)
                    {
                        ObjEPC.GS1Prefix = result.GS1Prefix;
                        ObjEPC.PartitionValue = Convert.ToInt32(result.Partiton);
                    }

                }
            }

            return ObjEPC;
        }

        private static bool ChkPrefixCorrect(string GTIN, string GSPrefix)
        {
            bool chk = false;
            if (!string.IsNullOrEmpty(GSPrefix))
            {
                int GSPrefixLen = GSPrefix.Length;
                string gtin13 = GTIN.Substring(0, 1) == "0" ? GTIN.Substring(1, 13) : GTIN;
                string Prefix = gtin13.Substring(0, GSPrefixLen);
                if (Prefix == GSPrefix)
                {
                    chk = true;
                }
            }

            return chk;
        }

        //public static string GS1_apiResponse(EPCRequest ObjEPC)
        //{
        //    return GS1_http.GS1_apiResponse(ObjEPC).Result;
        //}

        #region UPDATE FUCTION GS1 READ RESPONSE

        //public static string GS1_apiResponse_Restapi(EPCRequest ObjEPC)
        //{
        //    return GS1_http.GS1_apiResponse_Rest(ObjEPC).Result;
        //}

        public static List<GS1> GS1_apiResponse_Restapi(EPCRequest ObjEPC)
        {
            _ = new List<GS1>();
            string GS1_Response = GS1_http.GS1_apiResponse_Rest(ObjEPC).Result;

            List<GS1> ObjGS1 = GS1_http.GS1_NewVersion() ? GS1_V4(GS1_Response) : JsonConvert.DeserializeObject<List<GS1>>(GS1_Response);


            return ObjGS1;
        }

        public static List<GS1> GS1_V4(string GS1_Response)
        {
            List<GS1> ObjGS1 = new List<GS1>();
            List<GS1_V4> ObjGS1_V4 = new List<GS1_V4>();

            try
            {



                ObjGS1_V4 = JsonConvert.DeserializeObject<List<GS1_V4>>(GS1_Response);

                //object jsonObject = JsonConvert.DeserializeObject(GS1_Response);



                string dateUpdated = ObjGS1_V4.Count() == 0 ? "" : (ObjGS1_V4[0].dateUpdated == null ? "" : ObjGS1_V4[0].dateUpdated.ToString());

                ObjGS1 = (from c in ObjGS1_V4
                          select new GS1
                          {
                              Source = c.licensingMO.moName == null ? "" : c.licensingMO.moName.ToString(),
                              EntityGLN = c.licensingMO.moGLN == null ? "" : c.licensingMO.moGLN.ToString(),
                              CompanyName = c.licenseeName ?? "",
                              StreetAddress1 = c.address != null ? c.address.streetAddress == null ? "" : c.address.streetAddress.value : "",
                              StreetAddress2 = c.address != null ? c.address.streetAddressLine2 == null ? "" : c.address.streetAddressLine2.value : "",
                              StreetAddress3 = c.address != null ? c.address.streetAddressLine2 == null ? "" : c.address.streetAddressLine2.value : "",
                              ModifiedDate = Convert.ToString(dateUpdated),
                              City = c.address != null ? c.address.addressLocality == null ? "" : c.address.addressLocality.value : "",
                              StateProvince = c.address != null ? c.address.addressRegion == null ? "" : c.address.addressRegion.value : "",
                              ZipCode = c.address != null ? c.address.postalCode ?? "" : "",
                              Country = c.address != null ? c.address.countryCode ?? "" : "",
                              GSRN = c.gsrn ?? "",
                              Prefixes = new Prefixes()
                              {
                                  UPCPrefix = c.upcCompanyPrefix ?? "",
                                  GS1Prefix = c.licenceKey ?? "",
                                  PrefixStatus = c.licenceStatus ?? "",
                                  ModifiedDate = Convert.ToString(dateUpdated),

                              },


                          }).ToList();


            }
            catch (Exception)
            {
                try
                {
                    ObjGS1 = (from c in ObjGS1_V4
                              select new GS1
                              {
                                  EntityGLN = c.licenseeGln ?? "",
                                  CompanyName = c.licenseeName ?? "",
                                  ZipCode = c.address.postalCode ?? "",
                                  Country = c.address.countryCode ?? "",
                                  GSRN = c.gsrn ?? "",
                                  Prefixes = new Prefixes()
                                  {
                                      UPCPrefix = c.upcCompanyPrefix ?? "",
                                      GS1Prefix = c.licenceKey ?? "",
                                      PrefixStatus = c.licenceStatus ?? "",
                                      ModifiedDate = Convert.ToString(c.dateUpdated),

                                  },


                              }).ToList();
                }
                catch (Exception)
                {

                }
            }


            return ObjGS1;

        }

        #endregion

        public static void EmailNotification(string GS1JSON, List<GS1> listGs1, EPCRequest EPCReq)
        {
            try
            {
                if (listGs1.Count == 0)
                {
                    if (!string.IsNullOrEmpty(GS1JSON))
                    {
                        if (GS1JSON.Length <= 5)
                        {
                            GS1_http.SendEmail_AfterCheck_GS(1, GS1JSON, EPCReq.GTIN);
                        }
                    }
                }
                if (string.IsNullOrEmpty(EPCReq.GS1Prefix))
                {
                    if (listGs1.Count > 0)
                    {
                        GS1_http.SendEmail_AfterCheck_GS(2, GS1JSON, EPCReq.GTIN);

                    }
                }
            }
            catch
            {

            }


        }
    }
}
