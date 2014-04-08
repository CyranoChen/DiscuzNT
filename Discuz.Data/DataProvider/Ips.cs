using System;
using System.Text;
using System.Data;

using Discuz.Entity;
using Discuz.Common.Generic;
using Discuz.Common;

namespace Discuz.Data
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
        public static void AddBannedIp(IpInfo info)
        {
            DatabaseProvider.GetInstance().AddBannedIp(info);
        }

        /// <summary>
        /// 获得被禁止ip列表
        /// </summary>
        /// <returns></returns>
        public static List<IpInfo> GetBannedIpList()
        {
            return GetIpInfoList(DatabaseProvider.GetInstance().GetBannedIpList());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="num"></param>
        /// <param name="pageid"></param>
        /// <returns></returns>
        public static List<IpInfo> GetBannedIpList(int num, int pageid)
        {
            return GetIpInfoList(DatabaseProvider.GetInstance().GetBannedIpList(num, pageid));            
        }

        private static List<IpInfo> GetIpInfoList(IDataReader reader)
        {
            List<IpInfo> list = new List<IpInfo>();
            while (reader.Read())
            {
                IpInfo ipinfo = new IpInfo();
                ipinfo.Id = TypeConverter.ObjectToInt(reader["id"], 0);
                ipinfo.Ip1 = TypeConverter.ObjectToInt(reader["ip1"], 0);
                ipinfo.Ip2 = TypeConverter.ObjectToInt(reader["ip2"], 0);
                ipinfo.Ip3 = TypeConverter.ObjectToInt(reader["ip3"], 0);
                ipinfo.Ip4 = TypeConverter.ObjectToInt(reader["ip4"], 0);
                ipinfo.Username = reader["admin"].ToString();
                ipinfo.Dateline = Convert.ToDateTime(reader["dateline"].ToString()).ToString("yyyy-MM-dd");
                ipinfo.Expiration = Convert.ToDateTime(reader["expiration"].ToString()).ToString("yyyy-MM-dd");
                list.Add(ipinfo);
            }
            reader.Close();
            return list;
        }
           

        public static int GetBannedIpCount()
        { 
            return DatabaseProvider.GetInstance().GetBannedIpCount();
        }

        public static void DelBanIp(string iplist)
        {
            DatabaseProvider.GetInstance().DeleteBanIp(iplist);
        }

        public static void EditBanIp(int id, string endtime)
        {
            try
            {
                DateTime endTime;
                DateTime.TryParse(endtime, out endTime);
                DatabaseProvider.GetInstance().UpdateBanIpExpiration(id, endTime.ToString());
            }
            catch { }
        }
    }
}
