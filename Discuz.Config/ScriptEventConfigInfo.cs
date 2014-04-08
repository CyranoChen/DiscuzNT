using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Discuz.Config
{
    [Serializable]
    public class ScriptEventConfigInfo : IConfigInfo
    {
        [XmlArray("scriptevent")]
        public ScriptEventInfo[] ScriptEvents = { };
    }

    /// <summary>
    /// �ű�����������Ϣ
    /// </summary>
    [Serializable]
    public class ScriptEventInfo
    {
        private string _key;

        /// <summary>
        /// �����ʶ
        /// </summary>
        [XmlAttribute("key")]
        public string Key
        {
            get { return _key; }
            set { _key = value; }
        }
        private string _title;

        /// <summary>
        /// ����
        /// </summary>
        [XmlAttribute("title")]
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }
        private string _script;

        /// <summary>
        /// ��Ҫ��ִ�еĽű�����
        /// </summary>
        [XmlText]        
        public string Script
        {
            get { return _script; }
            set { _script = value; }
        }
        private int _timeofday;

        /// <summary>
        /// ÿ�չ̶�ʱ�����е�ʱ��, -1 Ϊ��ʱ��������
        /// </summary>
        [XmlAttribute("timeofday")]
        public int Timeofday
        {
            get { return _timeofday; }
            set { _timeofday = value; }
        }
        private int _miniutes;

        /// <summary>
        /// ��ʱ�������е�ʱ����(����), ��timeofdayΪ-1ʱ��Ч
        /// </summary>
        [XmlAttribute("miniutes")]
        public int Miniutes
        {
            get { return _miniutes; }
            set { _miniutes = value; }
        }

        /// <summary>
        /// �Ƿ�ñ�ִ��
        /// </summary>
        [XmlIgnore]
        public bool ShouldExecute
        {
            get { return true;}
        }

        private bool _enabled;
        /// <summary>
        /// �Ƿ����ô�����
        /// </summary>
        [XmlAttribute("enabled")]
        public bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

    }
}
