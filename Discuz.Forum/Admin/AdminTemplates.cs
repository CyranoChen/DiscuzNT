using System;
using System.IO;
using System.Data;
using System.Data.Common;

using Discuz.Common;
using Discuz.Config;
using Discuz.Entity;

namespace Discuz.Forum
{
    /// <summary>
    /// 后台模板操作类
    /// </summary>
    public class AdminTemplates : Templates
    {

        /// <summary>
        /// 删除指定的模板项列表,
        /// </summary>
        /// <param name="templateidlist">格式为： 1,2,3</param>
        public static void DeleteTemplateItem(string templateidlist)
        {
            Discuz.Data.Templates.DeleteTemplateItem(templateidlist);
        }

        /// <summary>
        /// 获得所有在模板目录下的模板列表(即:子目录名称)
        /// </summary>
        /// <param name="templatePath">模板所在路径</param>
        /// <example>GetAllTemplateList(Utils.GetMapPath(@"..\..\templates\"))</example>
        /// <returns>模板列表</returns>
        public static DataTable GetAllTemplateList(string templatePath)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(templatePath);
            DataTable dt = Data.Templates.GetAllTemplateList();
            dt.Columns.Add("valid", Type.GetType("System.Int16"));
            string directorylist = ",";
            foreach (DataRow dr in dt.Rows)
            {
                TemplateAboutInfo aboutInfo = GetTemplateAboutInfo(templatePath + dr["directory"].ToString());
                dr["valid"] = 1;// 是否是前台有效模板
                SetTemplateDataRow(dr, aboutInfo);
                directorylist += dr["directory"].ToString() + ",";
            }
            int count = TypeConverter.ObjectToInt(Data.Templates.GetValidTemplateList().Compute("Max(templateid)", "")) + 1;

            foreach (DirectoryInfo dir in dirInfo.GetDirectories())
            {
                if (dir != null && directorylist.IndexOf("," + dir + ",") < 0)
                {
                    TemplateAboutInfo aboutInfo = GetTemplateAboutInfo(dir.FullName);
                    DataRow dr = dt.NewRow();
                    dr["templateid"] = count++;
                    dr["directory"] = dir.Name;// 子目录名
                    dr["valid"] = 0;// 是否是前台有效模板
                    SetTemplateDataRow(dr, aboutInfo);
                    dt.Rows.Add(dr);
                }
            }
            dt.AcceptChanges();
            return dt;
        }


        private static void SetTemplateDataRow(DataRow dr, TemplateAboutInfo aboutInfo)
        {
            dr["name"] = aboutInfo.name;// 模板名称
            dr["author"] = aboutInfo.author;// 作者
            dr["createdate"] = aboutInfo.createdate;// 创建日期
            dr["ver"] = aboutInfo.ver;// 模板版本
            dr["fordntver"] = aboutInfo.fordntver;// 适用的论坛版本
            dr["copyright"] = aboutInfo.copyright;// 版权
        }


        /// <summary>
        /// 将模板从数据库中移除
        /// </summary>
        /// <param name="templateIdList">要移除的模板Id列表</param>
        /// <param name="uid">操作者的Uid</param>
        /// <param name="userName">操作者的用户名</param>
        /// <param name="groupId">操作者的组Id</param>
        /// <param name="groupTitle">操作者的组名称</param>
        /// <param name="ip">操作者的Ip</param>
        public static void RemoveTemplateInDB(string templateIdList, int uid, string userName, int groupId, string groupTitle, string ip)
        {
            #region 移除模板
            GeneralConfigInfo configInfo = GeneralConfigs.GetConfig();
            if (("," + templateIdList + ",").IndexOf("," + configInfo.Templateid + ",") >= 0) //当要删除的模板是系统的默认模板时
            {
                configInfo.Templateid = 1;
            }

            GeneralConfigs.Serialiaze(configInfo, Utils.GetMapPath("../../config/general.config"));

            Data.Forums.UpdateForumAndUserTemplateId(templateIdList);
            Data.Templates.DeleteTemplateItem(templateIdList);

            Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/TemplateList");
            Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/TemplateIDList");
            Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/UI/TemplateListBoxOptionsForForumIndex");
            Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/UI/TemplateListBoxOptions");
            AdminVistLogs.InsertLog(uid, userName, groupId, groupTitle, ip, "从数据库中删除模板文件", "ID为:" + templateIdList);
            #endregion
        }

        public static void DeleteTemplate(string templateIdList, int uid, string userName, int groupId, string groupTitle, string ip)
        {
            RemoveTemplateInDB(templateIdList, uid, userName, groupId, groupTitle, ip);
            foreach (string templateid in templateIdList.Split(','))
            {
                string foldername = DNTRequest.GetString("temp" + templateid);
                if (foldername == "") continue;
                string folderpath = Utils.GetMapPath(@"..\..\templates\" + foldername);
                if (Directory.Exists(folderpath))
                {
                    Directory.Delete(folderpath, true);
                }
                string folderaspx = Utils.GetMapPath(@"..\..\aspx\" + templateid);
                if (Directory.Exists(folderaspx))
                {
                    Directory.Delete(folderaspx, true);
                }
            }
            AdminVistLogs.InsertLog(uid, userName, groupId, groupTitle, ip, "从模板库中删除模板文件", "ID为:" + templateIdList);
        }
    }
}