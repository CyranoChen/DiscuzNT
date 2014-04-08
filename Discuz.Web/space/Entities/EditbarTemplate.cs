using System;
using System.Text;
using Discuz.Entity;

namespace Discuz.Space.Entities
{
	/// <summary>
	/// EditbarTemplate 的摘要说明。
	/// </summary>
	public class EditbarTemplate : ISpaceTemplate
	{
	    public static readonly ISpaceTemplate Instance = new EditbarTemplate();
        private EditbarTemplate()
        {
        }

		#region ISpaceTemplate 成员

        /// <summary>
        /// 获得编辑条模板的内容
        /// </summary>
        /// <param name="ht">模板变量的数组</param>
        /// <returns>返回模板内容的html</returns>
		public string GetHtml(System.Collections.Hashtable ht)
		{
			StringBuilder builder = new StringBuilder();
			builder.AppendFormat(@"	
									<a id=""DD_tg___theme"" href=""###"" onclick=""_DS_popup(event,'__theme');loadThemes();"">设置主题</a>&nbsp;|&nbsp;
									<a id=""DD_tg___template"" href=""###"" onclick=""_DS_popup(event,'__template');loadTemplates();"">选择版式</a>&nbsp;|&nbsp;
									<a id=""DD_tg___gadget"" href=""###"" onclick=""_DS_popup(event,'__gadget');loadModules();"">添加模块</a>
                                    <script type=""text/javascript"">var tid = {1};var themepath='{2}';var currenttabTemplate='{3}';</script>
									<script type=""text/javascript"" src=""{0}space/javascript/editbar.js""></script>", ht["forumpath"].ToString(), ht["tabid"].ToString(), (ht["config"] as SpaceConfigInfo).ThemePath, (ht["currenttab"] as TabInfo).Template);
			return builder.ToString();
		}

		#endregion
	}
}
