using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Discuz.Common;
using System.Collections;
using Discuz.Forum;
using Discuz.Entity;

namespace Discuz.Web.Services.Manyou.Actions
{
    public class Notifications : ActionBase
    {
        public string Get()
        {
            GetNoticeParams actionParams = JavaScriptConvert.DeserializeObject<GetNoticeParams>(JsonParams);

            if (actionParams.UId <= 0)
                return "";
            GetNoticeResponse getNoticeResponse = new GetNoticeResponse();

            getNoticeResponse.Message.UnRead = PrivateMessages.GetPrivateMessageCount(actionParams.UId, 0, 1);
            DateTime time = new DateTime();

            if (getNoticeResponse.Message.UnRead > 0)
                getNoticeResponse.Message.MostRecent = Utils.ConvertToUnixTimestamp(DateTime.TryParse(PrivateMessages.GetPrivateMessageInfo(PrivateMessages.GetLatestPMID(actionParams.UId)).Postdatetime, out time) ? time : DateTime.Now);

            getNoticeResponse.Notification.UnRead = Notices.GetNoticeCount(actionParams.UId, 1);
            if (getNoticeResponse.Notification.UnRead > 0)
                getNoticeResponse.Notification.MostRecent = Utils.ConvertToUnixTimestamp(DateTime.TryParse(Notices.GetNewNotices(actionParams.UId)[0].Postdatetime, out time) ? time : DateTime.Now);

            List<FriendshipRequestInfo> requestList = Friendship.GetUserFriendRequestList(actionParams.UId, 1, Friendship.GetUserFriendRequestCount(actionParams.UId));

            getNoticeResponse.FriendRequest.UIds = GetFriendRequestUid(requestList);
            if (getNoticeResponse.FriendRequest.UIds.Length > 0)
                getNoticeResponse.FriendRequest.MostRecent = Utils.ConvertToUnixTimestamp(DateTime.TryParse(requestList[requestList.Count - 1].DateTime, out time) ? time : DateTime.Now);

            return GetResult(getNoticeResponse);
        }

        private int[] GetFriendRequestUid(List<FriendshipRequestInfo> requestList)
        {
            List<int> resultList = new List<int>();
            foreach (FriendshipRequestInfo requestInfo in requestList)
            {
                resultList.Add(requestInfo.FromUid);
            }
            return resultList.ToArray();
        }

        public string Send()
        {
            SendNoticeParams actionParams = JavaScriptConvert.DeserializeObject<SendNoticeParams>(UnicodeToString(JsonParams));

            if (actionParams.UId <= 0)
                return "";

            Dictionary<int, int> noticeTable = new Dictionary<int, int>();

            foreach (int toUid in actionParams.RecipientIds)
            {
                noticeTable.Add(toUid, ManyouApplications.SendApplicationNotice(actionParams.UId, toUid, actionParams.Notification));
            }

            return GetResult(noticeTable);
        }
    }
}
