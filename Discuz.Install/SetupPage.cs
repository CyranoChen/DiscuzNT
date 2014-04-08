using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;

using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;
using System.Collections;

namespace Discuz.Install
{
    /// <summary>
    /// SetupPage ��ժҪ˵����
    /// </summary>
    public class SetupPage : System.Web.UI.Page
    {

        public static readonly string producename = Utils.GetAssemblyProductName();  //��ǰ��Ʒ�汾����

        public static readonly string footer = "";

        public static readonly string logo = "<img src=\"images/logo.jpg\" width=\"180\" height=\"300\">"; //��װ��LOGO

        public static readonly string header = ""; //htmlҳ�ĵ�<head>����
        public static int isError = 0;

        static SetupPage()
        {

            header = "<HEAD><title>��װ " + Utils.GetAssemblyProductName() + "</title><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">\r\n";
            header += "<LINK rev=\"stylesheet\" media=\"all\" href=\"css/styles.css\" type=\"text/css\" rel=\"stylesheet\"></HEAD>\r\n";
            header += "<script language=\"javascript\" src=\"js/setup.js\"></script>\r\n";

            footer = "\r\n<br />\r\n<br /><table width=\"700\" border=\"0\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\" ID=\"Table1\">";
            footer += "<tr><td align=\"center\"><div align=\"center\" style=\"position:relative ; padding-top:60px;font-size:14px; font-family: Arial\">";
            footer += "<hr style=\"height:1; width:600; height:1; color:#CCCCCC\" />Powered by <a style=\"COLOR: #000000\" href=\"http://nt.discuz.net\" target=\"_blank\">" + Utils.GetAssemblyProductName() + "</a>";
            footer += " &nbsp;<br />&copy; 2001-" + Utils.GetAssemblyCopyright().Split(',')[0] + " <a style=\"COLOR: #000000;font-weight:bold\" href=\"http://www.comsenz.com\" target=\"_blank\">Comsenz Inc.</a></div></td></tr></table>";
        }


        //���û������ťʱ��������Ϊ��Ч
        public void DisableSubmitBotton(Page mypage, System.Web.UI.WebControls.Button submitbutton)
        {
            RegisterAdminPageClientScriptBlock();

            //��֤ __doPostBack(eventTarget, eventArgument) ��ȷע��
#if NET1
            mypage.GetPostBackEventReference(submitbutton,"");        
#else
            mypage.ClientScript.GetPostBackEventReference(submitbutton, "");
#endif

            StringBuilder sb = new StringBuilder();

            //��֤��֤������ִ��
            sb.Append("if (typeof(Page_ClientValidate) == 'function') { if (Page_ClientValidate() == false) { return false; }}");

            // disable����submit��ť
            sb.Append("disableOtherSubmit();");

            //sb.Append("document.getElementById('Layer5').innerHTML ='�������в���</td></tr></table><BR />';");
            sb.Append("document.getElementById('success').style.display ='block';");

#if NET1   
    //��__doPostBack���ύ����֤��ť�ķ�������click�¼�ִ��
            sb.Append(this.GetPostBackEventReference(submitbutton,""));   
#else
            sb.Append(this.ClientScript.GetPostBackEventReference(submitbutton, ""));
#endif
            sb.Append(";");
            submitbutton.Attributes.Add("onclick", sb.ToString());
        }


