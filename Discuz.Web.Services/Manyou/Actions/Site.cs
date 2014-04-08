using System;
using System.Text;
using System.Data;

using Discuz.Common;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Forum;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Discuz.Web.Services.Manyou.Actions
{
    public class Site : ActionBase
    {
        public string GetAllUsers()
        {
            GetAllUsersParams actionParams = JavaScriptConvert.DeserializeObject<GetAllUsersParams>(JsonParams);

            int pageSize = actionParams.ReadCount;
            int pageIndex = (actionParams.From / pageSize) + 1;

            DataTable dt = Forum.Users.GetUserListOnService(pageSize, pageIndex, "", "");
            List<FullUserInfo> fullUserList = new List<FullUserInfo>();

            foreach (DataRow dataRow in dt.Rows)
            {
                FullUserInfo fullUserInfo = new FullUserInfo();
                fullUserInfo.Uid = TypeConverter.ObjectToInt(dataRow["uid"]);
                fullUserInfo.UserName = dataRow["username"].ToString().Trim();
                fullUserInfo.FriendsArray = GetUserFriendList(fullUserInfo.Uid, pageSize);
                fullUserList.Add(fullUserInfo);
            }

            GetAllUsersResponse gauResponse = new GetAllUsersResponse();
            gauResponse.FullUserList.AddRange(fullUserList);
            gauResponse.TotalNum = Forum.Users.GetUserCount("");

            return GetResult(gauResponse);
        }

        public string GetUpdatedUsers()
        {
            GetUpdatedUsers actionParams = JavaScriptConvert.DeserializeObject<GetUpdatedUsers>(JsonParams);

            List<UserLogInfo> userLogList = ManyouApplications.GetUserLog(actionParams.Count);

            List<UserInfoWithAction> userInfoWithActionList = new List<UserInfoWithAction>();

            foreach (UserLogInfo userLogInfo in userLogList)
            {
                UserInfoWithAction userInfoWithAction = new UserInfoWithAction();
                userInfoWithAction.Action = userLogInfo.Action.ToString();
                if (userLogInfo.Action != UserLogActionEnum.Delete)
                {
                    Discuz.Entity.UserInfo userInfo = Forum.Users.GetUserInfo(userLogInfo.UId);
                    userInfoWithAction.UserName = userInfo.Username;
                }
                userInfoWithAction.Action = userLogInfo.Action.ToString().ToLower();
                userInfoWithAction.Uid = userLogInfo.UId;

                userInfoWithActionList.Add(userInfoWithAction);
            }

            GetUpdateUsersResponse guuResponse = new GetUpdateUsersResponse();
            guuResponse.TotalNum = userInfoWithActionList.Count >= actionParams.Count ? actionParams.Count * 2 : userInfoWithActionList.Count;
            guuResponse.UserInfoWithActionList = userInfoWithActionList;
            return GetResult(guuResponse);

        }

        public string GetUpdatedFriends()
        {
            GetUpdatedFriendsParams actionParams = JavaScriptConvert.DeserializeObject<GetUpdatedFriendsParams>(JsonParams);

            List<FriendshipActionInfo> friendshipActionList = Friendship.GetFriendshipChangeLog(actionParams.Count);
            List<FriendsAction> actionList = new List<FriendsAction>();
            foreach (FriendshipActionInfo info in friendshipActionList)
            {
                FriendsAction action = new FriendsAction();
                action.Uid1 = info.Uid;
                action.Uid2 = info.FriendUid;
                action.Action = info.Action.ToString().ToLower();
                actionList.Add(action);
            }
            GetUpdatedFriendsResponse gufResponse = new GetUpdatedFriendsResponse();
            gufResponse.TotalNum = actionList.Count >= actionParams.Count ? actionParams.Count * 2 : actionList.Count;
            gufResponse.ActionList = actionList;
            return GetResult(gufResponse);
        }

        public string GetStat()
        {
            return "";
        }

        #region private method

        private string[] GetUserFriendList(int uid, int count)
        {
            List<FriendshipInfo> friendshipList = Friendship.GetUserFriendsList(uid, 1, count);
            List<string> resultList = new List<string>();
            foreach (FriendshipInfo info in friendshipList)
                resultList.Add(info.FriendUid.ToString());

            return resultList.ToArray();
        }

        #endregion
    }
}
