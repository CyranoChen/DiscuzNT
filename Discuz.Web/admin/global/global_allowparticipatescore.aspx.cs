using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;

using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// 积分策略编辑
    /// </summary>

    public partial class allowparticipatescore : AdminPage
    {
        protected DataTable templateDT = new DataTable("templateDT");
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (DNTRequest.GetString("groupid") != "")
                    BindData();
                else
                {
                    Response.Write("<script>history.go(-1);</script>");
                    Response.End();
                }
            }
        }

        public void BindData()
        {
            #region 绑定数据
            DataGrid1.AllowCustomPaging = false;
            DataGrid1.TableHeaderName = "允许评分范围列表";
            DataGrid1.DataSource = LoadDataInfo();
            DataGrid1.DataBind();
            #endregion
        }

        protected void Sort_Grid(Object sender, DataGridSortCommandEventArgs e)
        {
            DataGrid1.Sort = e.SortExpression.ToString();
        }


        protected void DataGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid1.LoadCurrentPageIndex(e.NewPageIndex);
            BindData();
        }

        protected void DataGrid_Edit(Object sender, DataGridCommandEventArgs E)
        {
            if (ViewState["validrow"].ToString().IndexOf("," + E.Item.ItemIndex + ",") >= 0)
            {
                DataGrid1.EditItemIndex = (int)E.Item.ItemIndex;
                DataGrid1.DataSource = LoadDataInfo();
                DataGrid1.DataBind();
            }
            else
            {
                base.RegisterStartupScript( "", GetMessageScript("操作失败,您所修改的积分行是无效的,具体操作请看注释!"));
                return;
            }
        }

        protected void DataGrid_Cancel(Object sender, DataGridCommandEventArgs e)
        {
            DataGrid1.EditItemIndex = -1;
            DataGrid1.DataSource = LoadDataInfo();
            DataGrid1.DataBind();
        }

        private DataTable LoadDataInfo()
        {
            #region 加载数据信息

            //DataTable dt = UserGroups.GroupParticipateScore(DNTRequest.GetInt("groupid", 0));
            //if (dt.Rows.Count <= 0)
            //    return null;

            //if (dt.Rows[0][0].ToString().Trim() == "")
            //    return RemoveEmptyRows(NewParticipateScore());
             
            //return RemoveEmptyRows(GroupParticipateScore(dt.Rows[0][0].ToString().Trim()));
            templateDT = UserGroups.GroupParticipateScore(DNTRequest.GetInt("groupid", 0));
            DataRow scoresetname = Scoresets.GetScoreSet().Rows[0];
            string validrow = "";

            for (int count = 0; count < 8; count++)
            {
                if ((scoresetname[count + 2].ToString().Trim() != "") && (scoresetname[count + 2].ToString().Trim() != "0"))
                {
                    templateDT.Rows[count]["ScoreName"] = scoresetname[count + 2].ToString().Trim();
                    validrow = validrow + "," + count;
                }

                if (IsValidScoreName(count + 1))
                    validrow = validrow + "," + count;
            }
            ViewState["validrow"] = validrow + ",";
            return templateDT;

            #endregion
        }

        //private DataTable RemoveEmptyRows(DataTable dt)
        //{
        //    DataRow[] drs = dt.Select("ScoreName=''");
        //    foreach (DataRow dr in drs)
        //    {
        //        dt.Rows.Remove(dr);
        //    }
        //    return dt;
        //}

        //public DataTable NewParticipateScore()
        //{
        //    #region 初始化并装入默认数据

        //    templateDT.Columns.Clear();
        //    templateDT.Columns.Add("id", Type.GetType("System.Int32"));
        //    templateDT.Columns.Add("available", Type.GetType("System.Boolean"));
        //    templateDT.Columns.Add("ScoreCode", Type.GetType("System.String"));
        //    templateDT.Columns.Add("ScoreName", Type.GetType("System.String"));
        //    templateDT.Columns.Add("Min", Type.GetType("System.String"));
        //    templateDT.Columns.Add("Max", Type.GetType("System.String"));
        //    templateDT.Columns.Add("MaxInDay", Type.GetType("System.String"));

        //    for (int rowcount = 0; rowcount < 8; rowcount++)
        //    {
        //        DataRow dr = templateDT.NewRow();
        //        dr["id"] = rowcount + 1;
        //        dr["available"] = false;
        //        dr["ScoreCode"] = "extcredits" + Convert.ToString(rowcount + 1);
        //        dr["ScoreName"] = "";
        //        dr["Min"] = "";
        //        dr["Max"] = "";
        //        dr["MaxInDay"] = "";
        //        templateDT.Rows.Add(dr);
        //    }
        //    DataRow scoresetname = Scoresets.GetScoreSet().Rows[0];
        //    string validrow = "";

        //    for (int count = 0; count < 8; count++)
        //    {
        //        if ((scoresetname[count + 2].ToString().Trim() != "") && (scoresetname[count + 2].ToString().Trim() != "0"))
        //        {
        //            templateDT.Rows[count]["ScoreName"] = scoresetname[count + 2].ToString().Trim();
        //            validrow = validrow + "," + count;
        //        }

        //        if (IsValidScoreName(count + 1))
        //            validrow = validrow + "," + count;
        //    }
        //    ViewState["validrow"] = validrow + ",";
        //    return templateDT;

        //    #endregion
        //}

        public bool IsValidScoreName(int scoreid)
        {
            #region 是否是有效的积分名称

            bool isvalid = false;

            foreach (DataRow dr in Scoresets.GetScoreSet().Rows)
            {
                if ((dr["id"].ToString() != "1") && (dr["id"].ToString() != "2") && (dr[scoreid + 1].ToString().Trim() != "0"))
                {
                    isvalid = true;
                    break;
                }
            }
            return isvalid;

            #endregion
        }

        //public DataTable GroupParticipateScore(string raterange)
        //{
        //    #region 用数据库中的记录更新已装入的默认数据

        //    NewParticipateScore();

        //    int i = 0;
        //    foreach (string raterangestr in raterange.Split('|'))
        //    {
        //        if (raterangestr.Trim() != "")
        //        {
        //            string[] scoredata = raterangestr.Split(',');
        //            if (scoredata[1].Trim() == "True")
        //                templateDT.Rows[i]["available"] = true;

        //            templateDT.Rows[i]["Min"] = scoredata[4].Trim();
        //            templateDT.Rows[i]["Max"] = scoredata[5].Trim();
        //            templateDT.Rows[i]["MaxInDay"] = scoredata[6].Trim();
        //        }
        //        i++;
        //    }
        //    return templateDT;

        //    #endregion
        //}


        protected void DataGrid_Update(Object sender, DataGridCommandEventArgs e)
        {
            #region 编辑相关的积分设置信息

            string id = DataGrid1.DataKeys[(int)e.Item.ItemIndex].ToString();
            bool available = ((CheckBox)e.Item.FindControl("available")).Checked;
            string Min = ((TextBox)e.Item.Cells[5].Controls[0]).Text.Trim();
            string Max = ((TextBox)e.Item.Cells[6].Controls[0]).Text.Trim();
            string MaxInDay = ((TextBox)e.Item.Cells[7].Controls[0]).Text.Trim();

            LoadDataInfo();
            int count = Convert.ToInt16(id) - 1;

            templateDT.Rows[count]["available"] = available;

            if (Min == "" || Max == "" || MaxInDay == "")
            {
                base.RegisterStartupScript( "", GetMessageScript("评分的最小值,最大值以及24小时最大评分数不能为空."));
                return;
            }

            if ((Min != "" && !Utils.IsNumeric(Min.Replace("-", ""))) || 
                (Max != "" && !Utils.IsNumeric(Max.Replace("-", ""))) || 
                (MaxInDay != "" && !Utils.IsNumeric(MaxInDay.Replace("-", ""))))
            {
                base.RegisterStartupScript( "", GetMessageScript("输入的数据必须是数字."));
                return;
            }

            if (Convert.ToInt16(Utils.SBCCaseToNumberic(Min)) >= Convert.ToInt16(Utils.SBCCaseToNumberic(Max)))
            {
                base.RegisterStartupScript( "", GetMessageScript("评分的最小值必须小于评分最大值."));
                return;
            }

            templateDT.Rows[count]["Min"] = Convert.ToInt16(Utils.SBCCaseToNumberic(Min));
            templateDT.Rows[count]["Max"] = Convert.ToInt16(Utils.SBCCaseToNumberic(Max));
            templateDT.Rows[count]["MaxInDay"] = Convert.ToInt16(Utils.SBCCaseToNumberic(MaxInDay));

            try
            {
                WriteScoreInf(templateDT);
                DataGrid1.EditItemIndex = -1;
                DataGrid1.DataSource = LoadDataInfo();
                DataGrid1.DataBind();
                base.RegisterStartupScript( "PAGE", "window.location.href='global_allowparticipatescore.aspx?pagename=" + DNTRequest.GetString("pagename") + "&groupid=" + DNTRequest.GetString("groupid") + "';");
            }
            catch
            {
                base.RegisterStartupScript( "", GetMessageScript("无法更新数据库."));
                return;
            }

            #endregion
        }

        private string GetMessageScript(string message)
        {
            return string.Format("<script>alert('{0}');window.location.href='global_allowparticipatescore.aspx?pagename={1}&groupid={2}';</script>", 
                message, DNTRequest.GetString("pagename"), DNTRequest.GetString("groupid"));
        }
        public void WriteScoreInf(DataTable dt)
        {
            #region 向数据库中写入允许的评分范围内容

            string scorecontent = "";
            foreach (DataRow dr in dt.Rows)
            {
                scorecontent += string.Format("{0},{1},{2},{3},{4},{5},{6}|", 
                    dr["id"].ToString(), 
                    dr["available"].ToString(), 
                    dr["ScoreCode"].ToString(), 
                    dr["ScoreName"].ToString(), 
                    dr["Min"].ToString(),
                    dr["Max"].ToString(),
                    dr["MaxInDay"].ToString());
            }
            Forum.UserGroups.UpdateUserGroupRaterange(scorecontent.Substring(0, scorecontent.Length - 1), DNTRequest.GetInt("groupid",0));
            templateDT.Clear();

            Caches.ReSetUserGroupList();

            #endregion
        }


        public bool GetAvailable(string available)
        {
            return available == "True";
        }

        public string GetImgLink(string available)
        {
            return available == "True" ? "<div align=center><img src=../images/OK.gif /></div>" : "<div align=center><img src=../images/Cancel.gif /></div>";
        }

        private void DataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            #region 数据绑定显示长度控制

            if (e.Item.ItemType == ListItemType.EditItem)
            {
                TextBox t = (TextBox)e.Item.Cells[5].Controls[0];
                t.Attributes.Add("maxlength", "3");
                t.Attributes.Add("size", "4");

                t = (TextBox)e.Item.Cells[6].Controls[0];
                t.Attributes.Add("maxlength", "3");
                t.Attributes.Add("size", "4");

                t = (TextBox)e.Item.Cells[7].Controls[0];
                t.Attributes.Add("maxlength", "4");
                t.Attributes.Add("size", "4");
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
            this.DataGrid1.ItemDataBound += new DataGridItemEventHandler(this.DataGrid_ItemDataBound);
            DataGrid1.LoadEditColumn();
        }

        #endregion
    }
}