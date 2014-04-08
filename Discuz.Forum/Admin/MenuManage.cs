using System;
using System.Data;
using System.IO;
using System.Text;
using Discuz.Common;
using Discuz.Common.Xml;
using System.Xml;
using Discuz.Config;

namespace Discuz.Forum
{
    public class MenuManage
    {
        private static readonly string configPath = Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + "admin/xml/navmenu.config");

        #region ���˵���������
        /// <summary>
        /// �������˵�
        /// </summary>
        /// <param name="title">���˵�����</param>
        /// <param name="defaulturl">���˵�Ĭ��չ����ҳ��</param>
        /// <returns>�����˵���ID</returns>
        public static int NewMainMenu(string title, string defaulturl)
        {
            XmlDocumentExtender doc = new XmlDocumentExtender();
            doc.Load(configPath);
            XmlNodeList mainmenus = doc.SelectNodes("/dataset/toptabmenu");
            int newMenuId = mainmenus.Count + 1;
            XmlElement newMainMenuItem = doc.CreateElement("toptabmenu");
            XmlElement node = doc.CreateElement("id");
            node.InnerText = newMenuId.ToString();
            newMainMenuItem.AppendChild(node);

            node = doc.CreateElement("title");
            node.InnerText = title;
            newMainMenuItem.AppendChild(node);

            node = doc.CreateElement("mainmenulist");
            node.InnerText = "";
            newMainMenuItem.AppendChild(node);

            node = doc.CreateElement("mainmenuidlist");
            node.InnerText = "";
            newMainMenuItem.AppendChild(node);

            node = doc.CreateElement("defaulturl");
            node.InnerText = defaulturl;
            newMainMenuItem.AppendChild(node);

            node = doc.CreateElement("system");
            node.InnerText = "0";
            newMainMenuItem.AppendChild(node);

            doc.SelectSingleNode("/dataset").AppendChild(newMainMenuItem);
            doc.Save(configPath);
            return newMenuId;
        }

        /// <summary>
        /// �༭һ�����˵�
        /// </summary>
        /// <param name="menuid">һ�����˵���ID</param>
        /// <param name="title">һ�����˵�����</param>
        /// <param name="defaulturl">һ�����˵�Ĭ��չ����ҳ��</param>
        /// <returns>�����ɹ���</returns>
        public static bool EditMainMenu(int menuid, string title, string defaulturl)
        {
            XmlDocumentExtender doc = new XmlDocumentExtender();
            doc.Load(configPath);
            XmlNodeList mainmenus = doc.SelectNodes("/dataset/toptabmenu");
            bool result = false;
            foreach (XmlNode menuitem in mainmenus)
            {
                if (menuitem["id"].InnerText == menuid.ToString())
                {
                    menuitem["title"].InnerText = title;
                    menuitem["defaulturl"].InnerText = defaulturl;
                    result = true;
                    break;
                }
            }
            if (result)
            {
                doc.Save(configPath);
            }
            return result;
        }

        /// <summary>
        /// �༭���˵�
        /// </summary>
        /// <param name="oldMainMenuTitle">�����˵�����</param>
        /// <param name="newMainMenuTitle">�����˵�����</param>
        /// <param name="defaulturl">Ĭ��չ����ҳ��</param>
        /// <returns></returns>
        public static bool EditMainMenu(string oldMainMenuTitle, string newMainMenuTitle, string defaulturl)
        {
            int mainid = FindMenuID(oldMainMenuTitle);
            if (mainid == -1)
                return false;
            return EditMainMenu(mainid, newMainMenuTitle, defaulturl);
        }

        /// <summary>
        /// ɾ��һ���˵��������Ӳ˵�����Ϊ��
        /// </summary>
        /// <param name="menuid">Ҫɾ����һ���˵�ID</param>
        /// <returns>�����ɹ���</returns>
        public static bool DeleteMainMenu(int menuid)
        {
            XmlDocumentExtender doc = new XmlDocumentExtender();
            doc.Load(configPath);
            XmlNodeList mainmenus = doc.SelectNodes("/dataset/toptabmenu");
            XmlNode delMenu = null;
            int newid = menuid;
            bool result = false;
            foreach (XmlNode menuitem in mainmenus)
            {
                if (menuitem["id"].InnerText == menuid.ToString())
                {
                    if (menuitem["mainmenulist"].InnerText.Trim() != "")
                        return false;
                    delMenu = menuitem;
                    result = true;
                    break;
                }
                else
                {
                    if (delMenu != null)
                    {
                        menuitem["id"].InnerText = newid.ToString();
                        newid++;
                    }
                }
            }
            if (result)
            {
                delMenu.ParentNode.RemoveChild(delMenu);
                doc.Save(configPath);
            }
            return result;
        }

