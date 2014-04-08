using System;
using System.Collections.Generic;
using System.Text;
using Discuz.Config;
using Discuz.Common;
using Discuz.Forum;

namespace Discuz.Web.UI
{
    public class StatusPage : PageBase
    {
        public StatusPage()
        {
            this.Load += new EventHandler(Status_Load);
        }

        void Status_Load(object sender, EventArgs e)
        {
            if (!APIConfigs.GetConfig().Enable)
                return;

            ApplicationInfo appInfo = null;
            foreach (ApplicationInfo newapp in APIConfigs.GetConfig().AppCollection)
            {
                if (newapp.APIKey == DNTRequest.GetString("api_key"))
                    appInfo = newapp;
            }

            if (appInfo == null)
                return;

            if (DNTRequest.GetString("format").Trim().ToLower() == "json")
            {
                Response.ContentType = "text/html";
                Response.Write((userid > 0).ToString().ToLower());
                Response.End();
            }
            else
            {
                Response.Redirect(string.Format("{0}{1}user_status={2}{3}",
                    appInfo.CallbackUrl,
                    appInfo.CallbackUrl.IndexOf("?") > 0 ? "&" : "?",
                    userid > 0 ? "1" : "0",
                    DNTRequest.GetString("next") == "" ? DNTRequest.GetString("next") : "&next=" + DNTRequest.GetString("next"))
                    );
            }
        }
    }
}
