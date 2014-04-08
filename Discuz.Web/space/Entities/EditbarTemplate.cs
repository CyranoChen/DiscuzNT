using System;
using System.Text;
using Discuz.Entity;

namespace Discuz.Space.Entities
{
	/// <summary>
	/// EditbarTemplate ��ժҪ˵����
	/// </summary>
	public class EditbarTemplate : ISpaceTemplate
	{
	    public static readonly ISpaceTemplate Instance = new EditbarTemplate();
        private EditbarTemplate()
        {
        }

		#region ISpaceTemplate ��Ա

        /// <summary>
        /// ��ñ༭��ģ�������
        /// </summary>
        /// <param name="ht">ģ�����������</param>
        /// <returns>����ģ�����ݵ�html</returns>
		public string GetHtml(System.Collections.Hashtable ht)
		{
			StringBuilder builder = new StringBuilder();
			builder.AppendFormat(@"	
									<a id=""DD_tg___theme"" href=""###"" onclick=""_DS_popup(event,'__theme');loadThemes();"">��������</a>&nbsp;|&nbsp;
									<a id=""DD_tg___template"" href=""###"" onclick=""_DS_popup(event,'__template');loadTemplates();"">ѡ���ʽ</a>&nbsp;|&nbsp;
									<a id=""DD_tg___gadget"" href=""###"" onclick=""_DS_popup(event,'__gadget');loadModules();"">���ģ��</a>
                                    <script type=""text/javascript"">var tid = {1};var themepath='{2}';var currenttabTemplate='{3}';</script>
									<script type=""text/javascript"" src=""{0}space/javascript/editbar.js""></script>", ht["forumpath"].ToString(), ht["tabid"].ToString(), (ht["config"] as SpaceConfigInfo).ThemePath, (ht["currenttab"] as TabInfo).Template);
			return builder.ToString();
		}

		#endregion
	}
}