        /// <summary>
        /// ɾ�����˵�
        /// </summary>
        /// <param name="menuTitle">Ҫɾ�������˵�����</param>
        /// <returns>�����ɹ���</returns>
        public static bool DeleteMainMenu(string menuTitle)
        {
            int mainId = FindMenuID(menuTitle);
            if(mainId == -1)
                return false;
            return DeleteMainMenu(mainId);
        }
        #endregion

        #region �Ӳ˵���������
        /// <summary>
        /// �༭�Ӳ˵�
        /// </summary>
        /// <param name="submenuid">�Ӳ˵�ID</param>
        /// <param name="menutitle">�Ӳ˵�����</param>
        /// <returns>�����ɹ���</returns>
        public static bool EditSubMenu(int submenuid, string menutitle)
        {
            XmlDocumentExtender doc = new XmlDocumentExtender();
            doc.Load(configPath);
            XmlNodeList submains = doc.SelectNodes("/dataset/mainmenu");
            bool result = false;
            foreach (XmlNode menuItem in submains)
            {
                if (menuItem["id"].InnerText == submenuid.ToString())
                {
                    menuItem["menutitle"].InnerText = menutitle;
                    result = true;
                    break;
                }
            }
            if (result)
            {
                doc.Save(configPath);
            }
            return result;
        }

        /// <summary>
        /// �༭�Ӳ˵�
        /// </summary>
        /// <param name="mainMenuTitle">���˵�����</param>
        /// <param name="oldSubMenuTitle">���Ӳ˵�����</param>
        /// <param name="newSubMenuTitle">���Ӳ˵�����</param>
        /// <returns>�����ɹ���</returns>
        public static bool EditSubMenu(string mainMenuTitle, string oldSubMenuTitle, string newSubMenuTitle)
        {
            int subid = FindMenuID(mainMenuTitle, oldSubMenuTitle);
            if (subid == -1)
                return false;
            return EditSubMenu(subid, newSubMenuTitle);
        }

        /// <summary>
        /// �����Ӳ˵�
        /// </summary>
        /// <param name="mainmenuid">���˵�ID</param>
        /// <param name="menutitle">�Ӳ˵�����</param>
        /// <returns>�½��Ӳ˵�ID</returns>
        public static int NewSubMenu(int mainmenuid, string menutitle)
        {
            int newid = 1;
            int newmenuid = 100;
            XmlDocumentExtender doc = new XmlDocumentExtender();
            doc.Load(configPath);
            XmlNodeList submains = doc.SelectNodes("/dataset/mainmenu");
            newid += int.Parse(submains.Item(submains.Count - 1)["id"].InnerText);
            newmenuid += int.Parse(submains.Item(submains.Count - 1)["menuid"].InnerText);
            XmlElement mainmenu = doc.CreateElement("mainmenu");
            XmlElement node = doc.CreateElement("id");
            node.InnerText = newid.ToString();
            mainmenu.AppendChild(node);

            node = doc.CreateElement("menuid");
            node.InnerText = newmenuid.ToString();
            mainmenu.AppendChild(node);

            node = doc.CreateElement("menutitle");
            node.InnerText = menutitle;
            mainmenu.AppendChild(node);

            doc.SelectSingleNode("/dataset").AppendChild(mainmenu);

            XmlNodeList mainMenus = doc.SelectNodes("/dataset/toptabmenu");
            foreach (XmlNode menuItem in mainMenus) //�������˵���Ϣ
            {
                if (menuItem["id"].InnerText == mainmenuid.ToString())
                {
                    menuItem["mainmenulist"].InnerText += "," + newid;
                    menuItem["mainmenuidlist"].InnerText += "," + newmenuid;
                    menuItem["mainmenulist"].InnerText = menuItem["mainmenulist"].InnerText.TrimStart(',');
                    menuItem["mainmenuidlist"].InnerText = menuItem["mainmenuidlist"].InnerText.TrimStart(',');
                    break;
                }
            }
            doc.Save(configPath);
            return newmenuid;
        }

