using System;
using System.Collections.Generic;
using System.Text;
using Discuz.Common;
using Discuz.Config;
using System.Security.Cryptography;

namespace Discuz.Forum
{
    public class Sync
    {
        //private const string UC_VERSION = "1.5.0";
        //private const string API_DELETEUSER = "1";
        //private const string API_RENAMEUSER = "1";
        //private const string API_GETTAG = "1";
        //private const string API_SYNLOGIN = "1";
        //private const string API_SYNLOGOUT = "1";
        //private const string API_UPDATEPW = "1";
        //private const string API_UPDATEBADWORDS = "1";
        //private const string API_UPDATEHOSTS = "1";
        //private const string API_UPDATEAPPS = "1";
        //private const string API_UPDATECLIENT = "1";
        //private const string API_UPDATECREDIT = "1";
        //private const string API_GETCREDITSETTINGS = "1";
        //private const string API_UPDATECREDITSETTINGS = "1";
        //private const string API_RETURN_SUCCEED = "1";
        //private const string API_RETURN_FAILED = "-1";
        //private const string API_RETURN_FORBIDDEN = "-2";

        private const string ASYNC_LOGIN = "login";
        private const string ASYNC_LOGOUT = "logout";
        private const string ASYNC_REGISTER = "register";
        private const string ASYNC_DELETE_USER = "deleteuser";
        private const string ASYNC_RENAME_USER = "renameuser";
        private const string ASYNC_UPDATE_PASSWORD = "updatepwd";
        private const string ASYNC_UPDATE_CREDITS = "updatecredits";
        private const string ASYNC_UPDATE_SIGNATURE = "updatesignature";
        private const string ASYNC_UPDATE_PROFILE = "updateprofile";
        private const string ASYNC_NEW_TOPIC = "newtopic";
        private const string ASYNC_REPLY = "reply";
        private const string ASYNC_TEST = "test";

        /// <summary>
        /// 获得同步登录脚本
        /// </summary>
        /// <param name="uId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static string GetLoginScript(int uId, string userName)
        {
            StringBuilder builder = new StringBuilder();
            ApplicationInfoCollection appCollection = GetAsyncTarget(ASYNC_LOGIN);
            List<DiscuzParam> paramList = new List<DiscuzParam>();
            paramList.Add(DiscuzParam.Create("uid", uId));
            paramList.Add(DiscuzParam.Create("user_name", userName));
            foreach (ApplicationInfo appInfo in appCollection)
            {
                builder.AppendFormat("<script src=\"{0}\" reload=\"1\"></script>", GetUrl(appInfo.SyncUrl, appInfo.Secret, ASYNC_LOGIN, paramList.ToArray()));
            }
            return builder.ToString();
        }

        /// <summary>
        /// 获得同步退出登录脚本
        /// </summary>
        /// <param name="uId"></param>
        /// <returns></returns>
        public static string GetLogoutScript(int uId)
        {
            StringBuilder builder = new StringBuilder();
            ApplicationInfoCollection appCollection = GetAsyncTarget(ASYNC_LOGOUT);
            List<DiscuzParam> paramList = new List<DiscuzParam>();
            paramList.Add(DiscuzParam.Create("uid", uId));
            foreach (ApplicationInfo appInfo in appCollection)
            {
                builder.AppendFormat("<script src=\"{0}\" reload=\"1\"></script>", GetUrl(appInfo.SyncUrl, appInfo.Secret, ASYNC_LOGOUT, paramList.ToArray()));
            }
            return builder.ToString();
        }

