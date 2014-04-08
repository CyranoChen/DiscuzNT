using System;
using System.Text;
using Discuz.Forum.ScheduledEvents;
using Discuz.Forum;

namespace Discuz.Event
{
    /// <summary>
    /// 有关未使用附件的计划任务
    /// </summary>
    public class AttachmentEvent : IEvent
    {
        #region IEvent 成员

        public void Execute(object state)
        {
            //删除未使用的附件及文件
            Attachments.DeleteNoUsedForumAttachment();
        }

        #endregion
    }
}
