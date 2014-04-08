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
    /// ��̨ģ�������
    /// </summary>
    public class AdminTemplates : Templates
    {

        /// <summary>
        /// ɾ��ָ����ģ�����б�,
        /// </summary>
        /// <param name="templateidlist">��ʽΪ�� 1,2,3</param>
        public static void DeleteTemplateItem(string templateidlist)
        {
            Discuz.Data.Templates.DeleteTemplateItem(templateidlist);
        }

        /// <summary>
        /// ���������ģ��Ŀ¼�µ�ģ���б�(��:��Ŀ¼����)
        /// </summary>
        /// <param name="templatePath">ģ������·��</param>
        /// <example>GetAllTemplateList(Utils.GetMapPath(@"..\..\templates\"))</example>
        /// <returns>ģ���б�</returns>
        public static DataTable GetAllTemplateList(string templatePath)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(templatePath);
            DataTable dt = Data.Templates.GetAllTemplateList();
            dt.Columns.Add("valid", Type.GetType("System.Int16"));
            string directorylist = ",";
            foreach (DataRow dr in dt.Rows)
            {
                TemplateAboutInfo aboutInfo = GetTemplateAboutInfo(templatePath + dr["directory"].ToString());
                dr["valid"] = 1;// �Ƿ���ǰ̨��Чģ��
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
                    dr["directory"] = dir.Name;// ��Ŀ¼��
                    dr["valid"] = 0;// �Ƿ���ǰ̨��Чģ��
                    SetTemplateDataRow(dr, aboutInfo);
                    dt.Rows.Add(dr);
                }
            }
            dt.AcceptChanges();
            return dt;
        }


        private static void SetTemplateDataRow(DataRow dr, TemplateAboutInfo aboutInfo)
        {
            dr["name"] = aboutInfo.name;// ģ������
            dr["author"] = aboutInfo.author;// ����
            dr["createdate"] = aboutInfo.createdate;// ��������
            dr["ver"] = aboutInfo.ver;// ģ��汾
            dr["fordntver"] = aboutInfo.fordntver;// ���õ���̳�汾
            dr["copyright"] = aboutInfo.copyright;// ��Ȩ
        }


        /// <summary>
        /// ��ģ������ݿ����Ƴ�
        /// </summary>
        /// <param name="templateIdList">Ҫ�Ƴ���ģ��Id�б�</param>
        /// <param name="uid">�����ߵ�Uid</param>
        /// <param name="userName">�����ߵ��û���</param>
        /// <param name="groupId">�����ߵ���Id</param>
        /// <param name="groupTitle">�����ߵ�������</param>
        /// <param name="ip">�����ߵ�Ip</param>
        public static void RemoveTemplateInDB(string templateIdList, int uid, string userName, int groupId, string groupTitle, string ip)
        {
            #region �Ƴ�ģ��
            GeneralConfigInfo configInfo = GeneralConfigs.GetConfig();
            if (("," + templateIdList + ",").IndexOf("," + configInfo.Templateid + ",") >= 0) //��Ҫɾ����ģ����ϵͳ��Ĭ��ģ��ʱ
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
            AdminVistLogs.InsertLog(uid, userName, groupId, groupTitle, ip, "�����ݿ���ɾ��ģ���ļ�", "IDΪ:" + templateIdList);
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
            AdminVistLogs.InsertLog(uid, userName, groupId, groupTitle, ip, "��ģ�����ɾ��ģ���ļ�", "IDΪ:" + templateIdList);
        }
    }
}