using System;

using Discuz.Common;
using Discuz.Config;
using Discuz.Forum;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// 单个生成模板页面
    /// </summary>
    public class createtemplate : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (DNTRequest.GetString("type") == "single")
            {
                string filename = DNTRequest.GetString("filename");
                string path = DNTRequest.GetString("path");
                CreateSingleTemplate(filename, path);
            }
            else if (DNTRequest.GetString("type") == "template")
            {
                string templatepath = DNTRequest.GetString("templatepath");
                CreateTemplate(templatepath);
            }
        }

        private void CreateSingleTemplate(string filename,string path)
        {
            int result = -1;
            int templateid = Convert.ToInt32(AdminTemplates.GetAllTemplateList(Utils.GetMapPath(@"..\..\templates\")).Select("directory='" + path + "'")[0]["templateid"].ToString());
            if (filename != "")
            {
                ForumPageTemplate forumpagetemplate = new ForumPageTemplate();
                forumpagetemplate.GetTemplate(BaseConfigs.GetForumPath, path, filename, 1, templateid);
                result = 1;
            }
            Response.Write(result);
            Response.ExpiresAbsolute = DateTime.Now.AddSeconds(-1);
            Response.Expires = -1;
            Response.End();
        }

        private void CreateTemplate(string templatepath)
        {
            Globals.BuildTemplate(templatepath);
        }
    }
}