        /// <summary>
        /// �����Ӳ˵�
        /// </summary>
        /// <param name="mainMenuTitle">���˵�����</param>
        /// <param name="newSubMenuTitle">�����Ӳ˵�����</param>
        /// <returns>�����ɹ���</returns>
        public static int NewSubMenu(string mainMenuTitle, string newSubMenuTitle)
        {
            int mainid = FindMenuID(mainMenuTitle);
            if(mainid == -1)
                return -1;
            return NewSubMenu(mainid, newSubMenuTitle);
        }

        /// <summary>
        /// ɾ���Ӳ˵�
        /// </summary>
        /// <param name="submenuid">�Ӳ˵�ID</param>
        /// <param name="mainmenuid">���˵�ID</param>
        /// <returns>�����ɹ���</returns>
        public static bool DeleteSubMenu(int submenuid, int mainmenuid)
        {
            XmlDocumentExtender doc = new XmlDocumentExtender();
            doc.Load(configPath);
            XmlNodeList submains = doc.SelectNodes("/dataset/mainmenu");
            bool result = false;
            string menuid = "";
            foreach (XmlNode menuItem in submains)
            {
                if (menuItem["id"].InnerText == submenuid.ToString())
                {
                    menuid = menuItem["menuid"].InnerText;
                    XmlNodeList items = doc.SelectNodes("/dataset/submain");
                    foreach (XmlNode item in items)
                    {
                        if (item["menuparentid"].InnerText == menuid)
                            return false;
                    }
                    menuItem.ParentNode.RemoveChild(menuItem);
                    result = true;
                    break;
                }
            }
            XmlNodeList mainMenus = doc.SelectNodes("/dataset/toptabmenu");
            foreach (XmlNode menuItem in mainMenus) //�������˵���Ϣ
            {
                if (menuItem["id"].InnerText == mainmenuid.ToString())
                {
                    menuItem["mainmenulist"].InnerText = ("," + menuItem["mainmenulist"].InnerText + ",").Replace("," + submenuid + ",", ",");
                    menuItem["mainmenuidlist"].InnerText = ("," + menuItem["mainmenuidlist"].InnerText + ",").Replace("," + menuid + ",", ",");
                    menuItem["mainmenulist"].InnerText = menuItem["mainmenulist"].InnerText.TrimStart(',').TrimEnd(',');
                    menuItem["mainmenuidlist"].InnerText = menuItem["mainmenuidlist"].InnerText.TrimStart(',').TrimEnd(',');
                    break;
                }
            }
            if (result)
            {
                doc.Save(configPath);
            }
            return result;
        }

        public static bool DownSubMenu(int submenuid, int mainmenuid)
        {
            return true;
        }

        /// <summary>
        /// ɾ���Ӳ˵�
        /// </summary>
        /// <param name="mainMenuTitle">���˵�����</param>
        /// <param name="subMenuTitle">Ҫɾ�����Ӳ˵�����</param>
        /// <returns></returns>
        public static bool DeleteSubMenu(string mainMenuTitle, string subMenuTitle)
        {
            int mainId = FindMenuID(mainMenuTitle);
            int subId = FindMenuID(mainMenuTitle, subMenuTitle);
            if (mainId == -1 || subId == -1)
                return false;
            return DeleteSubMenu(subId,mainId);
        }

        #endregion

        #region �˵����������

        /// <summary>
        /// �½��˵���
        /// </summary>
        /// <param name="menuparentid">���˵�ID</param>
        /// <param name="title">�˵�����</param>
        /// <param name="link">�˵�����</param>
        /// <returns>�����ɹ���</returns>
        public static bool NewMenuItem(int menuparentid, string title, string link)
        {
            XmlDocumentExtender doc = new XmlDocumentExtender();
            doc.Load(configPath);
            XmlNodeList submains = doc.SelectNodes("/dataset/submain");
            foreach (XmlNode sub in submains)
            {
                if (sub["link"].InnerText == link)
                {
                    return false;
                }
            }
            XmlElement submain = doc.CreateElement("submain");
            XmlElement node = doc.CreateElement("menuparentid");
            node.InnerText = menuparentid.ToString();
            submain.AppendChild(node);

            node = doc.CreateElement("menutitle");
            node.InnerText = title;
            submain.AppendChild(node);

            node = doc.CreateElement("link");
            node.InnerText = link;
            submain.AppendChild(node);

            node = doc.CreateElement("frameid");
            node.InnerText = "main";
            submain.AppendChild(node);

            doc.SelectSingleNode("/dataset").AppendChild(submain);
            doc.Save(configPath);
            return true;
        }

