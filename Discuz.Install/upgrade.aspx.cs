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
        /// 检测数据库链接
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
                Response.Write("{\"Result\":false,\"Message\":\"在与 SQL Server 建立连接时出现与网络相关的或特定于实例的错误,请检查dnt.config中的连接字符串是否正确\"}");
            }
            Response.End();
        }

        /// <summary>
        /// 升级表和存储过程的方法
        /// </summary>
        private void UpgradeProcess()
        {
            //当数据库不是Sql Server不允许升级
            if (baseconfig.Dbtype != "SqlServer")
                return;

            //将首页默认定为论坛首页
            GeneralConfigs.GetConfig().Forumurl = "forumindex.aspx";
            GeneralConfigs.Serialiaze(GeneralConfigs.GetConfig(), Server.MapPath("../config/general.config"));

            //获取升级脚本的文件夹
            string upgradeSqlScriptPath = string.Format("sqlscript/{0}/",baseconfig.Dbtype.ToString().Trim());
            string upgradeTableScriptFileName = Server.MapPath(string.Format("{0}upgradetable{1}.sql", upgradeSqlScriptPath, rblBBSVersion.SelectedValue));
            string upgradeProcedureScriptFileName = Server.MapPath(string.Format("{0}upgradeprocedure_2000.sql",upgradeSqlScriptPath));
            if (DbHelper.ExecuteScalar(CommandType.Text, "SELECT @@VERSION").ToString().Substring(20, 24).IndexOf("2000") == -1)    //查询SQLSERVER的版本
                upgradeProcedureScriptFileName = Server.MapPath(string.Format("{0}upgradeprocedure_2005.sql",upgradeSqlScriptPath));
            //升级脚本不存在
            if (!File.Exists(upgradeTableScriptFileName) || !File.Exists(upgradeProcedureScriptFileName))
                return;

            //升级表
            UpgradeTable(upgradeTableScriptFileName);
            //升级存储过程
            UpgradeProcedure(upgradeProcedureScriptFileName);
        }

        /// <summary>
        /// 表结构升级的方法
        /// </summary>
        /// <param name="upgradeTableScriptPath"></param>
        private void UpgradeTable(string upgradeTableScriptPath)
        {
            ExecuteScript(upgradeTableScriptPath);
        }

        /// <summary>
        /// 存储过程升级的方法
        /// </summary>
        /// <param name="upgradeProcedureScriptPath"></param>
        private void UpgradeProcedure(string upgradeProcedureScriptPath)
        {
            ExecuteScript(upgradeProcedureScriptPath);
            DatabaseProvider.GetInstance().UpdatePostSP();
        }

        /// <summary>
        /// 执行sql脚本
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
        /// 升级配置文件修改
        /// </summary>
        private void UpgradeConfig()
        {
            if (rblBBSVersion.SelectedIndex <= 2)   //Discuz!NT 2.5升级到Discuz!NT 2.6
            {
                UpgradeAdminMenu_V25();
                UpgradeScoreSet();
            }
            if (rblBBSVersion.SelectedIndex <= 3)   //Discuz!NT 2.6升级到Discuz!NT 3.0
            {
                UpgradeAdminMenu_V26();
            }
            if (rblBBSVersion.SelectedIndex <= 4)   //Discuz!NT 3.0升级到Discuz!NT 3.1
            {
                UpgradeAdminMenu_V30();
                UpgradeGeneralConfig();
                CreateUpdateUserCreditsProcedure();
                CreateInvitationSchedule();
                UpgradeScoresetForInvitation();
            }
            //插件菜单升级
            UpgradeAdminMenu_V35();
        }

        /// <summary>
        /// 移动commonupgradeini.config文件到Config目录
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

        #region 各版本升级配置文件的方法

        /// <summary>
        /// Discuz!NT 2.5升级更新后台管理菜单
        /// </summary>
        private void UpgradeAdminMenu_V25()
        {
            MenuManage.NewMenuItem(1020, "导航菜单管理", "global/global_navigationmanage.aspx");
            MenuManage.NewMenuItem(6010, "公共消息管理", "global/global_announceprivatemessage.aspx");
            MenuManage.DeleteMenuItem("全 局", "常规选项", "“我的”菜单");
            MenuManage.CreateMenuJson();
        }

        /// <summary>
        /// 更新积分设置
        /// </summary>
        private void UpgradeScoreSet()
        {
            string configPath = Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + "config/scoreset.config");
            XmlDocument doc = new XmlDocument();
            doc.Load(configPath);
            if (doc.SelectSingleNode("/scoreset/formula/bonuscreditstrans") == null)   //如果节点存在就不必再增加
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
                node.InnerText = node.InnerText.Replace("(＋)", "").Replace("(－)", "");
            }
            doc.Save(configPath);
        }

        /// <summary>
        /// Discuz!NT 2.6升级更新后台管理菜单
        /// </summary>
        private void UpgradeAdminMenu_V26()
        {
            MenuManage.DeleteMenuItem("其 他", "其它设置", "论坛头像列表");
            MenuManage.DeleteMenuItem("工 具", "数据库", "数据库优化");
            MenuManage.CreateMenuJson();
        }

        /// <summary>
        /// Discuz!NT 3.0升级更新后台管理菜单
        /// </summary>
        private void UpgradeAdminMenu_V30()
        {
            MenuManage.EditMenuItem("论 坛", "论坛聚合", "推荐版块", "推荐版块", "aggregation/aggregation_recommendtopic.aspx");
            MenuManage.NewMenuItem(2030, "论坛热帖", "aggregation/aggregation_edithottopic.aspx");
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
        /// 修改分享站点列表
        /// </summary>
        private void UpgradeGeneralConfig()
        {
            GeneralConfigInfo configInfo = GeneralConfigs.GetConfig();
            if (!configInfo.Sharelist.Contains("百度收藏"))
                configInfo.Sharelist += ",5|qq|qq书签|1,6|google|google书签|1,7|vivi|爱问收藏|1,8|live|live收藏|1,9|favorite|收藏夹|1,10|baidu|百度收藏|1";
            GeneralConfigs.Serialiaze(configInfo, Server.MapPath("../config/general.config"));
        }

        /// <summary>
        /// 建立用户积分存储过程
        /// </summary>
        private void CreateUpdateUserCreditsProcedure()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(Server.MapPath("../config/scoreset.config"));
            Discuz.Forum.AdminForums.CreateUpdateUserCreditsProcedure(xmlDoc.SelectSingleNode("/scoreset/formula/formulacontext").InnerText, false);
        }

        /// <summary>
        /// 建立邀请计划任务
        /// </summary>
        private void CreateInvitationSchedule()
        {
            ScheduleConfigInfo sci = ScheduleConfigs.GetConfig();
            //检查该事件是否存在
            foreach (Discuz.Config.Event ev1 in sci.Events)
            {
                if (ev1.Key == "InvitationEvent")
                    return;
            }

            //建立新的邀请计划任务
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
        /// 增加邀请注册的积分规则
        /// </summary>
        private void UpgradeScoresetForInvitation()
        {
            DataSet scoreset = new DataSet();
            scoreset.ReadXml(Server.MapPath("../config/scoreset.config"));
            if (scoreset.Tables[0].Select("id='18' AND name='邀请注册'").Length != 0)
                return;
            DataRow dr = scoreset.Tables[0].NewRow();
            dr["id"] = "18";
            dr["name"] = "邀请注册";
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
