using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Discuz.Config
{
    [Serializable]
    public class ScheduleConfigInfo : IConfigInfo
    {
        public ScheduleConfigInfo()
        { }

        [XmlElement("enabled")]
        public bool Enabled;

        [XmlArray("events")]
        public Event[] Events;

        [XmlElement("minutes_interval")]
        public int TimerMinutesInterval;
    }

    [Serializable]
    [XmlRoot ("event")]
    public class Event
    {
        public Event()
        {

        }

        private string _key;

        /// <summary>
        /// A unique key used to query the database. The name of the Server will also be used to ensure the "Key" is 
        /// unique in a cluster
        /// </summary>
        [XmlAttribute("key")]
        public string Key
        {
            get { return this._key; }
            set { this._key = value; }
        }

        private int _timeOfDay = -1;

        /// <summary>
        /// Absolute time in mintues from midnight. Can be used to assure event is only 
        /// executed once per-day and as close to the specified
        /// time as possible. Example times: 0 = midnight, 27 = 12:27 am, 720 = Noon
        /// </summary>
        [XmlAttribute("time_of_day")]
        public int TimeOfDay
        {
            get { return this._timeOfDay; }
            set { this._timeOfDay = value; }
        }

        private int _minutes = 60;

        /// <summary>
        /// The scheduled event interval time in minutes. If TimeOfDay has a value >= 0, Minutes will be ignored. 
        /// This values should not be less than the Timer interval.
        /// </summary>
        [XmlAttribute("minutes")]
        public int Minutes
        {
            get { return this._minutes; }
            set { this._minutes = value; }
        }

        private string _scheduleType;

        /// <summary>
        /// The Type of class which implements IEvent
        /// </summary>
        [XmlAttribute("type")]
        public string ScheduleType
        {
            get { return this._scheduleType; }
            set { this._scheduleType = value; }
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

        private bool _isSystemEvent;

        /// <summary>
        /// 是否是系统级任务(如果是, 则不允许关闭)
        /// </summary>
        [XmlAttribute("is_system_event")]
        public bool IsSystemEvent
        {
            get { return _isSystemEvent; }
            set { _isSystemEvent = value; }
        }
    }
}