        /// <summary>
        /// �½��˵���
        /// </summary>
        /// <param name="mainMenuTitle">���˵�����</param>
        /// <param name="subMenutitle">�Ӳ˵�����</param>
        /// <param name="newMenuItemTitle">�����˵������</param>
        /// <param name="link">�˵��������ҳ��</param>
        /// <returns>�����ɹ���</returns>
        public static bool NewMenuItem(string mainMenuTitle, string subMenutitle, string newMenuItemTitle, string link)
        {
            int subid = FindMenuMenuid(mainMenuTitle, subMenutitle);
            if(subid == -1)
                return false;
            return NewMenuItem(subid,newMenuItemTitle,link);
        }

        /// <summary>
        /// �༭�˵���
        /// </summary>
        /// <param name="id">�˵�������</param>
        /// <param name="title">�˵���ı���</param>
        /// <param name="link">�˵��������</param>
        /// <returns>�����ɹ���</returns>
        public static bool EditMenuItem(int id, string title, string link)
        {
            XmlDocumentExtender doc = new XmlDocumentExtender();
            doc.Load(configPath);
            XmlNodeList submains = doc.SelectNodes("/dataset/submain");
            int rowcount = 0;
            foreach (XmlNode sub in submains)
            {
                if (rowcount.ToString() != id.ToString() && sub["link"].InnerText == link)
                {
                    return false;
                }
                rowcount++;
            }
            string tmpLink = submains.Item(id)["link"].InnerText;
            submains.Item(id)["menutitle"].InnerText = title;
            submains.Item(id)["link"].InnerText = link;
            XmlNodeList shortcuts = doc.SelectNodes("/dataset/shortcut");
            foreach (XmlNode shortmenuitem in shortcuts)
            {
                if (shortmenuitem["link"].InnerText == tmpLink)
                {
                    shortmenuitem["link"].InnerText = link;
                    shortmenuitem["menutitle"].InnerText = title;
                    break;
                }
            }
            doc.Save(configPath);
            return true;
        }


        /// <summary>
        /// �༭�˵���
        /// </summary>
        /// <param name="mainMenuTitle">���˵�����</param>
        /// <param name="subMenuTitle">�Ӳ˵�����</param>
        /// <param name="oldItemTitle">�ɲ˵������</param>
        /// <param name="newItemTitle">�²˵������</param>
        /// <param name="link">�˵��������</param>
        /// <returns>�����ɹ���</returns>
        public static bool EditMenuItem(string mainMenuTitle,string subMenuTitle,string oldItemTitle,string newItemTitle, string link)
        {
            int itemid = FindMenuID(mainMenuTitle, subMenuTitle, oldItemTitle);
            if (itemid == -1)
                return false;
            return EditMenuItem(itemid, newItemTitle, link);
        }

        /// <summary>
        /// ɾ���˵���
        /// </summary>
        /// <param name="id">�˵�������</param>
        public static void DeleteMenuItem(int id)
        {
            XmlDocumentExtender doc = new XmlDocumentExtender();
            doc.Load(configPath);
            XmlNodeList submains = doc.SelectNodes("/dataset/submain");
            string link = submains.Item(id)["link"].InnerText;
            submains.Item(id).ParentNode.RemoveChild(submains.Item(id));
            XmlNodeList shortcuts = doc.SelectNodes("/dataset/shortcut");
            foreach (XmlNode shortmenuitem in shortcuts)
            {
                if (shortmenuitem["link"].InnerText == link)
                {
                    shortmenuitem.ParentNode.RemoveChild(shortmenuitem);
                    break;
                }
            }
            doc.Save(configPath);
        }

        /// <summary>
        /// ɾ���˵���
        /// </summary>
        /// <param name="mainMenuTitle">���˵�����</param>
        /// <param name="subMenuTitle">�Ӳ˵�����</param>
        /// <param name="menuItemTitle">Ҫɾ���Ĳ˵������</param>
        /// <returns>�����ɹ���</returns>
        public static bool DeleteMenuItem(string mainMenuTitle, string subMenuTitle, string menuItemTitle)
        {
            int itemId = FindMenuID(mainMenuTitle, subMenuTitle, menuItemTitle);
            if (itemId == -1)
                return false;
            DeleteMenuItem(itemId);
            return true;
        }

