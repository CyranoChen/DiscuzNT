using System;
using Discuz.Space.Entities;
using Discuz.Common;
using Discuz.Space.Provider;
using Discuz.Config;
using System.Text;
using Discuz.Entity;
using Discuz.Data;
using Newtonsoft.Json;
using System.IO;
using Discuz.Common.Generic;
using Discuz.Space.Data;
namespace Discuz.Space.Modules
{
    /// <summary>
    /// 我的日志(内置模块)
    /// </summary>
    public class PostListModule : ModuleBase
    {
        private string templateFile = Utils.GetMapPath(BaseConfigs.GetForumPath + "/space/modules/builtin/blog/postlistmodule.htm");
        /// <summary>
        /// 不带文件名的forumurl地址
        /// </summary>
        protected string forumurlnopage = "../";

        protected string forumurl = GeneralConfigs.GetConfig().Forumurl;

        protected string configspaceurlnopage = GeneralConfigs.GetConfig().Spaceurl;

        /// <summary>
        /// 自定义模块加载时的行为
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public override string OnMouduleLoad(string content)
        {
            UserPrefsSaved ups = new UserPrefsSaved(this.Module.UserPref);
            int charcount = Utils.StrToInt(ups.GetValueByName("charcount"), 500);
            //去掉http地址中的文件名称
            if (forumurl.ToLower().IndexOf("http://") == 0)
                forumurlnopage = forumurl.Substring(0, forumurl.LastIndexOf('/')) + "/";
            else
                forumurl = "../" + forumurl;

            if (configspaceurlnopage.ToLower().IndexOf("http://") < 0)
                configspaceurlnopage = forumurlnopage;
            else
                configspaceurlnopage = configspaceurlnopage.ToLower().Substring(0, configspaceurlnopage.LastIndexOf('/')) + "/";

            string templateContent = Utilities.Globals.GetFileContent(templateFile);//StaticFileProvider.GetContent(templateFile);
            StringBuilder sbMainTemplate = new StringBuilder();
            StringBuilder sbItemTemplate = new StringBuilder();
            StringBuilder sbTimeTemplate = new StringBuilder();
            StringBuilder sbLinkTemplate = new StringBuilder();
            StringBuilder sbTagTemplate = new StringBuilder();
            StringBuilder sbTagItem = new StringBuilder();
            StringBuilder sbContentTemplate = new StringBuilder();
            StringBuilder sbMoreContentTemplate = new StringBuilder();
            string[] templates = Utils.SplitString(templateContent, "/*Discuz Separator*/");

            if (templates.Length < 8)
                content = "模板文件加载出错, 请检查";
            else
            {
                sbMainTemplate.Append(templates[0]);
                sbItemTemplate.Append(templates[1]);
                sbTimeTemplate.Append(templates[2]);
                sbLinkTemplate.Append(templates[3]);
                sbTagTemplate.Append(templates[4]);
                sbTagItem.Append(templates[5]);
                sbContentTemplate.Append(templates[6]);
                sbMoreContentTemplate.Append(templates[7]);

                SpacePostInfo[] spaceposts = GetSpacePosts();
                string[] postids = new string[spaceposts.Length];
                for (int i = 0; i < spaceposts.Length; i++)
                {
                    postids[i] = spaceposts[i].Postid.ToString();
                }
                Dictionary<string, Dictionary<string, string>> categorys = null;
                if (postids.Length > 0)
                {
                    categorys = Spaces.GetSpacePostCategorys(string.Join(",", postids));
                    //获取分类
                }

                StringBuilder sbItemList = new StringBuilder();
                #region 循环遍历
                foreach (SpacePostInfo post in spaceposts)
                {
                    StringBuilder sbTemp = new StringBuilder(sbItemTemplate.ToString());
                    sbTemp.Replace("{$TimeTemplate}", ParseTimeTemplate(sbTimeTemplate.ToString(), post));
                    sbTemp.Replace("{$LinkTemplate}", ParseLinkTemplate(sbLinkTemplate, post, categorys));
                    StringBuilder sbTempTag = new StringBuilder(sbTagTemplate.ToString());
                    string tagItem = ParseTagItem(sbTagItem, post);
                    if (tagItem != string.Empty)
                        sbTemp.Replace("{$TagTemplate}", sbTempTag.Replace("{$TagItem}", tagItem).ToString());
                    else
                        sbTemp.Replace("{$TagTemplate}", string.Empty);

                    string postcontent = post.Content;

                    if (charcount == 0)//不显示正文
                        postcontent = string.Empty;
                    else if (charcount > 0)//截断
                    {
                        postcontent = Utils.GetTextFromHTML(Utils.HtmlDecode(postcontent));
                        if (postcontent.Length > charcount)
                        {
                            StringBuilder sbTempMoreContentTemplate = new StringBuilder(sbMoreContentTemplate.ToString());
                            sbTempMoreContentTemplate.Replace("{$Root}", configspaceurlnopage);
                            sbTempMoreContentTemplate.Replace("{$PostId}", post.Postid.ToString());
                            sbTempMoreContentTemplate.Replace("{$SpaceId}", SpaceConfig.SpaceID.ToString());
                            postcontent = postcontent.Substring(0, charcount) + "..." + sbTempMoreContentTemplate.ToString();
                        }
                    }

                    StringBuilder sbTempContent = new StringBuilder(sbContentTemplate.ToString());

                    sbTemp.Replace("{$ContentTemplate}", sbTempContent.Replace("{$Content}", postcontent).ToString());
                    sbItemList.Append(sbTemp);
                }
                #endregion

                sbMainTemplate.Replace("{$ItemTemplate}", sbItemList.ToString());
                sbMainTemplate.Replace("{$ForumPath}", forumurlnopage);
                sbMainTemplate.Replace("{$Skin}", base.SpaceConfig.ThemePath);
                content = sbMainTemplate.ToString() + "<div style='border-top:1px dashed #ccc; padding:10px 0; padding-left:10px;font-weight:bold;'><a href=\"viewspacepostlist.aspx?spaceid=" + base.SpaceConfig.SpaceID + "\">查看更多</a></div>";
            }
            return base.OnMouduleLoad(content);
        }

