using System;
using System.Diagnostics;
using System.Web;
using System.Threading;

using Discuz.Config;
using Discuz.Common.Generic;
using Discuz.Common;

namespace Discuz.Forum.ScheduledEvents
{
	/// <summary>
	/// EventManager is called from the EventHttpModule (or another means of scheduling a Timer). Its sole purpose
	/// is to iterate over an array of Events and deterimine of the Event's IEvent should be processed. All events are
	/// added to the managed threadpool. 
	/// </summary>
	public class EventManager
	{
        public static string RootPath;

		private EventManager()
		{
		}

		public static readonly int TimerMinutesInterval = 5;
        static EventManager()
        {
            if (ScheduleConfigs.GetConfig().TimerMinutesInterval > 0)
            {
                TimerMinutesInterval = ScheduleConfigs.GetConfig().TimerMinutesInterval;
            }
        }


		public static void Execute()
        {
            Discuz.Config.Event[] simpleItems = ScheduleConfigs.GetConfig().Events;
            Event[] items;
#if NET1
            ArrayList list = new ArrayList();
#else
            List<Event> list = new List<Event>();
#endif

            foreach (Discuz.Config.Event newEvent in simpleItems)
            {
                if (!newEvent.Enabled)
                {
                    continue;
                }
                Event e = new Event();
                e.Key = newEvent.Key;
                e.Minutes = newEvent.Minutes;
                e.ScheduleType = newEvent.ScheduleType;
                e.TimeOfDay = newEvent.TimeOfDay;

                list.Add(e);
            }


#if NET1
            items = (Event[])list.ToArray(typeof(Event));
#else
            items = list.ToArray();
#endif

            Event item = null;
			
			if(items != null)
			{
				
				for(int i = 0; i<items.Length; i++)
				{
					item = items[i];
					if(item.ShouldExecute)
					{
						item.UpdateTime();
						IEvent e = item.IEventInstance;
						ManagedThreadPool.QueueUserWorkItem(new WaitCallback(e.Execute));
					}
				}
			}
		}
	}
}
