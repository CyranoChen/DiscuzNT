using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Web.UI.WebControls;
using System.Xml;
using Discuz.Common;
using Discuz.Config;
using Discuz.Data;
using Discuz.Forum;

namespace Discuz.Install
{
    public class Upgrade : System.Web.UI.Page
    {
        public BaseConfigInfo baseconfig = BaseConfigs.GetBaseConfig();
        protected RadioButtonList rblBBSVersion;

        protected void Page_Load(object sender, EventArgs e)
        {
            DbHelper.ResetDbProvider();

            if (DNTRequest.GetString("connection") == "check")
                CheckConnection();

            if (DNTRequest.GetString("upgrade") == "true")
            {
                UpgradeProcess();
                UpgradeConfig();
                MoveCommonUpgradeIniConfig();
                Response.Write("{\"Result\":true,\"Message\":\"\"}");
                Response.End();
            }
        }

        /// <summary>
        /// ������ݿ�����
        /// </summary>
        private void CheckConnection()
        {
            try
            {
                DbHelper.ExecuteNonQuery("SELECT 1");
                Response.Write("{\"Result\":true,\"Message\":\"\"}");
            }
            catch (SqlException)
            {
                Response.Write("{\"Result\":false,\"Message\":\"���� SQL Server ��������ʱ������������صĻ��ض���ʵ���Ĵ���,����dnt.config�е������ַ����Ƿ���ȷ\"}");
            }
            Response.End();
        }

        /// <summary>
        /// ������ʹ洢���̵ķ���
        /// </summary>
        private void UpgradeProcess()
        {
            //�����ݿⲻ��Sql Server����������
            if (baseconfig.Dbtype != "SqlServer")
                return;

            //����ҳĬ�϶�Ϊ��̳��ҳ
            GeneralConfigs.GetConfig().Forumurl = "forumindex.aspx";
            GeneralConfigs.Serialiaze(GeneralConfigs.GetConfig(), Server.MapPath("../config/general.config"));

            //��ȡ�����ű����ļ���
            string upgradeSqlScriptPath = string.Format("sqlscript/{0}/",baseconfig.Dbtype.ToString().Trim());
            string upgradeTableScriptFileName = Server.MapPath(string.Format("{0}upgradetable{1}.sql", upgradeSqlScriptPath, rblBBSVersion.SelectedValue));
            string upgradeProcedureScriptFileName = Server.MapPath(string.Format("{0}upgradeprocedure_2000.sql",upgradeSqlScriptPath));
            if (DbHelper.ExecuteScalar(CommandType.Text, "SELECT @@VERSION").ToString().Substring(20, 24).IndexOf("2000") == -1)    //��ѯSQLSERVER�İ汾
                upgradeProcedureScriptFileName = Server.MapPath(string.Format("{0}upgradeprocedure_2005.sql",upgradeSqlScriptPath));
            //�����ű�������
            if (!File.Exists(upgradeTableScriptFileName) || !File.Exists(upgradeProcedureScriptFileName))
                return;

            //������
            UpgradeTable(upgradeTableScriptFileName);
            //�����洢����
            UpgradeProcedure(upgradeProcedureScriptFileName);
        }

        /// <summary>
        /// ��ṹ�����ķ���
        /// </summary>
        /// <param name="upgradeTableScriptPath"></param>
        private void UpgradeTable(string upgradeTableScriptPath)
        {
            ExecuteScript(upgradeTableScriptPath);
        }

        /// <summary>
        /// �洢���������ķ���
        /// </summary>
        /// <param name="upgradeProcedureScriptPath"></param>
        private void UpgradeProcedure(string upgradeProcedureScriptPath)
        {
            ExecuteScript(upgradeProcedureScriptPath);
            DatabaseProvider.GetInstance().UpdatePostSP();
        }

