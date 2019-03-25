using DataAccessLayer.CommonDataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagDataTranslation;

namespace BussinessLayer
{
    public class GTINBLL
    {
       
        public static string GetGTIN(string Schema, string GTIN, long Serial)
        {
            string res = string.Empty;
            GTINDO Objgtin = new GTINDO();

            Objgtin = GetSchemaDetails(Schema);

            if (Objgtin != null)
            {
                Objgtin.GTIN = GTIN;
                Objgtin.Serial = Serial;
                res = GetBinaryToHex(Objgtin);
            }
            return res;
        }

        public static GTINDO GetSchemaDetails(string Schema)
        {
            GTINDO Objgtin = new GTINDO();
            switch (Schema)
            {
                case "SGTIN96":
                    Objgtin.Filter = 1;
                    Objgtin.Companyprefixlengt = 7;
                    Objgtin.TagLength = 96;
                    break;
                default:
                    Objgtin = null;
                    break;
            }
            return Objgtin;
        }

        public static string GetBinaryToHex(GTINDO Obj)
        {
            string res = string.Empty;

            try
            {
                TDTEngine engine = new TDTEngine();

                string epcIdentifier = @"gtin=" + Obj.GTIN + ";serial=" + Obj.Serial + "";// @"gtin=00037000302414;serial=10419703";
                string parameterList = @"filter=" + Obj.Filter + ";gs1companyprefixlength=" + Obj.Companyprefixlengt + ";tagLength=96"; //@"filter=3;gs1companyprefixlength=7;tagLength=96";

                string binary = engine.Translate(epcIdentifier, parameterList, @"BINARY");
                string binaryHex = engine.BinaryToHex(binary); //3074257bf7194e4000001a85

                return binaryHex.ToUpper();
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
    }
}
