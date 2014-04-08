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
    /// 脚本任务设置信息
    /// </summary>
    [Serializable]
    public class ScriptEventInfo
    {
        private string _key;

        /// <summary>
        /// 任务标识
        /// </summary>
        [XmlAttribute("key")]
        public string Key
        {
            get { return _key; }
            set { _key = value; }
        }
        private string _title;

        /// <summary>
        /// 标题
        /// </summary>
        [XmlAttribute("title")]
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }
        private string _script;

        /// <summary>
        /// 将要被执行的脚本内容
        /// </summary>
        [XmlText]        
        public string Script
        {
            get { return _script; }
            set { _script = value; }
        }
        private int _timeofday;

        /// <summary>
        /// 每日固定时间运行的时间, -1 为按时间间隔运行
        /// </summary>
        [XmlAttribute("timeofday")]
        public int Timeofday
        {
            get { return _timeofday; }
            set { _timeofday = value; }
        }
        private int _miniutes;

        /// <summary>
        /// 按时间间隔运行的时间间隔(分钟), 当timeofday为-1时有效
        /// </summary>
        [XmlAttribute("miniutes")]
        public int Miniutes
        {
            get { return _miniutes; }
            set { _miniutes = value; }
        }

        /// <summary>
        /// 是否该被执行
        /// </summary>
        [XmlIgnore]
        public bool ShouldExecute
        {
            get { return true;}
        }

        private bool _enabled;
        /// <summary>
        /// 是否启用此任务
        /// </summary>
        [XmlAttribute("enabled")]
        public bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

    }
}
