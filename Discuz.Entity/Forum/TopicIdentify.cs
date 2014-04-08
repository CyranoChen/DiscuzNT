using System;

namespace Discuz.Entity
{
    /// <summary>
    /// 主题签定描述类
    /// </summary>
    [Serializable]
    public class TopicIdentify
    {
        private int m_identifyid = 0;	
        private string m_name = string.Empty;      
        private string m_filename = string.Empty;  
        //主题签定id
        public int Identifyid
        {
            set { m_identifyid = value; }
            get { return m_identifyid; }
        }
        //主题签定名称
        public string Name
        {
            set { m_name = value; }
            get { return m_name; }
        }
        //主题签定文件名
        public string Filename
        {
            set { m_filename = value; }
            get { return m_filename; }
        }
    }
}
