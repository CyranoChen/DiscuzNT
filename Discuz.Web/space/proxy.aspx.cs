using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Space.Utilities;

namespace Discuz.Space
{
	/// <summary>
	/// setp 的摘要说明。
	/// </summary>
	public class proxy : Page
	{
		//public proxy()
        protected override void OnInit(System.EventArgs e)
		{
			if (ForumUtils.IsCrossSitePost(DNTRequest.GetUrlReferrer(), DNTRequest.GetHost()))
				return;			

			string url = DNTRequest.GetQueryString("url");
			if (url == null || url == string.Empty)
				return;
			url = HttpUtility.UrlDecode(url);

			string enc = "utf-8";
			if (DNTRequest.GetQueryString("enc") != "")
			{
				enc = DNTRequest.GetQueryString("enc");
			}
			string err = "";
			if (enc == "gb2312")
			{
				url = Globals.EncodeStringAsGB2312(url);
			}
			HttpWebResponse response = Globals.GetPageResponse(url, out err);
			if (response == null)
			{
				HttpContext.Current.Response.Write(err);
				HttpContext.Current.Response.End();
				return;
			}
//			WebHeaderCollection headers = response.Headers;
//			foreach (string key in headers.Keys)
//			{
//				HttpContext.Current.Response.AppendHeader(key, headers[key]);
//			}
			Encoding encoding = Encoding.GetEncoding(enc);
			HttpContext.Current.Response.ContentType = response.ContentType;
			Stream instream = response.GetResponseStream();
			StreamReader sr = new StreamReader(instream, encoding);

			//返回结果网页（html）代码 
			string content = sr.ReadToEnd();
			HttpContext.Current.Response.ContentEncoding = encoding;
			HttpContext.Current.Response.Write(content);
			HttpContext.Current.Response.End();

		}


	}


}