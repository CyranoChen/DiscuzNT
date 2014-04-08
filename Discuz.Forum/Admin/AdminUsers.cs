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
	/// UserFactoryAdmin ��ժҪ˵����
	/// ��̨�û���Ϣ����������
	/// </summary>
	public class AdminUsers : Users
	{
		/// <summary>
		/// �����û�ȫ����Ϣ
		/// </summary>
		/// <param name="__userinfo"></param>
		/// <returns></returns>
		public static bool UpdateUserAllInfo(UserInfo userInfo)
		{
            Users.UpdateUser(userInfo);

			//���û����ǰ���(��������)�����Ա
			if ((userInfo.Adminid == 0) || (userInfo.Adminid > 3))
			{
				//ɾ���û��ڰ����б����������
                Data.Moderators.DeleteModerator(userInfo.Uid);
				//ͬʱ���°����صİ�����Ϣ
				UpdateForumsFieldModerators(userInfo.Username);
			}

			#region ����Ϊ���¸��û�����չ��Ϣ

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
		/// �����û���
		/// </summary>
        /// <param name="userInfo">��ǰ�û���Ϣ</param>
		/// <param name="oldusername">��ǰ�û�������</param>
		/// <returns></returns>
		public static bool UserNameChange(UserInfo userInfo, string oldusername)
		{
			//���������
            Data.Topics.UpdateTopicLastPoster(userInfo.Uid, userInfo.Username);
            Data.Topics.UpdateTopicPoster(userInfo.Uid, userInfo.Username);

			//�������ӱ�        
            //foreach (DataRow dr in Data.PostTables.GetAllPostTableName().Rows)
            //{
                Data.Posts.UpdatePostPoster(userInfo.Uid, userInfo.Username);
			//}

			//���¶���Ϣ
            Data.PrivateMessages.UpdatePMSenderAndReceiver(userInfo.Uid, userInfo.Username);
			//���¹���
            Data.Announcements.UpdateAnnouncementPoster(userInfo.Uid, userInfo.Username);
			//����ͳ�Ʊ��е���Ϣ
            if (Data.Statistics.UpdateStatisticsLastUserName(userInfo.Uid, userInfo.Username) != 0)
			{
                Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/Statistics");
			}

			//������̳���������Ϣ
            //foreach (DataRow dr in Data.Forums.GetModerators(oldusername).Rows)
            //{
            //    string moderators = "," + dr["moderators"].ToString().Trim() + ",";
            //    if (moderators.IndexOf("," + oldusername + ",") >= 0)
            //        Forums.UpdateForumField(Utils.StrToInt(dr["fid"], 0),"moderators",dr["moderators"].ToString().Trim().Replace(oldusername, userInfo.Username));
            //}

            //���°�����������
            Forums.UpdateModeratorName(oldusername, userInfo.Username);
			return true;
		}


		/// <summary>
		/// ɾ��ָ���û���������Ϣ
		/// </summary>
		/// <param name="uid">ָ�����û�uid</param>
		/// <param name="delposts">�Ƿ�ɾ������</param>
		/// <param name="delpms">�Ƿ�ɾ������Ϣ</param>
		/// <returns></returns>
		public static bool DelUserAllInf(int uid, bool delposts, bool delpms)
		{            
            bool val = Data.Users.DeleteUser(uid, delposts, delpms);
            if(val)
                DNTCache.GetCacheService().RemoveObject("/Forum/Statistics");

            return val;
		}


		/// <summary>
		/// ���µ�ǰ�û����ڰ�������еİ�����Ϣ
		/// </summary>
		/// <param name="username">��ǰ�û�������</param>
		public static void UpdateForumsFieldModerators(string username)
		{
            ////ɾ�������������û���Ϣ
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

            //ɾ������еİ���
            Forums.UpdateModeratorName(username, "");
		}


		/// <summary>
		/// �ϲ��û�
		/// </summary>
		/// <param name="srcuid">Դ�û�ID</param>
		/// <param name="targetuid">Ŀ���û�ID</param>
		/// <returns></returns>
		public static bool CombinationUser(int srcuid, int targetuid)
		{
			try
			{
				//���ֺϲ�
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
				//ɾ�����ϲ��û������������Ϣ
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