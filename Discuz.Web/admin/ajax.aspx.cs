using System;
using System.Collections.Generic;
using System.Web;
using System.Net;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Text;

using Discuz.Forum;
using Discuz.Common;

namespace Discuz.Web.Admin
{

    /// <summary>
    /// Ajax 的摘要说明。
    /// </summary>
    public partial class ajax : AdminPage
    {
        protected internal string ascxpath = "UserControls/"; //用户控件路径值

        private void InitializeComponent()
        {
            this.ID = "Ajax_CallBack_Form";
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void Page_Load(object sender, EventArgs e)
        {
            if (base.Request.Params["AjaxTemplate"] != null)
            {
                try
                {
                    this.AjaxCallBackForm.Controls.Add(base.LoadControl(base.Request.Params["AjaxTemplate"].ToLower().EndsWith(".ascx") ? ascxpath + base.Request.Params["AjaxTemplate"] : (ascxpath + base.Request.Params["AjaxTemplate"] + ".ascx")));
                }
                catch
                {
                }
            }
        }
    }

    public partial class RemoteScript : AdminPage
    {
        private void Page_Load(object sender, EventArgs e)
        {
            base.Response.ContentType = "text/javascript";
            string output = base.Request.QueryString["output"];
            string callback = base.Request.QueryString["callback"];
            string ajaxtemplate = base.Request.QueryString["AjaxTemplate"];
            if (ajaxtemplate != null)
            {
                try
                {
                    string readerstr = null;
                    WebClient client1 = new WebClient();
                    client1.Headers.Add("User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; .NET CLR 1.1.4322)");
                    object[] objArray = new object[] { base.Request.ServerVariables["QUERY_STRING"], base.Request.ServerVariables["SERVER_NAME"], base.Request.ServerVariables["SERVER_PORT"], base.Request.ServerVariables["SCRIPT_NAME"].ToLower().Replace("remotescript.aspx", "") };
                    string ajaxpath = string.Format("http://{1}:{2}{3}Ajax.aspx?{0}", objArray);
                    StreamReader reader1 = new StreamReader(client1.OpenRead(ajaxpath), Encoding.UTF8);
                    readerstr = reader1.ReadToEnd().Trim();
                    reader1.Close();
                    string ajaxcontext = "<!--AjaxContent-->";
                    readerstr = readerstr.Substring(readerstr.IndexOf(ajaxcontext, 0) + ajaxcontext.Length).Trim();
                    readerstr = readerstr.Substring(0, readerstr.Length - 7);
                    string tempstr = "";
                    if ((output != null) && (output.Length > 0))
                    {
                        tempstr += string.Format("if ($('{0}') != null) $('{0}').innerHTML = '{1}';", output, RemoteScript.ToJavaScriptString(readerstr));
                    }
                    if ((callback != null) && (callback.Length > 0))
                    {
                        tempstr += string.Format(" if ({0}) {0}('{1}');", callback, RemoteScript.ToJavaScriptString(readerstr));
                    }
                    base.Response.Write(tempstr);
                }
                catch
                {
                }
            }
        }

        public static string ToJavaScriptString(string str)
        {
            return str.Replace("\n", "").Replace("\r", "").Replace(@"\", @"\\").Replace("'", @"\'").Replace("\"", "\\\"").Replace("/", @"\/");
        }
    }

}