        public override string OnEditBoxLoad(string editbox)
        {
            this.Editable = true;
            UserPrefsSaved userprefs = new UserPrefsSaved(this.Module.UserPref);
            editbox = "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"95%\" align=\"center\">";
            string value = userprefs.GetValueByName("charcount");

            int charcount = Utils.StrToInt(value, 500);

            editbox += string.Format("<tr><td colspan=\"1\" align=\"right\" width=\"35%\"><nobr>{0}{3}</nobr></td><td colspan=\"2\" align=\"left\" nowrap=\"nowrap\" width=\"65%\">&nbsp;<input type=\"text\" size=\"20\" maxlen=\"200\" id=\"m___MODULE_ID___1\" name=\"m___MODULE_ID___up_{2}\" value=\"{1}\" /></td></tr>", "显示正文字数: ", charcount, "charcount", "");
            editbox += "</table>";

            return base.OnEditBoxLoad(editbox);
        }

        private string GetTagURL(int tagid)
        {
            if (GeneralConfigs.GetConfig().Aspxrewrite == 1)
                return string.Format("{0}spacetag-{1}{2}", BaseConfigs.GetForumPath, tagid, GeneralConfigs.GetConfig().Extname);
            else
                return string.Format("{0}tags.aspx?t=spacepost&tagid={1}", BaseConfigs.GetForumPath, tagid);
        }

        private string ParseTagItem(StringBuilder sbTagItem, SpacePostInfo post)
        {
            string xx = SpaceTags.GetSpacePostTagsCacheFile(post.Postid);
            object o = JavaScriptConvert.DeserializeObject(xx, typeof(TagInfo[]));

            TagInfo[] tags = o as TagInfo[];
            StringBuilder sbResult = new StringBuilder();
            if (tags != null)
            {
                foreach (TagInfo tag in tags)
                {
                    StringBuilder sbTemp = new StringBuilder(sbTagItem.ToString());
                    sbTemp.Replace("{$TagUrl}", GetTagURL(tag.Tagid));
                    sbTemp.Replace("{$TagName}", tag.Tagname);
                    sbResult.Append(sbTemp);
                }
            }
            return sbResult.ToString();
        }

        private string ParseLinkTemplate(StringBuilder sbLinkTemplate, SpacePostInfo post, Dictionary<string, Dictionary<string, string>> categorys)
        {
            StringBuilder sbTemp = new StringBuilder(sbLinkTemplate.ToString());
            sbTemp.Replace("{$Root}", configspaceurlnopage);
            sbTemp.Replace("{$PostId}", post.Postid.ToString());
            sbTemp.Replace("{$SpaceId}", SpaceConfig.SpaceID.ToString());
            sbTemp.Replace("{$Title}", post.Title);
            sbTemp.Replace("{$Views}", post.Views.ToString());
            sbTemp.Replace("{$ComentCount}", post.Commentcount.ToString());
            if (categorys != null && categorys.ContainsKey(post.Postid.ToString()))
            {

                StringBuilder sbCategory = new StringBuilder();
                foreach (System.Collections.Generic.KeyValuePair<string, string> category in categorys[post.Postid.ToString()])
                {
                    sbCategory.Append(category.Value);
                    sbCategory.Append(",");
                }

                if (sbCategory.Length > 0)
                    sbCategory.Length = sbCategory.Length - 1;

                sbTemp.Replace("{$Category}", sbCategory.ToString());
            }
            else
                sbTemp.Replace("{$Category}", string.Empty);

            return sbTemp.ToString();
        }

        private string ParseTimeTemplate(string sbTimeTemplate, SpacePostInfo post)
        {
            sbTimeTemplate=sbTimeTemplate.Replace("{$Year}", post.Postdatetime.Year.ToString());
            sbTimeTemplate=sbTimeTemplate.Replace("{$Month}", post.Postdatetime.Month.ToString("00"));
            sbTimeTemplate=sbTimeTemplate.Replace("{$Date}", post.Postdatetime.Day.ToString("00"));
            return sbTimeTemplate.ToString();
        }

        private SpacePostInfo[] GetSpacePosts()
        {
            int currentpage = DNTRequest.GetInt("currentpage", 1);
            if (currentpage < 1)
                currentpage = 1;

            SpacePostInfo[] postarray = BlogProvider.GetSpacepostsInfo(DbProvider.GetInstance().SpacePostsList(base.SpaceConfig.Bpp, currentpage, base.SpaceConfig.UserID, 1));
            if (postarray == null)
                return new SpacePostInfo[0];

            return postarray;
        }
    }
}
