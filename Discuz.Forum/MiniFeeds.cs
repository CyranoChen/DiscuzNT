using Discuz.Common.Generic;
using Discuz.Entity;

using Newtonsoft.Json;

namespace Discuz.Forum
{
    public class MiniFeeds
    {
        /// <summary>
        /// 发布feed信息
        /// </summary>
        /// <param name="feedInfo"></param>
        /// <returns></returns>
        public static int PublishFeed(MiniFeedInfo feedInfo)
        {
            if (feedInfo.Uid <= 0)
                return 0;
            return Data.MiniFeeds.PublishFeed(feedInfo);
        }

        /// <summary>
        /// 获取用户的feed
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static List<MiniFeedInfo> GetUserFeeds(int uid,int pageIndex)
        {
            if (uid <= 0)
                return new List<MiniFeedInfo>();

            return Data.MiniFeeds.GetUserFeeds(uid,pageIndex);
        }

        /// <summary>
        /// 绑定feed模板中的变量值
        /// </summary>
        /// <param name="template"></param>
        /// <param name="dataJson"></param>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public static string MergeFeedTemplateData(string template, string dataJson, UserInfo userInfo)
        {
            Dictionary<string, string> dataList = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(dataJson))
                dataList = JavaScriptConvert.DeserializeObject<Dictionary<string, string>>(dataJson);

            string actorData = string.Format("<a href=\"{0}\">{1}</a>", Urls.UserInfoAspxRewrite(userInfo.Uid), userInfo.Username);
            dataList.Add("actor", actorData);

            foreach (string key in dataList.Keys)
            {
                template = template.Replace("{" + key + "}", dataList[key]);
            }

            return template;
        }
    }
}
