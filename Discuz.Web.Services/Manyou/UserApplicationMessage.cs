using System;
using System.Collections.Generic;
using System.Text;

namespace Discuz.Web.Services.Manyou
{
    public class UserApplicationMessage
    {
        private int appId;

        public int AppId
        {
            get { return appId; }
            set { appId = value; }
        }
        private string privacy;

        /// <summary>
        /// 应用隐私设置(0:none;1:me;2:friends;3:public)
        /// </summary>
        public string Privacy
        {
            get { return privacy; }
            set { privacy = value; }
        }
        private bool allowSideNav;

        public bool AllowSideNav
        {
            get { return allowSideNav; }
            set { allowSideNav = value; }
        }
        private bool allowFeed;

        public bool AllowFeed
        {
            get { return allowFeed; }
            set { allowFeed = value; }
        }
        private bool allowProfileLink;

        public bool AllowProfileLink
        {
            get { return allowProfileLink; }
            set { allowProfileLink = value; }
        }
        private int displayOrder = 0;

        public int DisplayOrder
        {
            get { return displayOrder; }
            set { displayOrder = value; }
        }
    }
}
