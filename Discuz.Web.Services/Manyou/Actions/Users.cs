using System;
using System.Data;
using System.Text;
using System.Collections.Generic;

using Discuz.Common;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Forum;
using Newtonsoft.Json;

namespace Discuz.Web.Services.Manyou.Actions
{
    public class Users : ActionBase
    {
        public string GetInfo()
        {
            GetUsersInfoParams actionParams = JavaScriptConvert.DeserializeObject<GetUsersInfoParams>(JsonParams);

            if (!Utils.IsNumericList(actionParams.Uids))
                throw new Exception("Params Exception!");

            DataTable dt = Forum.Users.GetUserList(actionParams.UidArray.Length, 1, string.Format("[{0}users].[uid] IN({1})", BaseConfigs.GetTablePrefix, actionParams.Uids));

            List<UserInfo> userList = new List<UserInfo>();
            foreach (DataRow dr in dt.Rows)
            {
                UserInfo userInfo = new UserInfo();
                userInfo.Uid = TypeConverter.ObjectToInt(dr["uid"]);
                userInfo.UserName = dr["username"].ToString().Trim();
                userList.Add(userInfo);
            }

            return GetResult(userList);
        }

        public string GetFriendInfo()
        {
            GetFriendInfoParams actionParams = JavaScriptConvert.DeserializeObject<GetFriendInfoParams>(JsonParams);

            ShortUserInfo shortInfo = Forum.Users.GetShortUserInfo(actionParams.Uid);
            UserInfo user = new UserInfo();
            user.Uid = shortInfo.Uid;
            user.UserName = shortInfo.Username;

            GetFriendInfoResponse gfiResponse = new GetFriendInfoResponse();
            gfiResponse.TotalNum = Forum.Friendship.GetUserFriendsCount(user.Uid);
            gfiResponse.Me = user;

            List<FriendshipInfo> friendshipList = Friendship.GetUserFriendsList(user.Uid, 1, actionParams.ShowFriendsNum);
            List<Friend> friendList = new List<Friend>();

            foreach (FriendshipInfo info in friendshipList)
            {
                friendList.Add(new Friend(info.FriendUid, info.FriendUserName.Trim()));
            }

            gfiResponse.Friends = friendList;

            return GetResult(gfiResponse);
        }
    }
}
