using System;
using System.Net;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Text;

using Discuz.Forum;
using Discuz.Config;

namespace Discuz.Space.Manage
{
	/// <summary>
	/// Ajax 的摘要说明。
	/// </summary>
	public class ajax : Page
	{
		protected internal GeneralConfigInfo config;
		protected internal string ascxpath = ""; //用户控件路径值
		protected HtmlForm AjaxCallBackForm;

		public ajax()
		{
		}

		
		private void InitializeComponent()
		{
			this.ID = "Ajax_CallBack_Form";
			base.Load += new EventHandler(this.Page_Load);
		}


		protected override void OnInit(EventArgs e)
		{
			this.InitializeComponent();
			base.OnInit(e);
		}

		private void Page_Load(object sender, EventArgs e)
		{
            string ajaxtemplate = base.Request.Params["AjaxTemplate"].ToLower();
            if (ajaxtemplate != null && ajaxtemplate.IndexOf("uploadfile")<0)
			{
				try
				{
                    this.AjaxCallBackForm.Controls.Add(base.LoadControl(ajaxtemplate.EndsWith(".ascx") ? ascxpath + ajaxtemplate : (ascxpath + ajaxtemplate + ".ascx")));
				}
				catch
				{;}
			}
		}
	}


    /// <summary>
    /// 远程脚本
    /// </summary>
	public class RemoteScript : Page
	{
		public RemoteScript()
		{
		}

		private void InitializeComponent()
		{
			base.Load += new EventHandler(this.Page_Load);
		}

		protected override void OnInit(EventArgs e)
		{
			this.InitializeComponent();
			base.OnInit(e);

		}

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
					object[] objArray = new object[] {base.Request.ServerVariables["QUERY_STRING"], base.Request.ServerVariables["SERVER_NAME"], base.Request.ServerVariables["SERVER_PORT"], base.Request.ServerVariables["SCRIPT_NAME"].ToLower().Replace("remotescript.aspx", "")};
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