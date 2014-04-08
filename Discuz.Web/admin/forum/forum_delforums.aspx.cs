using System;
using System.Web.UI;

using Discuz.Cache;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Plugin.Mall;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// ɾ�����
    /// </summary>
    public partial class delforums : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                #region ��FIDɾ����Ӧ�İ��

                if(DNTRequest.GetString("istrade") == "1")
                {
                    //����ǽ��װ�飬��ѡ����󶨵���Ʒ
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
                    AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "ɾ����̳���", "ɾ����̳���,fidΪ:" + DNTRequest.GetString("fid"));
                    base.RegisterStartupScript( "", "<script>window.location.href='forum_ForumsTree.aspx';</script>");
                }
                else
                {
                    base.RegisterStartupScript("", "<script>alert('�Բ���,��ǰ�ڵ����滹���ӽ��,��˲���ɾ����');window.location.href='forum_ForumsTree.aspx';</script>");
                }

                #endregion
            }
        }
    }
}