using Discuz.Common;
using Discuz.Forum;
using Discuz.Entity;

namespace Discuz.Space.Pages
{
    /// <summary>
    /// 添加空间友情链接
    /// </summary>
    public class usercpspacelinkadd : PageBase
    {
        #region 页面变量
        /// <summary>
        /// 当前用户信息
        /// </summary>
        public UserInfo user = new UserInfo();
        #endregion

        protected override void ShowPage()
        {
            pagetitle = "用户控制面板";

            if (userid == -1)
            {
                AddErrLine("你尚未登录");
                return;
            }
            user = Users.GetUserInfo(userid);
            if (config.Enablespace != 1)
            {
                AddErrLine("个人空间功能已被关闭");
                return;
            }

            if (user.Spaceid <= 0)
            {
                AddErrLine("您尚未开通个人空间");
                return;
            }

            if (DNTRequest.IsPost())
            {
                if (ForumUtils.IsCrossSitePost())
                {
                    AddErrLine("您的请求来路不正确，无法提交。如果您安装了某种默认屏蔽来路信息的个人防火墙软件(如 Norton Internet Security)，请设置其不要禁止来路信息后再试。");
                    return;
                }

                string linktitle = Utils.HtmlEncode(DNTRequest.GetString("linktitle"));
                if ((linktitle.Length > 50) || (linktitle == ""))
                {
                    AddErrLine("链接标题不得超过50个字符且不能为空");
                    return;
                }

                string linkurl = Utils.HtmlEncode(DNTRequest.GetString("linkurl"));
                if ((linkurl.Length > 255) || (linkurl == ""))
                {
                    AddErrLine("链接地址不得超过255个字符且不能为空");
                    return;
                }

                if (DNTRequest.GetString("description").Length > 200)
                {
                    AddErrLine("链接描述不得超过200个字符");
                    return;
                }

                string errorinfo = "";
                SpaceLinkInfo __spacelinkinfo = new SpaceLinkInfo();
                __spacelinkinfo.UserId = userid;
                __spacelinkinfo.LinkTitle = linktitle;
                __spacelinkinfo.LinkUrl = linkurl;
                __spacelinkinfo.Description = Utils.HtmlEncode(DNTRequest.GetString("description"));

                Space.Data.DbProvider.GetInstance().AddSpaceLink(__spacelinkinfo);

                if (errorinfo == "")
                {
                    SetUrl("usercpspacelinklist.aspx");
                    SetMetaRefresh();
                    SetShowBackLink(true);
                    AddMsgLine("添加友情链接完毕");
                }
                else
                {
                    AddErrLine(errorinfo);
                    return;
                }
            }
        }
    }
}