        /// <summary>
        /// ִ��sql�ű�
        /// </summary>
        /// <param name="scriptPath"></param>
        private void ExecuteScript(string scriptPath)
        {
            string[] sqlArray = File.ReadAllText(scriptPath, Encoding.UTF8).Trim().Replace("dnt_", baseconfig.Tableprefix)
                .Split(new string[] { "GO\r\n", "go\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string sql in sqlArray)
            {
                if (sql.Trim() == string.Empty)
                    continue;
                try
                {
                    DbHelper.ExecuteNonQuery(CommandType.Text, sql);
                }
                catch
                {
                    ;
                }
            }
        }

        /// <summary>
        /// ���������ļ��޸�
        /// </summary>
        private void UpgradeConfig()
        {
            if (rblBBSVersion.SelectedIndex <= 2)   //Discuz!NT 2.5������Discuz!NT 2.6
            {
                UpgradeAdminMenu_V25();
                UpgradeScoreSet();
            }
            if (rblBBSVersion.SelectedIndex <= 3)   //Discuz!NT 2.6������Discuz!NT 3.0
            {
                UpgradeAdminMenu_V26();
            }
            if (rblBBSVersion.SelectedIndex <= 4)   //Discuz!NT 3.0������Discuz!NT 3.1
            {
                UpgradeAdminMenu_V30();
                UpgradeGeneralConfig();
                CreateUpdateUserCreditsProcedure();
                CreateInvitationSchedule();
                UpgradeScoresetForInvitation();
            }
            //����˵�����
            UpgradeAdminMenu_V35();
        }

        /// <summary>
        /// �ƶ�commonupgradeini.config�ļ���ConfigĿ¼
        /// </summary>
        private void MoveCommonUpgradeIniConfig()
        {
            string fileName = Utils.GetMapPath("commonupgradeini.config");
            if (File.Exists(fileName))
            {
                if (File.Exists(Utils.GetMapPath("../config/commonupgradeini.config")))
                    File.Delete(Utils.GetMapPath("../config/commonupgradeini.config"));
                File.Move(fileName, Utils.GetMapPath("../config/commonupgradeini.config"));
            }
        }

        #region ���汾���������ļ��ķ���

        /// <summary>
        /// Discuz!NT 2.5�������º�̨����˵�
        /// </summary>
        private void UpgradeAdminMenu_V25()
        {
            MenuManage.NewMenuItem(1020, "�����˵�����", "global/global_navigationmanage.aspx");
            MenuManage.NewMenuItem(6010, "������Ϣ����", "global/global_announceprivatemessage.aspx");
            MenuManage.DeleteMenuItem("ȫ ��", "����ѡ��", "���ҵġ��˵�");
            MenuManage.CreateMenuJson();
        }

        /// <summary>
        /// ���»�������
        /// </summary>
        private void UpgradeScoreSet()
        {
            string configPath = Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + "config/scoreset.config");
            XmlDocument doc = new XmlDocument();
            doc.Load(configPath);
            if (doc.SelectSingleNode("/scoreset/formula/bonuscreditstrans") == null)   //����ڵ���ھͲ���������
            {
                XmlElement bonuscreditstrans = doc.CreateElement("bonuscreditstrans");
                bonuscreditstrans.InnerText = "0";
                doc.SelectSingleNode("/scoreset/formula").InsertAfter(bonuscreditstrans, doc.SelectSingleNode("/scoreset/formula/creditstrans"));
            }
            if (doc.SelectSingleNode("/scoreset/formula/topicattachcreditstrans") == null)
            {
                XmlElement topicattachcreditstrans = doc.CreateElement("topicattachcreditstrans");
                topicattachcreditstrans.InnerText = "0";
                doc.SelectSingleNode("/scoreset/formula").InsertAfter(topicattachcreditstrans, doc.SelectSingleNode("/scoreset/formula/creditstrans"));
            }
            foreach (XmlNode node in doc.SelectNodes("/scoreset/record/name"))
            {
                node.InnerText = node.InnerText.Replace("(��)", "").Replace("(��)", "");
            }
            doc.Save(configPath);
        }

        /// <summary>
        /// Discuz!NT 2.6�������º�̨����˵�
        /// </summary>
        private void UpgradeAdminMenu_V26()
        {
            MenuManage.DeleteMenuItem("�� ��", "��������", "��̳ͷ���б�");
            MenuManage.DeleteMenuItem("�� ��", "���ݿ�", "���ݿ��Ż�");
            MenuManage.CreateMenuJson();
        }

        /// <summary>
        /// Discuz!NT 3.0�������º�̨����˵�
        /// </summary>
        private void UpgradeAdminMenu_V30()
        {
            MenuManage.EditMenuItem("�� ̳", "��̳�ۺ�", "�Ƽ����", "�Ƽ����", "aggregation/aggregation_recommendtopic.aspx");
            MenuManage.NewMenuItem(2030, "��̳����", "aggregation/aggregation_edithottopic.aspx");
            MenuManage.CreateMenuJson();
        }


        private void UpgradeAdminMenu_V35()
        {
            GeneralConfigInfo config = GeneralConfigs.GetConfig();
            if (config.Enablespace == 1 && Discuz.Plugin.Space.SpacePluginProvider.GetInstance() != null)
            {
                MenuManage.ImportPluginMenu(Utils.GetMapPath("space.xml"));
            }

            if (config.Enablealbum == 1 && Discuz.Plugin.Album.AlbumPluginProvider.GetInstance() != null)
            {
                MenuManage.ImportPluginMenu(Utils.GetMapPath("album.xml"));
            }
        }

        /// <summary>
        /// �޸ķ���վ���б�
        /// </summary>
        private void UpgradeGeneralConfig()
        {
            GeneralConfigInfo configInfo = GeneralConfigs.GetConfig();
            if (!configInfo.Sharelist.Contains("�ٶ��ղ�"))
                configInfo.Sharelist += ",5|qq|qq��ǩ|1,6|google|google��ǩ|1,7|vivi|�����ղ�|1,8|live|live�ղ�|1,9|favorite|�ղؼ�|1,10|baidu|�ٶ��ղ�|1";
            GeneralConfigs.Serialiaze(configInfo, Server.MapPath("../config/general.config"));
        }

        /// <summary>
        /// �����û����ִ洢����
        /// </summary>
        private void CreateUpdateUserCreditsProcedure()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(Server.MapPath("../config/scoreset.config"));
            Discuz.Forum.AdminForums.CreateUpdateUserCreditsProcedure(xmlDoc.SelectSingleNode("/scoreset/formula/formulacontext").InnerText, false);
        }

