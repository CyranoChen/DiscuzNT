using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI;

using Discuz.Common;
using Discuz.Forum;
using Discuz.Space;

using Button = Discuz.Control.Button;
using CheckBoxList = Discuz.Control.CheckBoxList;
using DataGrid = Discuz.Control.DataGrid;
using DropDownList = Discuz.Control.DropDownList;
using TextBox = Discuz.Control.TextBox;
using Discuz.Control;
using Discuz.Config;
using Discuz.Entity;
#if NET1
#else
using Discuz.Common.Generic;
using Discuz.Web.Admin;
using Discuz.Space.Data;
#endif

namespace Discuz.Space.Admin
{
	
#if NET1
    public class SpaceApplyManage : AdminPage
#else
    public partial class SpaceApplyManage : AdminPage
#endif
    {

#if NET1
        #region 控件声明
        protected Discuz.Control.Button PassApply;
        protected Discuz.Control.Button DeleteApply;
        protected Discuz.Control.DataGrid DataGrid1;
        #endregion
#endif
        private SpaceActiveConfigInfo spaceConfig = SpaceActiveConfigs.GetConfig();

        private void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				BindData();
			}
		}

		public void BindData()
		{
//			DataGrid1.AllowCustomPaging = true;
			//DataGrid1.VirtualItemCount = RecordCount();
			DataGrid1.DataSource = buildGridData();
			DataGrid1.DataBind();
		}


		protected void DataGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
		{
			DataGrid1.CurrentPageIndex = e.NewPageIndex;
			BindData();
		}

		private DataTable buildGridData()
		{
            return DbProvider.GetInstance().GetUnActiveSpaceList();
		}


		public void GoToPagerButton_Click(object sender, EventArgs e)
		{
			BindData();
		}

		#region Web Form Designer generated code

		protected override void OnInit(EventArgs e)
		{
			InitializeComponent();
			base.OnInit(e);
		}

		private void InitializeComponent()
		{
			this.PassApply.Click += new EventHandler(this.PassApply_Click);
			this.DeleteApply.Click += new EventHandler(this.DeleteApply_Click);
			this.DataGrid1.GoToPagerButton.Click += new EventHandler(GoToPagerButton_Click);

			this.Load += new EventHandler(this.Page_Load);
			DataGrid1.TableHeaderName = "空间申请列表";
			DataGrid1.DataKeyField = "uid";
			DataGrid1.AllowSorting = false;
			DataGrid1.ColumnSpan = 5;
		}

		#endregion


		private void PassApply_Click(object sender, EventArgs e)
		{
			if (DNTRequest.GetString("uid") != "")
			{
				string uidlist = DNTRequest.GetString("uid");
				foreach(string uid in uidlist.Split(','))
				{
                    Discuz.Data.DatabaseProvider.GetInstance().UpdateUserSpaceId(Convert.ToInt32(uid));
					//string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [spaceid]=ABS([spaceid]) WHERE [uid]=" + uid;
					//sql += ";SELECT [spaceid] FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [uid]=" + uid;
					//int spceid = (int)AdminDatabase.ExecuteScalar(sql);
                    int tabid = Spaces.GetNewTabId(Convert.ToInt32(uid));
                    TabInfo tab = new TabInfo();
                    tab.TabID = tabid;
                    tab.UserID = Convert.ToInt32(uid);
                    tab.DisplayOrder = 0;
                    tab.TabName = "首页";
                    tab.IconFile = "";
                    tab.Template = "template_25_75.htm";
                    Spaces.AddTab(tab);
                    //sql = "INSERT INTO [" + BaseConfigs.GetTablePrefix + "spacetabs] ([uid],[displayorder], [tabname], [iconfile],[template])  VALUES(" + uid + ",0,'默认','','template_25_75.htm');SELECT SCOPE_IDENTITY()";
                    //int tabid = Utils.StrToInt(AdminDatabase.ExecuteScalar(sql),0);
					Spaces.AddLocalModule("builtin_calendarmodule.xml",int.Parse(uid),tabid,1);
                    Spaces.AddLocalModule("builtin_statisticmodule.xml", int.Parse(uid), tabid, 1);
                    Spaces.AddLocalModule("builtin_postlistmodule.xml", int.Parse(uid), tabid, 2);

                    if (spaceConfig.Spacegreeting != string.Empty)
                    {
                        SpacePostInfo spacepostsinfo = new SpacePostInfo();
                        spacepostsinfo.Title = string.Format("欢迎使用 {0} {1}", config.Forumtitle, config.Spacename);
                        spacepostsinfo.Content = spaceConfig.Spacegreeting;
                        spacepostsinfo.Category = string.Empty;
                        spacepostsinfo.PostStatus = 1;
                        spacepostsinfo.CommentStatus = 0;
                        spacepostsinfo.Postdatetime = DateTime.Now;
                        spacepostsinfo.Author = username;
                        spacepostsinfo.Uid = Utils.StrToInt(uid, -1);
                        spacepostsinfo.PostUpDateTime = DateTime.Now;
                        spacepostsinfo.Commentcount = 0;

                        DbProvider.GetInstance().AddSpacePost(spacepostsinfo);
                    }
                    ///添加最新主题到日志
                    List<TopicInfo> list = Topics.GetTopicsByUserId(userid, 1, config.Topictoblog, 0, 0);
                    foreach (TopicInfo mytopic in list)
                    {
                        int pid = Posts.GetFirstPostId(mytopic.Tid);
                        PostInfo post = Posts.GetPostInfo(mytopic.Tid, pid);
                        if (post != null && post.Message.Trim() != string.Empty)
                        {
                            SpacePostInfo spacepost = new SpacePostInfo();
                            spacepost.Author = username;
                            string content = Posts.GetPostMessageHTML(post, new AttachmentInfo[0]);
                            spacepost.Category = "";
                            spacepost.Content = content;
                            spacepost.Postdatetime = DateTime.Now;
                            spacepost.PostStatus = 1;
                            spacepost.PostUpDateTime = DateTime.Now;
                            spacepost.Title = post.Title;
                            spacepost.Uid = Utils.StrToInt(uid, -1); ;

                            DbProvider.GetInstance().AddSpacePost(spacepost);
                        }
                    }

					//短信通知用户个人空间已经开通
                    PrivateMessageInfo privatemessageinfo = new PrivateMessageInfo();
                    ShortUserInfo userInfo = Users.GetShortUserInfo(Convert.ToInt32(uid));
                    privatemessageinfo.Msgto = userInfo == null ? "" : userInfo.Username;
                    privatemessageinfo.Msgtoid = Convert.ToInt32(uid);
                    privatemessageinfo.Postdatetime = Utils.GetDateTime();
                    privatemessageinfo.Message = "恭喜您，您的" + config.Spacename + "已经被管理员" + username + "开通！";
                    privatemessageinfo.Subject = "恭喜，您的" + config.Spacename + "已经开通";
                    privatemessageinfo.Msgfrom = username;
                    privatemessageinfo.Msgfromid = userid;
                    privatemessageinfo.New = 1;
                    privatemessageinfo.Folder = 0;
                    
                    PrivateMessages.CreatePrivateMessage(privatemessageinfo, 0);
					//AdminDatabase.ExecuteNonQuery("INSERT INTO [" + BaseConfigs.GetTablePrefix + "pms] (msgfrom,msgfromid,msgto,msgtoid,folder,new,subject,postdatetime,message) VALUES ('" + username + "','" + userid + "','" + msgto.Replace("'", "''") + "','" + uid + "','0','1','" + subject + "','" + curdatetime + "','" + message + "')");
					//AdminDatabase.ExecuteNonQuery("UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [newpmcount]=[newpmcount]+1  WHERE [uid] =" + uid);

				}
                base.RegisterStartupScript( "PAGE", "window.location.href='space_spaceapplymanage.aspx';");
			}
			else
			{
                base.RegisterStartupScript( "", "<script>alert('请选择要开通的空间!');window.location.href='space_spaceapplymanage.aspx';</script>");
			}
		}


		private void DeleteApply_Click(object sender, EventArgs e)
		{
			if (this.CheckCookie())
			{
				if (DNTRequest.GetString("uid") != "")
				{
                    DbProvider.GetInstance().DeleteSpaces(DNTRequest.GetString("uid"));
				}
				else
				{
                    base.RegisterStartupScript( "", "<script>alert('请选择要删除的空间!');window.location.href='space_spaceapplymanage.aspx';</script>");
				}
			}
		}



	}
}