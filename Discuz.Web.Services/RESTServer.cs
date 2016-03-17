using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using Discuz.Config;
using Discuz.Web.Services.API.Actions;
using Discuz.Common;
using System.Reflection;
using Discuz.Web.Services.API;
using System.Web;
using System.Collections;
using System.Security.Cryptography;
using Discuz.Forum;
using Newtonsoft.Json;

namespace Discuz.Web.Services
{
    public class RESTServer : Page
    {
        private static ErrorDetails _errorDetails = new ErrorDetails();
        public RESTServer()
        {
            this.Load += new EventHandler(RESTServer_Load);
        }

        void RESTServer_Load(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "text/xml";

            APIConfigInfo apiInfo = APIConfigs.GetConfig();
            if (!apiInfo.Enable)
            {
                ResponseErrorInfo((int)ErrorType.API_EC_SERVICE);
                return;
            }

            //check sig
            DNTParam[] parameters = GetParamsFromRequest(Request);


            //GetRequests

            /*---- optional ----*/

            //format
            string format = DNTRequest.GetString("format");
            //callback
            string callback = DNTRequest.GetString("callback");
           

            /*---- required ----*/

            //api_key
            string api_key = DNTRequest.GetString("api_key");
            //整合程序对象
            ApplicationInfo appInfo = null;
            ApplicationInfoCollection appcollection = apiInfo.AppCollection;
            foreach (ApplicationInfo newapp in appcollection)
            {
                if (newapp.APIKey == DNTRequest.GetString("api_key"))
                {
                    appInfo = newapp;
                }
            }

            if (appInfo == null)
            {
                //输出API Key错误
                ResponseErrorInfo((int)ErrorType.API_EC_APPLICATION);
                return;
            }
            //check request ip
            string ip = DNTRequest.GetIP();
            if (appInfo.IPAddresses != null && appInfo.IPAddresses.Trim() != string.Empty && !Utils.InIPArray(ip, appInfo.IPAddresses.Split(',')))
            {
                ResponseErrorInfo((int)ErrorType.API_EC_BAD_IP);
                return;
            }

            /*---- required by specific method----*/



            string sig = GetSignature(parameters, appInfo.Secret);
            //if (sig != DNTRequest.GetString("sig"))
            //{
            //    //输出签名错误
            //    ResponseErrorInfo((int)ErrorType.API_EC_SIGNATURE);
            //    return;
            //}

            //get session_key and check user
            string session_key = DNTRequest.GetString("session_key");
            int uid = GetUidFromSessionKey(session_key, appInfo.Secret);




            string method = DNTRequest.GetString("method");
            if (method == string.Empty)
            {
                ResponseErrorInfo((int)ErrorType.API_EC_METHOD);
                return;
            }
            string classname = method.Substring(0, method.LastIndexOf('.'));
            string methodname = method.Substring(method.LastIndexOf('.') + 1);

            string content;
            ActionBase action;
            double lastcallid = -1;
            double callid = -1;
            try
            {
                Type type = Type.GetType(string.Format("Discuz.Web.Services.API.Actions.{0}, Discuz.Web.Services", classname), false, true);
                action = (ActionBase)Activator.CreateInstance(type);
                action.ApiKey = api_key;
                action.Params = parameters;
                action.App = appInfo;
                action.Secret = appInfo.Secret;
                action.Uid = uid;
                action.Format = FormatType.XML;
                action.Signature = sig;

                //call_id    - milliseconds  record last callid
                double.TryParse(DNTRequest.GetString("call_id"), out callid);
                if (callid > -1)
                {
                    if (Session["call_id"] == null)
                        lastcallid = -1;
                    else
                        double.TryParse(Session["call_id"].ToString(), out lastcallid);
                }
                action.CallId = callid;
                action.LastCallId = lastcallid;

                if (format.Trim().ToLower() == "json")
                {
                    Response.ContentType = "text/html";
                    action.Format = FormatType.JSON;
                }

                content = type.InvokeMember(methodname, BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.IgnoreCase, null, action, new object[] { }).ToString();
            }
            catch
            {
                content = "";
                ResponseErrorInfo((int)ErrorType.API_EC_METHOD);
                return;
            }
            if (action.ErrorCode > 0)
            {
                ResponseErrorInfo(action.ErrorCode);
                return;
            }

            //update callid
            if (callid > lastcallid)
            {
                Session["call_id"] = callid;
            }

            //成功后适当的地方更新用户在线状态
            if (callback != string.Empty)
            {
                Response.ContentType = "text/html";
                if (action.Format == FormatType.JSON)
                {
                    content = callback + "(" + content + ");";
                }
                else
                {
                    content = callback + "(\"" + content.Replace("\"", "\\\"") + "\");";
                }
            }
            Response.Write(content);
            Response.End();

        }

