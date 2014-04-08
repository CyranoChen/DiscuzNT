using System;
using System.Collections;
using System.Text;

namespace Discuz.Space.Entities
{
    public class RssTemplate : ISpaceTemplate
    {
        public static readonly ISpaceTemplate Instance = new RssTemplate();
        private RssTemplate()
        {}

        /// <summary>
        /// ���RSSģ�������
        /// </summary>
        /// <param name="ht">ģ�����������</param>
        /// <returns>����ģ�����ݵ�html</returns>
        public string GetHtml(Hashtable ht)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(@"<div id=""FEED__MODULE_ID__msg"" class=""statusmsg"" style=""display:block;""><span id=""FEED__MODULE_ID__statusmsg"">������...</span></div><div id=""FEED__MODULE_ID__display"" style=""display:none;""><div id=""FEED__MODULE_ID__entries""></div><div class=""fpager"" style=""margin-top: 5px;"" id=""FEED___MODULE_ID__pages""></div></div>");
            builder.AppendFormat(
                @"<script type=""text/javascript"">var FEED__MODULE_ID__ = {{url : ""__MODULE_URL__"",page_count : 1,current_page : 1,items : {{}},num_items : __MODULE_VAL__,has_entries : false,is_fetching : false,TPL_entry :'<div' +' id=""ftl___MODULE_ID___%ENTRY_INDEX%"" class=""uftl""' +' style=""display: block;"">' +'<a id=""ft___MODULE_ID___%ENTRY_INDEX%"" class=""fmaxbox"" onclick=""_DS_FR_toggle(__MODULE_ID__,%ENTRY_INDEX%)""></a>' +'<a href=""%URL%"" target=""_blank"">%TITLE%</a>' +'<br />' +'<div class=""fpad"" id=""fb___MODULE_ID___%ENTRY_INDEX%"" style=""display:none;"">' +'<div id=""fb___MODULE_ID___%ENTRY_ID%"" style="""">��Rss��ʱ�޷�����.</div>' +'<div>' +'</div>',msg : function(txt) {{_gel(""FEED__MODULE_ID__statusmsg"").innerHTML = txt;_gel(""FEED__MODULE_ID__msg"").style.display = ""block"";_gel(""FEED__MODULE_ID__display"").style.display = ""none"";}},
init : function() {{_DS_FetchFeedAsJSON(""__MODULE_URL__"", FEED__MODULE_ID__.render, __MODULE_VAL__, false);}},
addEntry : function(title, url, id, index, has_body) {{var tpl = FEED__MODULE_ID__.TPL_entry;tpl = tpl.replace(/\%URL\%/g, _hesc(url));tpl = tpl.replace(/\%TITLE\%/g, _hesc(title));tpl = tpl.replace(/\%ENTRY_ID\%/g, id);tpl = tpl.replace(/\%ENTRY_INDEX\%/g, index);tpl = tpl.replace(/\%VISIBILITY\%/g, has_body ? """" : ""none"");_gel(""FEED__MODULE_ID__entries"").innerHTML += tpl;}},
render : function(obj) {{if (!obj) {{FEED__MODULE_ID__.msg(""��Rss��ʱ�޷�����."");return;}}if (obj.ErrorMsg && obj.ErrorMsg != """") {{FEED__MODULE_ID__.msg(obj.ErrorMsg);return;}}var title_span = _gel(""m___MODULE_ID___title"");if (title_span) {{if (title_span.innerHTML == """" && obj.title && obj.title != """") {{title_span.innerHTML = _hesc(obj.title);}}if (obj.link && obj.link != """") {{title_span.innerHTML = ""<a class=\""mtlink\"" href=\"""" +_hesc(obj.link) + ""\"">"" +title_span.innerHTML + ""</a>"";}}}}if (!obj.items || obj.items.length == 0) {{FEED__MODULE_ID__.msg(""��Rss��ʱ�޷�����."");return;}}_gel(""FEED__MODULE_ID__msg"").style.display = ""none"";_gel(""FEED__MODULE_ID__display"").style.display = ""block"";if (obj.items) {{FEED__MODULE_ID__.items=obj.items;_gel(""FEED__MODULE_ID__entries"").innerHTML = """";for (var i = 0; i < obj.items.length; i++) {{FEED__MODULE_ID__.addEntry(obj.items[i].title,obj.items[i].link,i,i,obj.items[i].bodyavailable);}}FEED__MODULE_ID__.page_count=Math.ceil(obj.items.length/FEED__MODULE_ID__.num_items); if (obj.items.length > FEED__MODULE_ID__.num_items){{ds_fgtp(__MODULE_ID__, 1);
}}}}}}
}};FEED__MODULE_ID__.init();setInterval(""FEED__MODULE_ID__.init();ds_fgtp(__MODULE_ID__, FEED__MODULE_ID__.current_page)"", (60 * 15 * 1000));</script>");
            //builder.AppendFormat("<script type=\"text/javascript\" src=\"{0}space/javascript/rss.js\"></script>", ht["forumpath"]);
            return builder.ToString();
        }
    }
}
