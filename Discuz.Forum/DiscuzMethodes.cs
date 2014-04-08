using System;
using System.Collections.Generic;
using System.Text;
using Discuz.Common;

namespace Discuz.Forum
{
    public class DiscuzMethodes
    {
        /// <summary>
        /// discuz authcode
        /// </summary>
        /// <param name="str"></param>
        /// <param name="operation"></param>
        /// <param name="key"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public static string AuthCode(string str, string operation, string key, int expiry)
        {
            int ckey_length = 4;
            key = Utils.MD5(key);
            string keya = Utils.MD5(key.Substring(0, 16));
            string keyb = Utils.MD5(key.Substring(16, 16));
            string md5MT = Utils.MD5(MicroTime());
            string keyc = ckey_length > 0 ? (operation.Equals("DECODE") ? str.Substring(0, ckey_length) : md5MT.Substring(md5MT.Length - ckey_length)) : string.Empty;

            string cryptkey = keya + Utils.MD5(keya + keyc);
            int key_length = cryptkey.Length;

            str = operation.Equals("DECODE") ? Base64Decode(str.Substring(ckey_length)) : (expiry > 0 ? expiry + Time() : 0).ToString("D10") + (Utils.MD5(str + keyb)).Substring(0, 16) + str;

            int string_length = str.Length;
            string result = string.Empty;

            int[] box = new int[256];
            for (int i = 0; i < 256; i++)
                box[i] = i;

            int[] rndkey = new int[256];
            for (int i = 0; i < 256; i++)
                rndkey[i] = (int)(cryptkey[i % key_length]);

            for (int i = 0, j = 0; i < 256; i++)
            {
                j = (j + box[i] + rndkey[i]) % 256;
                int tmp = box[i];
                box[i] = box[j];
                box[j] = tmp;
            }

            byte[] tmpResult = new byte[string_length];
            for (int a = 0, j = 0, i = 0; i < string_length; i++)
            {
                a = (a + 1) % 256;
                j = (j + box[a]) % 256;
                int tmp = box[a];
                box[a] = box[j];
                box[j] = tmp;
                tmpResult[i] = (byte)(str[i] ^ box[(box[a] + box[j]) % 256]);
            }

            result = System.Text.Encoding.UTF8.GetString(tmpResult);

            if (operation.Equals("DECODE"))
            {
                if ((long.Parse(result.Substring(0, 10)) == 0 || long.Parse(result.Substring(0, 10)) > Time()) && result.Substring(10, 16).Equals(Utils.MD5(result.Substring(26) + keyb).Substring(0, 16)))
                    return result.Substring(26);
                else
                    return string.Empty;
            }
            else
            {
                return keyc + Base64Encode(tmpResult).Replace("=", string.Empty);

            }
        }

        /// <summary>
        /// php time()
        /// </summary>
        /// <returns></returns>
        public static long Time()
        {
            DateTime timeStamp = new DateTime(1970, 1, 1);
            return (DateTime.UtcNow.Ticks - timeStamp.Ticks) / 10000000;
        }

        /// <summary>
        /// php microtime()
        /// </summary>
        /// <returns></returns>
        private static string MicroTime()
        {
            long sec = Time();
            int msec = DateTime.UtcNow.Millisecond;
            string strMsec = "0." + msec.ToString().PadRight(8, '0');
            return strMsec + " " + sec.ToString();
        }

        /// <summary>
        /// php base64_encode()
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string Base64Encode(byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// php base64_encode()
        /// </summary>
        /// <param name="thisEncode"></param>
        /// <returns></returns>
        public static string Base64Encode(string thisEncode)
        {
            string encode = "";
            try
            {
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(thisEncode);
                encode = Convert.ToBase64String(bytes);
            }
            catch
            {
                encode = thisEncode;
            }

            return encode;
        }

        public static string Base64Decode(string code)
        {
            while (code.Length % 4 != 0)
                code += "=";

            string decode = "";
            byte[] bytes = Convert.FromBase64String(code);

            foreach (byte b in bytes)
                decode += (char)b;
            return decode;
        }

        /// <summary>
        /// 特殊字符转移
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string EncodeCode(string str)
        {
            string tmp = string.Empty;
            string strSpecial = "_-.1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            for (int i = 0; i < str.Length; i++)
            {
                string crt = str.Substring(i, 1);
                if (strSpecial.Contains(crt))
                    tmp += crt;
                else
                {
                    byte[] bts = System.Text.Encoding.UTF8.GetBytes(crt);
                    foreach (byte bt in bts)
                    {
                        tmp += "%" + bt.ToString("X");
                    }
                }
            }
            return tmp;
        }


    }
}
