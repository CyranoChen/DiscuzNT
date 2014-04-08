using System.Data;
using System.Text.RegularExpressions;

using Discuz.Common;

namespace Discuz.Forum
{
	/// <summary>
	/// ��̳ģ��������
	/// </summary>
	public class ForumPageTemplate : PageTemplate
	{
		/// <summary>
		/// �����������
		/// </summary>
		/// <param name="forumPath">ģ��·��</param>
		/// <param name="skinName">Ƥ����</param>
		/// <param name="strTemplate">ģ������</param>
		/// <returns></returns>
		public override string ReplaceSpecialTemplate(string forumPath,string skinName,string strTemplate)
		{
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
			sb.Append(strTemplate);
			Match m;
            Regex r = new Regex(@"({([^\[\]/\{\}='\s]+)})", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);
			for (m = r.Match(strTemplate); m.Success; m = m.NextMatch()) 
			{
				if (m.Groups[0].ToString() == "{forumversion}")
					sb = sb.Replace(m.Groups[0].ToString(), Utils.GetAssemblyVersion());
				else if (m.Groups[0].ToString() == "{forumproductname}")
					sb = sb.Replace(m.Groups[0].ToString(), Utils.GetAssemblyProductName());
			}

			foreach(DataRow dr in GetTemplateVarList(forumPath,skinName).Rows)
			{
				sb = sb.Replace(dr["variablename"].ToString().Trim(), dr["variablevalue"].ToString().Trim());
			}
			return sb.ToString();
		}


	    /// <summary>
		/// ���ģ������б�
		/// </summary>
		/// <param name="forumpath">ģ��·��</param>
		/// <param name="skinName">Ƥ����</param>
		/// <returns></returns>
		public static DataTable GetTemplateVarList(string forumpath,string skinName)
		{
			Discuz.Cache.DNTCache cache = Cache.DNTCache.GetCacheService();
            DataTable dt = cache.RetrieveObject("/Forum/" + skinName + "/TemplateVariable") as DataTable;            

			if(dt == null)
			{
                DataSet dsSrc = new DataSet("template");

                string[] filename = { Utils.GetMapPath(forumpath + "templates/" + skinName + "/templatevariable.xml") };

                if (Utils.FileExists(filename[0]))
                {
                    dsSrc.ReadXml(filename[0]);
                    if (dsSrc.Tables.Count == 0)
                        dsSrc.Tables.Add(TemplateVariableTable());
                }
                else
                {
                    dsSrc.Tables.Add(TemplateVariableTable());
                }
                dt = dsSrc.Tables[0];
                cache.AddObject("/Forum/" + skinName + "/TemplateVariable", dt, filename);
			}
            return dt;
		}

        /// <summary>
        /// ������ʱ�����б�
        /// </summary>
        /// <returns></returns>
        private static DataTable TemplateVariableTable()
        {
             DataTable templatevariable = new DataTable("TemplateVariable");
             templatevariable.Columns.Add("id", System.Type.GetType("System.Int32"));
             templatevariable.Columns.Add("variablename", System.Type.GetType("System.String"));
             templatevariable.Columns.Add("variablevalue", System.Type.GetType("System.String"));
             return templatevariable;
        }
		
	}
}
