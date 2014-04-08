using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Diagnostics;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using Discuz.Config;
using Discuz.Forum;
using Discuz.Web.UI;
using Discuz.Entity;
using Discuz.Common;
using Discuz.Common.Generic;

namespace Discuz.Web
{
    public class help : PageBase
    {
        #region 页面变量
        /// <summary>
        /// 帮助列表
        /// </summary>
        public List<HelpInfo> helplist = DNTRequest.GetInt("hid", 0) > 0 ? Helps.GetHelpList(DNTRequest.GetInt("hid", 0)) : Helps.GetHelpList();
        /// <summary>
        /// 数据库类型
        /// </summary>
        public string dbtype = BaseConfigs.GetDbType;
        /// <summary>
        /// 产品名称
        /// </summary>
        public string assemblyproductname = Utils.GetAssemblyProductName();
        /// <summary>
        /// 版权
        /// </summary>
        public string Assemblycopyright = Utils.GetAssemblyCopyright();
        /// <summary>
        /// 显示版本信息
        /// </summary>
        public int showversion = DNTRequest.GetInt("version", 0);
        #endregion

        #region DLL文件的版本信息
        public string dllver_discuzaggregation = "";
        public string dllver_discuzcache = "";
        public string dllver_discuzcommon = "";
        public string dllver_discuzconfig = "";
        public string dllver_discuzcontrol = "";
        public string dllver_discuzdata = "";
        public string dllver_discuzdatasqlserver = "";
        public string dllver_discuzdataaccess = "";
        public string dllver_discuzdatamysql = "";
        public string dllver_discuzentity = "";
        public string dllver_discuzevent = "";
        public string dllver_discuzforum = "";
        public string dllver_discuzplugin = "";
        public string dllver_discuzpluginmailsysmail = "";
        public string dllver_discuzpluginpasswordmode = "";
        public string dllver_discuzpluginpreviewjpg = "";
        public string dllver_discuzpluginspread = "";
        public string dllver_discuzspace = "";
        public string dllver_discuzwebadmin = "";
        public string dllver_discuzweb = "";
        public string dllver_discuzwebservice = "";
        #endregion

        protected override void ShowPage()
        {
            pagetitle = "帮助";
   
            if (helplist == null)
            {
                AddErrLine("没有信息可读取！");
                return;
            }

            if (showversion == 1)
            {
                dllver_discuzaggregation = LoadDllVersion(HttpRuntime.BinDirectory + "Discuz.Aggregation.dll");
                dllver_discuzcache = LoadDllVersion(HttpRuntime.BinDirectory + "Discuz.Cache.dll");
                dllver_discuzcommon = LoadDllVersion(HttpRuntime.BinDirectory + "Discuz.Common.dll");
                dllver_discuzconfig = LoadDllVersion(HttpRuntime.BinDirectory + "Discuz.Config.dll");
                dllver_discuzcontrol = LoadDllVersion(HttpRuntime.BinDirectory + "Discuz.Control.dll");
                dllver_discuzdata = LoadDllVersion(HttpRuntime.BinDirectory + "Discuz.Data.dll");
                dllver_discuzdatasqlserver = LoadDllVersion(HttpRuntime.BinDirectory + "Discuz.Data.SqlServer.dll");
                dllver_discuzdataaccess = LoadDllVersion(HttpRuntime.BinDirectory + "Discuz.Data.Access.dll");
                dllver_discuzdatamysql = LoadDllVersion(HttpRuntime.BinDirectory + "Discuz.Data.MySql.dll");
                dllver_discuzentity = LoadDllVersion(HttpRuntime.BinDirectory + "Discuz.Entity.dll");
                dllver_discuzevent = LoadDllVersion(HttpRuntime.BinDirectory + "Discuz.Event.dll");
                dllver_discuzforum = LoadDllVersion(HttpRuntime.BinDirectory + "Discuz.Forum.dll");
                dllver_discuzplugin = LoadDllVersion(HttpRuntime.BinDirectory + "Discuz.Plugin.dll");
                dllver_discuzpluginmailsysmail = LoadDllVersion(HttpRuntime.BinDirectory + "Discuz.Plugin.Mail.SysMail.dll");
                dllver_discuzpluginpasswordmode = LoadDllVersion(HttpRuntime.BinDirectory + "Discuz.Plugin.PasswordMode.dll");
                dllver_discuzpluginpreviewjpg = LoadDllVersion(HttpRuntime.BinDirectory + "Discuz.Plugin.Preview.Jpg.dll");;
                dllver_discuzpluginspread = LoadDllVersion(HttpRuntime.BinDirectory + "Discuz.Plugin.Spread.dll");
                dllver_discuzspace = LoadDllVersion(HttpRuntime.BinDirectory + "Discuz.Space.dll");
                dllver_discuzwebadmin = LoadDllVersion(HttpRuntime.BinDirectory + "Discuz.Web.Admin.dll");
                dllver_discuzweb = LoadDllVersion(HttpRuntime.BinDirectory + "Discuz.Web.dll");
                dllver_discuzwebservice = LoadDllVersion(HttpRuntime.BinDirectory + "Discuz.Web.Services.dll");
            }
        }

        /// <summary>
        /// 获取指定DLL文件的版本信息
        /// </summary>
        /// <param name="fullfilename"></param>
        /// <returns></returns>
        private string LoadDllVersion(string fullfilename)
        {
            try
            {
                FileVersionInfo AssemblyFileVersion = FileVersionInfo.GetVersionInfo(fullfilename);
                return string.Format("{0}.{1}.{2}", AssemblyFileVersion.FileMajorPart, AssemblyFileVersion.FileMinorPart, AssemblyFileVersion.FileBuildPart);
            }
            catch
            {
                return "未能加载dll或该dll文件不存在!";
            }
        }
    }
}