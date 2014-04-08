using System;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Data.Common;
using System.Collections;

using Discuz.Control;
using Discuz.Common;
using Discuz.Forum;
using Button = Discuz.Control.Button;
using DataGrid = Discuz.Control.DataGrid;
using TextBox = Discuz.Control.TextBox;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Common.Generic;


namespace Discuz.Web.Admin
{
    public partial class helplist : AdminPage
    {
        protected DataGrid DataGrid1;
        public IDataReader ddr;
        public List<HelpInfo> helpInfoList;
        protected void Page_Load(object sender, EventArgs e)
        {
            helpInfoList = Helps.GetHelpList();
        }

        private void DelRec_Click(object sender, EventArgs e)
        {
            #region 删除
            string idlist = DNTRequest.GetFormString("id");
   
            if (this.CheckCookie())
            {
                if (idlist != "")
                {
                    del(idlist);
                }
                else
                {
                    base.RegisterStartupScript("", "<script>alert('您未选中任何选项');window.location.href='global_helplist.aspx';</script>");
                }
            }
            #endregion
        }

        protected void del(string idlist)
        {
            #region 删除帮助
            Helps.DelHelp(idlist);
            AdminVistLogs.InsertLog(userid, username, usergroupid, grouptitle, ip, "删除帮助", "删除帮助,帮助ID为: " + DNTRequest.GetString("id"));
            Response.Redirect("global_helplist.aspx");
            #endregion
        }

        private void Orderby_Click(object sender, EventArgs e)
        {
            #region 排序
            string[] orderlist = DNTRequest.GetFormString("orderbyid").Split(',');
            string[] idlist = DNTRequest.GetFormString("hidid").Split(',');

            if (!Helps.UpOrder(orderlist, idlist))
            {
                base.RegisterStartupScript("", "<script>alert('输入错误,排序号只能是数字');window.location.href='global_helplist.aspx';</script>");
                return;
            }
            //foreach (string s in orderlist)
            //{
            //    if (Utils.IsNumeric(s) == false)
            //    {
            //        base.RegisterStartupScript("", "<script>alert('输入错误,排序号只能是数字');window.location.href='global_helplist.aspx';</script>");
            //        return;
            //    }
            //}
         
            //for (int i = 0; i < idlist.Length; i++)
            //{
            //    Helps.UpOrder(orderlist[i].ToString(), idlist[i].ToString());
            //}

            base.RegisterStartupScript("", "<script>window.location.href='global_helplist.aspx';</script>");
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
            this.Orderby.Click += new EventHandler(this.Orderby_Click);
        }

        #endregion
    }
}
