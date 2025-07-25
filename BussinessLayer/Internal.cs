using DataAccessLayer.CommonDataModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BussinessLayer
{
    public class InternalRequest
    {
        //encapsulates the logic.

        private readonly Dictionary<string, Action<EPCRequest>> customerHandlers;

        public InternalRequest()
        {
            customerHandlers = new Dictionary<string, Action<EPCRequest>>()
    {
      { "ELCORTEINGLES", HandleElCorteInglesRequest },
      { "LIBERATEDBRANDS", HandleLiberatedBrandsRequest },
    };
        }

        public EPCRequest HandleRequest(EPCRequest request)
        {
            if (customerHandlers.TryGetValue(request.CustomerID.ToUpper(), out Action<EPCRequest> handler))
            {
                handler(request);
            }

            return request;
        }

        private static void HandleInternalRequest(EPCRequest request)
        {
            request.GS1Customer = false;
            // Set other properties if needed (e.g., Request.GS1Prefix)
        }

        private static bool IsInternalRequest(string customPara1)
        {
            if (customPara1 != null)
            {
                string[] splitValues = customPara1.Split('#');
                if (splitValues.Where(x => x == "Internal").FirstOrDefault() != null)
                {
                    return true;
                }
            }
            return false;

            //return customPara1 ? == null ? false : customPara1.Split('#').Where(X => (X == "Internal")).FirstOrDefault;
        }

        private static void HandleElCorteInglesRequest(EPCRequest request)
        {
            if (IsInternalRequest(request.CustomPara1))
            {
                HandleInternalRequest(request);
                request.GS1Prefix = "Default";
                request.PartitionValue = 5;
            }
        }

        private static void HandleLiberatedBrandsRequest(EPCRequest request)
        {
            if (IsInternalRequest(request.CustomPara1))
            {
                HandleInternalRequest(request);
            }
        }
    }


}
