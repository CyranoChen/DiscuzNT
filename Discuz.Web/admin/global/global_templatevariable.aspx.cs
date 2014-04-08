using System;
using System.Data;
using System.IO;
using System.Web.UI.WebControls;
using System.Web.UI;

using Discuz.Common;
using Discuz.Config;
using Discuz.Forum;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// 模板变量编辑列表
    /// </summary>
    public partial class templatevariable : AdminPage
    {
        public DataSet dsSrc = new DataSet();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if ((DNTRequest.GetString("path") != null) && (DNTRequest.GetString("path") != ""))
                {
                    BindData();
                }
            }
        }


        public void BindData()
        {
            DataGrid1.AllowCustomPaging = false;
            DataGrid1.DataSource = LoadDataTable();
            DataGrid1.DataBind();
        }

        protected void Sort_Grid(Object sender, DataGridSortCommandEventArgs e)
        {
            DataGrid1.Sort = e.SortExpression.ToString();
        }

        protected void SaveVar_Click(Object sender, EventArgs e)
        {
            #region 保存变量修改
            dsSrc = LoadDataTable();
            int row = 0;
            //bool error = false;
            foreach (object o in DataGrid1.GetKeyIDArray())
            {
                int id = int.Parse(o.ToString());
                string variablename = DataGrid1.GetControlValue(row, "variablename").Trim();
                string variablevalue = DataGrid1.GetControlValue(row, "variablevalue").Trim();
                if (variablename == "" || variablevalue == "")
                {
                    //error = true;
                    continue;
                }
                foreach (DataRow dr in dsSrc.Tables["TemplateVariable"].Rows)
                {
                    if (id.ToString() == dr["id"].ToString())
                    {
                        dr["variablename"] = variablename;
                        dr["variablevalue"] = variablevalue;
                        break;
                    }
                }
                try
                {
                    if (dsSrc.Tables[0].Rows.Count == 0)
                    {
                        File.Delete(Utils.GetMapPath("../../templates/" + DNTRequest.GetString("path") + "/templatevariable.xml"));
                        dsSrc.Reset();
                        dsSrc.Dispose();
                    }
                    else
                    {
                        string filename = Server.MapPath("../../templates/" + DNTRequest.GetString("path") + "/templatevariable.xml");
                        dsSrc.WriteXml(filename);
                        dsSrc.Reset();
                        dsSrc.Dispose();

                        Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();
                        cache.RemoveObject("/Forum/" + DNTRequest.GetString("path") + "/TemplateVariable");
                        base.RegisterStartupScript("PAGE", "window.location.href='global_templatevariable.aspx?templateid=" + DNTRequest.GetString("templateid") + "&path=" + DNTRequest.GetString("path") + "&templatename=" + DNTRequest.GetString("templatename") + "';");
                    }
                }
                catch
                {
                    base.RegisterStartupScript("", "<script>alert('无法更新数据库.');window.location.href='global_templatevariable.aspx?templateid=" + DNTRequest.GetString("templateid") + "&path=" + DNTRequest.GetString("path") + "&templatename=" + DNTRequest.GetString("templatename") + "';</script>");
                    return;
                }
                row++;
            }
            #endregion
        }

        protected void DelRec_Click(Object sender, EventArgs e)
        {
            #region 删除指定的模板变量

            dsSrc = LoadDataTable();
            string idlist = DNTRequest.GetString("delid");
            foreach (string id in idlist.Split(','))
            {
                foreach (DataRow dr in dsSrc.Tables["TemplateVariable"].Rows)
                {
                    if (id == dr["id"].ToString())
                    {
                        dsSrc.Tables[0].Rows.Remove(dr);
                        break;
                    }
                }
            }

            try
            {
                if (dsSrc.Tables[0].Rows.Count == 0)
                {
                    File.Delete(Utils.GetMapPath("../../templates/" + DNTRequest.GetString("path") + "/templatevariable.xml"));
                    dsSrc.Reset();
                    dsSrc.Dispose();
                }
                else
                {
                    string filename = Server.MapPath("../../templates/" + DNTRequest.GetString("path") + "/templatevariable.xml");
                    dsSrc.WriteXml(filename);
                    dsSrc.Reset();
                    dsSrc.Dispose();

                    Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();
                    cache.RetrieveObject("/Forum/" + DNTRequest.GetString("path") + "/TemplateVariable");

                    base.RegisterStartupScript("PAGE", "window.location.href='global_templatevariable.aspx?templateid=" + DNTRequest.GetString("templateid") + "&path=" + DNTRequest.GetString("path") + "&templatename=" + DNTRequest.GetString("templatename") + "';");
                }
            }
            catch
            {
                base.RegisterStartupScript("", "<script>alert('无法更新数据库.');window.location.href='global_templatevariable.aspx?templateid=" + DNTRequest.GetString("templateid") + "&path=" + DNTRequest.GetString("path") + "&templatename=" + DNTRequest.GetString("templatename") + "';</script>");
                return;
            }

            #endregion
        }


        public DataSet LoadDataTable()
        {
            #region 加载数据

            string path = DNTRequest.GetString("path");
            Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/" + path + "/TemplateVariable");
            DataSet ds = new DataSet();
            DataTable dt = ForumPageTemplate.GetTemplateVarList(BaseConfigs.GetForumPath, path).Copy();
            ds.Tables.Add(dt);
            return ds;

            #endregion
        }


        private void AddNewRec_Click(object sender, EventArgs e)
        {
            #region 添加模板变量

            dsSrc = LoadDataTable();

            DataRow dr = dsSrc.Tables[0].NewRow();
            if (dsSrc.Tables[0].Rows.Count == 0) dr["id"] = 1;
            else dr["id"] = Convert.ToInt32(dsSrc.Tables[0].Rows[dsSrc.Tables[0].Rows.Count - 1][0].ToString()) + 1;

            dr["variablename"] = variablename.Text;
            dr["variablevalue"] = variablevalue.Text;
            dsSrc.Tables[0].Rows.Add(dr);

            try
            {
                if (dsSrc.Tables[0].Rows.Count == 0)
                {
                    File.Delete(Utils.GetMapPath("../../templates/" + DNTRequest.GetString("path") + "/templatevariable.xml"));
                    dsSrc.Reset();
                    dsSrc.Dispose();
                }
                else
                {
                    string filename = Server.MapPath("../../templates/" + DNTRequest.GetString("path") + "/templatevariable.xml");
                    dsSrc.WriteXml(filename);
                    dsSrc.Reset();
                    dsSrc.Dispose();

                    Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();
                    cache.RetrieveObject("/Forum/" + DNTRequest.GetString("path") + "/TemplateVariable");
                    base.RegisterStartupScript("PAGE", "window.location.href='global_templatevariable.aspx?templateid=" + DNTRequest.GetString("templateid") + "&path=" + DNTRequest.GetString("path") + "&templatename=" + DNTRequest.GetString("templatename") + "';");

                }
            }
            catch
            {
                base.RegisterStartupScript("", "<script>alert('无法更新数据库.');window.location.href='global_templatevariable.aspx?templateid=" + DNTRequest.GetString("templateid") + "&path=" + DNTRequest.GetString("path") + "&templatename=" + DNTRequest.GetString("templatename") + "';</script>");
                return;
            }

            #endregion
        }

        protected void DataGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid1.CurrentPageIndex = e.NewPageIndex;
            BindData();
        }

        #region Web 窗体设计器生成的代码

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.AddNewRec.Click += new EventHandler(this.AddNewRec_Click);
            this.DelRec.Click += new EventHandler(this.DelRec_Click);
            this.SaveVar.Click += new EventHandler(this.SaveVar_Click);
            DataGrid1.DataKeyField = "id";
            DataGrid1.TableHeaderName = "模板变量列表";
            DataGrid1.ColumnSpan = 4;
        }
        #endregion
    }
}