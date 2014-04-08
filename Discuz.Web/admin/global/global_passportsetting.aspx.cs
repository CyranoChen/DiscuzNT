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
    public partial class global_passportsetting : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            applicationtype.Items[0].Attributes.Add("onclick", "$('showurl').style.display='';");
            applicationtype.Items[1].Attributes.Add("onclick", "$('showurl').style.display='none';");

            asyncmode.Items[0].Attributes.Add("onclick", "$('tr_asyncurl').style.display='';$('tr_asynclist').style.display='none';");
            asyncmode.Items[1].Attributes.Add("onclick", "$('tr_asyncurl').style.display='none';$('tr_asynclist').style.display='none';");
            asyncmode.Items[2].Attributes.Add("onclick", "$('tr_asyncurl').style.display='';$('tr_asynclist').style.display='';");
            if (!IsPostBack)
            {
                string apikey = DNTRequest.GetString("apikey");
                if (apikey != "")
                {
                    APIConfigInfo aci = APIConfigs.GetConfig();
                    foreach (ApplicationInfo ai in aci.AppCollection)
                    {
                        if (ai.APIKey == apikey)
                        {
                            appname.Text = ai.AppName;
                            applicationtype.SelectedValue = ai.ApplicationType.ToString();
                            if (applicationtype.SelectedIndex == 1)
                            {
                                base.RegisterStartupScript("applicationtype", "<script>$('showurl').style.display='none';</script>");
                            }
                            appurl.Text = ai.AppUrl;
                            callbackurl.Text = ai.CallbackUrl;
                            ipaddresses.Text = ai.IPAddresses;

                            asyncmode.SelectedValue = ai.SyncMode.ToString();
                            if (asyncmode.SelectedIndex == 1)
                            {
                                base.RegisterStartupScript("asyncmode", "<script>$('tr_asyncurl').style.display='none';$('tr_asynclist').style.display='none';</script>");
                            }
                            if (asyncmode.SelectedIndex == 2)
                            {
                                base.RegisterStartupScript("asyncmode", "<script>$('tr_asyncurl').style.display='';$('tr_asynclist').style.display='';</script>");
                            }
                            asyncurl.Text = ai.SyncUrl;
                            asynclist.Text = ai.SyncList;
                            break;
                        }
                    }
                }
                apikeyhidd.Value = apikey;
            }
        }

        protected void savepassportinfo_Click(object sender, EventArgs e)
        {
            if (appname.Text.Trim() == "")
            {
                base.RegisterStartupScript("PAGE", "alert('整合程序名称不能为空!');");
                return;
            }
            if (applicationtype.SelectedValue != "2")
            {
                if (appurl.Text.Trim() == "")
                {
                    base.RegisterStartupScript("PAGE", "alert('整合程序 Url 地址不能为空!');");
                    return;
                }
                if (applicationtype.SelectedValue == "1" && callbackurl.Text.Trim() == "")
                {
                    base.RegisterStartupScript("PAGE", "alert('登录完成后返回地址不能为空!');");
                    return;
                }
            }
            if (ipaddresses.Text.Trim() != "")
            {
                foreach (string ip in ipaddresses.Text.Replace("\r\n", "").Replace(" ", "").Split(','))
                {
                    if (!Utils.IsIP(ip))
                    {
                        base.RegisterStartupScript("PAGE", "alert('IP地址格式错误!');");
                        return;
                    }
                }
            }
            if (apikeyhidd.Value == "") //增加
            {
                ApplicationInfo ai = new ApplicationInfo();
                ai.AppName = appname.Text;
                ai.AppUrl = appurl.Text;
                ai.APIKey = Utils.MD5(System.Guid.NewGuid().ToString());
                ai.Secret = Utils.MD5(System.Guid.NewGuid().ToString());
                ai.ApplicationType = Convert.ToInt32(applicationtype.SelectedValue);
                if (ai.ApplicationType == 1)
                    ai.CallbackUrl = callbackurl.Text;
                else
                    ai.CallbackUrl = "";
                ai.CallbackUrl = callbackurl.Text;
                ai.IPAddresses = ipaddresses.Text.Replace("\r\n", "").Replace(" ", "");

                ai.SyncMode = Convert.ToInt32(asyncmode.SelectedValue);
                ai.SyncUrl = asyncurl.Text;
                ai.SyncList = asynclist.Text;

                APIConfigInfo aci = APIConfigs.GetConfig();
                if (aci.AppCollection == null)
                    aci.AppCollection = new ApplicationInfoCollection();
                aci.AppCollection.Add(ai);
                APIConfigs.SaveConfig(aci);
            }
            else   //修改
            {
                APIConfigInfo aci = APIConfigs.GetConfig();
                foreach (ApplicationInfo ai in aci.AppCollection)
                {
                    if (ai.APIKey == apikeyhidd.Value)
                    {
                        ai.AppName = appname.Text;
                        ai.AppUrl = appurl.Text;
                        ai.ApplicationType = Convert.ToInt32(applicationtype.SelectedValue);
                        if (ai.ApplicationType == 1)
                            ai.CallbackUrl = callbackurl.Text;
                        else
                            ai.CallbackUrl = "";
                        ai.CallbackUrl = callbackurl.Text;
                        ai.IPAddresses = ipaddresses.Text.Replace("\r\n", "").Replace(" ", "");

                        ai.SyncMode = Convert.ToInt32(asyncmode.SelectedValue);
                        ai.SyncUrl = asyncurl.Text;
                        ai.SyncList = asynclist.Text;
                        break;
                    }
                }
                APIConfigs.SaveConfig(aci);
            }
            Response.Redirect("global_passportmanage.aspx");
        }
    }
}
