using System;
using Discuz.Space.Entities;
using Discuz.Entity;
using Discuz.Common;
using Discuz.Config;
using Discuz.Space.Provider;
using Discuz.Space.Utilities;

namespace Discuz.Space.Modules
{
    /// <summary>
    /// 自定义面板
    /// </summary>
    public class CustomizePanel : ModuleBase
    {
        private string filename = Utils.GetMapPath(BaseConfigs.GetForumPath + "/space/Modules/builtin/customizepanel/customizepanel.config");
        
        public CustomizePanel()
        { }

        public override string GetModulePost(System.Web.HttpContext httpContext)
        {
            return base.GetModulePost(httpContext);
        }

        /// <summary>
        /// 自定义编辑框行为
        /// </summary>
        /// <param name="editbox"></param>
        /// <returns></returns>
        public override string OnEditBoxLoad(string editbox)
        {
            this.Editable = true;

            UserPrefsSaved userprefs = new UserPrefsSaved(this.Module.UserPref);
            string value = userprefs.GetValueByName("showborder");
            value = value == string.Empty ? "0" : value;
            string checkvalue = string.Empty;
            if (value == "1")
                checkvalue = "checked";
            editbox = "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"95%\" align=\"center\">";
            editbox += string.Format("<tr><td colspan=\"1\" align=\"right\" width=\"35%\"><nobr>{0}{4}</nobr></td><td colspan=\"2\" align=\"left\" nowrap=\"nowrap\" width=\"65%\">&nbsp;<input id=\"m___MODULE_ID___0\" name=\"m___MODULE_ID___up_{3}\" value=\"{1}\" type=\"hidden\" /><input type=\"checkbox\" {2} onclick=\"_gel('m___MODULE_ID___0').value = this.checked ? '1' : '0';\" /></td></tr>", "显示边框: ", value, checkvalue, "showborder", "");
            value = userprefs.GetValueByName("moduletitle");

            if (value != string.Empty)
            {
                this.ModulePref.Title = value;
            }

            editbox += string.Format("<tr><td colspan=\"1\" align=\"right\" width=\"35%\"><nobr>{0}{3}</nobr></td><td colspan=\"2\" align=\"left\" nowrap=\"nowrap\" width=\"65%\">&nbsp;<input type=\"text\" size=\"20\" maxlen=\"200\" id=\"m___MODULE_ID___1\" name=\"m___MODULE_ID___up_{2}\" value=\"{1}\" /></td></tr>", "设置标题: ", value, "moduletitle", "");
            editbox += string.Format("<tr><td colspan=\"1\" align=\"right\" width=\"35%\">&nbsp;</td><td colspan=\"1\" align=\"right\" width=\"65%\"><nobr><a id=\"DD_tg___cp___MODULE_ID__\" href=\"###\" onclick=\"_DS_popup(event,'__cp___MODULE_ID__');editpanel___MODULE_ID__();_gel('DD___cp___MODULE_ID__').style.zIndex = 2;\">点此编辑面板内容</a></nobr></td></tr>");
            editbox += "</table>";
            return base.OnEditBoxLoad(editbox);
        }

        /// <summary>
        /// 自定义模块加载时的行为
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public override string OnMouduleLoad(string content)
        {
            content = StaticFileProvider.GetContent(filename);

            string showborder = "0";
            if (UserID == this.Module.Uid)
            {
                showborder = "1";
            }
            else
            {
                UserPrefsSaved ups = new UserPrefsSaved(this.Module.UserPref);
                showborder = ups.GetValueByName("showborder");
            }
            content = content.Replace("{showborder}", showborder == string.Empty ? "1" : showborder);
            
            content = content.Replace("{customizepanelcontent}", Spaces.GetCustomizePanelContent(this.ModuleID, this.Module.Uid));
            content = content.Replace("{themepath}", this.SpaceConfig.ThemePath);
            content = content.Replace("{forumpath}", BaseConfigs.GetForumPath);
            return base.OnMouduleLoad(content);
        }

        /// <summary>
        /// 自定义当模块移除时的行为
        /// </summary>
        protected override void OnRemove()
        {
            Spaces.DeleteCustomizePanelContent(this.ModuleID, this.Module.Uid);

            base.OnRemove();
        }
    }
}
