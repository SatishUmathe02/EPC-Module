using DataAccessLayer;
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

        public static string GetGTIN(string Schema, string GTIN, long Serial, GTINDO Objgtin)
        {
            string res = string.Empty;
            //GTINDO Objgtin = new GTINDO();

            //Objgtin = GetSchemaDetails(Schema);

            if (Objgtin != null)
            {
                Objgtin.GTIN = GTIN;
                Objgtin.Serial = Convert.ToString(Serial);
                res = GetBinaryToHex(Objgtin);
            }
            return res;
        }

        public static GTINDO GetDecodeByEPC(GTINDO Objgtin)
        {
            if (Objgtin != null)
            {
                Objgtin = GetDecode(Objgtin);
            }

            return Objgtin;
        }

        private static string GetBinaryToHex(GTINDO Obj)
        {
            string res = string.Empty;

            try
            {
                TDTEngine engine = new TDTEngine();

                string epcIdentifier = @"gtin=" + Obj.GTIN + ";serial=" + Obj.Serial + "";// @"gtin=00037000302414;serial=10419703";
                string parameterList = @"filter=" + Obj.Filter + ";gs1companyprefixlength=" + Obj.Companyprefixlengt + ";tagLength=96";
                //@"filter=3;gs1companyprefixlength=7;tagLength=96";

                string binary = engine.Translate(epcIdentifier, parameterList, @"BINARY");
                string binaryHex = engine.BinaryToHex(binary); //3074257bf7194e4000001a85

                return binaryHex.ToUpper();
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        private static GTINDO GetDecode(GTINDO Obj)
        {
            string res = string.Empty;

            try
            {
                TDTEngine engine = new TDTEngine();

                string epcIdentifier = engine.HexToBinary(@"" + Obj.EPC + "");
                string parameterList = @"tagLength=96";
                string legacy = engine.Translate(epcIdentifier, parameterList, @"LEGACY");
                if (!string.IsNullOrEmpty(legacy))
                {
                    Obj.GTIN = legacy.Split(';')[0].Replace("gtin=", "");
                    Obj.Serial = legacy.Split(';')[1].Replace("serial=", "");

                }
            }
            catch (Exception ex)
            {
                //return ex.ToString();
            }

            return Obj;
        }

        public static List<EPCRequest> GetEPCTestRequestParam()
        {
            List<EPCRequest> ObjRes = new List<EPCRequest>();

            ObjRes = (from c in EPCDAL.GetCustomerWithGTINParameter()
                      select new EPCRequest()
                      {
                          CustomerID=c.CustomerId,
                          CustomerName=c.CustomerName,
                          GTIN=c.GTIN,
                          CustomPara1=c.CustomePara
                      }).ToList();

            
            return ObjRes;
        }
    }

    public class GTIN_Manul_BLL
    {
        // Table 6. The EPC SGTIN-96 bit allocation, header, and maximum decimal values. page 27.
        private static int? BIN = 2;
        private static int? HEX = 16;
        private static string sgtin96_bin_header = "00110000";
        private static int sgtin96_filter_value_bits = 3;
        private static int sgtin96_partition_value_bits = 3;
        private static int sgtin96_serial_number_bits = 38;
        private static int sgtin96_length_bits = 96;
        private static int sgtin96_length_hex = 24;
        private static Dictionary<int, int[]> sgtin96_company_prefix_len_partitions = sgtin96_company_prefix_len_partitions_();

        //static  {
        //    // Table 7. SGTIN-96 Partitions. page 28.
        //    // Column order: (L), P, M, N Nd
        //    sgtin96_company_prefix_len_partitions = new Dictionary<int, int[]>();
        //    sgtin96_company_prefix_len_partitions.put(12, new int[]{0, 40, 4, 1});
        //    sgtin96_company_prefix_len_partitions.put(11, new int[]{1, 37, 7, 2});
        //    sgtin96_company_prefix_len_partitions.put(10, new int[]{2, 34, 10, 3});
        //    sgtin96_company_prefix_len_partitions.put(9, new int[]{3, 30, 14, 4});
        //    sgtin96_company_prefix_len_partitions.put(8, new int[]{4, 27, 17, 5});
        //    sgtin96_company_prefix_len_partitions.put(7, new int[]{5, 24, 20, 6});
        //    sgtin96_company_prefix_len_partitions.put(6, new int[]{6, 20, 24, 7});
        //}

        private static Dictionary<int, int[]> sgtin96_company_prefix_len_partitions_()
        {
            sgtin96_company_prefix_len_partitions = new Dictionary<int, int[]>();
            sgtin96_company_prefix_len_partitions.Add(12, new int[] { 0, 40, 4, 1 });
            sgtin96_company_prefix_len_partitions.Add(11, new int[] { 1, 37, 7, 2 });
            sgtin96_company_prefix_len_partitions.Add(10, new int[] { 2, 34, 10, 3 });
            sgtin96_company_prefix_len_partitions.Add(9, new int[] { 3, 30, 14, 4 });
            sgtin96_company_prefix_len_partitions.Add(8, new int[] { 4, 27, 17, 5 });
            sgtin96_company_prefix_len_partitions.Add(7, new int[] { 5, 24, 20, 6 });
            sgtin96_company_prefix_len_partitions.Add(6, new int[] { 6, 20, 24, 7 });

            return sgtin96_company_prefix_len_partitions;
        }

        private static int[] getPartitionsByCompanyPrefixLengthInDigits(int company_prefix_length)
        {
            // column 3 (L)
            //return sgtin96_company_prefix_len_partitions.Add(company_prefix_length);

            foreach (KeyValuePair<int, int[]> parts in sgtin96_company_prefix_len_partitions)
            {
                // System.out.println("Key = " + parts.getKey() + ", Value = " + parts.getValue());
                if (parts.Key == company_prefix_length)
                {
                    int[] rv = parts.Value;
                    return rv;
                }
            }
            throw new System.ArgumentException("invalid partition value");
        }


        private static int[] getPartitionsByPartitionValue(int partition_value)
        {
            // column 1 (P)
            foreach (KeyValuePair<int, int[]> parts in sgtin96_company_prefix_len_partitions)
            {
                // System.out.println("Key = " + parts.getKey() + ", Value = " + parts.getValue());
                if (parts.Value[0] == partition_value)
                {
                    int[] rv = parts.Value;
                    rv[0] = parts.Key;
                    return rv;
                }
            }
            throw new System.ArgumentException("invalid partition value");
        }



        private static string longToBinaryWithFill(long? number, int bits)
        {
            // This is never called with any number larger than 40 bits, so Long is a good choice
            // return zeroFill(number.ToString(), bits);

            //string input = zeroFill(number.ToString(), bits);

            //return Convert.ToInt32(input, 2).ToString();

            return Convert.ToString(Convert.ToInt64(number), 2).PadLeft(bits, '0');
        }


        private static string zeroFill(string s, int n)
        {
            //int fill = n - s.Length;
            //string Zeroes = "";
            //if (fill > 0)
            //{
            //    Zeroes = (new string(new char[fill])).Replace("\0", "0");

            //}
            //return Zeroes + s;

            return s.Length >= n ? s : new String(new char[n - s.Length]).Replace('\0', '0') + s;

        }


        private static string binaryToHex(string binary)
        {
            // bin_epc is 96 bits, so need to use BitInt for hex conversion
            //return (new System.Numerics.BigInteger(bin)).ToString(HEX);
            StringBuilder result = new StringBuilder(binary.Length / 8 + 1);

            // TODO: check all 1's or 0's... Will throw otherwise

            int mod4Len = binary.Length % 8;
            if (mod4Len != 0)
            {
                // pad to length multiple of 8
                binary = binary.PadLeft(((binary.Length / 8) + 1) * 8, '0');
            }

            for (int i = 0; i < binary.Length; i += 8)
            {
                string eightBits = binary.Substring(i, 8);
                result.AppendFormat("{0:X2}", Convert.ToByte(eightBits, 2));
            }

            return result.ToString();
        }


        private static String hexToBinary(String hex)
        {
            // return new BigInteger(hex, HEX).toString(BIN);
            StringBuilder result = new StringBuilder();
            foreach (char c in hex)
            {
                // This will crash for non-hex characters. You might want to handle that differently.
                result.Append(hexCharacterToBinary[char.ToLower(c)]);
            }
            return result.ToString();
        }
        private static readonly Dictionary<char, string> hexCharacterToBinary = new Dictionary<char, string> {
    { '0', "0000" },
    { '1', "0001" },
    { '2', "0010" },
    { '3', "0011" },
    { '4', "0100" },
    { '5', "0101" },
    { '6', "0110" },
    { '7', "0111" },
    { '8', "1000" },
    { '9', "1001" },
    { 'a', "1010" },
    { 'b', "1011" },
    { 'c', "1100" },
    { 'd', "1101" },
    { 'e', "1110" },
    { 'f', "1111" }
};

        private static long? binaryToLong(string bin)
        {
            //return long.Parse(bin, BIN);
            var reversedBits = bin.Reverse().ToArray();
            var num = 0;
            for (var power = 0; power < reversedBits.Count(); power++)
            {
                var currentBit = reversedBits[power];
                if (currentBit == '1')
                {
                    var currentNum = (int)Math.Pow(2, power);
                    num += currentNum;
                }
            }

            return num;
        }


        private static int binaryToInt(string bin)
        {
            //return Integer.parseInt(bin, BIN);
            var reversedBits = bin.Reverse().ToArray();
            var num = 0;
            for (var power = 0; power < reversedBits.Count(); power++)
            {
                var currentBit = reversedBits[power];
                if (currentBit == '1')
                {
                    var currentNum = (int)Math.Pow(2, power);
                    num += currentNum;
                }
            }

            return num;
        }



        public static string encodeUPC(string upc, long? serial_number)
        {
            if (upc.Length != 12)
            {
                throw new System.ArgumentException("UPC must be 12 digits long");
            }
            // UPC-12 barcodes in this use case have 6 digit company identifier
            // Table 5. SGTIN Filter Values. page 24.
            return encode(zeroFill(upc, 14), 6, serial_number, 0);
        }


        /**
         * Encodes an SGTIN-96 encoded EPC ID.
         *
         * Follows the procedures details in document "EPC Generation 1 Tag Data
         * Standards Version 1.1 Rev.1.27" which can be found at:
         * http://www.gs1.org/sites/default/files/docs/epc/tds_1_1_rev_1_27-standard-20050510.pdf
         *
         * @param  gtin14 - the 14 character GTIN
         * @param  company_prefix_length - The length L of the Company Prefix portion of the GTIN
         * @param  serial_number - A Serial Number S where 0 ≤ S < 238, or an UCC/EAN-128 Application Identifier 21
         * @param  filter_value - A Filter Value F where 0 ≤ F < 8
         * @return       a 24 character EPC
         */
        public static string encode(string gtin14, int company_prefix_length, long? serial_number, int? filter_value)
        {
            // 3.4.2.1 SGTIN-96 Encoding Procedure. page 28.
            if (serial_number.ToString().Length > 1 && serial_number.ToString()[0] == '0')
            {
                throw new System.ArgumentException("serial number may not begin with 0");
            }
            int[] partitions = getPartitionsByCompanyPrefixLengthInDigits(company_prefix_length);
            if (partitions == null)
            {
                throw new System.ArgumentException("company prefix length must be <=12 and >= 6");
            }
            int partition_value = partitions[0];
            int company_prefix_bits = partitions[1];
            int item_reference_and_indicator_bits = partitions[2];

            if (gtin14.Length != 14)
            {
                throw new System.ArgumentException("GTIN must be 14 digits long");
            }

            long? company_prefix = Convert.ToInt64(gtin14.Substring(1, (company_prefix_length)));
            long? item_reference_and_indicator = Convert.ToInt64(gtin14[0] + gtin14.Substring(company_prefix_length + 1, 13 - (company_prefix_length + 1)));

            string bin_filter_value = longToBinaryWithFill((long)filter_value, sgtin96_filter_value_bits);
            string bin_partition_value = longToBinaryWithFill((long)partition_value, sgtin96_partition_value_bits);
            string bin_company_prefix = longToBinaryWithFill(company_prefix, company_prefix_bits);
            string bin_item_reference = longToBinaryWithFill(item_reference_and_indicator, item_reference_and_indicator_bits);
            string bin_serial_number = longToBinaryWithFill(serial_number, sgtin96_serial_number_bits);

            string bin_epc = sgtin96_bin_header + bin_filter_value + bin_partition_value + bin_company_prefix + bin_item_reference + bin_serial_number;

            return zeroFill(binaryToHex(bin_epc), sgtin96_length_hex);
        }


        /**
         * Decodes an SGTIN-96 encoded EPC ID.
         *
         * Follows the procedures details in document "EPC Generation 1 Tag Data;
         * Standards Version 1.1 Rev.1.27" which can be found at;
         * http://www.gs1.org/sites/default/files/docs/epc/tds_1_1_rev_1_27-standard-20050510.pdf
         *
         * @param  sgtin96_epc - the SGTIN-96 encoded EPC ID
         * @return       HashMap<String, String> with the keys:
         * filter_value - Encoded Filter Value
         * item_reference - Item Reference
         * serial_number - Serial Number
         * gtin14 - Encoded GTIN-14
         */
        public static Dictionary<string, string> decode(string sgtin96_epc)
        {

            //sgtin96_company_prefix_len_partitions = new Dictionary<int, int[]>();
            //sgtin96_company_prefix_len_partitions.Add(12, new int[] { 0, 40, 4, 1 });
            //sgtin96_company_prefix_len_partitions.Add(11, new int[] { 1, 37, 7, 2 });
            //sgtin96_company_prefix_len_partitions.Add(10, new int[] { 2, 34, 10, 3 });
            //sgtin96_company_prefix_len_partitions.Add(9, new int[] { 3, 30, 14, 4 });
            //sgtin96_company_prefix_len_partitions.Add(8, new int[] { 4, 27, 17, 5 });
            //sgtin96_company_prefix_len_partitions.Add(7, new int[] { 5, 24, 20, 6 });
            //sgtin96_company_prefix_len_partitions.Add(6, new int[] { 6, 20, 24, 7 });

            // 3.4.2.2 SGTIN-96 Decoding Procedure. page 29.
            if (sgtin96_epc.Length != sgtin96_length_hex)
            {
                throw new System.ArgumentException("EPC must be 24 characters long");
            }
            string binary = zeroFill(hexToBinary(sgtin96_epc), sgtin96_length_bits);

            string header = binary.Substring(0, 8);
            if (!header.Equals(sgtin96_bin_header))
            {
                throw new System.ArgumentException("EPC header does not correlate to SGTIN-96");
            }

            int filter_value = binaryToInt(binary.Substring(8, 3));
            int partition_value = binaryToInt(binary.Substring(11, 3));
            if (partition_value > 6)
            {
                throw new System.ArgumentException("Partition value cannot be greater than 6");
            }

            int[] lookup = getPartitionsByPartitionValue(partition_value);

            int company_prefix_len_bits = lookup[1];
            int company_prefix_len_digits = lookup[0];

            long? long_company_prefix_value = binaryToLong(binary.Substring(14, company_prefix_len_bits));
            if (long_company_prefix_value >= Math.Pow(10, company_prefix_len_digits))
            {
                throw new System.ArgumentException("Company Prefix exceeded specified length");
            }
            string company_prefix = zeroFill(long_company_prefix_value.ToString(), company_prefix_len_digits);

            int item_reference_and_indicator_len_digits = 13 - company_prefix_len_digits;
            int int_item_reference_and_indicator = binaryToInt(binary.Substring(14 + company_prefix_len_bits, 58 - (14 + company_prefix_len_bits)));
            if (int_item_reference_and_indicator >= Math.Pow(10, item_reference_and_indicator_len_digits))
            {
                throw new System.ArgumentException("Item Reference and Indicator exceeded specified length");
            }
            string item_reference_and_indicator = zeroFill(int_item_reference_and_indicator.ToString(), item_reference_and_indicator_len_digits);

            string thirteen = item_reference_and_indicator.Substring(0, 1) + company_prefix + item_reference_and_indicator.Substring(1);
            int termA = 0;
            int termB = 0;
            for (int i = 0; i < thirteen.Length; i++)
            {
                string c = thirteen.Substring(i, 1);
                if (i % 2 == 0)
                {
                    termB += int.Parse(c);
                }
                else
                {
                    termA += int.Parse(c);
                }
            }
            int check_digit = Math.Abs(((-3 * termA) - termB) % 10);
            string gtin14 = thirteen + check_digit.ToString();

            long? serial_number = binaryToLong(binary.Substring(58));

            Dictionary<string, string> rv = new Dictionary<string, string>();
            rv["filter_value"] = filter_value.ToString();
            rv["item_reference"] = item_reference_and_indicator;
            rv["serial_number"] = serial_number.ToString();
            rv["gtin14"] = gtin14;

            return rv;
        }

    }
}
