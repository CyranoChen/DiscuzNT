using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Collections;
using System.IO;

using Discuz.Common;
using Discuz.Forum;
using Button = Discuz.Control.Button;
using DataGrid = Discuz.Control.DataGrid;
using Discuz.Config;


namespace Discuz.Web.Admin
{
    /// <summary>
    /// 勋章列表
    /// </summary>
    public partial class medalgrid : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindData();
            }
        }

        public void BindData()
        {
            #region 数据绑定

            DataGrid1.AllowCustomPaging = false;
            DataGrid1.TableHeaderName = "论坛勋章列表";
            DataGrid1.BindData(Medals.GetMedal());

            #endregion
        }

        private void DataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            #region 设置数据绑定的长度

            if (e.Item.ItemType == ListItemType.EditItem)
            {
                TextBox t = (TextBox)e.Item.Cells[3].Controls[0];
                t.Attributes.Add("maxlength", "50");
                t.Attributes.Add("size", "30");

                t = (TextBox)e.Item.Cells[4].Controls[0];
                t.Attributes.Add("maxlength", "30");
                t.Attributes.Add("size", "30");
            }

            #endregion
        }

        private void SaveMedal_Click(object send, EventArgs e)
        {
            #region 保存勋章信息修改
            int row = 0;
            bool error = false;
            foreach (object o in DataGrid1.GetKeyIDArray())
            {
                int id = int.Parse(o.ToString());
                string name = DataGrid1.GetControlValue(row, "name").Trim();
                string image = DataGrid1.GetControlValue(row, "image").Trim();
                if (name == "" || image == "")
                {
                    error = true;
                    continue;
                }
                Medals.UpdateMedal(id, name, image);
                row++;
            }
            AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "批量更新勋章信息", "");
            Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/UI/MedalsList");
            if(error)
                base.RegisterStartupScript("PAGE", "alert('某些信息不完整，未能更新！');window.location.href='global_medalgrid.aspx';");
            else
                base.RegisterStartupScript("PAGE", "window.location.href='global_medalgrid.aspx';");
            #endregion
        }

        private void Available_Click(object sender, EventArgs e)
        {
            #region 将选取的勋章设置为无效

            if (DNTRequest.GetString("medalid") != "")
            {
                Medals.SetAvailableForMedal(1, DNTRequest.GetString("medalid"));
                AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "勋章文件为有效", "ID:" + DNTRequest.GetString("medalid").Replace("0 ", ""));
                Response.Redirect("global_medalgrid.aspx");
            }
            else
            {
                base.RegisterStartupScript("", "<script>alert('您未选中任何选项');window.location.href='global_medalgrid.aspx';</script>");
            }

            #endregion
        }

        private void ImportMedal_Click(object sender, EventArgs e)
        {
            #region 将已经上传并未存在于勋章列表中的勋章入库
            Medals.UpdateMedalList(GetMedalFileList());
            //ArrayList medalFiles = GetMedalFileList(); //获取勋章文件夹下所有文件列表
            //medalFiles.Remove("Thumbs.db".ToLower());
            //DataTable dt = Medals.GetExistMedalList();
            //foreach (DataRow dr in dt.Rows)
            //{
            //    medalFiles.Remove(dr["image"].ToString().ToLower()); //移除已经存在于勋章库中的勋章文件名
            //}
            //int newMedalBaseId = Convert.ToInt32(dt.Rows[dt.Rows.Count - 1]["medalid"].ToString()) + 1; //获取新的Medalid的基数
            //for (int i = 0; i < medalFiles.Count; i++) //将未入库的勋章入库
            //{
            //    int newMedalId = newMedalBaseId + i;
            //    Medals.UpdateMedal(newMedalId, "Medal No." + newMedalId, medalFiles[i].ToString());
            //}
            //Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/UI/MedalsList");
            Response.Redirect("global_medalgrid.aspx");
            #endregion
        }

        private ArrayList GetMedalFileList()
        {
            #region 获取勋章文件列表
            string path = BaseConfigs.GetForumPath + "images/medals";
            DirectoryInfo dir = new DirectoryInfo(Utils.GetMapPath(path));
            if (!dir.Exists)
            {
                throw new IOException("勋章文件夹不存在!");
            }
            FileInfo[] files = dir.GetFiles();
            ArrayList temp = new ArrayList();
            foreach (FileInfo file in files)
            {
                temp.Add(file.Name.ToLower());
            }
            return temp;
            #endregion
        }

        private void UnAvailable_Click(object sender, EventArgs e)
        {
            #region 将选取的勋章设置为无效

            if (DNTRequest.GetString("medalid") != "")
            {
                Medals.SetAvailableForMedal(0, DNTRequest.GetString("medalid"));
                AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "勋章文件为无效", "ID:" + DNTRequest.GetString("medalid"));
                Response.Redirect("global_medalgrid.aspx");
            }
            else
            {
                base.RegisterStartupScript("", "<script>alert('您未选中任何选项');window.location.href='global_medalgrid.aspx';</script>");
            }

            #endregion
        }

        protected void Sort_Grid(Object sender, DataGridSortCommandEventArgs e)
        {
            DataGrid1.Sort = e.SortExpression.ToString();
        }

        protected void DataGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid1.LoadCurrentPageIndex(e.NewPageIndex);
        }
        
        public string PicStr(string filename)
        {
            if (filename != "")
            {
                return "<img src=../../images/medals/" + filename + " height=25px width=14px />";
            }
            else
            {
                return "";
            }
        }

        #region Web Form Designer generated code

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.Available.Click += new EventHandler(this.Available_Click);
            this.UnAvailable.Click += new EventHandler(this.UnAvailable_Click);
            this.ImportMedal.Click += new EventHandler(this.ImportMedal_Click);
            this.SaveMedal.Click += new EventHandler(this.SaveMedal_Click);

            this.DataGrid1.ItemDataBound += new DataGridItemEventHandler(this.DataGrid_ItemDataBound);

            DataGrid1.TableHeaderName = "论坛勋章列表";
            DataGrid1.DataKeyField = "medalid";
            DataGrid1.PageSize = 15;
            DataGrid1.ColumnSpan = 6;
        }

        #endregion
    }
}
