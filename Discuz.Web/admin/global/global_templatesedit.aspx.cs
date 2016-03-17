using System;
using System.IO;
using System.Text;
using System.Web.UI;

using Discuz.Control;
using Discuz.Common;
using Discuz.Forum;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// 模板文件编辑
    /// </summary>
    public partial class templatesedit : AdminPage
    {
        public string filenamefullpath;
        public string path;
        public string filename;

        protected void Page_Load(object sender, EventArgs e)
        {
            path = DNTRequest.GetString("path");
            if (path == "")
            {
                Response.Redirect("global_templatetree.aspx");
                return;
            }

            filename = DNTRequest.GetString("filename");
            //仅允许修改静态文件
            if (filename.EndsWith(".htm") || filename.EndsWith(".html") || filename.EndsWith(".js") || filename.EndsWith(".css") || filename.EndsWith(".xml"))
            {
                filenamefullpath = "../../templates/" + path + "/" + filename;
                ViewState["path"] = path;
                ViewState["filename"] = filename;
                ViewState["templateid"] = DNTRequest.GetString("templateid");
                ViewState["templatename"] = DNTRequest.GetString("templatename");

                if (!Page.IsPostBack)
                {
                    using (StreamReader objReader = new StreamReader(Server.MapPath(filenamefullpath), Encoding.UTF8))
                    {
                        templatenew.Text = objReader.ReadToEnd();
                        objReader.Close();
                    }
                }
            }
            else
            {
                Response.Redirect("global_templatetree.aspx");
                return;
            }
        }

        private void SavaTemplateInfo_Click(object sender, EventArgs e)
        {
            #region 保存相关模板信息

            if (this.CheckCookie())
            {
                string path = ViewState["path"].ToString();
                string filename = ViewState["filename"].ToString();
                filenamefullpath = Server.MapPath("../../templates/" + path + "/" + filename);
                //仅允许修改静态文件
                if (filename.EndsWith(".htm") || filename.EndsWith(".html") || filename.EndsWith(".js") || filename.EndsWith(".css") || filename.EndsWith(".xml"))
                {
                    //只修改已存在的文件，防止创建木马文件
                    if (Utils.FileExists(filenamefullpath))
                    {
                        using (FileStream fs = new FileStream(filenamefullpath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                        {
                            Byte[] info = Encoding.UTF8.GetBytes(templatenew.Text);
                            fs.Write(info, 0, info.Length);
                            fs.Close();
                        }
                    }
                }
                base.RegisterStartupScript( "PAGE",  "window.location.href='global_templatetree.aspx?path=" + ViewState["path"].ToString().Split('\\')[0] + "&templateid=" + ViewState["templateid"].ToString() + "&templatename=" + ViewState["templatename"].ToString() + "';");
            }

            #endregion
        }

        #region Web 窗体设计器生成的代码

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.SavaTemplateInfo.Click += new EventHandler(this.SavaTemplateInfo_Click);
        }

        #endregion

    }
}