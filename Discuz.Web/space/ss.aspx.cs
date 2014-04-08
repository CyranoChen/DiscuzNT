using System.Text;
using System.Web;
using System.Web.UI;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Space.Entities;
 
using Discuz.Space.Utilities;
using Discuz.Entity;
using Discuz.Space.Provider;
using Discuz.Config;

namespace Discuz.Space
{
	/// <summary>
	/// 空间管理功能的数据请求页面
	/// </summary>
	public class ss : Page
	{
		public ss()
		{
			string type = DNTRequest.GetString("type");

			string xmlcontent = string.Empty;
			switch (type.ToLower())
			{
				case "theme":
					xmlcontent = OutPutTheme();
					break;
				case "template":
					xmlcontent = OutPutTemplate();
					break;
				case "icon":
					xmlcontent = OutPutIcon();
					break;
				default:
					return;
			}

			ResponseXML(xmlcontent);
		}

        /// <summary>
        /// 输出xml
        /// </summary>
        /// <param name="xmlcontent"></param>
		private static void ResponseXML(string xmlcontent)
		{
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ContentType = "Text/XML";
            HttpContext.Current.Response.Expires = 0;
			
			HttpContext.Current.Response.Cache.SetNoStore();			
			HttpContext.Current.Response.Write(xmlcontent);
			HttpContext.Current.Response.End();
		}

        /// <summary>
        /// 输出主题
        /// </summary>
        /// <returns></returns>
		private string OutPutTheme()
		{

#if NET1
			ThemeInfoCollection tic = SpaceProvider.GetThemeInfos();
            ThemeInfoCollection ticCategory = new ThemeInfoCollection();
            ThemeInfoCollection ticResult = new ThemeInfoCollection();
#else
            Discuz.Common.Generic.List<ThemeInfo> tic = SpaceProvider.GetThemeInfos();
            Discuz.Common.Generic.List<ThemeInfo> ticCategory = new Discuz.Common.Generic.List<ThemeInfo>();
            Discuz.Common.Generic.List<ThemeInfo> ticResult = new Discuz.Common.Generic.List<ThemeInfo>();
#endif
			StringBuilder sb = new StringBuilder("<?xml version=\"1.0\" encoding=\"UTF-8\" ?> \r\n");
		
   
			foreach (ThemeInfo ti in tic) //把分类级的ThemeInfo捡出来
			{
				if (ti.Type != 0)
					break;
				ticCategory.Add(ti);
			}
			sb.Append("<Skin> \r\n");
			foreach (ThemeInfo ti in ticCategory) //根据每个分类找其下属主题
			{
				sb.AppendFormat("<Category name=\"{0}\" themeid=\"{1}\"> \r\n", ti.Name, ti.ThemeId);
				ticResult.Add(ti);
				foreach (ThemeInfo t in tic)
				{
					if (t.Type == ti.ThemeId)
						sb.AppendFormat("<Theme themeid=\"{0}\" name=\"{1}\" directory=\"{2}\" createdate=\"{3}\" copyright=\"{4}\" author=\"{5}\" /> \r\n", t.ThemeId, t.Name, t.Directory, t.CreateDate, t.CopyRight, t.Author);
				}
				sb.Append("</Category> \r\n");
			}

//			foreach (ThemeInfo ti in ticResult)
//			{
//				sb.AppendFormat("<Theme themeid=\"{0}\" name=\"{1}\" directory=\"{2}\" createdate=\"{3}\" copyright=\"{4}\" author=\"{5}\"> \r\n", ti.ThemeId, ti.Name, ti.Directory, ti.CreateDate, ti.CopyRight, ti.Author);
//				if (ti.Type == 0)
//					sb.AppendFormat("<Category name=\"{0}\" /> \r\n", ti.Name);
//				sb.Append("</Theme> \r\n");
//			}
			sb.Append("</Skin> \r\n");

			return sb.ToString();
		}

        /// <summary>
        /// 输出版式
        /// </summary>
        /// <returns></returns>
		private string OutPutTemplate()
		{
			string path = Utils.GetMapPath(BaseConfigs.GetForumPath + "/space/skins/templates/");
			string[] templates = Globals.GetDirectoryFileList(path, "template_*.htm");

			StringBuilder sb = new StringBuilder("<?xml version=\"1.0\" encoding=\"UTF-8\" ?> \r\n");
			sb.Append("<Skin> \r\n");
			foreach (string template in templates)
			{
				sb.AppendFormat("<Template filename=\"{0}\" thumbnail=\"{1}\"/> \r\n", template, template.Replace(".htm", ".gif"));
			}
			sb.Append("</Skin> \r\n");
			return sb.ToString();
		}

        /// <summary>
        /// 输出小图标 (未来功能)
        /// </summary>
        /// <returns></returns>
		private string OutPutIcon()
		{
			return string.Empty;
		}
	}


}