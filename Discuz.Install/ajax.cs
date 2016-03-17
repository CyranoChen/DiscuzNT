using System;
using System.Collections.Generic;
using System.Text;

using Discuz.Common;
using Discuz.Config;
using Discuz.Config.Provider;

namespace Discuz.Install
{
    public class ajax : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string t = DNTRequest.GetString("t");

            string sqlIp = DNTRequest.GetString("ip");
            string sqlName = DNTRequest.GetString("name");
            string sqlLoginName = DNTRequest.GetString("loginname");
            string sqlLoginAuth = DNTRequest.GetString("loginpwd");
            string tablePrefix = DNTRequest.GetString("prefix");

            string result = "";

            switch (t)
            {
                case "checkdbconnection"://检查数据库连接
                    result = InstallUtils.CheckDBConnection(sqlIp, sqlLoginName, sqlLoginAuth, sqlName);
                    break;
                case "createdb"://创建新的数据库
                    result = InstallUtils.CreateDatabase(sqlIp, sqlLoginName, sqlLoginAuth, sqlName);
                    break;
                case "checkdbcollation"://检查数据库排序规则
                    result = InstallUtils.CheckDBCollation(sqlIp, sqlLoginName, sqlLoginAuth, sqlName);
                    break;
                case "dbsourceexist":
                    result = InstallUtils.DBSourceExist(sqlIp, sqlLoginName, sqlLoginAuth, sqlName, tablePrefix);
                    break;
                case "savedbset"://保存数据库配置
                    InstallUtils.EditDntConfig(sqlIp, sqlLoginName, sqlLoginAuth, sqlName, tablePrefix);
                    result = "{result:true,message:\"配置保存成功\"}";
                    break;
                case "createtable"://创建论坛数据表
                    result = InstallUtils.CreateTable();
                    break;
                case "createsp"://创建论坛存储过程
                    result = InstallUtils.CreateStorePocedure();
                    break;
                case "initsource"://生成初始数据
                    result = InstallUtils.InitialForumSource(DNTRequest.GetString("admin"), DNTRequest.GetString("pwd"));
                    break;
            }
            Response.Write(result);
        }
    }
}
