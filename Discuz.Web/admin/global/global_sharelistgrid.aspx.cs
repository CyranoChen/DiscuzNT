using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Discuz.Web.Admin;
using Discuz.Config;
using Discuz.Common;
using System.Text;

namespace Discuz.Web.Admin
{
    public partial class sharelistgrid : AdminPage
    {
        public string[] list;
        protected void Page_Load(object sender, EventArgs e)
        {

                //BindData();
                GeneralConfigInfo configInfo = GeneralConfigs.GetConfig();
                list = Utils.SplitString(configInfo.Sharelist.ToString(), ",");


        }

        private void UpdateShare_Click(object sender, EventArgs e)
        {
            StringBuilder shareList = new StringBuilder();
            string[] newDisplayOrder = DNTRequest.GetFormString("newdisplayorder").Split(',');
            string[] title = DNTRequest.GetFormString("title").Split(',');
            string[] site  = DNTRequest.GetFormString("site").Split(',');
            string[] newDisplayOrder_clone = DNTRequest.GetFormString("newdisplayorder").Split(',');
            string[] shareDisable = DNTRequest.GetFormString("sharedisable").Split(',');
            if (Utils.IsNumericArray(newDisplayOrder))
            {


                newDisplayOrder_clone = InsertionSort(newDisplayOrder_clone);
                for (int i = 0; i < newDisplayOrder_clone.Length; i++)
                {
                     for (int j = 0; j < newDisplayOrder.Length; j++)
                     {
                        if(newDisplayOrder_clone[i] == newDisplayOrder[j])
                        {
                            if (TypeConverter.StrToInt(newDisplayOrder[j]) < 0)
                                shareList.Append("0");
                            else
                                shareList.Append(newDisplayOrder[j]);

                            shareList.Append("|");
                            shareList.Append(site[j]);
                            shareList.Append("|");
                            shareList.Append(title[j]);
                            shareList.Append("|");
                            if (Utils.InArray(site[j], shareDisable))
                            shareList.Append("1");
                            else
                            shareList.Append("0");
                          
                            shareList.Append(",");
                            break;
                        }
                     }

                }
                GeneralConfigInfo configInfo = GeneralConfigs.GetConfig();
                configInfo.Sharelist = shareList.ToString().TrimEnd(',');
                GeneralConfigs.Serialiaze(configInfo, Server.MapPath("../../config/general.config"));
            }
            base.RegisterStartupScript("PAGE", "window.location.href='global_sharelistgrid.aspx';");
        }


        public static string[]  InsertionSort(string[] list)
        {
            for (int i = 1; i < list.Length; i++)
            {
                int t = TypeConverter.StrToInt(list[i]);
                int j = i;
                while ((j > 0) && (TypeConverter.StrToInt(list[j - 1]) > t))
                {
                    list[j] = list[j - 1];
                    --j;
                }
                list[j] = t.ToString();
            }
            return list;
        }

        #region Web Form Designer generated code

        override protected void OnInit(EventArgs e)
        {
            this.UpdateShare.Click += new EventHandler(this.UpdateShare_Click);
            base.OnInit(e);
        }

        //private void InitializeComponent()
        //{
        //    //this.DataGrid1.EditCommand += new DataGridCommandEventHandler(this.DataGrid_Edit);
        //    //this.DataGrid1.CancelCommand += new DataGridCommandEventHandler(DataGrid_Cancel);
        //    //this.DataGrid1.ItemDataBound += new DataGridItemEventHandler(this.DataGrid_ItemDataBound);
            
        //    //this.DataGrid1.UpdateCommand += new DataGridCommandEventHandler(this.DataGrid_Update);
        //    //DataGrid1.LoadEditColumn();
        //    //DataGrid1.TableHeaderName = "在线列表";
        //    //DataGrid1.DataKeyField = "shareid";
        //    //DataGrid1.ColumnSpan = 5;
        //    //DataGrid1.SaveDSViewState = true;
        //}

        #endregion
    }
}
