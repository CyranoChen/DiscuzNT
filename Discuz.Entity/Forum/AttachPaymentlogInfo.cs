using System;
using Newtonsoft.Json;

namespace Discuz.Entity
{
    /// <summary>
    /// 附件交易日志
    /// </summary>
    public class AttachPaymentlogInfo
    {
        private int _id;
        /// <summary>
        /// 购买者 uid
        /// </summary>
        [JsonProperty("id")] 
        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        private int _uid;
        /// <summary>
        /// 购买者 uid
        /// </summary>
        [JsonProperty("uid")]
        public int Uid
        {
            get
            {
                return _uid;
            }
            set
            {
                _uid = value;
            }
        }

        private string _username;
        /// <summary>
        /// 购买者用户名
        /// </summary>
        [JsonProperty("username")]
        public string UserName
        {
            get
            {
                return _username;
            }
            set
            {
                _username = value;
            }
        }


        private int _aid;
        /// <summary>
        /// 被购买的附件 aid
        /// </summary>
        [JsonProperty("aid")]
        public int Aid
        {
            get
            {
                return _aid;
            }
            set
            {
                _aid = value;
            }
        }

        private int _authorid;
        /// <summary>
        /// 附件所属主题作者的 uid
        /// </summary>
        [JsonProperty("authorid")]
        public int Authorid
        {
            get
            {
                return _authorid;
            }
            set
            {
                _authorid = value;
            }
        }

        private DateTime _postdatetime;
        /// <summary>
        /// 购买日期Json字符串
        /// </summary>
        [JsonProperty("postdatetime")]
        public string PostDateTimeString
        {
            get { return _postdatetime.ToString("yyyy-MM-dd HH:mm:ss"); }
        }

        /// <summary>
        /// 购买日期
        /// </summary>
        [JsonIgnore]
        public DateTime PostDateTime
        {
            get
            {
                return _postdatetime;
            }
            set
            {
                _postdatetime = value;
            }
        }

        private int _amount;
        /// <summary>
        /// 售价
        /// </summary>
        [JsonProperty("amount")]
        public int Amount
        {
            get
            {
                return _amount;
            }
            set
            {
                _amount = value;
            }
        }

        private int _netamount;
        /// <summary>
        /// 作者所得(去除了积分交易税)
        /// </summary>
        [JsonProperty("netamount")]
        public int NetAmount
        {
            get
            {
                return _netamount;
            }
            set
            {
                _netamount = value;
            }
        }
    }
}
