using System;
using System.IO;
using System.Web.UI.WebControls;
using System.Xml;
using Discuz.Common;
using Discuz.Config;
using Discuz.Web.Admin.AutoUpdateManager;


namespace Discuz.Web.Admin
{
    /// <summary>
    /// ��������ģ��ҳ��
    /// </summary>
    public class onlineupgrade : AdminPage
    {
        protected System.Web.UI.WebControls.Label info = new Label();
        protected bool isNew = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                try
                {
                    //�ϲ�������Ϣ
                    MergeUpgradeInfo();
                    //��ȡ�ٷ������汾�б�����ŵ�����upgrade/versionlist.config��
                    AutoUpdate autoUpdate = new AutoUpdate();
                    string fileContnet = autoUpdate.GetVersionList();
                    StreamWriter writer = new StreamWriter(Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + "config/upgradeini.config"));
                    writer.Write(fileContnet.Replace("\n", "\r\n"));
                    writer.Close();
                    //ȡ���ظ��°汾����ٷ������°汾�űȽϣ�����ٷ��и��£�����ʾ������
                    XmlDocument lastupdate = new XmlDocument();
                    lastupdate.Load(Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + "config/localupgradeini.config"));
                    DateTime build = StrToDateTime(lastupdate.SelectSingleNode("/localupgrade/requiredupgrade").InnerText);
                    XmlDocument currentupdate = new XmlDocument();
                    currentupdate.LoadXml(fileContnet);
                    XmlNodeList items = currentupdate.SelectNodes("/versionlist/" + BaseConfigs.GetDbType.ToLower() + "/requiredupgrade/item");
                    XmlNode lastItem = items.Item(items.Count - 1);
                    DateTime version = StrToDateTime(lastItem["version"].InnerText);
                    isNew = version > build;
                    if (isNew)
                    {
                        info.Text = "<span style='font-size:20px;padding:0px 0px 15px 0px;display:block;'>��⵽���°汾��" + lastItem["versiondescription"].InnerText + "</span>" +
                            "<div style='border: 1px dotted rgb(219, 221, 211);background: #FFFFCC'>" + lastItem["description"].InnerText + "</div>";
                        string script = "var versionList = [";
                        foreach (XmlNode item in items)
                        {
                            if (StrToDateTime(item.FirstChild.InnerText) > build)
                            {
                                script += "{\"version\":\"" + item["version"].InnerText + "\",\"versiondescription\":\"" + item["versiondescription"].InnerText + "\",\"link\":\"" + item["link"].InnerText + "\"},";
                            }
                        }
                        script = script.TrimEnd(',') + "];";
                        base.RegisterStartupScript("", "<script>" + script + "</script>");
                    }
                    else
                    {
                        info.Text = "�����°汾�ɸ��£�";
                        base.RegisterStartupScript("", "<script>var versionList = []; </script>");
                    }
                }
                catch
                {
                    info.Text = "���������쳣�����Ժ����ԡ���";
                }
            }
        }

        private void MergeUpgradeInfo()
        {
            if (!File.Exists(Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + "config/commonupgradeini.config")))
            {
                return;
            }
            XmlDocument configfile = new XmlDocument();
            //��ȡ��ͨ��������Ϣ�ļ�
            string requiredupgradeversion = "";
            string optionalupgradeversion = "";
            configfile.Load(Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + "config/commonupgradeini.config"));
            if (configfile.SelectSingleNode("/requiredupgrade") != null)
            {
                requiredupgradeversion = configfile.SelectSingleNode("/requiredupgrade").InnerText;
            }
            else
            {
                optionalupgradeversion = configfile.SelectSingleNode("/optionalupgrade").InnerText;
            }
            File.Delete(Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + "config/commonupgradeini.config"));

            configfile.Load(Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + "config/localupgradeini.config"));
            if (requiredupgradeversion != "")
            {
                if (StrToDateTime(configfile.SelectSingleNode("/localupgrade/requiredupgrade").InnerText) >= StrToDateTime(requiredupgradeversion))
                {
                    return;
                }
                configfile.SelectSingleNode("/localupgrade/requiredupgrade").InnerText = requiredupgradeversion;
            }
            else
            {
                XmlNode dntver = configfile.SelectSingleNode("/localupgrade/optionalupgrade/dnt" + Utils.GetAssemblyVersion());
                if (dntver == null)
                {
                    dntver = configfile.CreateElement("dnt" + Utils.GetAssemblyVersion());
                }
                XmlElement item = configfile.CreateElement("item");
                item.InnerText = optionalupgradeversion;
                dntver.AppendChild(item);
                configfile.SelectSingleNode("/localupgrade/optionalupgrade").AppendChild(dntver);
            }
            configfile.Save(Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + "config/localupgradeini.config"));
        }

        private DateTime StrToDateTime(string str)
        {
            string date = str.Substring(0, 4) + "-" + str.Substring(4, 2) + "-" + str.Substring(6, 2);
            if (str.Length == 8)
            {
                date += " 00:00:00";
            }
            else
            {
                date += " " + str.Substring(8, 2) + ":" + str.Substring(10, 2) + ":" + str.Substring(12, 2);
            }
            return Convert.ToDateTime(date);
        }
    }
}