using System;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Entity;
using Discuz.Config;
using Discuz.Space.Entities;
using Discuz.Space.Provider;

namespace Discuz.Space.Modules
{
	/// <summary>
	/// notepad 的摘要说明。
	/// </summary>
	public class Notepad : ModuleBase
	{
		private string filename = Utils.GetMapPath(BaseConfigs.GetForumPath+"/space/Modules/builtin/notepad/notepad.config");
		private GeneralConfigInfo config = GeneralConfigs.GetConfig();
		public Notepad()
		{			
		}

		public override string GetModulePost(System.Web.HttpContext httpContext)
		{
			return base.GetModulePost (httpContext);
		}

		public override string OnEditBoxLoad(string editbox)
		{
			this.Editable = true;

            UserPrefsSaved userprefs = new UserPrefsSaved(this.Module.UserPref);
            string value = userprefs.GetValueByName("moduletitle");
            //标题
            if (value != string.Empty)
                this.ModulePref.Title = value;

            editbox = "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"95%\" align=\"center\">";
            editbox += string.Format("<tr><td colspan=\"1\" align=\"right\" width=\"35%\"><nobr>{0}{3}</nobr></td><td colspan=\"2\" align=\"left\" nowrap=\"nowrap\" width=\"65%\">&nbsp;<input type=\"text\" size=\"20\" maxlen=\"200\" id=\"m___MODULE_ID___0\" name=\"m___MODULE_ID___up_{2}\" value=\"{1}\" /></td></tr>", "设置标题: ", value, "moduletitle", "");
            
            //底色
            value = userprefs.GetValueByName("bgcolor");
            value = value == string.Empty ? "#ffffcc" : value;

            string options = string.Empty;
            #region 颜色选项
            if (value == "#ffffcc")
                options += "<option value='#ffffcc' selected>Yellow</option>";
            else
                options += "<option value='#ffffcc'>Yellow</option>";

            if (value == "#e5ecf9")
                options += "<option value='#e5ecf9' selected>Blue</option>";
            else
                options += "<option value='#e5ecf9'>Blue</option>";

            if (value == "white")
                options += "<option value='white' selected>White</option>";
            else
                options += "<option value='white'>White</option>";

            if (value == "#e0eee0")
                options += "<option value='#e0eee0' selected>Green</option>";
            else
                options += "<option value='#e0eee0'>Green</option>";

            if (value == "#fff0f5")
                options += "<option value='#fff0f5' selected>Pink</option>";
            else
                options += "<option value='#fff0f5'>Pink</option>";

            if (value == "#fff5ee")
                options += "<option value='#fff5ee' selected>Orange</option>";
            else
                options += "<option value='#fff5ee'>Orange</option>";

            #endregion


            editbox += string.Format("<tr><td colspan=\"1\" align=\"right\" width=\"35%\"><nobr>{0}{3}</nobr></td><td colspan=\"2\" align=\"left\" nowrap=\"nowrap\" width=\"65%\">&nbsp;<select id=\"m___MODULE_ID___1\" name=\"m___MODULE_ID___up_{1}\">{2}</select></td></tr>", "背景颜色:  ", "bgcolor", options, "");
             
            //文字颜色
            value = userprefs.GetValueByName("txtcolor");
            value = value == string.Empty ? "Black" : value;
            #region 颜色选项
            if (value == "Black")
                options = "<option value='Black' selected>Black</option>";
            else
                options = "<option value='Black'>Black</option>";

            if (value == "blue")
                options += "<option value='blue' selected>Blue</option>";
            else
                options += "<option value='blue'>Blue</option>";

            if (value == "green")
                options += "<option value='green' selected>Green</option>";
            else
                options += "<option value='green'>Green</option>";

            if (value == "red")
                options += "<option value='red' selected>Red</option>";
            else
                options += "<option value='red'>Red</option>";

            if (value == "#ff007f")
                options += "<option value='#ff007f' selected>Pink</option>";
            else
                options += "<option value='#ff007f'>Pink</option>";

            if (value == "#ff3300")
                options += "<option value='#ff3300' selected>Orange</option>";
            else
                options += "<option value='#ff3300'>Orange</option>";
            #endregion
            editbox += string.Format("<tr><td colspan=\"1\" align=\"right\" width=\"35%\"><nobr>{0}{3}</nobr></td><td colspan=\"2\" align=\"left\" nowrap=\"nowrap\" width=\"65%\">&nbsp;<select id=\"m___MODULE_ID___2\" name=\"m___MODULE_ID___up_{1}\">{2}</select></td></tr>", "文字颜色:  ", "txtcolor", options, "");
            editbox += "</table>";

			return base.OnEditBoxLoad(editbox) ;
		}

		public override string OnMouduleLoad(string content)
		{
			UserPrefsSaved ups = new UserPrefsSaved(this.Module.UserPref);
			string savedContent = ups.GetValueByName("content");
			savedContent = Utils.HtmlEncode(ForumUtils.BanWordFilter(savedContent));
			content = StaticFileProvider.GetContent(filename);
			if (this.UserID == this.Module.Uid)
			{
                content = content.Replace("${ContentArea}", string.Format("<textarea class=\"notepadcontent\" style=\"background-color: {1}; color: {2};\" id=\"content__MODULE_ID__\" onkeyup=\"setRows__MODULE_ID__();\" onblur=\"saveResult__MODULE_ID__();\">{0}</textarea>", savedContent, ups.GetValueByName("bgcolor") == string.Empty ? "#ffffcc" : ups.GetValueByName("bgcolor"), ups.GetValueByName("txtcolor")));
			}
			else
			{
				savedContent = savedContent.Replace("\n","<br />");
                content = content.Replace("${ContentArea}", string.Format("<div class=\"notepadcontent\" style=\"background-color: {1}; color: {2};padding: 8px;\">{0}</div>", savedContent, ups.GetValueByName("bgcolor") == string.Empty ? "#ffffcc" : ups.GetValueByName("bgcolor"), ups.GetValueByName("txtcolor")));
			}
			return base.OnMouduleLoad (content);
		}
	}
}
