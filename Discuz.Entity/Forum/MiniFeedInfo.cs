using System;
using System.Collections.Generic;
using System.Text;

namespace Discuz.Entity
{
    public class MiniFeedInfo
    {
        private int feedId;

        /// <summary>
        /// 
        /// </summary>
        public int FeedId
        {
            get { return feedId; }
            set { feedId = value; }
        }

        private int uid;

        /// <summary>
        /// 
        /// </summary>
        public int Uid
        {
            get { return uid; }
            set { uid = value; }
        }

        private FeedTypeEnum feedType;

        /// <summary>
        /// Feed类型
        /// </summary>
        public FeedTypeEnum FeedType
        {
            get { return feedType; }
            set { feedType = value; }
        }

        private int appId;

        /// <summary>
        /// 应用id，如果FeedType不是应用，则为0
        /// </summary>
        public int AppId
        {
            get { return appId; }
            set { appId = value; }
        }

        private string userName;

        /// <summary>
        /// 
        /// </summary>
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        private string dateTime;
        /// <summary>
        /// 
        /// </summary>
        public string DateTime
        {
            get { return dateTime; }
            set { dateTime = value; }
        }

        private string titleTemplate;

        /// <summary>
        /// 标题模板
        /// </summary>
        public string TitleTemplate
        {
            get { return titleTemplate; }
            set { titleTemplate = value; }
        }

        private string titleData;

        /// <summary>
        /// 标题参数和值
        /// </summary>
        public string TitleData
        {
            get { return titleData; }
            set { titleData = value; }
        }

        private string bodyTemplate;

        /// <summary>
        /// 正文模板
        /// </summary>
        public string BodyTemplate
        {
            get { return bodyTemplate; }
            set { bodyTemplate = value; }
        }

        private string bodyData;

        /// <summary>
        /// 正文参数和值
        /// </summary>
        public string BodyData
        {
            get { return bodyData; }
            set { bodyData = value; }
        }

        private string bodyGeneral;

        /// <summary>
        /// 用户附加内容
        /// </summary>
        public string BodyGeneral
        {
            get { return bodyGeneral; }
            set { bodyGeneral = value; }
        }

        private string image1Url;

        /// <summary>
        /// 图片1地址
        /// </summary>
        public string Image1Url
        {
            get { return image1Url; }
            set { image1Url = value; }
        }

        private string image1Link;

        /// <summary>
        /// 图片1链接地址
        /// </summary>
        public string Image1Link
        {
            get { return image1Link; }
            set { image1Link = value; }
        }

        private string image2Url;

        /// <summary>
        /// 图片2地址
        /// </summary>
        public string Image2Url
        {
            get { return image2Url; }
            set { image2Url = value; }
        }

        private string image2Link;

        /// <summary>
        /// 图片2链接地址
        /// </summary>
        public string Image2Link
        {
            get { return image2Link; }
            set { image2Link = value; }
        }

        private string image3Url;

        /// <summary>
        /// 图片3地址
        /// </summary>
        public string Image3Url
        {
            get { return image3Url; }
            set { image3Url = value; }
        }

        private string image3Link;

        /// <summary>
        /// 图片3链接地址
        /// </summary>
        public string Image3Link
        {
            get { return image3Link; }
            set { image3Link = value; }
        }

        private string image4Url;

        /// <summary>
        /// 图片4地址
        /// </summary>
        public string Image4Url
        {
            get { return image4Url; }
            set { image4Url = value; }
        }

        private string image4Link;

        /// <summary>
        /// 图片4链接地址
        /// </summary>
        public string Image4Link
        {
            get { return image4Link; }
            set { image4Link = value; }
        }
    }

    public enum FeedTypeEnum
    {
        /// <summary>
        /// 发帖
        /// </summary>
        Post = 1,
        /// <summary>
        /// 回帖
        /// </summary>
        Reply = 2,
        /// <summary>
        /// 投票
        /// </summary>
        Vote = 3,
        /// <summary>
        /// 辩论
        /// </summary>
        Debate = 4,
        /// <summary>
        /// 悬赏
        /// </summary>
        Reward = 5,
        /// <summary>
        /// 应用
        /// </summary>
        Application = 6
    }
}
