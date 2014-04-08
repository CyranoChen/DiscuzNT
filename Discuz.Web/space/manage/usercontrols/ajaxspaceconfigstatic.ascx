<%@ Control Language="c#" AutoEventWireup="false" Codebehind="ajaxspaceconfigstatic.ascx.cs" Inherits="Discuz.Space.Manage.ajaxspaceconfigstatic" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%if(Discuz.Common.DNTRequest.GetString("load")!="true")
{
   Response.Write(WriteLoadingDiv("loadajaxspaceconfigstatic","正在加载数据统计"));
}
else
{
    if(hidetitle == 0)
    {
        Response.Write("<h2 class=\"modtitle\"><span class=\"modtitle_text\">数据统计</span></h2>");
	}		
	Response.Write("<div class=\"dnt-statistic\">");
    if (__spaceconfiginfo != null)
    {%>
		<ul class="ItemContent">
			<li>访问量: <%=__spaceconfiginfo.VisitedTimes%></li>
			<li>日志数: <%=__spaceconfiginfo.PostCount%></li>
			<li>建立时间: <%=__spaceconfiginfo.CreateDateTime.ToString("yyyy-MM-dd")%></li>
			<li>更新时间: <%=__spaceconfiginfo.UpdateDateTime.ToString("yyyy-MM-dd")%></li>
		</ul>
	<%}				
	Response.Write("</div>");
}%>
