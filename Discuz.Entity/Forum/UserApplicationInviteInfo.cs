using System;
using System.Collections.Generic;
using System.Text;

namespace Discuz.Entity
{
    public class UserApplicationInviteInfo
    {
        #region 成员变量

        private int id;
        private string typeName;
        private int appId;
        private int type;
        private int fromUid;
        private int toUid;
        private string myml;
        private int hash;
        private string createDateTime;

        #endregion

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public string TypeName
        {
            get { return typeName; }
            set { typeName = value; }
        }

        public int AppId
        {
            get { return appId; }
            set { appId = value; }
        }

        public int Type
        {
            get { return type; }
            set { type = value; }
        }

        public int FromUid
        {
            get { return fromUid; }
            set { fromUid = value; }
        }

        public int ToUid
        {
            get { return toUid; }
            set { toUid = value; }
        }

        public string MYML
        {
            get { return myml; }
            set { myml = value; }
        }

        public int Hash
        {
            get { return hash; }
            set { hash = value; }
        }

        public string CreateDateTime
        {
            get { return createDateTime; }
            set { createDateTime = value; }
        }

    }
}
