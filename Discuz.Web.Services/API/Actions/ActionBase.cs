using System;
#if NET1
#else
using System.Collections.Generic;
#endif
using System.Text;
using Discuz.Config;
using Discuz.Common;

namespace Discuz.Web.Services.API.Actions
{
    public abstract class ActionBase
    {
        private int uid = -1;
        private string secret;
        private string api_key;
        private FormatType format;
        private DNTParam[] parameters;
        private ApplicationInfo app;
        private int error_code;
        private double last_call_id;
        private double call_id;
        private string signature;

        internal GeneralConfigInfo Config = GeneralConfigs.GetConfig();

        internal int Uid
        {
            get { return uid; }
            set { uid = value; }
        }

        internal string Secret
        {
            get { return secret; }
            set { secret = value; }
        }

        internal string ApiKey
        {
            get { return api_key; }
            set { api_key = value; }
        }

        internal FormatType Format
        {
            get { return format; }
            set { format = value; }
        }

        internal ApplicationInfo App
        {
            get { return app; }
            set { app = value; }
        }

        internal DNTParam[] Params
        {
            get { return parameters; }
            set { parameters = value; }
        }

        internal int ErrorCode
        {
            get { return error_code; }
            set { error_code = value; }
        }

        internal double LastCallId
        {
            get { return last_call_id; }
            set { last_call_id = value; }
        }

        internal double CallId
        {
            get { return call_id; }
            set { call_id = value; }
        }

        internal string Signature
        {
            get { return signature; }
            set { signature = value; }
        }

        internal string ForumUrl
        {
            get { return Utils.GetRootUrl(BaseConfigs.GetForumPath.ToLower()); }
        }

        /// <summary>
        /// 获得参数对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        internal Object GetParam(string key)
        { 
            if (parameters == null)
                return null;
            foreach (DNTParam p in parameters)
            {
                if (p.Name.ToLower() == key.ToLower())
                {
                    return p.Value;
                }
            }
            return null;
        }

        /// <summary>
        /// 获得整形参数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        internal int GetIntParam(string key)
        {
            return TypeConverter.ObjectToInt(GetParam(key));
        }

        /// <summary>
        /// 获得整形参数，如果没有则返回默认值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        internal int GetIntParam(string key, int defaultValue)
        {
            return TypeConverter.ObjectToInt(GetParam(key), defaultValue);
        }

        /// <summary>
        /// 检查需要的参数是否存在
        /// </summary>
        /// <param name="paramArray">参数数组字符串</param>
        /// <returns></returns>
        internal bool CheckRequiredParams(string paramArray)
        {
            string[] parms = paramArray.Split(',');
            for (int i = 0; i < parms.Length; i++)
            {
                if (GetParam(parms[i]) == null || GetParam(parms[i]).ToString().Trim() == string.Empty)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 判断参数是否为空或者为0
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        internal bool AreParamsNullOrZeroOrEmptyString(params object[] parms)
        {
            foreach (object obj in parms)
            {
                if (obj == null) 
                {
                    return true;
                }

                if (obj.GetType() == typeof(int) && Convert.ToInt32(obj) == 0)
                {
                    return true;
                }

                if (obj.GetType() == typeof(string) && obj.ToString() == string.Empty)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
