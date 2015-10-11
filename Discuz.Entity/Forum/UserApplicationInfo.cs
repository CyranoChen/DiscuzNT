using System;
using System.Collections.Generic;
using System.Text;

namespace Discuz.Entity
{
    public class UserApplicationInfo
    {
        private int appId;

        public int AppId
        {
            get { return appId; }
            set { appId = value; }
        }
        private int uid;

        public int Uid
        {
            get { return uid; }
            set { uid = value; }
        }
        private string appName;

        public string AppName
        {
            get { return appName; }
            set { appName = value; }
        }
        private PrivacyEnum privacy;

        /// <summary>
        /// 应用隐私设置(0:none;1:me;2:friends;3:public)
        /// </summary>
        public PrivacyEnum Privacy
        {
            get { return privacy; }
            set { privacy = value; }
        }
        private int allowSideNav = 0;

        public int AllowSideNav
        {
            get { return allowSideNav; }
            set { allowSideNav = value; }
        }
        private int allowFeed = 0;

        public int AllowFeed
        {
            get { return allowFeed; }
            set { allowFeed = value; }
        }
        private int allowProfileLink = 0;

        public int AllowProfileLink
        {
            get { return allowProfileLink; }
            set { allowProfileLink = value; }
        }
        private int narrow = 0;

        public int Narrow
        {
            get { return narrow; }
            set { narrow = value; }
        }
        private int displayOrder = 0;

        public int DisplayOrder
        {
            get { return displayOrder; }
            set { displayOrder = value; }
        }
        private int menuOrder = 0;

        public int MenuOrder
        {
            get { return menuOrder; }
            set { menuOrder = value; }
        }
        private string profileLink = "";

        public string ProfileLink
        {
            get { return profileLink; }
            set { profileLink = value; }
        }
        private string myml = "";

        public string MYML
        {
            get { return myml; }
            set { myml = value; }
        }
    }


    public enum PrivacyEnum
    {
        /// <summary>
        /// 不可见
        /// </summary>
        None = 0,
        /// <summary>
        /// 自己可见
        /// </summary>
        Me = 1,
        /// <summary>
        /// 好友可见
        /// </summary>
        Friends = 2,
        /// <summary>
        /// 所有人都可见 
        /// </summary>
        Public = 3

    }
}
