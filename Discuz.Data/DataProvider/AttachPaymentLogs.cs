using System;
using System.Text;
using System.Data;

using Discuz.Entity;
using Discuz.Common;
using Discuz.Common.Generic;

namespace Discuz.Data
{
    /// <summary>
    /// 附件交易操作类
    /// </summary>
    public class AttachPaymentLogs
    {
        /// <summary>
        /// 创建附件交易信息
        /// </summary>
        /// <param name="attachPaymentLogInfo">要创建的附件交易信息</param>
        /// <returns>创建的交易id</returns>
        public static int CreateAttachPaymentLog(AttachPaymentlogInfo attachPaymentLogInfo)
        {
            return DatabaseProvider.GetInstance().CreateAttachPaymetLog(attachPaymentLogInfo);
        }

        /// <summary>
        /// 更新的附件交易信息
        /// </summary>
        /// <param name="attachPaymentLogInfo">要更新的附件交易信息</param>
        /// <returns></returns>
        public static int UpdateAttachPaymetLog(AttachPaymentlogInfo attachPaymentLogInfo)
        {
            return DatabaseProvider.GetInstance().UpdateAttachPaymetLog(attachPaymentLogInfo);
        }

        /// <summary>
        /// 获取指定符件id的附件交易日志
        /// </summary>
        /// <param name="aid">指定附件id</param>
        /// <returns>附件交易日志</returns>
        public static List<AttachPaymentlogInfo> GetAttachPaymentLogList(int aid)
        {
            List<AttachPaymentlogInfo> list = new List<AttachPaymentlogInfo>();
            IDataReader reader = DatabaseProvider.GetInstance().GetAttachPaymentLogByAid(aid);
            while (reader.Read())
            {
                list.Add(LoadSingleAttachPaymentlogInfo(reader));
            }
            reader.Close();
            return list;
        }

        /// <summary>
        /// 查看用户是否购买过指定附件
        /// </summary>
        /// <param name="userid">购买用户id</param>
        /// <param name="aid">附件id</param>
        /// <returns></returns>
        public static bool HasBoughtAttach(int userid, int aid)
        {
            return DatabaseProvider.GetInstance().HasBoughtAttach(userid, aid);
        }

        #region 装载对象
        /// <summary>
        /// 加载单个实体对象
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private static AttachPaymentlogInfo LoadSingleAttachPaymentlogInfo(IDataReader reader)
        {
            AttachPaymentlogInfo attachPaymentlogInfo = new AttachPaymentlogInfo();
            attachPaymentlogInfo.Id = TypeConverter.ObjectToInt(reader["id"]);
            attachPaymentlogInfo.Uid = TypeConverter.ObjectToInt(reader["uid"]);
            attachPaymentlogInfo.UserName = reader["username"].ToString().Trim();
            attachPaymentlogInfo.Aid = TypeConverter.ObjectToInt(reader["aid"]);
            attachPaymentlogInfo.Authorid = TypeConverter.ObjectToInt(reader["authorid"]);
            attachPaymentlogInfo.PostDateTime = Convert.ToDateTime(reader["postdatetime"]);
            attachPaymentlogInfo.Amount = TypeConverter.ObjectToInt(reader["amount"]);
            attachPaymentlogInfo.NetAmount = TypeConverter.ObjectToInt(reader["netamount"]);
            return attachPaymentlogInfo;
        }
        #endregion
    }
}