        #endregion

        #region �˵�����
        private static int FindMenuID(string mainMenuTitle)
        {
            XmlDocumentExtender doc = new XmlDocumentExtender();
            doc.Load(configPath);
            XmlNodeList mainmenus = doc.SelectNodes("/dataset/toptabmenu");
            foreach (XmlNode menuitem in mainmenus)
            {
                if (menuitem["title"].InnerText == mainMenuTitle)
                {
                    return int.Parse(menuitem["id"].InnerText); 
                }
            }
            return -1;
        }

        private static int FindMenuID(string mainMenuTitle, string subMenuTitle)
        {
            XmlDocumentExtender doc = new XmlDocumentExtender();
            doc.Load(configPath);
            XmlNodeList mainmenus = doc.SelectNodes("/dataset/toptabmenu");
            bool find = false;
            string mainmenulist = "";
            foreach (XmlNode menuitem in mainmenus)
            {
                if (menuitem["title"].InnerText == mainMenuTitle)
                {
                    mainmenulist = menuitem["mainmenulist"].InnerText;
                    find = true;
                }
            }
            if (!find)
                return -1;
            XmlNodeList submains = doc.SelectNodes("/dataset/mainmenu");
            foreach (XmlNode menuItem in submains)
            {
                if (("," + mainmenulist + ",").IndexOf("," + menuItem["id"].InnerText + ",") != -1 && menuItem["menutitle"].InnerText == subMenuTitle)
                {
                    return int.Parse(menuItem["id"].InnerText);
                }
            }
            return -1;
        }

        private static int FindMenuID(string mainMenuTitle, string subMenuTitle, string menuItemTitle)
        {
            XmlDocumentExtender doc = new XmlDocumentExtender();
            doc.Load(configPath);
            XmlNodeList mainmenus = doc.SelectNodes("/dataset/toptabmenu");
            bool find = false;
            string mainmenulist = "";
            foreach (XmlNode menuitem in mainmenus)
            {
                if (menuitem["title"].InnerText == mainMenuTitle)
                {
                    mainmenulist = menuitem["mainmenulist"].InnerText;
                    find = true;
                }
            }
            if (!find)
                return -1;
            XmlNodeList submains = doc.SelectNodes("/dataset/mainmenu");
            find = false;
            string menuid = "";
            foreach (XmlNode menuItem1 in submains)
            {
                if (("," + mainmenulist + ",").IndexOf("," + menuItem1["id"].InnerText + ",") != -1 && menuItem1["menutitle"].InnerText == subMenuTitle)
                {
                    menuid = menuItem1["menuid"].InnerText;
                    find = true;
                }
            }
            if (!find)
            {
                return -1;
            }
            XmlNodeList submains1 = doc.SelectNodes("/dataset/submain");
            int rowcount = 0;
            foreach (XmlNode sub in submains1)
            {
                if (sub["menuparentid"].InnerText == menuid && sub["menutitle"].InnerText == menuItemTitle)
                {
                    return rowcount;
                }
                rowcount++;
            }
            return -1;
        }

        private static int FindMenuMenuid(string mainMenuTitle, string subMenuTitle)
        {
            XmlDocumentExtender doc = new XmlDocumentExtender();
            doc.Load(configPath);
            XmlNodeList mainmenus = doc.SelectNodes("/dataset/toptabmenu");
            bool find = false;
            string mainmenulist = "";
            foreach (XmlNode menuitem in mainmenus)
            {
                if (menuitem["title"].InnerText == mainMenuTitle)
                {
                    mainmenulist = menuitem["mainmenulist"].InnerText;
                    find = true;
                }
            }
            if (!find)
                return -1;
            XmlNodeList submains = doc.SelectNodes("/dataset/mainmenu");
            foreach (XmlNode menuItem in submains)
            {
                if (("," + mainmenulist + ",").IndexOf("," + menuItem["id"].InnerText + ",") != -1 && menuItem["menutitle"].InnerText == subMenuTitle)
                {
                    return int.Parse(menuItem["menuid"].InnerText);
                }
            }
            return -1;
        }

