using System;
using System.Text;
using Discuz.Entity;

namespace Discuz.Space.Entities
{
    public class MainTemplate : ISpaceTemplate
    {
        public static readonly ISpaceTemplate Instance = new MainTemplate();
        private MainTemplate()
        {
        }
        #region ISpaceTemplate Members

        /// <summary>
        /// 获得主模板的内容
        /// </summary>
        /// <param name="ht">模板变量的数组</param>
        /// <returns>返回模板内容的html</returns>
        public string GetHtml(System.Collections.Hashtable ht)
        {
            string spacerssurl = "";
            if(Discuz.Config.GeneralConfigs.GetConfig().Aspxrewrite == 1)
                spacerssurl = string.Format("spacerss-{0}{1}", ((SpaceConfigInfo) ht["config"]).UserID.ToString(),Discuz.Config.GeneralConfigs.GetConfig().Extname);
            else
                spacerssurl = string.Format("rss.aspx?uid={0}&type=space", ((SpaceConfigInfo) ht["config"]).UserID.ToString());

            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(@"<html>
<head>
	<meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"" />
	<meta name=""keywords"" content=""ASP.net,论坛,space,blog,{0}"" />
	<meta name=""description"" content=""{0} {1}"" />
    <meta name=""generator"" content=""Discuz!NT"" />
    <meta name=""copyright"" content=""Comsenz Inc."" />
    <meta http-equiv=""x-ua-compatible"" content=""ie=7"" />
    <meta name=""MSSmartTagsPreventParsing"" content=""True"" />
    <meta http-equiv=""MSThemeCompatible"" content=""Yes"" />",
            (ht["config"] as SpaceConfigInfo).Spacetitle,
            ht["spacename"].ToString());
            builder.AppendFormat("<title>{0} - {1} - Powered by Discuz!NT</title>", ((TabInfo)ht["currenttab"]).TabName, ((SpaceConfigInfo)ht["config"]).Spacetitle);
            builder.AppendFormat("<link href=\"{0}space/skins/themes/space.css\" rel=\"stylesheet\" type=\"text/css\" id=\"css\" />", ht["forumpath"].ToString());
            builder.AppendFormat("<link rel=\"stylesheet\" type=\"text/css\" href=\"{0}space/skins/themes/{1}/style.css\" />", ht["forumpath"].ToString(), ((SpaceConfigInfo)ht["config"]).ThemePath);
            builder.AppendFormat("<link rel=\"alternate\" type=\"application/rss+xml\" title=\"{0} 最新日志\" href=\"{1}tools/{2}\" />", ((SpaceConfigInfo)ht["config"]).Spacetitle, ht["forumpath"].ToString(), spacerssurl);
            builder.AppendFormat("<script type=\"text/javascript\">var forumpath=\"{0}space/\";var aspxrewrite = \"{1}\";</script><script type=\"text/javascript\" src=\"{0}space/javascript/space.js\"></script>", ht["forumpath"].ToString(), Discuz.Config.GeneralConfigs.GetConfig().Aspxrewrite);
            builder.AppendFormat("\r\n<script>var domain = document.location.hostname;_et=\"{0}\";_source=\"\";_uli=true;_pnlo=false;_mpnlo=false;_pl=false;_mod=true;_pid=\"\";_old_html = false;_cbp=true;_is_dasher=false;var _pl_data = {1};</script><script><!--\r\n", ht["userkey"].ToString(), "{}");
            builder.Append("function save_chkbox_value(hidden_elem_name, checked) {var hidden_elem = document.getElementById(hidden_elem_name);hidden_elem.value = checked ? \"1\" : \"0\";}function RemoteModule(spec_url, id, render_inline, base_iframe_url,caching_disabled) {this.spec_url = spec_url;this.id = id;this.render_inline = render_inline;this.base_iframe_url = base_iframe_url;this.caching_disabled = caching_disabled;this.old_width = 0;this.wants_scaling = false;      this.is_inlined = function() { return this.base_iframe_url == \"\"; };};var remote_modules = [];_DS_RegisterOnloadHandler(function() {for (var i=0;i<remote_modules.length;i++){var rm=remote_modules[i];var el=_gel(\"remote_iframe_\"+rm.id);if(el){el.src=rm.base_iframe_url;}}});function ifpc_resizeIframe(iframe_id, height) {var el = document.getElementById(iframe_id);if (el) {el.style.height = height + \"px\";}}_IFPC.registerService('resize_iframe', ifpc_resizeIframe);function _DS_gmid_(iframe_id) {return(iframe_id.split(\"_\")[2]);}function _setModTitle(title, module_id) {var title_element = _gel(\"m_\" + module_id + \"_title\");if (title_element) {title_element.innerHTML = _hesc(title);}}function ifpc_setTitle(iframe_id, title) {if (typeof(iframe_id) == undefined|| !iframe_id || iframe_id == \"undefined\") {return;}_setModTitle(title, _DS_gmid_(iframe_id));}_IFPC.registerService('set_title', ifpc_setTitle);function _DS_SetTitle(title, specified_module_id) {if (typeof(specified_module_id) == \"undefined\"|| !specified_module_id || specified_module_id == \"undefined\") {throw new Error(\"模块使用_DS_SetTitle时必须指定他们的id\"+ \"__MODULE_ID__\");} else {_setModTitle(title, specified_module_id);}}function ifpc_setPref(iframe_id, var_args) {var module_id = _DS_gmid_(iframe_id);var prefs = new _DS_Prefs(module_id);var keyValues = new Array();for (var n = 1; n < arguments.length; n += 2) {keyValues.push(arguments[n], arguments[n + 1]);}prefs.set.apply(prefs, keyValues);}_IFPC.registerService('set_pref', ifpc_setPref);// -->");
            
            builder.Append(@"</script>
		<style>
		.modbox {margin:5px 5px;}
		.col {vertical-align:top;height:100px;}
		</style>
	</head>
	<body>
<div id=""topbar"" style=""left: 0px; top: 0px; position: absolute""></div>
<div align=""right"" class=""menu""><div id=""duser"" width=100%><nobr>
");

            if ((bool)ht["islogged"])
                builder.AppendFormat("&nbsp;<a href=\"{0}usercp.aspx\"><b>{1}</b></a>(<a href=\"{0}logout.aspx?userkey={2}\">退出</a>)&nbsp;|&nbsp;<a href=\"{3}space/?uid={4}\">我的{5}</a>(<a href=\"{0}usercpspacemanageblog.aspx\">设置</a>)&nbsp;|&nbsp;<a href=\"{0}forumindex.aspx\">论坛</a>&nbsp;|&nbsp;<a href=\"{6}\">{7}</a>&nbsp;|&nbsp;<a href=\"{8}\">{5}首页</a>&nbsp;", ht["forumurlnopage"].ToString(), ht["username"].ToString(), ht["userkey"].ToString(), ht["configspaceurlnopage"].ToString(), ht["userid"].ToString(), ht["spacename"].ToString(), ht["configalbumurl"].ToString(), ht["albumname"].ToString(), ht["configspaceurl"].ToString());
            else
                builder.AppendFormat("<a href=\"#\" onclick=\"window.location.href='{0}login.aspx?reurl=' + _esc(document.location);\">登录</a>&nbsp;|&nbsp;<a href=\"{3}\">论坛</a>&nbsp;|&nbsp;<a href=\"{0}albumindex.aspx\">{2}</a>&nbsp;|&nbsp;<a href=\"{0}spaceindex.aspx\">{1}首页</a>", ht["forumurlnopage"].ToString(), ht["spacename"].ToString(), ht["albumname"].ToString(), ht["forumurl"].ToString());

            builder.AppendFormat("</nobr></div></div>");
            builder.Append(@"<script>_DS_DD_init();</script>
<div id=""doc"">
<div id=""nhdrwrap""><div id=""nhdrwrapinner"">
<div id=""nhdrwrapsizer"">
");
            builder.AppendFormat("<div class=\"title\"><h1>{0}</h1><span><a href=\"{1}\">{1}</a><a target=\"_blank\" href=\"{2}tools/{3}\"><img title=\"rss\" alt=\"rss\" width=\"12\" height=\"12\" src=\"{2}space/images/rss.gif\"/></a></span></div><div class=\"subtitle\">{4}</div>", (ht["config"] as SpaceConfigInfo).Spacetitle, ht["spaceurl"].ToString(), ht["forumpath"].ToString(), spacerssurl, (ht["config"] as SpaceConfigInfo).Description);
            builder.Append(@"<script>_DS_time.stop(""parse_header"");</script><div id=""new_user_demo"" align=""center""></div></div> 
<div id=""tabs""><ul><li class=""tab spacer"">&nbsp;</li>
");
            builder.Append(ht["tabs"].ToString());
            builder.Append("<li class=\"tab addtab\">");
            if ((bool)ht["can_be_added"])
                builder.Append("<a href=\"###\" onclick=\"_dlsetp('action=addtab');\">添加页面</a>");
            else
                builder.Append("&nbsp;");

            builder.Append("</li><li class=\"tab\" id=\"addstuff\">");

            if ((bool)ht["editable"])
                builder.Append(ht["editbar"].ToString());

            builder.Append("</li></ul></div></div></div><div id=\"modules\">");
            builder.Append(ht["modules"]);
            builder.Append("</div></div><div id=\"footer\"><div id=\"copyright\">");
            builder.Append(ht["footer"].ToString());
            builder.Append(@"</div>
</div>

<noscript><div id=""noscript_msg"" class=""msg""><div id=""noscript_box"" class=""msg_box"">本功能需要JavaScript支持，开启以获得更多功能。</div></div></noscript>

<script type=""text/javascript"">
_pl=true;_DS_time.stop(""upcstart"");");
            if ((bool)ht["editable"])
                builder.Append("_table=_gel(\"t_1\");_tabs=_gel(\"tabs\");_upc();");

            builder.Append(@"_DS_time.stop(""upcend"");_DS_time.stop(""domloadstart"");_DS_AddEventHandler(""domload"", function() {_DS_time.stop(""domloadend"");});_DS_TriggerDelayedEvent(""domload"");_DS_AddEventHandler(""load"", function() {_DS_time.stop(""load"");if (parseInt(Math.random() * 100.0) == 1) {var url = ""dls="" + _DS_time.get(""domloadstart"") +""&dle="" + _DS_time.get(""domloadend"") +""&upcs="" + _DS_time.get(""upcstart"") +""&upce="" + _DS_time.get(""upcend"") +""&load="" + _DS_time.get(""load"");_sendx(""nop.aspx?timing="" + _esc(url), null, false);}});</script>
</body>
</html>");
            return builder.ToString();
        }

        #endregion
    }
}
