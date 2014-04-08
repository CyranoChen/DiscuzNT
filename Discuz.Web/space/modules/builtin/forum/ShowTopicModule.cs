using System;
using System.Data;
using System.Text;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Space.Entities;

using Discuz.Space.Utilities;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Space.Provider;

namespace Discuz.Space.Modules.Forum
{
    /// <summary>
    /// ShowTopicModule 的摘要说明。
    /// </summary>
    public class ShowTopicModule : ModuleBase
    {
        private string jsFile = Utils.GetMapPath(BaseConfigs.GetForumPath + "/space/Modules/builtin/forum/showtopicmodule.config");
        public ShowTopicModule()
        {
        }

        public override string GetModulePost(System.Web.HttpContext httpContext)
        {
            return base.GetModulePost(httpContext);
        }

        public override string OnEditBoxLoad(string editbox)
        {
            this.Editable = true;
            UserPrefsSaved userprefs = new UserPrefsSaved(this.Module.UserPref);
            editbox = "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"95%\" align=\"center\">";

            //显示主题条数
            string value = userprefs.GetValueByName("topiccount");
            value = value == "" ? "10" : value;
            string options = string.Empty;
            for (int i = 10; i <= 50; i += 10)
            {
                if (value == i.ToString())
                    options += string.Format("<option value=\"{0}\" selected>{0} 条 </option>", i);
                else
                    options += string.Format("<option value=\"{0}\">{0} 条 </option>", i);
            }
            editbox += string.Format("<tr><td colspan=\"1\" align=\"right\" width=\"35%\"><nobr>{0}{3}</nobr></td><td colspan=\"2\" align=\"left\" nowrap=\"nowrap\" width=\"65%\">&nbsp;<select id=\"m___MODULE_ID___0\" name=\"m___MODULE_ID___up_{1}\">{2}</select></td></tr>", "显示主题数: ", "topiccount", options, "");

            //是否是精华
            value = userprefs.GetValueByName("iselite");
            value = value == "" ? "0" : value;
            string checkedvalue = "";
            if (value == "1")
                checkedvalue = "checked";
            editbox += string.Format("<tr><td colspan=\"1\" align=\"right\" width=\"35%\"><nobr>{0}{4}</nobr></td><td colspan=\"2\" align=\"left\" nowrap=\"nowrap\" width=\"65%\">&nbsp;<input id=\"m___MODULE_ID___1\" name=\"m___MODULE_ID___up_{3}\" value=\"{1}\" type=\"hidden\" /><input type=\"checkbox\" {2} onclick=\"_gel('m___MODULE_ID___1').value = this.checked ? '1' : '0';\" /></td></tr>", "仅显示精华帖: ", value, checkedvalue, "iselite", "");

            //			//是否仅显示自己的
            //			value = userprefs.GetValueByName("ismine");
            //			value = value == "" ? "0" : value;
            //			checkedvalue = "";
            //			if (value == "1")
            //				checkedvalue = "checked";
            //			editbox += string.Format("<tr><td colspan=\"1\" align=\"right\" width=\"35%\"><nobr>{0}{4}</nobr></td><td colspan=\"2\" align=\"left\" nowrap=\"nowrap\" width=\"65%\">&nbsp;<input id=\"m___MODULE_ID___2\" name=\"m___MODULE_ID___up_{3}\" value=\"{1}\" type=\"hidden\" /><input type=\"checkbox\" {2} onclick=\"_gel('m___MODULE_ID___2').value = this.checked ? '1' : '0';\" /></td></tr>", "仅显示我的主题: ", value, checkedvalue, "ismine", "");

            editbox += "</table>";
            return base.OnEditBoxLoad(editbox);
        }

        public override string OnMouduleLoad(string content)
        {
            UserPrefsSaved userprefs = new UserPrefsSaved(this.Module.UserPref);
            int topiccount = Utils.StrToInt(userprefs.GetValueByName("topiccount"), 10);
            bool iselite = Utils.StrToInt(userprefs.GetValueByName("iselite"), 0) > 0;

            DataTable topics = Focuses.GetTopicList(topiccount, 0, 0, "", TopicTimeType.All, TopicOrderType.ID, iselite, 30, false);
            StringBuilder sb = new StringBuilder(StaticFileProvider.GetContent(jsFile).Replace("${forumpath}", BaseConfigs.GetForumPath).Replace("${themepath}", this.SpaceConfig.ThemePath));
            GeneralConfigInfo config = GeneralConfigs.GetConfig();
            sb.Append("\r\n<div class='dnt-theme'><ul>\r\n");
            foreach (DataRow r in topics.Rows)
            {
                string img = string.Format("<img onerror='this.src=\"{0}space/modules/builtin/forum/images/item_extend.gif\";' src='{0}space/skins/themes/{1}/images/item_extend.gif' id='imgButton_{2}' onclick='showtree({2},10);' title='展开' alt='展开' style='cursor:pointer;'/>", BaseConfigs.GetForumPath, this.SpaceConfig.ThemePath, r["tid"]);
                if (Utils.StrToInt(r["replies"], 0) < 1)
                    img = string.Format("<img onerror='this.src=\"{0}space/modules/builtin/forum/images/item_collapsed.gif\";' src='{0}space/skins/themes/{1}/images/item_collapsed.gif' />", BaseConfigs.GetForumPath, this.SpaceConfig.ThemePath);

                if (config.Aspxrewrite == 1)
                    sb.AppendFormat("<li>{0}<a href='" + BaseConfigs.GetForumPath + "showtopic-{1}.aspx' title='{2}' target='_blank'>{2}</a></li>", img, r["tid"], r["title"]);
                else
                    sb.AppendFormat("<li>{0}<a href='" + BaseConfigs.GetForumPath + "showtopic.aspx?topicid={1}' title='{2}' target='_blank'>{2}</a></li>", img, r["tid"], r["title"]);

                sb.AppendFormat("<div id='divTopic{0}'></div>", r["tid"]);
            }
            sb.Append("\r\n</ul></div>\r\n");
            content = sb.ToString();
            return base.OnMouduleLoad(content);
        }
    }
}
