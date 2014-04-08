using System;
using System.Text;
using Discuz.Forum.ScheduledEvents;
using Discuz.Forum;

namespace Discuz.Event
{
    /// <summary>
    /// 有关通知的计划任务
    /// </summary>
    public class NoticesEvent : IEvent
    {
        #region IEvent 成员

        public void Execute(object state)
        {
            //删除相关通知表数据
            if (Discuz.Config.GeneralConfigs.GetConfig().Notificationreserveddays > 0)
            {
                Notices.DeleteNotice();
            }
        }

        #endregion
    }
}
