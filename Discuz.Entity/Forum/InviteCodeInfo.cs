using System;

namespace Discuz.Entity
{
    public class InviteCodeInfo
    {
        private int m_inviteid;//邀请码id
        private string m_invitecode;//邀请码
        private int m_creatorid;//创建者id
        private string m_creator;//创建者username
        private int m_successcount;//已使用次数
        private string m_createtime;//创建日期
        private string m_expiretime;//失效日期
        private int m_maxcount;//最大使用次数
        private int m_invitetype;//邀请码类型（2为开放式，3为封闭式）


        public int InviteId
        {
            get { return m_inviteid; }
            set { m_inviteid = value; }
        }

        public string Code
        {
            get { return m_invitecode; }
            set { m_invitecode = value; }
        }

        public int CreatorId
        {
            get { return m_creatorid; }
            set { m_creatorid = value; }
        }

        public string Creator
        {
            get { return m_creator; }
            set { m_creator = value; }
        }

        public int SuccessCount
        {
            get { return m_successcount; }
            set { m_successcount = value; }
        }

        public string CreateTime
        {
            get { return m_createtime; }
            set { m_createtime = value; }
        }

        public string ExpireTime
        {
            get { return m_expiretime; }
            set { m_expiretime = value; }
        }

        public int MaxCount
        {
            get { return m_maxcount; }
            set { m_maxcount = value; }
        }

        public int InviteType
        {
            get { return m_invitetype; }
            set { m_invitetype = value; }
        }
    }
}
