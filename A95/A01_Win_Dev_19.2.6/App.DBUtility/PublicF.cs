using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.DBUtility
{
    public static class PublicF
    {
        static string S_Key = "GG520520@COSMO";
        /// <summary>
        ///作用：将字符串内容转化为16进制数据编码 
        /// </summary>
        /// <param name="strEncode"></param>
        /// <returns></returns>
        public static string Encode(string strEncode)
        {
            string strReturn = "";//  存储转换后的编码
            foreach (short shortx in strEncode.ToCharArray())
            {
                strReturn += shortx.ToString("X2");
            }
            return strReturn;
        }
        /// <summary>
        /// 作用：将16进制数据编码转化为字符串
        /// </summary>
        /// <param name="strDecode"></param>
        /// <returns></returns>
        public static string Decode(string strDecode)
        {
            string sResult = "";
            for (int i = 0; i < strDecode.Length / 2; i++)
            {
                sResult += (char)short.Parse(strDecode.Substring(i * 2, 2), global::System.Globalization.NumberStyles.HexNumber);
            }
            return sResult;
        }
        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="v_Password"></param>
        /// <param name="v_key"></param>
        /// <returns></returns>
        public static string EncryptPassword(string v_Password, string v_key)
        {
            if (v_key == "") { v_key = S_Key; }

            int i, j;
            int a = 0, b = 0, c = 0;
            string hexS = "", hexskey = "", midS = "", tmpstr = "";

            hexS = Encode(v_Password);
            hexskey = Encode(v_key);
            midS = hexS;

            for (i = 1; i <= hexskey.Length / 2; i++)
            {
                if (i != 1)
                {
                    midS = tmpstr;
                }
                tmpstr = "";
                for (j = 1; j <= midS.Length / 2; j++)
                {
                    a = (char)short.Parse(midS.Substring((j - 1) * 2, 2), global::System.Globalization.NumberStyles.HexNumber);
                    b = (char)short.Parse(hexskey.Substring((i - 1) * 2, 2), global::System.Globalization.NumberStyles.HexNumber);

                    //a = (char)short.Parse(Convert.ToString(midS[2 * j - 2]) + Convert.ToString(midS[2 * j-1]), global::System.Globalization.NumberStyles.HexNumber);
                    //b = (char)short.Parse(Convert.ToString(hexskey[2 * i - 2]) + Convert.ToString(hexskey[2 * i-1]), global::System.Globalization.NumberStyles.HexNumber);

                    c = a ^ b;
                    tmpstr += Encode(Convert.ToString((Convert.ToChar(c))));
                }
            }
            return tmpstr;
        }

        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="v_Password"></param>
        /// <param name="v_key"></param>
        /// <returns></returns>
        public static string DecryptPassword(string v_Password, string v_key)
        {
            if (v_key == "") { v_key = S_Key; }
            int i, j;
            int a = 0, b = 0, c = 0;
            string hexS = "", hexskey = "", midS = "", tmpstr = "";

            try
            {
                hexS = v_Password;
                if (hexS.Length % 2 == 1)
                {
                    return "-1:ciphertext is wrong";
                    //Response.Write("<script>alert(\"密文错误，无法解密字符串\");</script>");
                }
                hexskey = Encode(v_key);
                tmpstr = hexS;
                midS = hexS;
                for (i = hexskey.Length / 2; i >= 1; i--)
                {
                    if (i != hexskey.Length / 2)
                    {
                        midS = tmpstr;
                    }
                    tmpstr = "";
                    for (j = 1; j <= midS.Length / 2; j++)
                    {
                        a = (char)short.Parse(midS.Substring((j - 1) * 2, 2), global::System.Globalization.NumberStyles.HexNumber);
                        b = (char)short.Parse(hexskey.Substring((i - 1) * 2, 2), global::System.Globalization.NumberStyles.HexNumber);
                        c = a ^ b;
                        tmpstr += Encode(Convert.ToString((Convert.ToChar(c))));
                    }
                }
            }
            catch 
            {
                return "-1:ciphertext is wrong";
            }
            return Decode(tmpstr);
        }


        public static string DynPWd()
        {
            string S_Date = DateTime.Now.ToString("yyyy-MM-dd_HH");
            string S_DynPWD = EncryptPassword
            (
            "QwAsZxReFdVc" +
            S_Date,
            S_Date
            );

            return S_DynPWD;
        }


        /// <summary>
        /// DataTableToJson
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static string DataTableToJson(DataTable table)
        {
            var JsonString = new StringBuilder();
            if (table.Rows.Count > 0)
            {
                JsonString.Append("[" + "\r\n");
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    JsonString.Append("{" + "\r\n");
                    for (int j = 0; j < table.Columns.Count; j++)
                    {
                        string S_Value = table.Rows[i][j].ToString();
                        //int i1 = Regex.Matches(S_Value, @"-").Count;
                        //int i2 = Regex.Matches(S_Value, @":").Count;
                        //if (i1 == 2 && i2 == 2) 
                        //{
                        //    S_Value = S_Value.Insert(10, "T");
                        //    S_Value = S_Value.Substring(0, 11) + S_Value.Substring(12, 8);
                        //}

                        if (j < table.Columns.Count - 1)
                        {
                            JsonString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + S_Value + "\"," + "\r\n");
                        }
                        else if (j == table.Columns.Count - 1)
                        {
                            JsonString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + S_Value + "\"" + "\r\n");
                        }
                    }
                    if (i == table.Rows.Count - 1)
                    {
                        JsonString.Append("}" + "\r\n");
                    }
                    else
                    {
                        JsonString.Append("}," + "\r\n");
                    }
                }
                JsonString.Append("]");
            }
            else
            {
                JsonString.Append("[" + "\r\n");

                JsonString.Append("{" + "\r\n");
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    string S_Value = "";

                    if (j < table.Columns.Count - 1)
                    {
                        JsonString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + S_Value + "\"," + "\r\n");
                    }
                    else if (j == table.Columns.Count - 1)
                    {
                        JsonString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + S_Value + "\"" + "\r\n");
                    }
                }
                JsonString.Append("}" + "\r\n");

                JsonString.Append("]");
            }
            return JsonString.ToString();
        }
    }
}
