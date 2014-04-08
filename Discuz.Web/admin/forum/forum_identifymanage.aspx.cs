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
    /// �����б�
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
            #region �󶨼����������
            identifygrid.AllowCustomPaging = false;
            identifygrid.TableHeaderName = "��̳�����б�";
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
            #region ���ݰ���ʾ���ȿ���

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
            #region ɾ��ָ���ı����¼

            if (this.CheckCookie())
            {
                if (DNTRequest.GetString("id") != "")
                {
                    string idlist = DNTRequest.GetString("id");
                    Identifys.DeleteIdentify(idlist);
                    AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "�����ļ�ɾ��", idlist);

                    Response.Redirect("forum_identifymanage.aspx");
                }
                else
                {
                    base.RegisterStartupScript( "", "<script>alert('��δѡ���κ�ѡ��');window.location.href='forum_identifymanage.aspx';</script>");
                }
            }

            #endregion
        }


        private void EditIdentify_Click(object sender, EventArgs e)
        {
            #region ��������������
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
            AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "�����ļ��޸�", "");
            if (!ok)
            {
                base.RegisterStartupScript("", "<script>alert('ĳЩ��¼δ�ܸ��£���Ϊ��ԭ�еļ�¼������ͬ');window.location.href='forum_identifymanage.aspx';</script>");
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
            #region ��ȡ��������ļ��б�
            string path = BaseConfigs.GetForumPath + "images/identify/";
            DirectoryInfo dir = new DirectoryInfo(Utils.GetMapPath(path));
            if (!dir.Exists)
            {
                throw new IOException("����ͼƬ�ļ��в�����!");
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
            #region ����������ļ��б�
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
                    fileinfoList.Text += "<td nowrap='nowrap' style='border-color:#EAE9E1;border-width:1px;border-style:solid;width:50px;' align='left'><input type='text' id='name" + i + "' name='name" + i + "' value='������" + i + "' class=\"FormBase\" onfocus=\"this.className='FormFocus';\" onblur=\"this.className='FormBase';\" style='width:200px' /></td>\n";
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
            #region �����½��������
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
                        base.RegisterStartupScript( "", "<script>alert('���ִ��󣬿������Ƴ������ȣ�');window.location.href='forum_identifymanage.aspx';</script>");
                    }
                }
            }
            AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "�����ļ�����", "");
            if (!ok)
            {
                base.RegisterStartupScript("", "<script>alert('ĳЩ��¼δ�ܲ��룬��Ϊ�����ݿ���ԭ�е�������ͬ');window.location.href='forum_identifymanage.aspx';</script>");
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
