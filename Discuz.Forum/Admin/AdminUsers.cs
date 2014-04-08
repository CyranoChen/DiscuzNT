using System;
using System.Data;
using System.Data.Common;

using Discuz.Common;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Cache;

namespace Discuz.Forum
{
	/// <summary>
	/// UserFactoryAdmin 的摘要说明。
	/// 后台用户信息操作管理类
	/// </summary>
	public class AdminUsers : Users
	{
		/// <summary>
		/// 更新用户全部信息
		/// </summary>
		/// <param name="__userinfo"></param>
		/// <returns></returns>
		public static bool UpdateUserAllInfo(UserInfo userInfo)
		{
            Users.UpdateUser(userInfo);

			//当用户不是版主(超级版主)或管理员
			if ((userInfo.Adminid == 0) || (userInfo.Adminid > 3))
			{
				//删除用户在版主列表中相关数据
                Data.Moderators.DeleteModerator(userInfo.Uid);
				//同时更新版块相关的版主信息
				UpdateForumsFieldModerators(userInfo.Username);
			}

			#region 以下为更新该用户的扩展信息

			string signature = Utils.HtmlEncode(ForumUtils.BanWordFilter(userInfo.Signature));

			UserGroupInfo usergroupinfo = AdminUserGroups.AdminGetUserGroupInfo(userInfo.Groupid);
            GeneralConfigInfo config = GeneralConfigs.GetConfig();

			PostpramsInfo postPramsInfo = new PostpramsInfo();
			postPramsInfo.Usergroupid = usergroupinfo.Groupid;
			postPramsInfo.Attachimgpost = config.Attachimgpost;
			postPramsInfo.Showattachmentpath = config.Showattachmentpath;
			postPramsInfo.Hide = 0;
			postPramsInfo.Price = 0;
			postPramsInfo.Sdetail = userInfo.Signature;
			postPramsInfo.Smileyoff = 1;
			postPramsInfo.Bbcodeoff = 1 - usergroupinfo.Allowsigbbcode;
			postPramsInfo.Parseurloff = 1;
			postPramsInfo.Showimages = usergroupinfo.Allowsigimgcode;
			postPramsInfo.Allowhtml = 0;
			postPramsInfo.Smiliesinfo = Smilies.GetSmiliesListWithInfo();
			postPramsInfo.Customeditorbuttoninfo = Editors.GetCustomEditButtonListWithInfo();
			postPramsInfo.Smiliesmax = config.Smiliesmax;
			postPramsInfo.Signature = 1;
			postPramsInfo.Onlinetimeout = config.Onlinetimeout;

            userInfo.Signature = signature;
            userInfo.Authstr = ForumUtils.CreateAuthStr(20);
            userInfo.Sightml = UBB.UBBToHTML(postPramsInfo);
            Users.UpdateUser(userInfo);

			#endregion

			Users.UpdateUserForumSetting(userInfo);
			return true;
		}

		/// <summary>
		/// 更新用户名
		/// </summary>
        /// <param name="userInfo">当前用户信息</param>
		/// <param name="oldusername">以前用户的名称</param>
		/// <returns></returns>
		public static bool UserNameChange(UserInfo userInfo, string oldusername)
		{
			//将新主题表
            Data.Topics.UpdateTopicLastPoster(userInfo.Uid, userInfo.Username);
            Data.Topics.UpdateTopicPoster(userInfo.Uid, userInfo.Username);

			//更新帖子表        
            //foreach (DataRow dr in Data.PostTables.GetAllPostTableName().Rows)
            //{
                Data.Posts.UpdatePostPoster(userInfo.Uid, userInfo.Username);
			//}

			//更新短消息
            Data.PrivateMessages.UpdatePMSenderAndReceiver(userInfo.Uid, userInfo.Username);
			//更新公告
            Data.Announcements.UpdateAnnouncementPoster(userInfo.Uid, userInfo.Username);
			//更新统计表中的信息
            if (Data.Statistics.UpdateStatisticsLastUserName(userInfo.Uid, userInfo.Username) != 0)
			{
                Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/Statistics");
			}

			//更新论坛版主相关信息
            //foreach (DataRow dr in Data.Forums.GetModerators(oldusername).Rows)
            //{
            //    string moderators = "," + dr["moderators"].ToString().Trim() + ",";
            //    if (moderators.IndexOf("," + oldusername + ",") >= 0)
            //        Forums.UpdateForumField(Utils.StrToInt(dr["fid"], 0),"moderators",dr["moderators"].ToString().Trim().Replace(oldusername, userInfo.Username));
            //}

            //更新版块版主的名字
            Forums.UpdateModeratorName(oldusername, userInfo.Username);
			return true;
		}


		/// <summary>
		/// 删除指定用户的所有信息
		/// </summary>
		/// <param name="uid">指定的用户uid</param>
		/// <param name="delposts">是否删除帖子</param>
		/// <param name="delpms">是否删除短消息</param>
		/// <returns></returns>
		public static bool DelUserAllInf(int uid, bool delposts, bool delpms)
		{            
            bool val = Data.Users.DeleteUser(uid, delposts, delpms);
            if(val)
                DNTCache.GetCacheService().RemoveObject("/Forum/Statistics");

            return val;
		}


		/// <summary>
		/// 更新当前用户名在版块属性中的版主信息
		/// </summary>
		/// <param name="username">当前用户的名称</param>
		public static void UpdateForumsFieldModerators(string username)
		{
            ////删除版主表的相关用户信息
            //DataTable dt = Data.Forums.GetModerators(username);
            //if (dt.Rows.Count > 0)
            //{
            //    string updatestr = "";
            //    foreach (DataRow dr in dt.Rows)
            //    {
            //        updatestr = dr["moderators"].ToString().Replace(username + ",", "");
            //        updatestr = updatestr.Replace("," + username, "");
            //        updatestr = updatestr.Replace(username, "");
            //        Forums.UpdateForumField(Utils.StrToInt(dr["fid"], 0), "moderators", updatestr);
            //    }
            //}

            //删除版块中的版主
            Forums.UpdateModeratorName(username, "");
		}


		/// <summary>
		/// 合并用户
		/// </summary>
		/// <param name="srcuid">源用户ID</param>
		/// <param name="targetuid">目标用户ID</param>
		/// <returns></returns>
		public static bool CombinationUser(int srcuid, int targetuid)
		{
			try
			{
				//积分合并
                UserInfo sourceUserInfo = Discuz.Data.Users.GetUserInfo(srcuid);
                UserInfo targetUserInfo = Discuz.Data.Users.GetUserInfo(targetuid);
                targetUserInfo.Credits += sourceUserInfo.Credits;
                targetUserInfo.Extcredits1 += sourceUserInfo.Extcredits1;
                targetUserInfo.Extcredits2 += sourceUserInfo.Extcredits2;
                targetUserInfo.Extcredits3 += sourceUserInfo.Extcredits3;
                targetUserInfo.Extcredits4 += sourceUserInfo.Extcredits4;
                targetUserInfo.Extcredits5 += sourceUserInfo.Extcredits5;
                targetUserInfo.Extcredits6 += sourceUserInfo.Extcredits6;
                targetUserInfo.Extcredits7 += sourceUserInfo.Extcredits7;
                targetUserInfo.Extcredits8 += sourceUserInfo.Extcredits8;
                Users.UpdateUser(targetUserInfo);
                Data.Users.CombinationUser(Posts.GetPostTableName(), targetUserInfo, sourceUserInfo);
				//删除被合并用户的所有相关信息
				DelUserAllInf(srcuid, true, true);

				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}