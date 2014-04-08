using System;
using System.Data;
using System.Data.SqlClient;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Web.UI;
using Discuz.Entity;
using Discuz.Config;
using System.Text;

namespace Discuz.Web
{
    /// <summary>
    /// 用户中心
    /// </summary>
    public class usercp : UserCpPage
    {
        #region 页面变量
        /// <summary>
        /// 头像地址
        /// </summary>
        public string avatarurl = "";
        /// <summary>
        /// 头像类型
        /// </summary>
        public int avatartype = 1;
        /// <summary>
        /// 头像宽度
        /// </summary>
        public int avatarwidth = 0;
        /// <summary>
        /// 是否有管理权限
        /// </summary>
        public int ismoder;
        /// <summary>
        /// 头像高度
        /// </summary>
        public int avatarheight = 0;
        public string usergroupattachtype;
        public AdminGroupInfo admingroupinfo;
        public string score1 = "", score2 = "", score3 = "", score4 = "", score5 = "", score6 = "", score7 = "", score8 = "";
        #endregion

        protected override void ShowPage()
        {
            pagetitle = "用户控制面板";

            if (!IsLogin()) return; 
           
            score1 = ((decimal)user.Extcredits1).ToString();
            score2 = ((decimal)user.Extcredits2).ToString();
            score3 = ((decimal)user.Extcredits3).ToString();
            score4 = ((decimal)user.Extcredits4).ToString();
            score5 = ((decimal)user.Extcredits5).ToString();
            score6 = ((decimal)user.Extcredits6).ToString();
            score7 = ((decimal)user.Extcredits7).ToString();
            score8 = ((decimal)user.Extcredits8).ToString();

            if (!IsErr() && useradminid > 0)
                admingroupinfo = AdminGroups.GetAdminGroupInfo(usergroupid);

            StringBuilder sbAttachmentTypeSelect = new StringBuilder();
            if (!Utils.StrIsNullOrEmpty(usergroupinfo.Attachextensions))
                sbAttachmentTypeSelect.AppendFormat("[id] in ({0})", usergroupinfo.Attachextensions);

            usergroupattachtype = Attachments.GetAttachmentTypeString(sbAttachmentTypeSelect.ToString());
            newnoticecount = Notices.GetNewNoticeCountByUid(userid);

            //if (user.Avatar.Trim().ToLower().StartsWith("http://"))
            //{
            //    avatarurl = user.Avatar;
            //    avatartype = 2;
            //    avatarwidth = user.Avatarwidth;
            //    avatarheight = user.Avatarheight;
            //}
            //else if (user.Avatar.ToLower().Trim().StartsWith(@"avatars\common\"))
            //    avatartype = 0;
        }
    }
}
