using Discuz.Common;
using Discuz.Forum;
using Discuz.Entity;
using Discuz.Space.Provider;
using Discuz.Config;
using Discuz.Space.Utilities;

namespace Discuz.Space.Pages
{
	/// <summary>
	/// 个人中心空间信息设置
	/// </summary>
	public class usercpspaceset : PageBase
    {
        #region 页面变量
        /// <summary>
        /// 空间设置信息
        /// </summary>
		public SpaceConfigInfo spaceconfiginfo = new SpaceConfigInfo();
        /// <summary>
        /// 空间激活设置
        /// </summary>
        public SpaceActiveConfigInfo spaceactiveconfig = SpaceActiveConfigs.GetConfig();
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

			if(DNTRequest.IsPost())
			{
				if (ForumUtils.IsCrossSitePost())
				{
					AddErrLine("您的请求来路不正确，无法提交。如果您安装了某种默认屏蔽来路信息的个人防火墙软件(如 Norton Internet Security)，请设置其不要禁止来路信息后再试。");
					return;
				}
				if (DNTRequest.GetString("spacetitle").Length > 100)		
				{
					AddErrLine("BLOG标题不得超过100个字符");
					return;
				}
				if (DNTRequest.GetString("_description").Length > 200)		
				{
					AddErrLine("空间描述不得超过200个字符");
					return;
				}	

				if (page_err == 0)
				{
					spaceconfiginfo = BlogProvider.GetSpaceConfigInfo(userid);
					spaceconfiginfo.UserID = userid;
					spaceconfiginfo.Spacetitle = Utils.HtmlEncode(DNTRequest.GetString("spacetitle"));
					spaceconfiginfo.Description = Utils.HtmlEncode(DNTRequest.GetString("_description"));
					spaceconfiginfo.BlogDispMode = DNTRequest.GetInt("blogdispmode",0);
					spaceconfiginfo.Bpp = DNTRequest.GetInt("bpp",0);
					spaceconfiginfo.Commentpref = DNTRequest.GetInt("commentpref",0);
					spaceconfiginfo.MessagePref =DNTRequest.GetInt("messagepref", 0);

                    if (spaceactiveconfig.Enablespacerewrite == 1)
                    {
                        string rewritename = DNTRequest.GetFormString("rewritename").Trim();

                        if (!Utils.StrIsNullOrEmpty(rewritename))
                        {
                            if (Globals.CheckSpaceRewriteNameAvailable(rewritename) == 0)
                                Space.Data.DbProvider.GetInstance().UpdateUserSpaceRewriteName(userid, rewritename);
                            else
                            {
                                AddErrLine("您输入的 个性域名 不可用或含有非法字符");
                                return;
                            }
                        }
                    }
			
					string errorinfo = "";
					Space.Data.DbProvider.GetInstance().SaveSpaceConfigData(spaceconfiginfo);		
					if(errorinfo=="")
					{
						SetShowBackLink(true);
						AddMsgLine("个人空间基本设置完毕");
					}
					else
					{
						AddErrLine(errorinfo);
						return;
					}
				}
			}
			else
			{
                spaceconfiginfo = BlogProvider.GetSpaceConfigInfo(userid);	
			}
		}	
	}
}
