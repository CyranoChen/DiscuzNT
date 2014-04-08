using Discuz.Common;
using Discuz.Forum;
 
using Discuz.Space.Utilities;
using Discuz.Space.Provider;
using Discuz.Config;
using Discuz.Entity;
using System.Text;
using Discuz.Data;
using System.Text.RegularExpressions;
using Discuz.Space.Data;

namespace Discuz.Space.Modules
{
	/// <summary>
	/// NewPostModule 的摘要说明。
	/// </summary>
	public class NewPostModule :  BlogModule
	{
		
		private string filename = BaseConfigs.GetForumPath+"/space/Modules/builtin/blog/defaultmodule.config";
        private string templatefile = Utils.GetMapPath(BaseConfigs.GetForumPath + "/space/Modules/builtin/blog/newpostmodule.config");

		public NewPostModule()
		{
            modulename = "最新日志列表";
		}

	
		public override string OnEditBoxLoad(string editbox)
		{
			this.ModulePref.Title = this.modulename;
			return StaticFileProvider.GetContent(Utils.GetMapPath(filename)).Replace("m___MODULE_ID___val", "m___MODULE_ID___val\" value=\""+this.Module.Val+"\""); 
		}

		public override string OnMouduleLoad(string content)
		{
			IsScalableAndEditable();

			this.ModulePref.Title = this.modulename;

            int pagesize = this.Module.Val;
            SpacePostInfo[] spacepostinfos = BlogProvider.GetSpacepostsInfo(DbProvider.GetInstance().SpacePostsList(pagesize, 1, this.SpaceConfig.UserID, 1));

            string template = StaticFileProvider.GetContent(templatefile);
            string separator = "/*Discuz Separator*/" ;
            string[] temp = Utils.SplitString(template, separator);

            if (temp.Length < 2)
                return content;

            string itemtemplate = temp[1].Trim();
            StringBuilder sbitemlist = new StringBuilder();
            if (spacepostinfos != null)
            {
                foreach (SpacePostInfo sp in spacepostinfos)
                { 
                    string tmp = itemtemplate;
                    tmp = tmp.Replace("{postid}", sp.Postid.ToString());
                    tmp = tmp.Replace("{spaceid}", this.SpaceConfig.SpaceID.ToString());
                    tmp = tmp.Replace("{title}", sp.Title);
                    tmp = tmp.Replace("{forumpath}", BaseConfigs.GetForumPath);
                    Regex r = new Regex(@"{title,(\d+)}");

                    foreach (Match m in r.Matches(tmp))
                    {
                        if (m.Success)
                            tmp = tmp.Replace(m.Value, Utils.GetSubString(sp.Title, TypeConverter.StrToInt(m.Groups[1].Value, 20), ".."));                        
                    }
                    sbitemlist.Append(tmp);
                }
            }
            content = temp[0].Replace("{ItemList}", sbitemlist.ToString());

            return base.OnMouduleLoad(content);
            //output.Append("<div id=\"ajaxtopnewpost\">正在加载数据...</div>\r\n");
            //output.Append("<script language=\"javascript1.2\" type=\"text/javascript\">AjaxProxyUrl = new String(\"manage/ajax.aspx\"); \r\n AjaxHelper.Updater('usercontrols/ajaxtopnewpost.ascx','ajaxtopnewpost','load=true&hidetitle=1&spaceid="+this.SpaceConfig.SpaceID+"&postnumber="+this.Module.Val+"');</script>\r\n");
            //return output.ToString();
		}
	}
}