        public void RegisterAdminPageClientScriptBlock()
        {

            string script = "<div id=\"success\" style=\"position:absolute;z-index:300;height:120px;width:284px;left:50%;top:50%;margin-left:-150px;margin-top:-80px;\">\r\n" +
                          "	<div id=\"Layer2\" style=\"position:absolute;z-index:300;width:270px;height:90px;background-color: #FFFFFF;border:solid #000000 1px;font-size:14px;\">\r\n" +
                          "		<div id=\"Layer4\" style=\"height:26px;background:#333333;line-height:26px;padding:0px 3px 0px 3px;font-weight:bolder;color:#fff \">������ʾ</div>\r\n" +
                          "		<div id=\"Layer5\" style=\"height:64px;line-height:150%;padding:0px 3px 0px 3px;\" align=\"center\"><br />����ִ�в���,���Ե�...</div>\r\n" +
                          "	</div>\r\n" +
                          "	<div id=\"Layer3\" style=\"position:absolute;width:270px;height:90px;z-index:299;left:4px;top:5px;background-color: #cccccc;\"></div>\r\n" +
                          "</div>\r\n" +
                          "<script> \r\n" +
                          "document.getElementById('success').style.display ='none'; \r\n" +
                          "</script> \r\n" +
                          "<script language=\"JavaScript1.2\" src=\"../js/divcover.js\"></script>\r\n";


#if NET1
            base.RegisterClientScriptBlock("Page", script);
#else
            base.ClientScript.RegisterClientScriptBlock(this.GetType(), "Page", script);
#endif
        }

        public new void RegisterStartupScript(string key, string scriptstr)
        {

            string message = "<BR />�����ɹ�, ϵͳ��������̳��ҳ...";

            if (key == "PAGE")
            {
                string script = "";

                script = "<div id=\"success\" style=\"position:absolute;z-index:300;height:120px;width:284px;left:50%;top:50%;margin-left:-150px;margin-top:-80px;\">\r\n" +
                       "	<div id=\"Layer2\" style=\"position:absolute;z-index:300;width:270px;height:90px;background-color: #FFFFFF;border:solid #000000 1px;font-size:14px;\">\r\n" +
                       "		<div id=\"Layer4\" style=\"height:26px;background:#333;line-height:26px;padding:0px 3px 0px 3px;font-weight:bolder;color:#fff \">������ʾ</div>\r\n" +
                       "		<div id=\"Layer5\" style=\"height:64px;line-height:150%;padding:0px 3px 0px 3px;\" align=\"center\">" + message + "</div>\r\n" +
                       "	</div>\r\n" +
                       "	<div id=\"Layer3\" style=\"position:absolute;width:270px;height:90px;z-index:299;left:4px;top:5px;background-color: #cccccc;\"></div>\r\n" +
                       "</div>\r\n" +
                       "<script> \r\n" +
                       "var bar=0;\r\n" +
                       "document.getElementById('success').style.display = \"block\"; \r\n" +
                       "count() ; \r\n" +
                       "function count(){ \r\n" +
                       "bar=bar+4; \r\n" +
                       "if (bar<99) \r\n" +
                       "{setTimeout(\"count()\",100);} \r\n" +
                       "else { \r\n" +
                       "	document.getElementById('success').style.display = \"none\"; \r\n" +
                       scriptstr + "} \r\n" +
                       "} \r\n" +
                       "</script> \r\n" +
                       "<script language=\"JavaScript1.2\" src=\"../admin/js/divcover.js\"></script>\r\n" +
                       "<script> window.onload = function(){HideOverSels('success')};</script>\r\n";

                Response.Write(script);
            }
            else
            {
#if NET1
                base.RegisterStartupScript(key, scriptstr);
#else
                base.ClientScript.RegisterStartupScript(this.GetType(), key, scriptstr);
#endif
            }
        }