        /// <summary>
        /// 测试消息同步
        /// </summary>
        /// <param name="asyncUrl"></param>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        public static string Test(string asyncUrl)
        {
            return Utils.GetHttpWebResponse(string.Format("{0}?action={1}", asyncUrl, ASYNC_TEST));
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="uId">用户ID</param>
        /// <param name="userName">用户名</param>
        /// <param name="password">跟数据库一致的密码</param>
        public static void UserRegister(int uId, string userName, string password, string apiKey)
        {
            List<DiscuzParam> paramList = new List<DiscuzParam>();
            paramList.Add(DiscuzParam.Create("uid", uId));
            paramList.Add(DiscuzParam.Create("user_name", userName));
            paramList.Add(DiscuzParam.Create("password", password));
            SendRequest(ASYNC_REGISTER, paramList.ToArray(),apiKey);
        }

        /// <summary>
        /// 删除用户同步
        /// </summary>
        /// <param name="uIds">逗号分隔的uid列表字符串</param>
        /// <param name="apiKey">操作的应用站点id,论坛调用请添空字符串</param>
        /// <returns></returns>
        public static void DeleteUsers(string uIds, string apiKey)
        {
            List<DiscuzParam> paramList = new List<DiscuzParam>();
            paramList.Add(DiscuzParam.Create("uids", uIds));

            SendRequest(ASYNC_DELETE_USER, paramList.ToArray(), apiKey);
        }

        /// <summary>
        /// 修改用户名同步
        /// </summary>
        /// <param name="uId"></param>
        /// <param name="oldUserName"></param>
        /// <param name="newUserName"></param>
        /// <param name="apiKey">操作的应用站点id,论坛调用请添空字符串</param>
        /// <returns></returns>
        public static void RenameUser(int uId, string oldUserName, string newUserName, string apiKey)
        {
            List<DiscuzParam> paramList = new List<DiscuzParam>();
            paramList.Add(DiscuzParam.Create("uid", uId));
            paramList.Add(DiscuzParam.Create("old_user_name", oldUserName));
            paramList.Add(DiscuzParam.Create("new_user_name", newUserName));
            SendRequest(ASYNC_RENAME_USER, paramList.ToArray(), apiKey);
        }

        /// <summary>
        /// 更新密码同步
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="apiKey">操作的应用站点id,论坛调用请添空字符串</param>
        /// <returns></returns>
        public static void UpdatePassword(string userName, string password, string apiKey)
        {
            List<DiscuzParam> paramList = new List<DiscuzParam>();
            paramList.Add(DiscuzParam.Create("user_name", userName));
            paramList.Add(DiscuzParam.Create("password", password));
            SendRequest(ASYNC_UPDATE_PASSWORD, paramList.ToArray(), apiKey);
        }

        /// <summary>
        /// 更新积分同步
        /// </summary>
        /// <param name="uId"></param>
        /// <param name="creditIndex">扩展积分序号</param>
        /// <param name="amount"></param>
        /// <param name="apiKey">操作的应用站点id,论坛调用请添空字符串</param>
        /// <returns></returns>
        public static void UpdateCredits(int uId, int creditIndex, string amount, string apiKey)
        {
            List<DiscuzParam> paramList = new List<DiscuzParam>();
            paramList.Add(DiscuzParam.Create("uid", uId));
            paramList.Add(DiscuzParam.Create("credit_index", creditIndex.ToString()));
            paramList.Add(DiscuzParam.Create("amount", amount));
            SendRequest(ASYNC_UPDATE_CREDITS, paramList.ToArray(), apiKey);
        }

        /// <summary>
        /// 更新签名同步
        /// </summary>
        /// <param name="uId"></param>
        /// <param name="signature"></param>
        /// <param name="apiKey">操作的应用站点id,论坛调用请添空字符串</param>
        /// <returns></returns>
        public static void UpdateSignature(int uId, string userName, string signature, string apiKey)
        {
            List<DiscuzParam> paramList = new List<DiscuzParam>();
            paramList.Add(DiscuzParam.Create("uid", uId));
            paramList.Add(DiscuzParam.Create("user_name", userName));
            paramList.Add(DiscuzParam.Create("signature", signature));
            SendRequest(ASYNC_UPDATE_SIGNATURE, paramList.ToArray(), apiKey);
        }

        /// <summary>
        /// 更新用户资料同步
        /// </summary>
        /// <param name="uId"></param>
        /// <param name="signature"></param>
        /// <param name="apiKey">操作的应用站点id,论坛调用请添空字符串</param>
        /// <returns></returns>
        public static void UpdateProfile(int uId, string userName, string apiKey)
        {
            List<DiscuzParam> paramList = new List<DiscuzParam>();
            paramList.Add(DiscuzParam.Create("uid", uId));
            paramList.Add(DiscuzParam.Create("user_name", userName));
            SendRequest(ASYNC_UPDATE_PROFILE, paramList.ToArray(), apiKey);
        }

        /// <summary>
        /// 新主题同步
        /// </summary>
        /// <param name="topicId">主题id</param>
        /// <param name="title">标题</param>
        /// <param name="author">作者</param>
        /// <param name="authorId">作者uid</param>
        /// <param name="fid">版块id</param>
        /// <param name="apiKey">操作的应用站点id,论坛调用请添空字符串</param>
        public static void NewTopic(string topicId, string title, string author, string authorId, string fid, string apiKey)
        {
            List<DiscuzParam> paramList = new List<DiscuzParam>();
            paramList.Add(DiscuzParam.Create("tid", topicId));
            paramList.Add(DiscuzParam.Create("title", title));
            paramList.Add(DiscuzParam.Create("author", author));
            paramList.Add(DiscuzParam.Create("author_id", authorId));
            paramList.Add(DiscuzParam.Create("fid", fid));
            SendRequest(ASYNC_NEW_TOPIC, paramList.ToArray(), apiKey);
        }

        /// <summary>
        /// 回复同步
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="topicId"></param>
        /// <param name="topicTitle"></param>
        /// <param name="poster"></param>
        /// <param name="posterId"></param>
        /// <param name="fid"></param>
        /// <param name="apiKey">操作的应用站点id,论坛调用请添空字符串</param>
        public static void Reply(string postId, string topicId, string topicTitle, string poster, string posterId, string fid,string apiKey)
        {
            List<DiscuzParam> paramList = new List<DiscuzParam>();
            paramList.Add(DiscuzParam.Create("pid", postId));
            paramList.Add(DiscuzParam.Create("tid", topicId));
            paramList.Add(DiscuzParam.Create("topic_title", topicTitle));
            paramList.Add(DiscuzParam.Create("poster", poster));
            paramList.Add(DiscuzParam.Create("poster_id", posterId));
            paramList.Add(DiscuzParam.Create("fid", fid));
            SendRequest(ASYNC_REPLY, paramList.ToArray(),apiKey);
        }

        /// <summary>
        /// 判断是否需要同步登录
        /// </summary>
        /// <returns></returns>
        public static bool NeedAsyncLogin()
        {
            return GetAsyncTarget(ASYNC_LOGIN).Count > 0;
        }

        /// <summary>
        /// 判断是否需要同步登出
        /// </summary>
        /// <returns></returns>
        public static bool NeedAsyncLogout()
        {
            return GetAsyncTarget(ASYNC_LOGOUT).Count > 0;
        }

        /// <summary>
        /// 发送请求
        /// </summary>
        /// <param name="action"></param>
        /// <param name="data"></param>
        private static void SendRequest(string action, DiscuzParam[] data, string apiKey)
        {
            ApplicationInfoCollection appCollection = GetAsyncTarget(action);
            foreach (ApplicationInfo appInfo in appCollection)
            {
                if (appInfo.APIKey != apiKey)
                    new ProcessAsync(GetUrl(appInfo.SyncUrl, appInfo.Secret, action, data)).Enqueue();
            }
        }

        /// <summary>
        /// 将参数绑定到url
        /// </summary>
        /// <param name="asyncUrl"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private static string GetUrl(string asyncUrl, string secret, string action, DiscuzParam[] parameters)
        {
            List<DiscuzParam> list = new List<DiscuzParam>(parameters);
            list.Add(DiscuzParam.Create("time", DiscuzMethodes.Time()));
            list.Add(DiscuzParam.Create("action", action));
            list.Sort();

            StringBuilder values = new StringBuilder();

            foreach (DiscuzParam param in list)
            {
                if (!string.IsNullOrEmpty(param.Value))
                    values.Append(param.ToString());
            }

            values.Append(secret);

            byte[] md5_result = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(values.ToString()));

            StringBuilder sig_builder = new StringBuilder();

            foreach (byte b in md5_result)
                sig_builder.Append(b.ToString("x2"));

            list.Add(DiscuzParam.Create("sig", sig_builder.ToString()));

            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < list.Count; i++)
            {
                if (i > 0)
                    builder.Append("&");

                builder.Append(list[i].ToEncodedString());
            }


