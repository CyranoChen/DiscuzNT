using System;
using System.Data;
using System.Text;

namespace Discuz.Data
{
    public class Templates
    {
        /// <summary>
        /// 获得前台有效的模板列表
        /// </summary>
        /// <returns>模板列表</returns>
        public static DataTable GetValidTemplateList()
        {
            return DatabaseProvider.GetInstance().GetValidTemplateList();
        }

        /// <summary>
		/// 获得前台有效的模板ID列表
		/// </summary>
		/// <returns>模板ID列表</returns>
        public static string GetValidTemplateIDList()
        {
            StringBuilder sb = new StringBuilder();
            IDataReader reader = DatabaseProvider.GetInstance().GetValidTemplateIDList();
            while (reader.Read())
            {
                sb.Append("," + reader["templateid"]);
            }
            reader.Close();
            return sb.ToString();
        }

        /// <summary>
        /// 添加模板
        /// </summary>
        /// <param name="name">模版名称</param>
        /// <param name="directory">模版目录</param>
        /// <param name="copyright">版权信息</param>
        /// <param name="author">作者</param>
        /// <param name="createdate">创建日期</param>
        /// <param name="ver">版本</param>
        /// <param name="fordntver">适用论坛版本</param>
        public static void CreateTemplate(string name, string directory, string copyright, string author, string createdate, string ver, string fordntver)
        {
            DatabaseProvider.GetInstance().AddTemplate(name, directory, copyright, author, createdate, ver, fordntver);
        }

        /// <summary>
        /// 添加新的模板项
        /// </summary>
        /// <param name="templateName">模板名称</param>
        /// <param name="directory">模板文件所在目录</param>
        /// <param name="copyright">模板版权文字</param>
        /// <returns>模板id</returns>
        public static int CreateTemplateItem(string templateName, string directory, string copyright)
        {
            return DatabaseProvider.GetInstance().AddTemplate(templateName, directory, copyright);
        }



        /// <summary>
        /// 删除指定的模板项
        /// </summary>
        /// <param name="templateid">模板id</param>
        public static void DeleteTemplateItem(int templateid)
        {
            DatabaseProvider.GetInstance().DeleteTemplateItem(templateid);
        }



        /// <summary>
        /// 删除指定的模板项列表,
        /// </summary>
        /// <param name="templateidlist">格式为： 1,2,3</param>
        public static void DeleteTemplateItem(string templateidlist)
        {
            DatabaseProvider.GetInstance().DeleteTemplateItem(templateidlist);
        }

        /// <summary>
        /// 获取模板列表
        /// </summary>
        /// <param name="templatePath">模版路径</param>
        /// <returns></returns>
        public static DataTable GetAllTemplateList()
        {
            return DatabaseProvider.GetInstance().GetAllTemplateList();
        }
    }
}
