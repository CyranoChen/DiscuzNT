using System;
using System.Collections.Generic;
using System.Text;

namespace Discuz.Entity
{
    public class ApplicationInviteInfo
    {
        private int id = 0;
        private string typeName = "";
        private int appId = 0;
        private int type = 0;
        private int fromUid = 0;
        private int toUid = 0;
        private string myml = "";
        private string dateTime = "";
        private int hash = 0;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public string TypeName
        {
            get { return typeName.Trim(); }
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

        public string DateTime
        {
            get { return dateTime; }
            set { dateTime = value; }
        }

        public int Hash
        {
            get { return hash; }
            set { hash = value; }
        }


    }
}
