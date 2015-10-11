using Discuz.Common;
using Discuz.Forum;
using Discuz.Web.UI;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Common.Generic;

namespace Discuz.Web
{
    /// <summary>
    /// 查看用户信息页
    /// </summary>
    public class userinfo : PageBase
    {
        /// <summary>
        /// 当前用户信息
        /// </summary>
        public UserInfo user;
        /// <summary>
        /// 当前用户用户组信息
        /// </summary>
        public UserGroupInfo group;
        /// <summary>
        /// 当前用户管理组信息
        /// </summary>
        public AdminGroupInfo admininfo;
        /// <summary>
        /// 可用的扩展积分名称列表
        /// </summary>
        public string[] score;
        /// <summary>
        /// 是否需要快速登录
        /// </summary>
        public bool needlogin = false;

        public string score1, score2, score3, score4, score5, score6, score7, score8;

        public int id = DNTRequest.GetInt("userid", -1);

        protected override void ShowPage()
        {
            pagetitle = "查看用户信息";

            if (usergroupinfo.Allowviewpro != 1 && userid != id)
            {
                AddErrLine(string.Format("您当前的身份 \"{0}\" 没有查看用户资料的权限", usergroupinfo.Grouptitle));
                if (userid < 1)
                    needlogin = true;

                return;
            }

            if (Utils.StrIsNullOrEmpty(DNTRequest.GetString("username")) && Utils.StrIsNullOrEmpty(DNTRequest.GetString("userid")))
            {
                AddErrLine("错误的URL链接");
                return;
            }



            if (id == -1)
                id = Users.GetUserId(Utils.UrlDecode(DNTRequest.GetString("username")));

            if (id == -1)
            {
                AddErrLine("该用户不存在");
                return;
            }

            user = Users.GetUserInfo(id);
            if (user == null)
            {
                AddErrLine("该用户不存在");
                return;
            }

            //用户设定Email保密时，清空用户的Email属性以避免被显示
            if (user.Showemail != 1 && id != userid)
                user.Email = "";

            //获取积分机制和用户组信息，底层有缓存
            score = Scoresets.GetValidScoreName();
            group = UserGroups.GetUserGroupInfo(user.Groupid);
            admininfo = AdminUserGroups.AdminGetAdminGroupInfo(usergroupid);
            score1 = ((decimal)user.Extcredits1).ToString();
            score2 = ((decimal)user.Extcredits2).ToString();
            score3 = ((decimal)user.Extcredits3).ToString();
            score4 = ((decimal)user.Extcredits4).ToString();
            score5 = ((decimal)user.Extcredits5).ToString();
            score6 = ((decimal)user.Extcredits6).ToString();
            score7 = ((decimal)user.Extcredits7).ToString();
            score8 = ((decimal)user.Extcredits8).ToString();
        }
    }
}
