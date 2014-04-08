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
    public partial class global_passportmanage : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                APIConfigInfo aci = APIConfigs.GetConfig();
                allowpassport.SelectedValue = aci.Enable ? "1" : "0";
                passportbody.Attributes.Add("style", "display:" + (aci.Enable ? "block" : "none"));
                allowpassport.Items[0].Attributes.Add("onclick", "setAllowPassport(1)");
                allowpassport.Items[1].Attributes.Add("onclick", "setAllowPassport(0)");
                ApplicationInfoCollection appColl = aci.AppCollection;
                DataTable dt = new DataTable();
                dt.Columns.Add("appname");
                dt.Columns.Add("apptype");
                dt.Columns.Add("callbackurl");
                dt.Columns.Add("apikey");
                dt.Columns.Add("secret");
                foreach (ApplicationInfo ai in appColl)
                {
                    DataRow dr = dt.NewRow();
                    dr["appname"] = ai.AppName;
                    dr["apptype"] = ai.ApplicationType == 1 ? "Web" : "桌面";
                    dr["callbackurl"] = ai.CallbackUrl;
                    dr["apikey"] = ai.APIKey;
                    dr["secret"] = ai.Secret;
                    dt.Rows.Add(dr);
                }
                DataGrid1.TableHeaderName = "整合程序列表";
                DataGrid1.DataKeyField = "apikey";
                DataGrid1.DataSource = dt;
                DataGrid1.DataBind();
            }
        }

        protected void DelRec_Click(object sender, EventArgs e)
        {
            string apikeylist = DNTRequest.GetString("apikey");
            if (apikeylist == "")
                return;
            foreach (string apikey in apikeylist.Split(','))
            {
                APIConfigInfo aci = APIConfigs.GetConfig();
                ApplicationInfoCollection appColl = aci.AppCollection;
                foreach (ApplicationInfo ai in appColl)
                {
                    if (ai.APIKey == apikey)
                    {
                        aci.AppCollection.Remove(ai);
                        break;
                    }
                }
                APIConfigs.SaveConfig(aci);
            }
            Response.Redirect("global_passportmanage.aspx");
        }
    }
}