        /// <summary>
        /// ��������ƻ�����
        /// </summary>
        private void CreateInvitationSchedule()
        {
            ScheduleConfigInfo sci = ScheduleConfigs.GetConfig();
            //�����¼��Ƿ����
            foreach (Discuz.Config.Event ev1 in sci.Events)
            {
                if (ev1.Key == "InvitationEvent")
                    return;
            }

            //�����µ�����ƻ�����
            Discuz.Config.Event ev = new Discuz.Config.Event();
            ev.Key = "InvitationEvent";
            ev.Enabled = true;
            ev.IsSystemEvent = true;
            ev.ScheduleType = "Discuz.Event.InvitationEvent, Discuz.Event";
            ev.TimeOfDay = 2;
            ev.Minutes = 1;
            ScheduleConfigs.SaveConfig(sci);
        }

        /// <summary>
        /// ��������ע��Ļ��ֹ���
        /// </summary>
        private void UpgradeScoresetForInvitation()
        {
            DataSet scoreset = new DataSet();
            scoreset.ReadXml(Server.MapPath("../config/scoreset.config"));
            if (scoreset.Tables[0].Select("id='18' AND name='����ע��'").Length != 0)
                return;
            DataRow dr = scoreset.Tables[0].NewRow();
            dr["id"] = "18";
            dr["name"] = "����ע��";
            dr["extcredits1"] = "5";
            dr["extcredits2"] = "5";
            dr["extcredits3"] = "0";
            dr["extcredits4"] = "0";
            dr["extcredits5"] = "0";
            dr["extcredits6"] = "0";
            dr["extcredits7"] = "0";
            dr["extcredits8"] = "0";
            scoreset.Tables[0].Rows.Add(dr);
            scoreset.WriteXml(Server.MapPath("../config/scoreset.config"));
        }

        #endregion
    }
}
