using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

using Discuz.Common;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Forum;
using Newtonsoft.Json;

namespace Discuz.Web.Services.Manyou.Actions
{
    public class Friends : ActionBase
    {
        public string AreFriends()
        {
            AreFriendsParams actionParams = JavaScriptConvert.DeserializeObject<AreFriendsParams>(JsonParams);

            return GetResult(Friendship.IsFriendshipExist(actionParams.UId1, actionParams.UId2) == IsFriendshipExistEnum.Exist);
        }

        public string Get()
        {
            GetFriendsParams actionParams = JavaScriptConvert.DeserializeObject<GetFriendsParams>(JsonParams);
            Dictionary<int, int[]> friendListTable = new Dictionary<int, int[]>();

            foreach (int uid in actionParams.UIds)
            {
                List<int> friendUidList = new List<int>();
                List<FriendshipInfo> friendshipList = Friendship.GetUserFriendsList(uid, 1, actionParams.FriendNum);

                foreach (FriendshipInfo friendUid in friendshipList)
                    friendUidList.Add(friendUid.FriendUid);

                friendListTable.Add(uid, friendUidList.ToArray());
            }

            return GetResult(friendListTable);
        }
    }
}