        public static string InitialSystemValidCheck(ref bool error)
        {
            error = false;
            StringBuilder sb = new StringBuilder();

            HttpContext context = HttpContext.Current;

            string filename = null;
            if (context != null)
                filename = context.Server.MapPath("/DNT.config");
            else
                filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DNT.config");

            //ϵͳBINĿ¼���
            sb.Append(IISSystemBINCheck(ref error));


            //���Dnt.config�ļ�����Ч��
            if (!GetRootDntconfigPath())
            {
                sb.Append("<li><cite><img src=\"images/error.gif\" alt=\"ʧ��\"/></cite><a href=\"#\">DNT.config ����д��û�з�����ȷ, ������������װ�ĵ�!</a></li>");
                error = true;
                isError = 1;
            }
            else
            {
                sb.Append("<li><cite><img src=\"images/ok.gif\" alt=\"�ɹ�\"/></cite><a href=\"#\">�� DNT.config ��֤ͨ��!</a></li>");
            }

            //���ϵͳĿ¼����Ч��
            string folderstr = "admin,aspx,avatars,cache,config,editor,images,templates,upload";
            foreach (string foldler in folderstr.Split(','))
            {
                if (!SystemFolderCheck(foldler))
                {
                    sb.Append("<li><cite><img src=\"images/error.gif\" alt=\"ʧ��\"/></cite><a href=\"#\">�� " + foldler + " Ŀ¼û��д���ɾ��Ȩ��!</a></li>");
                    error = true;
                    isError = 1;
                }
                else
                {
                    sb.Append("<li><cite><img src=\"images/ok.gif\" alt=\"�ɹ�\"/></cite><a href=\"#\">�� " + foldler + " Ŀ¼Ȩ����֤ͨ��!</a></li>");
                }
            }
            string filestr = "admin\\xml\\navmenu.config,javascript\\common.js,install\\systemfile.aspx,upgrade\\systemfile.aspx";
            foreach (string file in filestr.Split(','))
            {
                if (!SystemFileCheck(file))
                {
                    sb.Append("<li><cite><img src=\"images/error.gif\" alt=\"ʧ��\"/></cite><a href=\"#\">>�� " + file.Substring(0, file.LastIndexOf('\\')) + " Ŀ¼û��д���ɾ��Ȩ��!</a></li>");
                    error = true;
                    isError = 1;
                }
                else
                {
                    sb.Append("<li><cite><img src=\"images/ok.gif\" alt=\"�ɹ�\"/></cite><a href=\"#\">�� " + file.Substring(0, file.LastIndexOf('\\')) + " Ŀ¼Ȩ����֤ͨ��!</a></li>");
                }
            }

           if(!TempTest())
           {
               sb.Append("<li><cite><img src=\"images/error.gif\" alt=\"ʧ��\"/></cite><a href=\"#\">>��û�ж� " + Path.GetTempPath() + " �ļ��з���Ȩ�ޣ�����μ���װ�ĵ�.</a></li>");
               error = true;
               isError = 1;
           }
           else
           {
            if (!SerialiazeTest())
            {
                sb.Append("<li><cite><img src=\"images/error.gif\" alt=\"ʧ��\"/></cite><a href=\"#\">>��config�ļ������л�ʧ�ܣ�����μ���װ�ĵ�.</a></li>");
                error = true;
                isError = 1;
            }
            else
                sb.Append("<li><cite><img src=\"images/ok.gif\" alt=\"�ɹ�\"/></cite><a href=\"#\">�����л���֤ͨ��!</a></li>");
           }
            return sb.ToString();
        }
        public static bool GetRootDntconfigPath()
        {
            try
            {

                HttpContext context = HttpContext.Current;

                string webconfigfile = "";
                if (!Utils.FileExists(webconfigfile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dnt.config"))
                    && (!Utils.FileExists(webconfigfile = Utils.GetMapPath("~/dnt.config"))) 
                    && (!Utils.FileExists(webconfigfile = Path.Combine(context.Request.PhysicalApplicationPath, "dnt.config")))
                    && (!Utils.FileExists(webconfigfile = Utils.GetMapPath("../dnt.config")))
                    && (!Utils.FileExists(webconfigfile = Utils.GetMapPath("../../dnt.config")))
                    && (!Utils.FileExists(webconfigfile = Utils.GetMapPath("../../../dnt.config"))))
                {
                    return false;
                }
                else
                {
                    StreamReader sr = new StreamReader(webconfigfile);
                    string content = sr.ReadToEnd();
                    sr.Close();
                    content += " ";
                    StreamWriter sw = new StreamWriter(webconfigfile, false);
                    sw.Write(content);
                    sw.Close();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool SystemRootCheck()
        {
            HttpContext context = HttpContext.Current;

            string physicsPath = null;

            if (context != null)
                physicsPath = context.Server.MapPath("/");
            else
                physicsPath = AppDomain.CurrentDomain.BaseDirectory;

            try
            {
                using (FileStream fs = new FileStream(physicsPath + "\\a.txt", FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    fs.Close();
                }

                System.IO.File.Delete(physicsPath + "\\a.txt");

                return true;
            }
            catch
            {
                return false;
            }

        }


        public static string IISSystemBINCheck(ref bool error)
        {
            string binfolderpath = HttpRuntime.BinDirectory;

            string result = "";
            try
            {
                string[] assemblylist = new string[] { "Discuz.Aggregation.dll", "Discuz.Cache.dll", "Discuz.Common.dll", "Discuz.Config.dll", 
                    "Discuz.Control.dll", "Discuz.Data.dll", "Discuz.Data.SqlServer.dll","Discuz.Entity.dll","Discuz.Event.dll", "Discuz.Forum.dll",
                    "Discuz.Install.dll", "Discuz.Plugin.dll","Discuz.Plugin.Spread.dll", "Discuz.Web.Admin.dll",
                    "Discuz.Web.dll", "Discuz.Web.Services.dll","Interop.SQLDMO.dll","Newtonsoft.Json.dll" };
                bool isAssemblyInexistence = false;
                ArrayList inexistenceAssemblyList = new ArrayList();
                foreach (string assembly in assemblylist)
                {
                    if (!File.Exists(binfolderpath + assembly))
                    {
                        isAssemblyInexistence = true;
                        error = true;
                        inexistenceAssemblyList.Add(assembly);
                    }
                }
                if (isAssemblyInexistence)
                {
                    foreach (string assembly in inexistenceAssemblyList)
                    {
                        result += "<li><cite><img src=\"images/error.gif\" alt=\"ʧ��\"/></cite><a href='#'>" + assembly + " �ļ����ò���ȷ</a><p>�뽫���е�dll�ļ����Ƶ�Ŀ¼ " + binfolderpath + " ��.</p></li>";
                    }
                }
            }
            catch
            {
                result += "<li><cite><img src=\"images/error.gif\" alt=\"ʧ��\"/></cite><a href='#'>�뽫���е�dll�ļ����Ƶ�Ŀ¼ " + binfolderpath + " ��.</a></li>";
                error = true;
            }

            return result;
        }

        public static bool SystemFolderCheck(string foldername)
        {
            string physicsPath = Utils.GetMapPath(@"..\" + foldername);
            try
            {
                using (FileStream fs = new FileStream(physicsPath + "\\a.txt", FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    fs.Close();
                }
                if (File.Exists(physicsPath + "\\a.txt"))
                {
                    System.IO.File.Delete(physicsPath + "\\a.txt");
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool SystemFileCheck(string filename)
        {
            filename = Utils.GetMapPath(@"..\" + filename);
            try
            {
                if (filename.IndexOf("systemfile.aspx") == -1 && !File.Exists(filename))
                    return false;
                if (filename.IndexOf("systemfile.aspx") != -1)  //��ɾ������
                {
                    File.Delete(filename);
                    return true;
                }
                StreamReader sr = new StreamReader(filename);
                string content = sr.ReadToEnd();
                sr.Close();
                content += " ";
                StreamWriter sw = new StreamWriter(filename, false);
                sw.Write(content);
                sw.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static bool SerialiazeTest()
        {
            try
            {
                string configPath = HttpContext.Current.Server.MapPath("../config/general.config");
                GeneralConfigInfo __configinfo = GeneralConfigs.Deserialize(configPath);
                __configinfo.Passwordkey = ForumUtils.CreateAuthStr(10);
                SerializationHelper.Save(__configinfo, configPath);
                return true;
            }
            catch
            {
                return false;
            }
        }


        private static bool TempTest()
        {
            string UserGuid = Guid.NewGuid().ToString();
            string TempPath = Path.GetTempPath();
            string path = TempPath + UserGuid;
            try
            {

                using (StreamWriter sw = new StreamWriter(path))
                {
                    sw.WriteLine(DateTime.Now);
                }

                using (StreamReader sr = new StreamReader(path))
                {
                    sr.ReadLine();
                    return true;
                }


            }
            catch
            {
                return false;

            }

        }
    }
}