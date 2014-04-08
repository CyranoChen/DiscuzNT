using System;
using System.Xml.Serialization;
using Discuz.Data;

namespace Discuz.Forum.ScheduledEvents
{
	/// <summary>
	/// Event is the configuration of an IEvent. 
	/// </summary>
	public class Event
	{
		public Event()
		{

		}

		private IEvent _ievent = null;

		/// <summary>
		/// The current implementation of IEvent
		/// </summary>
		public IEvent IEventInstance
		{
			get
			{
				LoadIEvent();
				return _ievent;
			}
		}

		/// <summary>
		/// Private method for loading an instance of IEvent
		/// </summary>
		private void LoadIEvent()
		{
			if(_ievent == null)
			{
				if(this.ScheduleType == null)
				{
                    EventLogs.WriteFailedLog("计划任务没有定义其 type 属性");
				}

                Type type = Type.GetType(this.ScheduleType);
                if (type == null)
                {
                    EventLogs.WriteFailedLog(string.Format("计划任务 {0} 无法被正确识别", this.ScheduleType));
                }
                else
                {
                    _ievent = (IEvent)Activator.CreateInstance(type);
                    if (_ievent == null)
                    {
                        EventLogs.WriteFailedLog(string.Format("计划任务 {0} 未能正确加载", this.ScheduleType));
                    }
                }
			}
		}

		private string _key;
		
		/// <summary>
		/// A unique key used to query the database. The name of the Server will also be used to ensure the "Key" is 
		/// unique in a cluster
		/// </summary>
		public string Key
		{
			get {return this._key;}
			set {this._key = value;}
		}

		private int _timeOfDay = -1;
		
		/// <summary>
		/// Absolute time in mintues from midnight. Can be used to assure event is only 
		/// executed once per-day and as close to the specified
		/// time as possible. Example times: 0 = midnight, 27 = 12:27 am, 720 = Noon
		/// </summary>
		public int TimeOfDay
		{
			get {return this._timeOfDay;}
			set {this._timeOfDay = value;}
		}

		private int _minutes = 60;
		
		/// <summary>
		/// The scheduled event interval time in minutes. If TimeOfDay has a value >= 0, Minutes will be ignored. 
		/// This values should not be less than the Timer interval.
		/// </summary>
		public int Minutes
		{
			get 
			{
				if(this._minutes < EventManager.TimerMinutesInterval)
				{
					return EventManager.TimerMinutesInterval;
				}
				return this._minutes;
			}
			set {this._minutes = value;	}
		}

		private string _scheduleType;
		
		/// <summary>
		/// The Type of class which implements IEvent
		/// </summary>
		[XmlAttribute("type")]
		public string ScheduleType
		{
			get {return this._scheduleType;}
			set {this._scheduleType = value;}
		}

		private DateTime _lastCompleted;
		
		/// <summary>
		/// Last Date and Time this event was processed/completed.
		/// </summary>
		[XmlIgnoreAttribute]
		public DateTime LastCompleted
		{
			get {return this._lastCompleted;}
			set 
			{
				dateWasSet = true;
				this._lastCompleted = value;
			}
		}

		//internal testing variable
		bool dateWasSet = false;

		[XmlIgnoreAttribute]
		public bool ShouldExecute
		{
			get
			{
				if(!dateWasSet) //if the date was not set (and it can not be configured), check the data store
				{
					LastCompleted = DatabaseProvider.GetInstance().GetLastExecuteScheduledEventDateTime(this.Key,Environment.MachineName);
				}

				//If we have a TimeOfDay value, use it and ignore the Minutes interval
				if(this.TimeOfDay > -1)
				{
					//Now
					DateTime dtNow = DateTime.Now;  //now
					//We are looking for the current day @ 12:00 am
					DateTime dtCompare = new DateTime(dtNow.Year,dtNow.Month,dtNow.Day);
					//Check to see if the LastCompleted date is less than the 12:00 am + TimeOfDay minutes
					return LastCompleted < dtCompare.AddMinutes(this.TimeOfDay) && dtCompare.AddMinutes(this.TimeOfDay) <= DateTime.Now;
					
				}
				else
				{
					//Is the LastCompleted date + the Minutes interval less than now?
					return LastCompleted.AddMinutes(this.Minutes) < DateTime.Now;
				}
			}
		}

		/// <summary>
		/// Call this method BEFORE processing the ScheduledEvent. This will help protect against long running events 
		/// being fired multiple times. Note, it is not absolute protection. App restarts will cause events to look like
		/// they were completed, even if they were not. Again, ScheduledEvents are helpful...but not 100% reliable
		/// </summary>
		public void UpdateTime()
		{
			this.LastCompleted = DateTime.Now;
			DatabaseProvider.GetInstance().SetLastExecuteScheduledEventDateTime(this.Key,Environment.MachineName,this.LastCompleted);
		}

        public static void SetLastExecuteScheduledEventDateTime(string key, string servername, DateTime lastexecuted)
        {
           Discuz.Data.Event.SetLastExecuteScheduledEventDateTime(key, servername, lastexecuted);
        }

        public static DateTime GetLastExecuteScheduledEventDateTime(string key, string servername)
        {
            return Discuz.Data.Event.GetLastExecuteScheduledEventDateTime(key, servername);
        }
	}
}
