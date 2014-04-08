using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Entity;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// 论坛版块批量设置
    /// </summary>
     
    public partial class forumbatchset : AdminPage
    {
        public ForumInfo forumInfo = new ForumInfo();

        public void LoadCurrentForumInfo(int fid)
        {
            #region 提取基本信息

            if (fid > 0)
            {
                forumInfo = Forums.GetForumInfo(fid);
            }
            else
            {
                return;
            }

            if (forumInfo.Allowsmilies == 1) setting.Items[0].Selected = true;
            if (forumInfo.Allowrss == 1) setting.Items[1].Selected = true;
            if (forumInfo.Allowbbcode == 1) setting.Items[2].Selected = true;
            if (forumInfo.Allowimgcode == 1) setting.Items[3].Selected = true;
            if (forumInfo.Recyclebin == 1) setting.Items[4].Selected = true;
            if (forumInfo.Modnewposts == 1) setting.Items[5].Selected = true;
            if (forumInfo.Modnewtopics == 1) setting.Items[6].Selected = true;
            if (forumInfo.Jammer == 1) setting.Items[7].Selected = true;
            if (forumInfo.Disablewatermark == 1) setting.Items[8].Selected = true;
            if (forumInfo.Inheritedmod == 1) setting.Items[9].Selected = true;
            if (forumInfo.Allowthumbnail == 1) setting.Items[10].Selected = true;
            if (forumInfo.Allowtag == 1) setting.Items[11].Selected = true;
            if ((forumInfo.Allowpostspecial & 1) != 0) setting.Items[12].Selected = true;
            if ((forumInfo.Allowpostspecial & 16) != 0) setting.Items[13].Selected = true;
            if ((forumInfo.Allowpostspecial & 4) != 0) setting.Items[14].Selected = true;
            if ((forumInfo.Alloweditrules == 1)) setting.Items[15].Selected = true;

            //DataTable dt = DatabaseProvider.GetInstance().GetUserGroupsTitle();
            viewperm.SetSelectByID(forumInfo.Viewperm.Trim());
            postperm.SetSelectByID(forumInfo.Postperm.Trim());
            replyperm.SetSelectByID(forumInfo.Replyperm.Trim());
            getattachperm.SetSelectByID(forumInfo.Getattachperm.Trim());
            postattachperm.SetSelectByID(forumInfo.Postattachperm.Trim());

            //dt = DatabaseProvider.GetInstance().GetAttachTypes();
                
            attachextensions.SetSelectByID(forumInfo.Attachextensions.Trim());

            #endregion
        }

        public int BoolToInt(bool a)
        {
            return a ? 1 : 0;
        }

        private void SubmitBatchSet_Click(object sender, EventArgs e)
        {
            #region 写入批量论坛设置信息

            string targetlist = DNTRequest.GetString("Forumtree1");

            if ((targetlist == "") || (targetlist == ",") || (targetlist == "0"))
            {
                base.RegisterStartupScript( "", "<script>alert('您未选中任何版块, 系统无法提交! ');</script>");
                return;
            }

            forumInfo = Forums.GetForumInfo(DNTRequest.GetInt("fid", -1));
            forumInfo.Allowhtml = 0;
            forumInfo.Allowblog = 0;
            forumInfo.Istrade = 0;
            forumInfo.Alloweditrules = 0;
            forumInfo.Allowsmilies = BoolToInt(setting.Items[0].Selected);
            forumInfo.Allowrss = BoolToInt(setting.Items[1].Selected);
            forumInfo.Allowbbcode = BoolToInt(setting.Items[2].Selected);
            forumInfo.Allowimgcode = BoolToInt(setting.Items[3].Selected);
            forumInfo.Recyclebin = BoolToInt(setting.Items[4].Selected);
            forumInfo.Modnewposts = BoolToInt(setting.Items[5].Selected);
            forumInfo.Jammer = BoolToInt(setting.Items[6].Selected);
            forumInfo.Disablewatermark = BoolToInt(setting.Items[7].Selected);
            forumInfo.Inheritedmod = BoolToInt(setting.Items[8].Selected);
            forumInfo.Allowthumbnail = BoolToInt(setting.Items[9].Selected);
            forumInfo.Allowtag = BoolToInt(setting.Items[10].Selected);
            int temppostspecial = 0;
            temppostspecial = setting.Items[11].Selected ? temppostspecial | 1 : temppostspecial & ~1;
            temppostspecial = setting.Items[12].Selected ? temppostspecial | 16 : temppostspecial & ~16;
            temppostspecial = setting.Items[13].Selected ? temppostspecial | 4 : temppostspecial & ~4;
            forumInfo.Allowpostspecial = temppostspecial;
            forumInfo.Alloweditrules = BoolToInt(setting.Items[14].Selected);
            forumInfo.Password = password.Text;
            forumInfo.Attachextensions = attachextensions.GetSelectString(",");
            forumInfo.Viewperm = viewperm.GetSelectString(",");
            forumInfo.Postperm = postperm.GetSelectString(",");
            forumInfo.Replyperm = replyperm.GetSelectString(",");
            forumInfo.Getattachperm = getattachperm.GetSelectString(",");
            forumInfo.Postattachperm = postattachperm.GetSelectString(",");

            BatchSetParams bsp = new BatchSetParams();
            bsp.SetPassWord = setpassword.Checked;
            bsp.SetAttachExtensions = setattachextensions.Checked;
            bsp.SetPostCredits = setpostcredits.Checked;
            bsp.SetReplyCredits = setreplycredits.Checked;
            bsp.SetSetting = setsetting.Checked;
            bsp.SetViewperm = setviewperm.Checked;
            bsp.SetPostperm = setpostperm.Checked;
            bsp.SetReplyperm = setreplyperm.Checked;
            bsp.SetGetattachperm = setgetattachperm.Checked;
            bsp.SetPostattachperm = setpostattachperm.Checked;

            if (AdminForums.BatchSetForumInf(forumInfo, bsp, targetlist))
            {
                ForumOperator.RefreshForumCache();
                AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "复制版块设置", "编辑论坛版块列表为:" + targetlist.Trim());
                base.RegisterStartupScript( "PAGE", "window.location.href='forum_ForumsTree.aspx';");
            }
            else
            {
                base.RegisterStartupScript( "", "<script>alert('提交不成功!');window.location.href='forum_ForumsTree.aspx';</script>");
            }

            #endregion
        }

        #region 把VIEWSTATE写入容器

        protected override void SavePageStateToPersistenceMedium(object viewState)
        {
            base.DiscuzForumSavePageState(viewState);
        }

        protected override object LoadPageStateFromPersistenceMedium()
        {
            return base.DiscuzForumLoadPageState();
        }

        #endregion

        #region Web 窗体设计器生成的代码

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.SubmitBatchSet.Click += new EventHandler(this.SubmitBatchSet_Click);

            #region 控件数据绑定

            DataTable dt = UserGroups.GetUserGroupForDataTable();
            viewperm.AddTableData(dt, "grouptitle", "groupid");
            postperm.AddTableData(dt, "grouptitle", "groupid");
            replyperm.AddTableData(dt, "grouptitle", "groupid");
            getattachperm.AddTableData(dt, "grouptitle", "groupid");
            postattachperm.AddTableData(dt, "grouptitle", "groupid");

            attachextensions.AddTableData(Attachments.GetAttachmentType());
            LoadCurrentForumInfo(DNTRequest.GetInt("fid", -1));

            #endregion
        }

        #endregion
    }
}