using System;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Entity;
using Discuz.Space.Provider;

namespace Discuz.Space.Pages
{
    /// <summary>
    /// 个人空间友情链接编辑
    /// </summary>
    public class usercpspacecategoryedit : PageBase
    {
        #region 页面变量
        /// <summary>
        /// 空间分类信息
        /// </summary>
        public SpaceCategoryInfo spacecategoryinfo;
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

                string title = Utils.HtmlEncode(DNTRequest.GetString("title"));
                if ((title.Length > 50) || (title == ""))
                {
                    AddErrLine("分类名称不得超过50个字符且不能为空");
                    return;
                }
                if (DNTRequest.GetString("description").Length > 1000)
                {
                    AddErrLine("分类描述不得超过1000个字符");
                    return;
                }
                if (!Utils.IsNumeric(DNTRequest.GetString("displayorder")))
                {
                    AddErrLine("分类描述序号必须为数字");
                    return;
                }

                string errorinfo = "";
                int categoryid = DNTRequest.GetInt("categoryid", 0);
                if (categoryid == 0)
                {
                    AddErrLine("请正确选择分类信息");
                    return;
                }

                spacecategoryinfo = BlogProvider.GetSpaceCategoryInfo(Space.Data.DbProvider.GetInstance().GetSpaceCategoryByCategoryID(categoryid));
                if (spacecategoryinfo == null || spacecategoryinfo.Uid != userid)
                {
                    AddErrLine("您选择分类不存在");
                    return;
                }
                spacecategoryinfo.Title = title;
                spacecategoryinfo.Description = Utils.HtmlEncode(DNTRequest.GetString("description"));
                spacecategoryinfo.Displayorder = Convert.ToInt32(DNTRequest.GetString("displayorder"));
                Space.Data.DbProvider.GetInstance().SaveSpaceCategory(spacecategoryinfo);

                if (errorinfo == "")
                {
                    SetUrl("usercpspacemanagecategory.aspx");
                    SetMetaRefresh();
                    SetShowBackLink(true);
                    AddMsgLine("修改文章分类完毕");
                }
                else
                {
                    AddErrLine(errorinfo);
                    return;
                }
            }
            else
            {
                int categoryid = DNTRequest.GetInt("categoryid", 0);
                if (categoryid == 0)
                {
                    AddErrLine("请正确选择分类信息");
                    return;
                }

                spacecategoryinfo = BlogProvider.GetSpaceCategoryInfo(Space.Data.DbProvider.GetInstance().GetSpaceCategoryByCategoryID(categoryid));
                if (spacecategoryinfo.Uid != userid)
                {
                    AddErrLine("请选择的分类不存在");
                    return;
                }
                if (spacecategoryinfo == null || spacecategoryinfo.Uid != userid)
                {
                    AddErrLine("您选择分类不存在");
                    return;
                }
            }
        }
    }
}