            return string.Format("{0}?{1}", asyncUrl, builder.ToString());
        }

        /// <summary>
        /// 获取需要数据同步的应用程序列表
        /// </summary>
        /// <param name="actionName"></param>
        /// <returns></returns>
        private static ApplicationInfoCollection GetAsyncTarget(string action)
        {
            ApplicationInfoCollection appCollection = new ApplicationInfoCollection();
            APIConfigInfo apiInfo = APIConfigs.GetConfig();
            if (!apiInfo.Enable)
                return appCollection;
            foreach (ApplicationInfo appInfo in apiInfo.AppCollection)
            {
                if (appInfo.SyncMode == 1 || (appInfo.SyncMode == 2 && Utils.InArray(action, appInfo.SyncList)))
                {
                    if (appInfo.SyncUrl.Trim() == string.Empty)
                        continue;
                    appCollection.Add(appInfo);
                }
            }
            return appCollection;
        }

    }

    /// <summary>
    /// 线程管理发送数据同步请求
    /// </summary>
    public class ProcessAsync
    {
        public ProcessAsync(string url)
        {
            _url = url;
            //_postData = postData;
        }

        protected string _url;
        //protected string _postData;

        /// <summary>
        /// 执行统计操作
        /// </summary>
        public void Enqueue()
        {
            ManagedThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(Process));
        }

        /// <summary>
        /// 处理当前操作
        /// </summary>
        /// <param name="state"></param>
        private void Process(object state)
        {
            try
            {
                string result = Utils.GetHttpWebResponse(this._url); //成功1,失败-1,禁止-2
            }
            catch //(Exception ex)
            {
                //log failed requests
            }
        }
    }

}
