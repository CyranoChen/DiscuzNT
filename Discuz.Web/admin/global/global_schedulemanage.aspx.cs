using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using Discuz.Forum;
using Discuz.Common;
using Discuz.Config;

namespace Discuz.Web.Admin
{
    public partial class global_schedulemanage : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("key");
            dt.Columns.Add("scheduletype");
            dt.Columns.Add("exetime");
            dt.Columns.Add("lastexecute");
            dt.Columns.Add("issystemevent");
            dt.Columns.Add("enable");
            Discuz.Config.Event[] events = ScheduleConfigs.GetConfig().Events;
            foreach (Discuz.Config.Event ev in events)
            {
                DataRow dr = dt.NewRow();
                dr["key"] = ev.Key;
                dr["scheduletype"] = ev.ScheduleType;
                if (ev.TimeOfDay != -1)
                {
                    dr["exetime"] = "定时执行:" + (ev.TimeOfDay / 60) + "时" + (ev.TimeOfDay % 60) + "分";
                }
                else
                {
                    dr["exetime"] = "周期执行:" + ev.Minutes + "分钟";
                }
                DateTime lastExecute =Discuz.Forum.ScheduledEvents.Event.GetLastExecuteScheduledEventDateTime(ev.Key, Environment.MachineName);
                if (lastExecute == DateTime.MinValue)
                {
                    dr["lastexecute"] = "从未执行";
                }
                else
                {
                    dr["lastexecute"] = lastExecute.ToString("yyyy-MM-dd HH:mm:ss");
                }
                dr["issystemevent"] = ev.IsSystemEvent ? "系统级" : "非系统级";
                dr["enable"] = ev.Enabled ? "启用" : "禁用";
                dt.Rows.Add(dr);
            }
            DataGrid1.DataSource = dt;
            DataGrid1.DataKeyField = "key";
                DataGrid1.DataBind();
        }

        protected void DataGrid1_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "exec")
            {
                Discuz.Config.Event[] events = ScheduleConfigs.GetConfig().Events;
                foreach (Discuz.Config.Event ev in events)
                {
                    if (ev.Key == e.CommandArgument.ToString())
                    {
                        ((Discuz.Forum.ScheduledEvents.IEvent)Activator.CreateInstance(Type.GetType(ev.ScheduleType))).Execute(HttpContext.Current);
                        Discuz.Forum.ScheduledEvents.Event.SetLastExecuteScheduledEventDateTime(ev.Key, Environment.MachineName, DateTime.Now);
                        break;
                    }
                }
                //base.RegisterStartupScript("exec", "window.location.href=window.location;");
            }
        }


        public void DataGrid_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                e.Item.Attributes.Add("onmouseover", "this.className='mouseoverstyle'");
                e.Item.Attributes.Add("onmouseout", "this.className='mouseoutstyle'");
                e.Item.Style["cursor"] = "hand";
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
            DataGrid1.CssClass = "datalist";
            DataGrid1.ShowHeader = true;
            DataGrid1.AutoGenerateColumns = false;

            DataGrid1.SelectedItemStyle.CssClass = "datagridSelectedItem";
            DataGrid1.HeaderStyle.CssClass = "category";
            DataGrid1.AutoGenerateColumns = false;

            DataGrid1.BorderWidth = 1;
            DataGrid1.BorderStyle = BorderStyle.Solid;
            DataGrid1.BorderColor = System.Drawing.Color.FromArgb(234, 233, 225);

            DataGrid1.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.DataGrid_ItemDataBound);
        }

        #endregion
          
    }
}
