using System;
using System.Web.UI;

using Discuz.Cache;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Plugin.Mall;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// 删除版块
    /// </summary>
    public partial class delforums : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                #region 按FID删除相应的版块

                if(DNTRequest.GetString("istrade") == "1")
                {
                    //如果是交易版块，则选清除绑定的商品
                    if(MallPluginProvider.GetInstance() != null)
                    {
                        MallPluginProvider.GetInstance().EmptyGoodsCategoryFid(DNTRequest.GetInt("fid",0));
                        MallPluginProvider.GetInstance().StaticWriteJsonFile();
                        DNTCache.GetCacheService().RemoveObject("/Mall/MallSetting/GoodsCategories");
                    }
                }

                if (Forums.DeleteForum(DNTRequest.GetString("fid")))
                {
                    ForumOperator.RefreshForumCache();
                    AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "删除论坛版块", "删除论坛版块,fid为:" + DNTRequest.GetString("fid"));
                    base.RegisterStartupScript( "", "<script>window.location.href='forum_ForumsTree.aspx';</script>");
                }
                else
                {
                    base.RegisterStartupScript("", "<script>alert('对不起,当前节点下面还有子结点,因此不能删除！');window.location.href='forum_ForumsTree.aspx';</script>");
                }

                #endregion
            }
        }
    }
}