using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using Discuz.Common;
using Discuz.Config;
using Discuz.Web.Admin.WebUpgradeManager;

namespace Discuz.Web.Admin
{
    public class UpgradeInfo
    {
        private string officialUpgradeini = Utils.GetMapPath("~/upgrade/officialUpgradeini.config");
        private string localUpgradeini = Utils.GetMapPath("~/upgrade/localUpgradeini.config");

        /// <summary>
        /// 获取升级信息
        /// </summary>
        public UpgradeInfo()
        {
            //获取官方升级版本列表，并存放到本地upgrade/versionlist.config中
            WebUpgrade webupgrade = new WebUpgrade();
            string fileContnet = webupgrade.GetVersionList();
            StreamWriter writer = new StreamWriter(officialUpgradeini);
            writer.Write(fileContnet.Replace("\n", "\r\n"));
            writer.Close();
        }

        /// <summary>
        /// 检查最新更新
        /// </summary>
        /// <returns>是否能更新</returns>
        public bool IsNewUpgrade()
        {
            //取本地更新版本号与官方的最新版本号比较，如果官方有更新，则提示有升级
            return OfficialLastUpgradeDate() > LocalLastUpgradeDate();
        }

        /// <summary>
        /// 获取官方最后更新日期
        /// </summary>
        /// <returns>官方最后更新日期</returns>
        public DateTime OfficialLastUpgradeDate()
        {
            XmlDocument currentupdate = new XmlDocument();
            currentupdate.Load(officialUpgradeini);
            XmlNodeList items = currentupdate.SelectNodes("/versionlist/" + BaseConfigs.GetDbType.ToLower() + "/necessaryupgrade/item");
            return StrToDateTime(items.Item(items.Count - 1).FirstChild.InnerText);
        }

        /// <summary>
        /// 获取本地最后更新日期
        /// </summary>
        /// <returns>本地最后更新日期</returns>
        public DateTime LocalLastUpgradeDate()
        {
            XmlDocument lastupdate = new XmlDocument();
            lastupdate.Load(localUpgradeini);
            return StrToDateTime(lastupdate.SelectSingleNode("/localupgrade/necessaryupgrade").InnerText);
        }

        /// <summary>
        /// 将字符串格式化为标准的日期
        /// </summary>
        /// <param name="str">日期字符串</param>
        /// <returns>格式化的日期</returns>
        private DateTime StrToDateTime(string str)
        {
            string date = str.Substring(0, 4) + "-" + str.Substring(4, 2) + "-" + str.Substring(6, 2);
            return Convert.ToDateTime(date);
        }
    }
}
