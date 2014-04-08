using Discuz.Common;
using Discuz.Data;
using Discuz.Entity;

namespace Discuz.Forum
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
            return Data.AttachPaymentLogs.CreateAttachPaymentLog(attachPaymentLogInfo);
        }

        /// <summary>
        /// 获取指定符件id的附件交易日志
        /// </summary>
        /// <param name="aid">指定附件id</param>
        /// <returns>附件交易日志</returns>
        public static string GetAttachPaymentLogJsonByAid(int aid)
        {
            return aid > 0 ? Newtonsoft.Json.JavaScriptConvert.SerializeObject(Data.AttachPaymentLogs.GetAttachPaymentLogList(aid).ToArray()) : "";
        }

        /// <summary>
        /// 查看用户是否购买过指定附件
        /// </summary>
        /// <param name="userid">购买用户id</param>
        /// <param name="aid">附件id</param>
        /// <returns></returns>
        public static bool HasBoughtAttach(int userid, int radminid, AttachmentInfo attachmentinfo)
        {
            //检查附件是否存在
            return attachmentinfo.Attachprice > 0 && attachmentinfo.Uid != userid && radminid != 1 && !Data.AttachPaymentLogs.HasBoughtAttach(userid, attachmentinfo.Aid);
        }
    }
}
