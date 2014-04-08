<%@ Control CodeBehind="spacecalendar.ascx.cs" Language="c#" AutoEventWireup="false" Inherits="Discuz.Space.Manage.spacecalendar" %>
<div class="dnt-calendar">
				<h2 class="modtitle"><span class="modtitle_text">日历</span></h2>
	

<div id="usercalendar" class="NtSpace-sideblock">
</div>
<script type="text/javascript" src="manage/js/spacecalender.js"></script>
<script language="javascript1.2" type="text/javascript">	
var spaceid = "<% = spaceid%>";
var hidetitle = "<% = hidetitle%>";
if(spaceid <= 0)
{
	spaceid = "<% = Discuz.Common.DNTRequest.GetInt("spaceid",0)%>";
}
//加载日历
HS_setDate(document.getElementById('usercalendar'));  
</script>
</div>