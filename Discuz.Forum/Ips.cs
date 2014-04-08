using System;
using System.Text;

using Discuz.Data;
using Discuz.Entity;
using System.Data;
using Discuz.Common;
using Discuz.Common.Generic;

namespace Discuz.Forum
{
    public class Ips
    {
        /// <summary>
        /// 添加被禁止的ip
        /// </summary>
        /// <param name="ip1">ip段</param>
        /// <param name="ip2">ip段</param>
        /// <param name="ip3">ip段</param>
        /// <param name="ip4">ip段</param>
        /// <param name="username">添加人</param>
        /// <param name="deteline">起始时间</param>
        /// <param name="expiration">过期时间</param>
        public static void AddBannedIp(string ipkey, Double deteline, string username)
        {
            string[] ip = ipkey.Split('.');
            deteline = deteline == 0 ? 1 : Math.Round(deteline);

            if ((Utils.StrToInt(ip[0], 0) < 255 || Utils.StrToInt(ip[1], 0) < 255 || Utils.StrToInt(ip[2], 0) < 255 || Utils.StrToInt(ip[3], 0) < 255) && (ip[0] != "0" && ip[1] != "0" && ip[2] != "0" && ip[3] != "0"))
            {
                IpInfo info = new IpInfo();
                info.Ip1 = TypeConverter.StrToInt(ip[0]);
                info.Ip2 = TypeConverter.StrToInt(ip[1], 0);
                info.Ip3 = TypeConverter.StrToInt(ip[2], 0);
                info.Ip4 = TypeConverter.StrToInt(ip[3], 0);
                info.Username = username;
                info.Dateline = DateTime.Now.ToShortDateString();
                info.Expiration = DateTime.Now.AddDays(deteline).ToString("yyyy-MM-dd"); ;
                Discuz.Data.Ips.AddBannedIp(info);
            }
        }

        /// <summary>
        /// 获得被禁止ip列表
        /// </summary>
        /// <returns></returns>
        public static List<IpInfo> GetBannedIpList()
        {
            List<IpInfo> ipInfoList = Discuz.Data.Ips.GetBannedIpList();

            foreach (IpInfo info in ipInfoList)
            {
                info.Location = GetLocation(info);
            }
            return ipInfoList;
        }

        private static string GetLocation(IpInfo info)
        { 
            string ip = string.Format("{0}.{1}.{2}.{3}", info.Ip1, info.Ip2, info.Ip3, info.Ip4);
            Discuz.Forum.IpSearch.PHCZIP phczip = new IpSearch.PHCZIP();
            return phczip.GetAddressWithIP(ip) == "" ? "未知地址": phczip.GetAddressWithIP(ip);
        }

        /// <summary>
        /// 获取禁止IP列表
        /// </summary>
        /// <param name="num"></param>
        /// <param name="pageid"></param>
        /// <returns></returns>
        public static List<IpInfo> GetBannedIpList(int num, int pageid,out int counts)
        {
            List<IpInfo> ipInfoList = Discuz.Data.Ips.GetBannedIpList(num,pageid);

            foreach (IpInfo info in ipInfoList)
            {
                info.Location = GetLocation(info);
            }
            counts = Discuz.Data.Ips.GetBannedIpCount();
            return ipInfoList;
        }


        public static void DelBanIp(string iplist)
        {
            if (!Utils.IsNumericList(iplist))
                return;

            Discuz.Data.Ips.DelBanIp(iplist);
        }

        public static void EditBanIp(string[] expiration, string[] hiddenexpiration, string[] hiddenid, int useradminid, int userid)
        { 
            for (int i = 0; i < expiration.Length; i++)
            {
                //1-管理员 2-超版
                if (useradminid != 1 && userid != Users.GetShortUserInfo(TypeConverter.StrToInt(hiddenid[i])).Uid)
                    continue;

                if (expiration[i] != hiddenexpiration[i])
                    Discuz.Data.Ips.EditBanIp(Utils.StrToInt(hiddenid[i].ToString(), -1), expiration[i]);
            }
        }
    }
}
