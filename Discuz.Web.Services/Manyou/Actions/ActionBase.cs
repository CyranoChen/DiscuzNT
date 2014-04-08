using System;
#if NET1
#else
using System.Collections.Generic;
#endif
using System.Text;
using Discuz.Config;
using Discuz.Common;
using Newtonsoft.Json;

namespace Discuz.Web.Services.Manyou.Actions
{
    public abstract class ActionBase
    {
        private string module;
        private string method;
        private string jsonParams;

        internal string Module
        {
            get { return module; }
            set { module = value; }
        }

        internal string Method
        {
            get { return method; }
            set { method = value; }
        }

        internal string JsonParams
        {
            get { return jsonParams; }
            set { jsonParams = value; }
        }

        /// <summary>
        /// 将code中所有的Unicode编码解析成文字返回
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string UnicodeToString(string code)
        {
            string result = "";
            int count = 0;
            foreach (string b in Utils.SplitString(code, "\\u"))
            {
                if (count == 0)
                {
                    result += b; count++; continue;
                }
                char c = (char)Int32.Parse(b.Substring(0, 4), System.Globalization.NumberStyles.AllowHexSpecifier);
                result += c.ToString();
                if (b.Length > 4)
                    result += b.Substring(4);
                count++;
            }
            return result;
        }

        public string GetResult<T>(T obj)
        {
            GetResponse<T> response = new GetResponse<T>();
            response.Result = obj;
            return JavaScriptConvert.SerializeObject(response);
        }

        #region 注释掉的代码，不确定作废

        //public string ConvertJson(string json)
        //{
        //    string result = "";
        //    string[] keyNameList = { "appName", "requestName", "myml" };

        //    foreach (string keyName in keyNameList)
        //    {
        //        if (json.IndexOf(keyName) <= -1)
        //            continue;

        //        string keyValue = json.Substring(json.IndexOf(keyName) + keyName.Length + 3);
        //        int keyValueEnd = keyValue.IndexOf("\",");
        //        string keyValueSuffix = keyValue.Substring(keyValueEnd);
        //        keyValue = keyValue.Substring(0, keyValueEnd);

        //        string subResult = UnicodeToString(keyValue);

        //        result = json.Substring(0, json.IndexOf(keyName) + keyName.Length + 3) + subResult + keyValueSuffix;
        //        json = result;
        //    }

        //    return json;
        //}


        //public string ConvertJson(string json)
        //{
        //    string result = "";
        //    string[] keyNameList = { "appName", "requestName", "myml" };

        //    foreach (string keyName in keyNameList)
        //    {
        //        if (json.IndexOf(keyName) <= -1)
        //            continue;

        //        string keyValue = json.Substring(json.IndexOf(keyName) + keyName.Length + 3);
        //        int keyValueEnd = keyValue.IndexOf(",") - 1;
        //        string keyValueSuffix = keyValue.Substring(keyValueEnd);
        //        keyValue = keyValue.Substring(0, keyValueEnd);

        //        string subResult = UnicodeToString(keyValue);

        //        result = json.Substring(0, json.IndexOf(keyName) + keyName.Length + 3) + subResult + keyValueSuffix;
        //        json = result;
        //    }

        //    return json.Replace("\\", "\\\\");
        //}

        //private string UnicodeToString(string a)
        //{
        //    string result = "";
        //    foreach (string b in Utils.SplitString(a, "\\u"))
        //    {
        //        if (!string.IsNullOrEmpty(b))
        //        {
        //            char c = (char)Int32.Parse(b.Substring(0, 4), System.Globalization.NumberStyles.AllowHexSpecifier);
        //            result += c.ToString();
        //            if (b.Length > 4)
        //                result += b.Substring(4);
        //        }
        //    }
        //    return result;
        //}

        #endregion
    }
}