        #region private methods

        /// <summary>
        /// 根据SessionKey获得Uid
        /// </summary>
        /// <param name="session_key">会话key</param>
        /// <param name="secret">整合程序密码</param>
        /// <returns>uid</returns>
        private int GetUidFromSessionKey(string session_key, string secret)
        {
            if (session_key.Trim() == string.Empty)
                return -1;
            string[] sessionArray = session_key.Split('-');
            if (sessionArray.Length != 2)
                return -1;
            int uid = Utils.StrToInt(sessionArray[1], -1);
            int olid = OnlineUsers.GetOlidByUid(uid);

            byte[] md5_result = MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(olid.ToString() + secret));

            StringBuilder sessionkey_builder = new StringBuilder();

            foreach (byte b in md5_result)
                sessionkey_builder.Append(b.ToString("x2"));
            if (sessionkey_builder.ToString() != sessionArray[0])
                return -1;
            return uid > 0 ? uid : -1;
        }

        /// <summary>
        /// 输出错误信息
        /// </summary>
        /// <param name="errorCode">错误代码</param>
        private void ResponseErrorInfo(int errorCode)
        {
            string format = DNTRequest.GetString("format").Trim().ToLower();
            Error error = new Error();
            error.ErrorCode = errorCode;
            error.ErrorMsg = _errorDetails[errorCode].ToString();

            ArrayList list = new ArrayList();
            foreach (string key in Request.QueryString.AllKeys)
            {
                list.Add(new Arg(key, Utils.UrlDecode(Request.QueryString[key])));
            }
            foreach (string key in Request.Form.AllKeys)
            {
                list.Add(new Arg(key, Utils.UrlDecode(Request.Form[key])));
            }
            if (list.Count > 0)
            {
                ArgResponse ar = new ArgResponse();
                ar.Args = (Arg[])list.ToArray(typeof(Arg));
                ar.List = true;

                error.Args = ar;
            }
            string responseStr = string.Empty;
            if (format == "json")
            {
                Response.ContentType = "text/html";
                error.ArgArrary = error.Args.Args;
                responseStr = JavaScriptConvert.SerializeObject(error);
            }
            else
            {
                responseStr = SerializationHelper.Serialize(error);
            }

            Response.Write(responseStr);
            Response.End();

        }

        /// <summary>
        /// 获取API提交的参数
        /// </summary>
        /// <param name="request">request对象</param>
        /// <returns>参数数组</returns>
        private DNTParam[] GetParamsFromRequest(HttpRequest request)
        {
            List<DNTParam> list = new List<DNTParam>();
            foreach (string key in request.QueryString.AllKeys)
            {
                list.Add(DNTParam.Create(key, request.QueryString[key]));
            }
            foreach (string key in request.Form.AllKeys)
            {
                list.Add(DNTParam.Create(key, request.Form[key]));
            }
            list.Sort();
            return list.ToArray();
        }

        /// <summary>
        /// 根据参数和密码生成签名字符串
        /// </summary>
        /// <param name="parameters">API参数</param>
        /// <param name="secret">密码</param>
        /// <returns>签名字符串</returns>
        private string GetSignature(DNTParam[] parameters, string secret)
        {
            StringBuilder values = new StringBuilder();

            foreach (DNTParam param in parameters)
            {
                if (param.Name == "sig" || string.IsNullOrEmpty(param.Value))
                    continue;
                values.Append(param.ToString());
            }
            values.Append(secret);

            byte[] md5_result = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(values.ToString()));

            StringBuilder sig_builder = new StringBuilder();

            foreach (byte b in md5_result)
                sig_builder.Append(b.ToString("x2"));

            return sig_builder.ToString();
        }

        #endregion
    }
}
