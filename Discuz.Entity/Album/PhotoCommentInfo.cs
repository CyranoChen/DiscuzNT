using System;
using System.Text;

namespace Discuz.Entity
{
    public class PhotoCommentInfo
    {
        private int _commentID;
        /// <summary>
        /// ��Ƭ����ID
        /// </summary>
        public int Commentid
        {
            get { return _commentID; }
            set { _commentID = value; }
        }

        /// <summary>
        /// ����ID
        /// </summary>
        private int _postID;
        public int Photoid
        {
            get { return _postID; }
            set { _postID = value; }
        }
        /// <summary>
        /// �û���ID
        /// </summary>
        private string _username;
        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }
        /// <summary>
        /// �û�ID
        /// </summary>
        private int _userid;
        public int Userid
        {
            get { return _userid; }
            set { _userid = value; }
        }
        /// <summary>
        /// �û�IP
        /// </summary>
        private string _ip;
        public string Ip
        {
            get { return _ip; }
            set { _ip = value; }
        }

        /// <summary>
        /// ����ʱ��
        /// </summary>
        private DateTime _postDateTime;
        public DateTime Postdatetime
        {
            get { return _postDateTime; }
            set { _postDateTime = value; }
        }
        /// <summary>
        /// ��������
        /// </summary>
        private string _content;
        public string Content
        {
            get { return _content; }
            set { _content = value; }
        }
        /// <summary>
        /// �ϼ�����
        /// </summary>
        private int _parentID;
        public int Parentid
        {
            get { return _parentID; }
            set { _parentID = value; }
        }��������
    }
}
