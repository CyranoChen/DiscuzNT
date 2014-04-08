using System;
using System.Data;
using System.Data.Common;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Collections;
using System.IO;

using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Cache;


namespace Discuz.Web.Admin
{
    /// <summary>
    /// 鉴定列表
    /// </summary>
    public partial class identifymanage : AdminPage
    {
        private ArrayList fileList = new ArrayList();

        private void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindData();
            }
            BindFilesList();
        }

        public void BindData()
        {
            #region 绑定鉴定主题分类
            identifygrid.AllowCustomPaging = false;
            identifygrid.TableHeaderName = "论坛鉴定列表";
            identifygrid.DataKeyField = "identifyid";
            identifygrid.BindData(Identifys.GetAllIdentify());
            #endregion
        }

        protected void DataGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            identifygrid.LoadCurrentPageIndex(e.NewPageIndex);
        }

        private void DataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            #region 数据绑定显示长度控制

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //TextBox t = (TextBox)e.Item.Cells[1].Controls[0];
                //t.Attributes.Add("maxlength", "50");
                //t.Attributes.Add("style", "width:200px");
            }

            #endregion
        }


        private void DelRec_Click(object sender, EventArgs e)
        {
            #region 删除指定的表情记录

            if (this.CheckCookie())
            {
                if (DNTRequest.GetString("id") != "")
                {
                    string idlist = DNTRequest.GetString("id");
                    Identifys.DeleteIdentify(idlist);
                    AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "鉴定文件删除", idlist);

                    Response.Redirect("forum_identifymanage.aspx");
                }
                else
                {
                    base.RegisterStartupScript( "", "<script>alert('您未选中任何选项');window.location.href='forum_identifymanage.aspx';</script>");
                }
            }

            #endregion
        }


        private void EditIdentify_Click(object sender, EventArgs e)
        {
            #region 保存鉴定主题分类
            int row = 0;
            bool ok = true;
            foreach (object o in identifygrid.GetKeyIDArray())
            {
                if (!Identifys.UpdateIdentifyById(int.Parse(o.ToString()), identifygrid.GetControlValue(row, "name")))
                {
                    ok = false;
                }
                row++;
            }
            AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "鉴定文件修改", "");
            if (!ok)
            {
                base.RegisterStartupScript("", "<script>alert('某些记录未能更新，因为与原有的记录名称相同');window.location.href='forum_identifymanage.aspx';</script>");
            }
            else
            {
                base.RegisterStartupScript("", "<script>window.location.href='forum_identifymanage.aspx';</script>");
            }
            #endregion
        }

        public string PicStr(string filename,int size)
        {
            return "<img src='../../images/identify/" + filename + "'" + (size != 0 ? " height='" + size + "px' width='" + size + "px'" : "") + " border='0' />";
        }

        public string PicStr(string filename)
        {
            return PicStr(filename, 0);
        }

        private ArrayList GetIdentifyFileList()
        {
            #region 获取主题鉴定文件列表
            string path = BaseConfigs.GetForumPath + "images/identify/";
            DirectoryInfo dir = new DirectoryInfo(Utils.GetMapPath(path));
            if (!dir.Exists)
            {
                throw new IOException("鉴定图片文件夹不存在!");
            }
            FileInfo[] files = dir.GetFiles();
            ArrayList temp = new ArrayList();
            foreach (FileInfo file in files)
            {
                temp.Add(file.Name);
            }
            return temp;
            #endregion
        }

        private void BindFilesList()
        {
            #region 绑定主题鉴定文件列表
            try
            {
                fileinfoList.Text = "";
                fileList = GetIdentifyFileList();
                DataTable dt = Identifys.GetAllIdentify();
                foreach (DataRow Identify in dt.Rows)
                {
                    ViewState["code"] += Identify["name"] + ",";
                    fileList.Remove(Identify["filename"].ToString());
                }
                fileList.Remove("Thumbs.db");
                int i = 1;
                foreach (string file in fileList)
                {
                    fileinfoList.Text += "<tr class='mouseoutstyle' onmouseover='this.className=\"mouseoverstyle\"' onmouseout='this.className=\"mouseoutstyle\"' >\n";
                    fileinfoList.Text += "<td nowrap='nowrap' style='border-color:#EAE9E1;border-width:1px;border-style:solid;width:50px;' align='center'><input type='checkbox' id='id" + i + "' name='id" + i + "' value='" + i + "'/></td>\n";
                    fileinfoList.Text += "<td nowrap='nowrap' style='border-color:#EAE9E1;border-width:1px;border-style:solid;width:50px;' align='left'><input type='text' id='name" + i + "' name='name" + i + "' value='鉴定帖" + i + "' class=\"FormBase\" onfocus=\"this.className='FormFocus';\" onblur=\"this.className='FormBase';\" style='width:200px' /></td>\n";
                    fileinfoList.Text += "<td nowrap='nowrap' style='border-color:#EAE9E1;border-width:1px;border-style:solid;width:50px;'><input type='hidden' name='file" + i + "' value='" + file + "' /><div id='ilayer" + i + "' onmouseover='showMenu(this.id,false)'>" + PicStr(file, 20) + "</div>";
                    fileinfoList.Text += "<div id='ilayer" + i + "_menu' style='display:none'>" + PicStr(file) + "</div></td>\n";
                    fileinfoList.Text += "</tr>\n";
                    i++;
                }
                SubmitButton.Visible = fileList.Count != 0;
            }
            catch (IOException err)
            {
                base.RegisterStartupScript("", "<script>alert('" + err.Message + "');window.location.href='forum_identifymanage.aspx';</script>");
            }
            #endregion
        }

        //private void UpDateInfo(string active,string other)
        //{
        //    AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, active, other);
        //}
        public void SubmitButton_Click(object sender, EventArgs e)
        {
            #region 保存新建主题鉴定
            bool ok = true;
            for (int i = 1; i <= fileList.Count; i++)
            {
                if (DNTRequest.GetFormString("id" + i) != "")
                {
                    try
                    {
                        if (!Identifys.AddIdentify(DNTRequest.GetString("name" + i), DNTRequest.GetString("file" + i)))
                        {
                            ok = false;
                        }
                    }
                    catch
                    {
                        base.RegisterStartupScript( "", "<script>alert('出现错误，可能名称超出长度！');window.location.href='forum_identifymanage.aspx';</script>");
                    }
                }
            }
            AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "鉴定文件增加", "");
            if (!ok)
            {
                base.RegisterStartupScript("", "<script>alert('某些记录未能插入，因为与数据库中原有的名称相同');window.location.href='forum_identifymanage.aspx';</script>");
            }
            else
            {
                base.RegisterStartupScript("", "<script>window.location.href='forum_identifymanage.aspx';</script>");
            }
            #endregion
        }

        #region Web Form Designer generated code

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.DelRec.Click += new EventHandler(this.DelRec_Click);
            this.identifygrid.ItemDataBound += new DataGridItemEventHandler(this.DataGrid_ItemDataBound);
            this.EditIdentify.Click += new EventHandler(this.EditIdentify_Click);
            this.SubmitButton.Click += new EventHandler(this.SubmitButton_Click);
            this.SubmitButton.Attributes.Add("onclick", "return validate()");
            identifygrid.ColumnSpan = 3;
        }

        #endregion


    }
}