        /// <summary>
        /// ���Ҳ˵���
        /// </summary>
        /// <param name="mainMenuTitle">���˵�����</param>
        /// <returns>�Ƿ����</returns>
        public static bool FindMenu(string mainMenuTitle)
        {
            return FindMenuID(mainMenuTitle) != -1;
        }

        /// <summary>
        /// ���Ҳ˵���
        /// </summary>
        /// <param name="mainMenuTitle">���˵�����</param>
        /// <param name="subMenuTitle">�Ӳ˵�����</param>
        /// <returns>�Ƿ����</returns>
        public static bool FindMenu(string mainMenuTitle, string subMenuTitle)
        {
            return FindMenuID(mainMenuTitle, subMenuTitle) != -1;
        }

        /// <summary>
        /// ���Ҳ˵���
        /// </summary>
        /// <param name="mainMenuTitle">���˵�����</param>
        /// <param name="subMenuTitle">�Ӳ˵�����</param>
        /// <param name="menuItemTitle">�˵������</param>
        /// <returns>�Ƿ����</returns>
        public static bool FindMenu(string mainMenuTitle, string subMenuTitle, string menuItemTitle)
        {
            return FindMenuID(mainMenuTitle, subMenuTitle, menuItemTitle) != -1;
        }

        /// <summary>
        /// ������չ�˵�
        /// </summary>
        /// <returns>��չ�˵���ID</returns>
        public static int FindPluginMainMenu()
        {
            XmlDocumentExtender doc = new XmlDocumentExtender();
            doc.Load(configPath);
            XmlNodeList mainmenus = doc.SelectNodes("/dataset/toptabmenu");
            foreach (XmlNode menuitem in mainmenus)
            {
                if (menuitem["system"].InnerText == "2")
                {
                    return int.Parse(menuitem["id"].InnerText);
                }
            }
            return -1;
        }
        #endregion

        #region ���ɲ˵�Json�ļ�
        public static void CreateMenuJson()
        {
            string jsPath = Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + "admin/xml/navmenu.js"); 
            System.Data.DataSet dsSrc = new System.Data.DataSet();
            dsSrc.ReadXml(configPath);
            StringBuilder menustr = new StringBuilder();
            menustr.Append("var toptabmenu = ");
            menustr.Append(Utils.DataTableToJSON(dsSrc.Tables[2]));
            menustr.Append("\r\nvar mainmenu = ");
            menustr.Append(Utils.DataTableToJSON(dsSrc.Tables[0]));
            menustr.Append("\r\nvar submenu = ");
            menustr.Append(Utils.DataTableToJSON(dsSrc.Tables[1]));
            menustr.Append("\r\nvar shortcut = ");
            if (dsSrc.Tables.Count < 4)
                menustr.Append("[]");
            else
                menustr.Append(Utils.DataTableToJSON(dsSrc.Tables[3]));
            WriteJsonFile(jsPath, menustr);
            dsSrc.Dispose();
        }

        private static void WriteJsonFile(string path, StringBuilder json_sb)
        {
            using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                Byte[] info = System.Text.Encoding.UTF8.GetBytes(json_sb.ToString());
                fs.Write(info, 0, info.Length);
                fs.Close();
            }
        }
        #endregion

        #region �������˵�
        public static void ImportPluginMenu(string menuConfigFile)
        {
            //���ݲ˵�
            BackupMenuFile();
            XmlDocumentExtender doc = new XmlDocumentExtender();
            doc.Load(menuConfigFile);
            XmlNodeList menuitems = doc.SelectNodes("/pluginmenu/menuitem");
            int pluginMainId = FindPluginMainMenu();
            foreach(XmlNode menuitem in menuitems)
            {
                int newsubmenuid = NewSubMenu(pluginMainId, menuitem.Attributes["title"].InnerText);
                XmlNodeList items = menuitem.ChildNodes;
                foreach(XmlNode item in items)
                {
                    NewMenuItem(newsubmenuid, item["title"].InnerText, item["link"].InnerText);
                }
            }
            CreateMenuJson();
        }
        #endregion

        #region ���ݲ˵�����
        public static void BackupMenuFile()
        {
            Utils.BackupFile(configPath, Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + "admin/xml/backup/" + DateTime.Now.ToString("yyyy-MM-dd HH_mm_ss") + ".config"));
        }
        #endregion
    }
}
