using System;
using System.Text;
using Discuz.Forum.ScheduledEvents;
using Discuz.Forum;

namespace Discuz.Event
{
    /// <summary>
    /// �й���̳�ļƻ�����
    /// </summary>
    public class ForumEvent : IEvent
    {
        #region IEvent ��Ա

        public void Execute(object state)
        {
            //������������
            Topics.NeatenRelateTopics();
        }

        #endregion
    }
}
