using System;
using Discuz.Forum;
using Discuz.Config;

namespace Discuz.Space.Provider
{
	/// <summary>
	/// ExecSqlProvider ��ժҪ˵����
	/// </summary>
	public class SqlForControlsProvider
	{
		public SqlForControlsProvider()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}

		public static string GetThemeDropDownTreeSql()
		{
			return "SELECT [themeid], [name], [type] AS [parentid] FROM [" + BaseConfigs.GetTablePrefix + "spacethemes] ORDER BY [themeid]";
		}

		public static string GetTemplateDropDownSql()
		{
			return "SELECT [templateid], [name]  FROM [" + BaseConfigs.GetTablePrefix + "spacetemplates] ORDER BY [templateid]";
		}

		public static string GetCategoryCheckListSql(int userid)
		{
			return "SELECT [categoryid], [title] FROM [" + BaseConfigs.GetTablePrefix + "spacecategories] WHERE [uid]="+userid+" ORDER BY [displayorder], [categoryid]";
		}
	}
}
