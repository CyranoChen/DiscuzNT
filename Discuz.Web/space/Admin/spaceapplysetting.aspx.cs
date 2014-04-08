using System;
using System.Web.UI.WebControls;
using System.Xml;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Web.UI;

using Discuz.Forum;
using Button = Discuz.Control.Button;
using CheckBoxList = Discuz.Control.CheckBoxList;
using TextBox = Discuz.Control.TextBox;
using Discuz.Config;
using Discuz.Data;
using Discuz.Common.Xml;
using Discuz.Common;
using Discuz.Web.Admin;
using Discuz.Space.Data;


namespace Discuz.Space.Admin
{
	
#if NET1
    public class SpaceApplySetting : AdminPage
#else
    public partial class SpaceApplySetting : AdminPage
#endif
    {

#if NET1
        #region 控件声明
        protected Discuz.Control.TextBox Postcount;
        protected Discuz.Control.TextBox Digestcount;
        protected Discuz.Control.TextBox Score;
        protected Discuz.Control.CheckBoxList UserGroup;
        protected Discuz.Control.RadioButtonList ActiveType;
        protected Discuz.Control.Button Submit;
		protected Discuz.Control.RadioButtonList EnableSpace;

        protected System.Web.UI.WebControls.Panel searchtable;
        protected System.Web.UI.WebControls.CheckBox allowPostcount;
        protected System.Web.UI.WebControls.CheckBox allowDigestcount;
        protected System.Web.UI.WebControls.CheckBox allowScore;
        protected System.Web.UI.WebControls.CheckBox allowUserGroup;
        protected System.Web.UI.HtmlControls.HtmlGenericControl ShowSpaceOption;
        protected System.Web.UI.HtmlControls.HtmlGenericControl ShowUserGroup;
		protected System.Web.UI.HtmlControls.HtmlTable groupattachsize;
        #endregion
#endif


        protected void Page_Load(object sender, EventArgs e)
		{
			// 在此处放置用户代码以初始化页面
            allowUserGroup.Attributes.Add("onclick", "ChanageUserGroupStatus(this.checked)");
			if (!IsPostBack)
			{
                EnableSpace.SelectedValue = config.Enablespace.ToString();
                EnableSpace.Items[0].Attributes.Add("onclick", "ShowHiddenOption(true);");
                EnableSpace.Items[1].Attributes.Add("onclick", "ShowHiddenOption(false);");
                ShowSpaceOption.Attributes.Add("style", config.Enablespace == 1 ? "display:block" : "display:none");
                ShowUserGroup.Attributes.Add("style", config.Enablespace == 1 ? "display:block" : "display:none");
				LoadUserGroup();
                XmlDocumentExtender xmlDoc = new XmlDocumentExtender();
				xmlDoc.Load(Server.MapPath("../../config/space.config"));
                XmlNode root = xmlDoc.SelectSingleNode("SpaceActiveConfigInfo");
                XmlNodeInnerTextVisitor rootvisitor = new XmlNodeInnerTextVisitor();
                rootvisitor.SetNode(root);
                allowPostcount.Checked = rootvisitor["AllowPostcount"] == "1" ? true : false;
                Postcount.Text = rootvisitor["Postcount"];
                allowDigestcount.Checked = rootvisitor["AllowDigestcount"] == "1" ? true : false;
                Digestcount.Text = rootvisitor["Digestcount"];
                allowScore.Checked = rootvisitor["AllowScore"] == "1" ? true : false;
                Score.Text = rootvisitor["Score"];
                allowUserGroup.Checked = rootvisitor["AllowUsergroups"] == "1" ? true : false;
                string groupList = rootvisitor["Usergroups"];
				if (!allowUserGroup.Checked || groupList == "")
					return;
				else
					BindUserGroup(groupList);
                ActiveType.SelectedValue = rootvisitor["ActiveType"];
                BindUserGorupMaxspaceattachsize();
			}
		}

		private void LoadUserGroup()
		{
            UserGroup.DataSource = DatabaseProvider.GetInstance().GetUserGroupsTitle();
			UserGroup.DataValueField = "groupid";
			UserGroup.DataTextField = "grouptitle";
			UserGroup.DataBind();
		}

        private void BindUserGorupMaxspaceattachsize()
        {
            DataTable dt = DatabaseProvider.GetInstance().GetUserGroupMaxspaceattachsize();
            int i = 1;
            HtmlTableRow tr = new HtmlTableRow();
            foreach (DataRow dr in dt.Rows)
            {
                if (i % 2 == 1)
                {
                    tr = new HtmlTableRow();
                }
                HtmlTableCell td = new HtmlTableCell("td");
                td.Controls.Add(new LiteralControl(dr["grouptitle"].ToString()));
                tr.Cells.Add(td);
                td = new HtmlTableCell("td");
                Discuz.Control.TextBox tb = new Discuz.Control.TextBox();
                tb.ID = "maxspaceattachsize" + dr["groupid"].ToString();
                tb.Size = 10;
                tb.MaxLength = 9;
                tb.Text = dr["maxspaceattachsize"].ToString();
                tb.RequiredFieldType = "数据校验";
                td.Controls.Add(tb);
                tr.Cells.Add(td);
                tr.Cells.Add(GetTD("maxspaceattachsize" + dr["groupid"].ToString()));
                groupattachsize.Rows.Add(tr);
                i++;
            }

        }

        private HtmlTableCell GetTD(string targetId)
        {
            LiteralControl select = new LiteralControl();
            select.Text = "<select onchange=\"document.getElementById('" + targetId + "').value=this.value\">\n<option value=\"\">请选择</option>\n";
            select.Text += "<option value=\"51200\">50K</option>\n<option value=\"102400\">100K</option>\n<option value=\"153600\">150K</option>\n";
            select.Text += "<option value=\"204800\">200K</option>\n<option value=\"256000\">250K</option>\n<option value=\"307200\">300K</option>\n";
            select.Text += "<option value=\"358400\">350K</option>\n<option value=\"409600\">400K</option>\n<option value=\"512000\">500K</option>\n";
            select.Text += "<option value=\"614400\">600K</option>\n<option value=\"716800\">700K</option>\n<option value=\"819200\">800K</option>\n";
            select.Text += "<option value=\"921600\">900K</option>\n<option value=\"1024000\">1M</option>\n<option value=\"2048000\">2M</option>\n";
            select.Text += "<option value=\"4096000\">4M</option>\n<option value=\"6144000\">6M</option>\n<option value=\"8192000\">8M</option>\n";
            select.Text += "<option value=\"10240000\">10M</option>\n<option value=\"12288000\">12M</option>\n<option value=\"14336000\">14M</option>\n";
            select.Text += "<option value=\"16384000\">16M</option>\n<option value=\"18432000\">18M</option>\n<option value=\"20480000\">20M</option>\n";
            select.Text += "<option value=\"22528000\">22M</option>\n<option value=\"24576000\">24M</option>\n<option value=\"26624000\">26M</option>\n";
            select.Text += "<option value=\"28672000\">28M</option>\n<option value=\"30720000\">30M</option></select>";
            HtmlTableCell td = new HtmlTableCell("td");
            td.Controls.Add(select);
            return td;

        }

		private void BindUserGroup(string groupList)
		{
			string[] list = groupList.Split(',');
			foreach (string id in list)
			{
				for (int i = 0; i < UserGroup.Items.Count; i++)
				{
					if (UserGroup.Items[i].Value == id)
					{
						UserGroup.Items[i].Selected = true;
						break;
					}
				}
			}
		}

		public void Submit_Click(object sender, EventArgs e)
		{
            config.Enablespace = int.Parse(EnableSpace.SelectedValue);
            GeneralConfigs.Serialiaze(config, Server.MapPath("../../config/general.config"));
            if (!Utils.IsInt(Postcount.Text))
            {

                base.RegisterStartupScript("", "<script>alert('论坛发帖数超过输入错误,请检查');window.location.href='space_spaceapplysetting.aspx';</script>");
                return;
            }
            if (!Utils.IsInt(Digestcount.Text))
            {

                base.RegisterStartupScript("", "<script>alert('论坛精华帖数输入错误,请检查');window.location.href='space_spaceapplysetting.aspx';</script>");
                return;
            }
            if (!Utils.IsInt(Score.Text))
            {

                base.RegisterStartupScript("", "<script>alert('论坛用户积分输入错误,请检查');window.location.href='space_spaceapplysetting.aspx';</script>");
                return;
            }


            if (config.Enablespace == 1)
            {
                XmlDocument xmlDoc = new XmlDocument();
                string filePath = Server.MapPath("../../config/space.config");
                xmlDoc.Load(filePath);
                XmlNode root = xmlDoc.SelectSingleNode("SpaceActiveConfigInfo");
                XmlNodeInnerTextVisitor rootvisitor = new XmlNodeInnerTextVisitor();
                rootvisitor.SetNode(root);
                rootvisitor["AllowPostcount"] = allowPostcount.Checked ? "1" : "0";
                rootvisitor["Postcount"] = Postcount.Text;
                rootvisitor["AllowDigestcount"] = allowDigestcount.Checked ? "1" : "0";
                rootvisitor["Digestcount"] = Digestcount.Text;
                rootvisitor["AllowScore"] = allowScore.Checked ? "1" : "0";
                rootvisitor["Score"] = Score.Text;
                rootvisitor["AllowUsergroups"] = allowUserGroup.Checked ? "1" : "0";
                string groupList = "";
                for (int i = 0; i < UserGroup.Items.Count; i++)
                {
                    if (UserGroup.Items[i].Selected)
                    {
                        groupList += UserGroup.Items[i].Value + ",";
                    }
                }
                if (groupList == "")
                    rootvisitor["Usergroups"] = "";
                else
                    rootvisitor["Usergroups"] = groupList.Substring(0, groupList.Length - 1);
                rootvisitor["ActiveType"] = ActiveType.SelectedValue;
                xmlDoc.Save(filePath);
                //保存个人空间最大附件空间
                DataTable dt = DatabaseProvider.GetInstance().GetUserGroupMaxspaceattachsize();
                foreach (DataRow dr in dt.Rows)
                {
                    if (!Utils.IsInt(DNTRequest.GetString("maxspaceattachsize" + dr["groupid"].ToString()).ToString()))
                    {
                        base.RegisterStartupScript("", "<script>alert('输入错误,空间附件最大空间只能是0或者正整数');window.location.href='space_spaceapplysetting.aspx';</script>");
                        return;
                    } 

                    int attachsize = DNTRequest.GetInt("maxspaceattachsize" + dr["groupid"].ToString(), 0);
                    Discuz.Entity.UserGroupInfo __usergroupinfo = AdminUserGroups.AdminGetUserGroupInfo(Convert.ToInt32(dr["groupid"].ToString()));
                    __usergroupinfo.Maxspaceattachsize = attachsize;
                    AdminUserGroups.UpdateUserGroupInfo(__usergroupinfo);
                }
                Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/UserGroupList");
            }
			Response.Redirect("space_spaceapplysetting.aspx");
		}
	}
}