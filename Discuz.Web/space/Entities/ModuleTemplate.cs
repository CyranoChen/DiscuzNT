using System;
using System.Collections;
using System.Text;

namespace Discuz.Space.Entities
{
    public class ModuleTemplate : ISpaceTemplate
    {
        public static readonly ISpaceTemplate Instance = new ModuleTemplate();
        private ModuleTemplate()
        {
        }
        public string GetHtml(Hashtable ht)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(
                @"<div id=""m___MODULE_ID__"" class=""modbox"">
<h2 class=""modtitle"">	
	<div class=""controlBox"">");
            if ((bool)ht["deletable"])
                builder.AppendFormat(
                    @"<a class=""delbox"" alt=""ɾ����ģ��"" title=""ɾ����ģ��"" href=""###"" onclick=""if(confirm('��ȷ��Ҫɾ�����ģ����?')){{return _del('__MODULE_ID__',__TAB_ID__,'n___TAB_ID__=');}}return false;""></a>");
            if ((bool)ht["scalable"])
                builder.AppendFormat(
                    @"<a class=""minbox"" alt=""չ��/�����ģ��"" title=""չ��/�����ģ��"" id=""m___MODULE_ID___zippy"" href=""###"" onclick=""this.blur();return _zm('__MODULE_ID__', '__TAB_ID__');return false;""></a>");
            if ((bool)ht["editable"])
                builder.AppendFormat(@"<a class=""ddbox"" alt=""�༭��ģ������"" title=""�༭��ģ������"" id=""DD_tg___MODULE_ID__"" href=""###"" onclick=""_edit(__MODULE_ID__);return false;""></a>");
            builder.AppendFormat(@"
	</div>
	<span class=""my_gadget"" style=""display:none""></span>
	<div id=""m___MODULE_ID___h"" class=""moduletitle"">");
            builder.Append(ht["title"]);
            builder.AppendFormat(@"
	</div>
	<div class=""meditbox"">");
            if ((bool)ht["editable"])
            {
                builder.AppendFormat(
                    @"<form id=""m___MODULE_ID___form"" onsubmit=""return _fsetp(this,'__MODULE_ID__',__TAB_ID__);"">");
                builder.Append(ht["meditbox"]);
                builder.AppendFormat(@"
		<table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""95%"" align=""center""><tr><td colspan=""2"" nowrap align=""left""><br /><a href=""""><font size=""-1"" color=""#7777cc""></font></a></td><td colspan=""1"" align=""right"" valign=""bottom""><input id=""m___MODULE_ID___numfields"" type=""hidden"" value=""__USERPREF_COUNT__""><input id=""save___MODULE_ID__"" class=""submitbtn"" type=""submit"" value=""����""><input type=""button"" value=""ȡ��"" onclick=""return _cedit('__MODULE_ID__')""></td></tr></table>
	</form>");
            }
            builder.AppendFormat(@"
	</div>
</h2>
<div id=""m___MODULE_ID___b"" class=""modboxin"" >");
            builder.Append(ht["modboxin"]);
            builder.AppendFormat(@"
</div>
</div>");
            return builder.ToString();
        }
    }
}
