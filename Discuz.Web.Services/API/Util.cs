using System;
#if NET1
#else
using System.Collections.Generic;
#endif
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Collections;
using Discuz.Common;
using System.Text.RegularExpressions;

namespace Discuz.Web.Services.API
{
    public class Util
    {
        private const string LINE = "\r\n";
#if NET1
        private static Hashtable serializer_dict = new Hashtable();
#else
		private static Dictionary<int, XmlSerializer> serializer_dict = new Dictionary<int, XmlSerializer>();
#endif

        private DNTParam VersionParam = DNTParam.Create("v", "1.0");
        private string api_key;
        private string secret;
        private bool use_json;

        private static Regex[] r = new Regex[3];
        private const string REGEX_FORMAT = @"(\<{0}\>)([\s\S]+?)(\<\/{0}\>)";
        static Util()
        {

            r[0] = new Regex(string.Format(REGEX_FORMAT, "message"), RegexOptions.IgnoreCase);
            r[1] = new Regex(string.Format(REGEX_FORMAT, "title"), RegexOptions.IgnoreCase);
            r[2] = new Regex(string.Format(REGEX_FORMAT, "signature"), RegexOptions.IgnoreCase);
        }

        private static XmlSerializer ErrorSerializer
        {
            get
            {
                return GetSerializer(typeof(Error));
            }
        }

        public Util(string api_key, string secret)
        {
            this.api_key = api_key;
            this.secret = secret;
        }

        /// <summary>
        /// 是否使用json格式返回数据
        /// </summary>
        public bool UseJson
        {
            get { return use_json; }
            set { use_json = value; }
        }

        /// <summary>
        /// 整合程序密钥
        /// </summary>
        internal string SharedSecret
        {
            get { return secret; }
            set { secret = value; }
        }

        /// <summary>
        /// 整合程序Key
        /// </summary>
        internal string ApiKey
        {
            get { return api_key; }
        }

        //public T GetResponse<T>(string method_name, params DNTParam[] parameters)
        //{
        //    string url = FormatGetUrl(method_name, parameters);
        //    byte[] response_bytes = GetResponseBytes(url);

        //    XmlSerializer response_serializer = GetSerializer(typeof(T));
        //    try
        //    {
        //        T response = (T)response_serializer.Deserialize(new MemoryStream(response_bytes));
        //        return response;
        //    }
        //    catch
        //    {
        //        Error error = (Error)ErrorSerializer.Deserialize(new MemoryStream(response_bytes));
        //        throw new DNTException(error.ErrorCode, error.ErrorMsg);
        //    }
        //}

        //public XmlDocument GetResponse(string method_name, params DNTParam[] parameters)
        //{
        //    string url = FormatGetUrl(method_name, parameters);
        //    byte[] response_bytes = GetResponseBytes(url);

        //    XmlDocument doc = new XmlDocument();
        //    doc.LoadXml(Encoding.Default.GetString(response_bytes));

        //    return doc;
        //}

        /// <summary>
        /// 获得远程页面内容
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static byte[] GetResponseBytes(string url)
        {
            WebRequest request = HttpWebRequest.Create(url);
            WebResponse response = null;

            try
            {
                response = request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    return Encoding.UTF8.GetBytes(reader.ReadToEnd());
                }
            }
            finally
            {
                if (response != null)
                    response.Close();
            }
        }

        /// <summary>
        /// 获得序列器
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static XmlSerializer GetSerializer(Type t)
        {
            int type_hash = t.GetHashCode();

            if (!serializer_dict.ContainsKey(type_hash))
                serializer_dict.Add(type_hash, new XmlSerializer(t));

            return serializer_dict[type_hash] as XmlSerializer;
        }

        /// <summary>
        /// 转换参数
        /// </summary>
        /// <param name="method_name"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        internal DNTParam[] Sign(string method_name, DNTParam[] parameters)
        {
            ArrayList list = new ArrayList();
            list.Add(DNTParam.Create("method", method_name));
            list.Add(DNTParam.Create("api_key", api_key));
            list.Add(VersionParam);
            list.Sort();

            StringBuilder values = new StringBuilder();

            foreach (DNTParam param in list)
                values.Append(param.ToString());

            values.Append(secret);

            byte[] md5_result = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(values.ToString()));

            StringBuilder sig_builder = new StringBuilder();

            foreach (byte b in md5_result)
                sig_builder.Append(b.ToString("x2"));

            list.Add(DNTParam.Create("sig", sig_builder.ToString()));

            return (DNTParam[])list.ToArray();
        }

        /// <summary>
        /// 将字符型转为浮点型
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        internal static float GetFloatFromString(string input)
        {
            float returnValue;
#if NET1
            try
            {
                returnValue = Convert.ToSingle(input);
            }
            catch 
            {
                returnValue = -1;
            }
#else
            float.TryParse(input, out returnValue);
#endif
            return returnValue;
        }

        /// <summary>
        /// 去除xml中的空节点
        /// </summary>
        /// <param name="xml"></param>
        /// <returns>整理过的xml字符串</returns>
        internal static string RemoveEmptyNodes(string xml, string whitelist)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            XmlNodeList nodes = doc.SelectNodes("//node()");

            foreach (XmlNode node in nodes)
                if (node.ChildNodes.Count == 0 && node.InnerText == string.Empty && !Utils.InArray(node.Name, whitelist))
                    node.ParentNode.RemoveChild(node);
            StringWriter sw = new StringWriter();
            XmlTextWriter xw = new XmlTextWriter(sw);
            xw.Formatting = Formatting.Indented;
            xw.Indentation = 2;
            doc.PreserveWhitespace = true;
            doc.WriteTo(xw);
            xml = sw.ToString();
            sw.Close();
            xw.Close();
            return xml;
        }

        /// <summary>
        /// 去除json的空属性
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        internal static string RemoveJsonNull(string json)
        {
            //return System.Text.RegularExpressions.Regex.Replace(json, @",?""\w*"":null,?", string.Empty);
            json = System.Text.RegularExpressions.Regex.Replace(json, @",""\w*"":null", string.Empty);
            json = System.Text.RegularExpressions.Regex.Replace(json, @"""\w*"":null,", string.Empty);
            json = System.Text.RegularExpressions.Regex.Replace(json, @"""\w*"":null", string.Empty);
            return json;
        }

        /// <summary>
        /// 为message添加cdata标记
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        internal static string AddMessageCDATA(string xml)
        {
            return AddCDATA(xml, r[0]);
        }

        /// <summary>
        /// 为title添加cdata标记
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        internal static string AddTitleCDATA(string xml)
        {
            return AddCDATA(xml, r[1]);
        }

        /// <summary>
        /// 为signature添加cdata标记
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        internal static string AddSignatureCDATA(string xml)
        {
            return AddCDATA(xml, r[2]);
        }

        /// <summary>
        /// 为元素内容添加CDATA
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        internal static string AddCDATA(string xml, Regex r)
        {
            Match m;

            for (m = r.Match(xml); m.Success; m = m.NextMatch())
            {
                xml = xml.Replace(m.Groups[0].ToString(), string.Format("{0}<![CDATA[\r\n{1}\r\n]]>{2}", m.Groups[1].ToString(), m.Groups[2].ToString(), m.Groups[3].ToString()));
            }
            return xml;
        }


    }
}
