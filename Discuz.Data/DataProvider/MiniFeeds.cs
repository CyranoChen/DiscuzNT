using System.Data;

using Discuz.Common;
using Discuz.Common.Generic;
using Discuz.Entity;

namespace Discuz.Data
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
            return DatabaseProvider.GetInstance().PublishFeed(feedInfo);
        }

        /// <summary>
        /// 获取用户的feed
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static List<MiniFeedInfo> GetUserFeeds(int uid, int pageIndex)
        {
            IDataReader reader = DatabaseProvider.GetInstance().GetUserFeeds(uid, pageIndex);

            List<MiniFeedInfo> feedList = new List<MiniFeedInfo>();

            while (reader.Read())
                feedList.Add(LoadSingleFeedInfo(reader));

            reader.Close();

            return feedList;
        }

        /// <summary>
        /// 加载feed信息
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private static MiniFeedInfo LoadSingleFeedInfo(IDataReader reader)
        {
            MiniFeedInfo feedInfo = new MiniFeedInfo();

            feedInfo.FeedId = TypeConverter.ObjectToInt(reader["feedid"]);
            feedInfo.Uid = TypeConverter.ObjectToInt(reader["uid"]);
            feedInfo.FeedType = (FeedTypeEnum)TypeConverter.ObjectToInt(reader["feedtype"]);
            feedInfo.AppId = TypeConverter.ObjectToInt(reader["appid"]);
            feedInfo.UserName = reader["username"].ToString();
            feedInfo.DateTime = reader["datetime"].ToString();
            feedInfo.TitleTemplate = reader["titletemplate"].ToString();
            feedInfo.TitleData = reader["titledata"].ToString();
            feedInfo.BodyTemplate = reader["bodytemplate"].ToString();
            feedInfo.BodyData = reader["bodydata"].ToString();
            feedInfo.BodyGeneral = reader["bodygeneral"].ToString();
            feedInfo.Image1Url = reader["image1"].ToString();
            feedInfo.Image1Link = reader["image1link"].ToString();
            feedInfo.Image2Url = reader["image2"].ToString();
            feedInfo.Image2Link = reader["image2link"].ToString();
            feedInfo.Image3Url = reader["image3"].ToString();
            feedInfo.Image3Link = reader["image3link"].ToString();
            feedInfo.Image4Url = reader["image4"].ToString();
            feedInfo.Image4Link = reader["image4link"].ToString();

            return feedInfo;
        }
    }
}
