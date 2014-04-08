using System;
using System.Text;
using Discuz.Forum.ScheduledEvents;
using Discuz.Forum;

namespace Discuz.Event
{
    /// <summary>
    /// 有关论坛的计划任务
    /// </summary>
    public class ForumEvent : IEvent
    {
        #region IEvent 成员

        public void Execute(object state)
        {
            //整理相关主题表
            Topics.NeatenRelateTopics();
        }

        #endregion
    }
}
