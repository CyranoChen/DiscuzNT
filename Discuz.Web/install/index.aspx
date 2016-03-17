<%@ Page Language="C#"%>
<script runat="server">
    public string httpModuleTip = "<br/>请在web.config中configuration->system.web->httpModules中添加节点<br/>" + 
         HttpUtility.HtmlEncode("<add type=\"Discuz.Forum.HttpModule, Discuz.Forum\" name=\"HttpModule\" />") + 
         ",<br/>并在configuration->system.webServer->modules中添加节点<br/>" +  
         HttpUtility.HtmlEncode("<add name=\"HttpModule\" type=\"Discuz.Forum.HttpModule, Discuz.Forum\" preCondition=\"managedHandler\" />");

    public string msg="";


    protected void Page_Load(object sender, EventArgs e)
    {
        bool isAssemblyInexistence = false;
        string binfolderpath = HttpRuntime.BinDirectory;
        try
        {
            string[] assemblylist = new string[] { "Discuz.Aggregation.dll", "Discuz.Cache.dll", "Discuz.Common.dll", "Discuz.Config.dll", 
                "Discuz.Control.dll", "Discuz.Data.dll", "Discuz.Data.SqlServer.dll","Discuz.Entity.dll","Discuz.Event.dll", "Discuz.Forum.dll",
                "Discuz.Install.dll", "Discuz.Plugin.dll","Discuz.Plugin.Spread.dll", "Discuz.Web.Admin.dll",
                "Discuz.Web.dll", "Discuz.Web.Services.dll","Interop.SQLDMO.dll","Newtonsoft.Json.dll" };

            ArrayList inexistenceAssemblyList = new ArrayList();
            foreach (string assembly in assemblylist)
            {
                if (!System.IO.File.Exists(binfolderpath + assembly))
                {
                    isAssemblyInexistence = true;
                    inexistenceAssemblyList.Add(assembly);
                }
            }
            if (isAssemblyInexistence)
            {
                foreach (string assembly in inexistenceAssemblyList)
                {
                    msg += "<li>" + assembly + " 文件放置不正确,请将所有的dll文件复制到目录" + binfolderpath + " 中.</li>";
                }
            }
        }
        catch
        {
            msg += "<li>请将所有的dll文件复制到目录 " + binfolderpath + " 中.</li>";
        }

        if (!System.IO.File.Exists(binfolderpath.Replace("bin\\", "") + "web.config"))
        {
            isAssemblyInexistence = true;
            msg += "<li>web.config文件不存在,请将该文件放置在"+ binfolderpath.Replace("bin\\", "") +" 目录下.</li>";
        }
        else
        {
            string xPath1 = "/configuration/system.web/httpModules";
            string xPath2 = "/configuration/system.webServer/modules";
            System.Xml.XmlDocument webConfig = new System.Xml.XmlDocument();
            System.Xml.XmlDocument webConfigOrigin = new System.Xml.XmlDocument();

            webConfig.Load(binfolderpath.Replace("bin\\", "") + "web.config");
            webConfigOrigin = webConfig;

            System.Xml.XmlNode node1 = webConfig.SelectSingleNode(xPath1);
            System.Xml.XmlNode node2 = webConfig.SelectSingleNode(xPath2);

            if (node1 == null || node1.ChildNodes.Count <= 0 || node1.InnerXml.IndexOf("Discuz.Forum.HttpModule") < 0
                || node2 == null || node2.ChildNodes.Count <= 0 || node2.InnerXml.IndexOf("Discuz.Forum.HttpModule") < 0)
            {
                isAssemblyInexistence = true;
                msg += "<li>web.config中缺少了Discuz!NT的HttpModule," + httpModuleTip + ".<br/><a href=\"http://nt.discuz.net/showtopic-140673.html\">详情请点击...</a></li>";
            }
        }
        if(!isAssemblyInexistence)
            Response.Redirect("install.aspx");
    }
</script>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>Discuz!NT安装</title>
<meta name="keywords" content="Discuz!NT安装" />
<meta name="description" content="Discuz!NT安装" />
<meta name="generator" content="Discuz!NT 3.0.0" />
<meta http-equiv="x-ua-compatible" content="ie=7" />
<link rel="icon" href="favicon.ico" type="image/x-icon" />
<link rel="shortcut icon" href="favicon.ico" type="image/x-icon" />
<link rel="stylesheet" href="main.css" type="text/css" media="all" />
</head>

<body>
<div class="wrap cl">
	<h2><img alt="Discuz!NT|BBS|论坛" src="images/logo.png"/><cite>安装程序</cite></h2>
	<div class="main cl">
		<h1>基本系统环境检测</h1>
		<div class="inner">
            <ol>
			    <%=msg %>
            </ol>
            <span style="color:Red; font-weight:bold">请将上述问题全部解决再刷新该页面继续安装! </span>
		</div>
	</div>
	<div class="copy">
		北京康盛新创科技有限责任公司 &copy; 2001 - 2011 Comsenz Inc. 
	</div>
</div>
</body>
</html>