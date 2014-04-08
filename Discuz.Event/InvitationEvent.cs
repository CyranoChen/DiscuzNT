using System;
using System.Text;
using Discuz.Forum.ScheduledEvents;
using Discuz.Forum;

namespace Discuz.Event
{
    /// <summary>
    /// 有关未使用附件的计划任务
    /// </summary>
    public class InvitationEvent : IEvent
    {
        #region IEvent 成员

        public void Execute(object state)
        {
            //清理表invitation中过期和失效的邀请码信息，保持表中数据的
            Invitation.ClearExpireInviteCode();
        }

        #endregion
    }
}
