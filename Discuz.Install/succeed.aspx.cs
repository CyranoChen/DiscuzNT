using System;
using Discuz.Install;
using Discuz.Config;

namespace Discuz.Install
{
    public class Succeed : SetupPage
    {
        // 是否显示插件安装链接
        //public bool showInstallPluginLink = false;
        protected string userID = "";
        protected string password = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //插件是否可以安装检测
                //if (Discuz.Plugin.Space.SpacePluginProvider.GetInstance() != null ||
                //    Discuz.Plugin.Album.AlbumPluginProvider.GetInstance() != null ||
                //    Discuz.Plugin.Mall.MallPluginProvider.GetInstance() != null)
                //{
                //    showInstallPluginLink = true;
                //    return;
                //}
                userID = Request.Form["adminName"];
                password = Request.Form["adminPassword"];

                GeneralConfigInfo config = GeneralConfigs.GetConfig();
                config.Installation = 1;
                GeneralConfigs.Serialiaze(config, Server.MapPath("../config/general.config"));
            }
        }

        
      
    }
}
