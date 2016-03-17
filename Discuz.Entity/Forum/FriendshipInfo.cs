using System;

namespace Discuz.Entity
{
    /// <summary>
    /// 好友关系请求信息
    /// </summary>
    public class FriendshipRequestInfo
    {
        private int fromUid;
        private int toUid;
        private string fromUserName;
        private int groupId;
        private string note;
        private string dateTime;

        /// <summary>
        /// 好友关系发起人ID
        /// </summary>
        public int FromUid
        {
            get { return fromUid; }
            set { fromUid = value; }
        }

        /// <summary>
        /// 好友关系接收人ID
        /// </summary>
        public int ToUid
        {
            get { return toUid; }
            set { toUid = value; }
        }

        /// <summary>
        /// 好友关系发起人用户名
        /// </summary>
        public string FromUserName
        {
            get { return fromUserName; }
            set { fromUserName = value; }
        }

        /// <summary>
        /// 好友关系分组ID
        /// </summary>
        public int GroupId
        {
            get { return groupId; }
            set { groupId = value; }
        }

        /// <summary>
        /// 好友关系描述
        /// </summary>
        public string Note
        {
            get { return note.Trim(); }
            set { note = value; }
        }

        /// <summary>
        /// 发起时间
        /// </summary>
        public string DateTime
        {
            get { return dateTime; }
            set { dateTime = value; }
        }
    }

    /// <summary>
    /// 好友关系信息
    /// </summary>
    public class FriendshipInfo
    {
        private int uid;
        private int friendUid;
        private string friendUserName;
        private int groupId;
        private string dateTime;
        private int exchangeNum;

        /// <summary>
        /// 用户ID
        /// </summary>
        public int Uid
        {
            get { return uid; }
            set { uid = value; }
        }

        /// <summary>
        /// 好友ID
        /// </summary>
        public int FriendUid
        {
            get { return friendUid; }
            set { friendUid = value; }
        }

        /// <summary>
        /// 好友用户名
        /// </summary>
        public string FriendUserName
        {
            get { return friendUserName.Trim(); }
            set { friendUserName = value; }
        }

        /// <summary>
        /// 好友分类
        /// </summary>
        public int GroupId
        {
            get { return groupId; }
            set { groupId = value; }
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string DateTime
        {
            get { return dateTime.Trim(); }
            set { dateTime = value; }
        }

        /// <summary>
        /// 好友活动关系数
        /// </summary>
        public int ExchangeNum
        {
            get { return exchangeNum; }
            set { exchangeNum = value; }
        }

    }

    /// <summary>
    /// 好友关系动作日志信息
    /// </summary>
    public class FriendshipActionInfo
    {
        private int uid;
        private int friendUid;
        private FriendshipActionEnum action;

        /// <summary>
        /// 用户ID
        /// </summary>
        public int Uid
        {
            get { return uid; }
            set { uid = value; }
        }

        /// <summary>
        /// 好友ID
        /// </summary>
        public int FriendUid
        {
            get { return friendUid; }
            set { friendUid = value; }
        }

        /// <summary>
        /// 动作(add or delete)
        /// </summary>
        public FriendshipActionEnum Action
        {
            get { return action; }
            set { action = value; }
        }
    }

    public enum FriendshipActionEnum
    {
        /// <summary>
        /// 增加
        /// </summary>
        Add,
        /// <summary>
        /// 删除
        /// </summary>
        Delete
    }

    public enum FriendshipListSerachEnum
    {
        /// <summary>
        /// 用户名
        /// </summary>
        FriendUserName,
        /// <summary>
        /// 指定时间之前建立的好友关系
        /// </summary>
        LastDateTime,
        /// <summary>
        /// 好友组别
        /// </summary>
        FriendGroupId
    }

    public enum CreateNewFriendshipRequestEnum
    {
        /// <summary>
        /// 发起人好友个数超过上限
        /// </summary>
        UserFriendshipOverflow = -3,
        /// <summary>
        /// 请求信息错误，创建失败
        /// </summary>
        MessageError = -2,
        /// <summary>
        /// 好友关系已存在
        /// </summary>
        FriendshipAlreadyExists = -1,
        /// <summary>
        /// 请求关系已存在
        /// </summary>
        RequestAlreadyExists = 0,
        /// <summary>
        /// 成功
        /// </summary>
        Success = 1
    }

    public enum PassFriendshipEnum
    {
        FromUserFriendshipOverflow = -4,
        /// <summary>
        /// 被请求人好友数量超过上限
        /// </summary>
        ToUserFriendshipOverflow = -3,
        /// <summary>
        /// 请求信息错误，创建失败
        /// </summary>
        MessageError = -2,
        /// <summary>
        /// 好友关系已存在
        /// </summary>
        FriendshipAlreadyExists = -1,
        /// <summary>
        /// 成功
        /// </summary>
        Success = 1
    }

    public enum IsFriendshipExistEnum
    {
        /// <summary>
        /// 存在
        /// </summary>
        Exist = 2,
        /// <summary>
        /// 请求存在
        /// </summary>
        RequestExist = 1,
        /// <summary>
        /// 不存在
        /// </summary>
        NotExist = 0
    }

    public class FriendshipGroupInfo
    {
        private int groupId;

        /// <summary>
        /// 好友分组ID
        /// </summary>
        public int GroupId
        {
            get { return groupId; }
            set { groupId = value; }
        }

        private string groupName;

        /// <summary>
        /// 好友分组名称
        /// </summary>
        public string GroupName
        {
            get { return groupName.Trim(); }
            set { groupName = value; }
        }

        private int ownerId;

        /// <summary>
        /// 所有者用户ID
        /// </summary>
        public int OwnerId
        {
            get { return ownerId; }
            set { ownerId = value; }
        }

        private int friendshipCount;

        /// <summary>
        /// 组中的好友个数
        /// </summary>
        public int FriendshipCount
        {
            get { return friendshipCount; }
            set { friendshipCount = value; }
        }
    }


}
