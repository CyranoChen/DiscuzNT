using System;
using System.Data;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;

using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Plugin.PasswordMode;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// login 的摘要说明. 
    /// </summary>
    public partial class syslogin : Page
    {
        /// <summary>
        /// 当前登陆用户的在线ID
        /// </summary>
        public int olid;
        /// <summary>
        /// 论坛配置文件变量
        /// </summary>
        protected internal GeneralConfigInfo config;
        /// <summary>
        /// 页面尾部信息
        /// </summary>
        public string footer = "";


        public syslogin()
        {
            #region 加载尾部信息
            footer = "<div align=\"center\" style=\" padding-top:60px;font-size:11px; font-family: Arial\">";
            footer += "<hr style=\"height:1; width:600; height:1; color:#CCCCCC\" />Powered by ";
            footer += "<a style=\"COLOR: #000000\" href=\"http://nt.discuz.net\" target=\"_blank\">";
            footer += Utils.GetAssemblyProductName();
            footer += "</a> &nbsp;&copy; 2001-";
            footer += Utils.GetAssemblyCopyright().Split(',')[0];
            footer += ", <a style=\"COLOR: #000000;font-weight:bold\" href=\"http://www.comsenz.com\" target=\"_blank\">Comsenz Inc.</a></div>";
            #endregion
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            UserName.Attributes.Remove("class");
            PassWord.Attributes.Remove("class");
            UserName.AddAttributes("style", "width:200px");
            PassWord.AddAttributes("style", "width:200px");
           
            config = GeneralConfigs.GetConfig();

            OnlineUserInfo oluserinfo = Discuz.Forum.OnlineUsers.UpdateInfo(config.Passwordkey, config.Onlinetimeout);

            olid = oluserinfo.Olid;

            if (!Page.IsPostBack)
            {
                #region 如果IP访问列表有设置则进行判断
                if (config.Adminipaccess.Trim() != "")
                {
                    string[] regctrl = Utils.SplitString(config.Adminipaccess, "\n");
                    if (!Utils.InIPArray(DNTRequest.GetIP(), regctrl))
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append("<br /><br /><div style=\"width:100%\" align=\"center\"><div align=\"center\" style=\"width:600px; border:1px dotted #FF6600; background-color:#FFFCEC; margin:auto; padding:20px;\">");
                        sb.Append("<img src=\"images/hint.gif\" border=\"0\" alt=\"提示:\" align=\"absmiddle\" />&nbsp; 您的IP地址不在系统允许的范围之内</div></div>");
                        Response.Write(sb.ToString());
                        Response.End();
                        return;
                    }
                }
                #endregion

                #region 用户身份判断
                UserGroupInfo usergroupinfo = AdminUserGroups.AdminGetUserGroupInfo(oluserinfo.Groupid);
                if (oluserinfo.Userid <= 0 || usergroupinfo.Radminid != 1)
                {
                    string message = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">";
                    message += "<html xmlns=\"http://www.w3.org/1999/xhtml\"><head><title>无法确认您的身份</title><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">";
                    message += "<link href=\"styles/default.css\" type=\"text/css\" rel=\"stylesheet\"></head><script type=\"text/javascript\">if(top.location!=self.location){top.location.href = \"syslogin.aspx\";}</script><body><br /><br /><div style=\"width:100%\" align=\"center\">";
                    message += "<div align=\"center\" style=\"width:600px; border:1px dotted #FF6600; background-color:#FFFCEC; margin:auto; padding:20px;\"><img src=\"images/hint.gif\" border=\"0\" alt=\"提示:\" align=\"absmiddle\" width=\"11\" height=\"13\" /> &nbsp;";
                    message += "无法确认您的身份, 请<a href=\"../login.aspx\">登录</a></div></div></body></html>";
                    Response.Write(message);
                    Response.End();
                    return;
                }
                #endregion


                #region 判断安装目录文件信息
                if (IsExistsSetupFile())
                {
                    string message = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">";
                    message += "<html xmlns=\"http://www.w3.org/1999/xhtml\"><head><title>请将您的安装目录即install/目录下的文件全部删除, 以免其它用户运行安装该程序!</title><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">";
                    message += "<link href=\"styles/default.css\" type=\"text/css\" rel=\"stylesheet\"></head><script type=\"text/javascript\">if(top.location!=self.location){top.location.href = \"syslogin.aspx\";}</script><body><br /><br /><div style=\"width:100%\" align=\"center\">";
                    message += "<div align=\"center\" style=\"width:660px; border:1px dotted #FF6600; background-color:#FFFCEC; margin:auto; padding:20px;\"><img src=\"images/hint.gif\" border=\"0\" alt=\"提示:\" align=\"absmiddle\" width=\"11\" height=\"13\" /> &nbsp;";
                    message += "请将您的安装目录(install/)下和升级目录(upgrade/)下的.aspx文件及bin/Discuz.Install.dll全部删除, 以免其它用户运行安装或升级程序!</div></div></body></html>";
                    Response.Write(message);
                    Response.End();
                    return;
                }
                #endregion
                               

                #region 显示相关页面登陆提交信息
                if (Context.Request.Cookies["dntadmin"] == null || Context.Request.Cookies["dntadmin"]["key"] == null || 
                    ForumUtils.GetCookiePassword(Context.Request.Cookies["dntadmin"]["key"].ToString(), config.Passwordkey) != 
                    (oluserinfo.Password + Discuz.Forum.Users.GetUserInfo(oluserinfo.Userid).Secques + oluserinfo.Userid.ToString()))
                {
                    Msg.Text = "<IMG alt=\"提示:\" src=\"images/warning.gif\" align=\"absMiddle\" border=\"0\" width=\"16\" height=\"16\">请重新进行管理员登录";
                }

                if (oluserinfo.Userid > 0 && usergroupinfo.Radminid == 1 && oluserinfo.Username.Trim() != "")
                {
                    UserName.Text = oluserinfo.Username;
                    UserName.AddAttributes("readonly", "true");
                    UserName.CssClass = "nofocus";
                    UserName.Attributes.Add("onfocus", "this.className='nofocus';");
                    UserName.Attributes.Add("onblur", "this.className='nofocus';");
                }

                if (DNTRequest.GetString("result") == "1")
                {
                    Msg.Text = "<IMG alt=\"提示:\" src=\"images/warning.gif\" align=\"absMiddle\" border=\"0\" width=\"16\" height=\"16\"><font color=\"red\">用户不存在或密码错误</font>";
                    return;
                }

                if (DNTRequest.GetString("result") == "2")
                {
                    Msg.Text = "<IMG alt=\"提示:\" src=\"images/warning.gif\" align=\"absMiddle\" border=\"0\" width=\"16\" height=\"16\"><font color=\"red\">用户不是管理员身分,因此无法登陆后台</font>";
                    return;
                }

                if (DNTRequest.GetString("result") == "3")
                {
                    Msg.Text = "<IMG alt=\"提示:\" src=\"images/warning.gif\" align=\"absMiddle\" border=\"0\" width=\"16\" height=\"16\"><font color=\"red\">验证码错误,请重新输入</font>";
                    return;
                }

                if (DNTRequest.GetString("result") == "4")
                {
                    Msg.Text = "";
                    return;
                }
                #endregion
            }
            
            if (Page.IsPostBack)
                VerifyLoginInf();//对提供的信息进行验证
            else
                Response.Redirect("syslogin.aspx?result=4");
        }


        /// <summary>
        /// 检查安装用录下是否有安装文件,但有就删除,如删除出问题就返回true
        /// </summary>
        /// <returns></returns>
        public bool IsExistsSetupFile()
        {
            #region 检查安装目录
            string[] installFiles = {
#if !DEBUG
                                        "../install/index.aspx","../install/step2.aspx","../install/step3.aspx","../install/step4.aspx","../install/succeed.aspx",
                                     "../install/systemfile.aspx","../install/pluginsetup.aspx","../install/album.xml","../install/space.xml","../install/mall.xml",
                                        "../install/ajax.aspx","../install/install.aspx",
#endif
                                        "../bin/Discuz.Install.dll"
                                    };
            foreach(string file in installFiles)
            {
                if (CheckAndDeleteFile(Utils.GetMapPath(file)))
                {
                    return true;
                }
            }          
            #endregion

            #region 检查升级目录
            string[] upgradeFiles = {
#if !DEBUG

                                        "../upgrade/index.aspx", "../upgrade/step2.aspx", "../upgrade/succeed.aspx",  "../upgrade/changeavatars.aspx" 
#endif
                                    };

            foreach (string file in upgradeFiles)
            {
                if (CheckAndDeleteFile(Utils.GetMapPath(file)))
                {
                    return true;
                } 
            }
            #endregion

            return false;//表示无安装文件
        }


        //检查并删除指定路径的文件
        private bool CheckAndDeleteFile(string path)
        {
            if (Utils.FileExists(path))
            {
                try
                {
                    File.Delete(path);
                    return false;
                }
                catch
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }


        public void VerifyLoginInf()
        {
            if (!Discuz.Forum.OnlineUsers.CheckUserVerifyCode(olid, DNTRequest.GetString("vcode")))
            {
                Response.Redirect("syslogin.aspx?result=3");
                return;
            }
            
            UserInfo userInfo = null;
            if (config.Passwordmode == 1)
                userInfo = Users.GetUserInfo(Users.CheckDvBbsPassword(DNTRequest.GetString("username"), DNTRequest.GetString("password")));
            else if (config.Passwordmode == 0)
                userInfo = Users.GetUserInfo(Users.CheckPassword(DNTRequest.GetString("username"), Utils.MD5(DNTRequest.GetString("password")), false));
            else//第三方加密验证模式
                userInfo = Users.CheckThirdPartPassword(DNTRequest.GetString("username"), DNTRequest.GetString("password"), -1, null);

            if (userInfo != null)
            {
                UserGroupInfo usergroupinfo = AdminUserGroups.AdminGetUserGroupInfo(userInfo.Groupid);

                if (usergroupinfo.Radminid == 1)
                {
                    ForumUtils.WriteUserCookie(userInfo.Uid, 1440, GeneralConfigs.GetConfig().Passwordkey);

                    UserGroupInfo userGroupInfo = AdminUserGroups.AdminGetUserGroupInfo(userInfo.Groupid);

                    HttpCookie cookie = new HttpCookie("dntadmin");
                    cookie.Values["key"] = ForumUtils.SetCookiePassword(userInfo.Password + userInfo.Secques + userInfo.Uid, config.Passwordkey);
                    cookie.Expires = DateTime.Now.AddMinutes(30);
                    HttpContext.Current.Response.AppendCookie(cookie);

                    AdminVistLogs.InsertLog(userInfo.Uid, userInfo.Username, userInfo.Groupid, userGroupInfo.Grouptitle, DNTRequest.GetIP(), "后台管理员登陆", "");

                    try
                    {
                        SoftInfo.LoadSoftInfo();
                    }
                    catch
                    {
                        Response.Write("<script type=\"text/javascript\">top.location.href='index.aspx';</script>");
                        Response.End();
                    }

                    //升级general.config文件
                    try
                    {
                        GeneralConfigs.Serialiaze(GeneralConfigs.GetConfig(), Server.MapPath("../config/general.config"));
                    }
                    catch { }

                    Response.Write("<script type=\"text/javascript\">top.location.href='index.aspx';</script>");
                    Response.End();
                }
                else
                    Response.Redirect("syslogin.aspx?result=2");
            }
            else
                Response.Redirect("syslogin.aspx?result=1");
        }

        protected override void SavePageStateToPersistenceMedium(object viewState)
        {
            base.SavePageStateToPersistenceMedium(viewState);
        }

        protected override object LoadPageStateFromPersistenceMedium()
        {
            object o = new object();
            try
            {
                o = base.LoadPageStateFromPersistenceMedium();
            }
            catch
            {
                o = null;
            }
            return o;
        }

    }
}