using System;
using System.Data;
using System.Text;

using Discuz.Entity;
using Discuz.Common;

namespace Discuz.Data
{
    public class Invitation
    {
        /// <summary>
        /// 创建邀请码
        /// </summary>
        /// <param name="inviteCode"></param>
        /// <returns></returns>
        public static int CreateInviteCode(InviteCodeInfo inviteCode)
        {
            return DatabaseProvider.GetInstance().CreateInviteCode(inviteCode);
        }

        /// <summary>
        /// 检查该邀请码code是否已存在于数据库
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static bool IsInviteCodeExist(string code)
        {
            return DatabaseProvider.GetInstance().IsInviteCodeExist(code);
        }

        /// <summary>
        /// 通过邀请码ID获取邀请码信息
        /// </summary>
        /// <param name="inviteid"></param>
        /// <returns></returns>
        public static InviteCodeInfo GetInviteCodeById(int inviteId)
        {
            IDataReader reader = DatabaseProvider.GetInstance().GetInviteCodeById(inviteId);
            InviteCodeInfo inviteCode = null;
            if (reader.Read())
            {
                inviteCode = LoadInviteCode(reader);
            }
            reader.Close();
            return inviteCode;
        }

        /// <summary>
        /// 通过创建人id获取邀请码信息
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static InviteCodeInfo GetInviteCodeByUid(int userId)
        {
            IDataReader reader = DatabaseProvider.GetInstance().GetInviteCodeByUid(userId);
            InviteCodeInfo inviteCode = null;
            if (reader.Read())
            {
                inviteCode = LoadInviteCode(reader);
            }
            reader.Close();
            return inviteCode;
        }

        /// <summary>
        /// 通过code获取邀请码信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static InviteCodeInfo GetInviteCodeByCode(string code)
        {
            IDataReader reader = DatabaseProvider.GetInstance().GetInviteCodeByCode(code);
            InviteCodeInfo inviteCode = null;
            if (reader.Read())
            {
                inviteCode = LoadInviteCode(reader);
            }
            reader.Close();
            return inviteCode;
        }

        /// <summary>
        /// 删除邀请码
        /// </summary>
        /// <param name="inviteid"></param>
        public static void DeleteInviteCode(int inviteId)
        {
            DatabaseProvider.GetInstance().DeleteInviteCode(inviteId);
        }

        /// <summary>
        /// 更新邀请码成功注册次数
        /// </summary>
        /// <param name="inviteid"></param>
        public static void UpdateInviteCodeSuccessCount(int inviteId)
        {
            DatabaseProvider.GetInstance().UpdateInviteCodeSuccessCount(inviteId);
        }

        /// <summary>
        /// 获取用户拥有的邀请码列表(封闭)
        /// </summary>
        /// <param name="creatorid"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public static Discuz.Common.Generic.List<InviteCodeInfo> GetUserInviteCodeList(int creatorId, int pageIndex)
        {
            Discuz.Common.Generic.List<InviteCodeInfo> list = new Discuz.Common.Generic.List<InviteCodeInfo>();
            IDataReader reader = DatabaseProvider.GetInstance().GetUserInviteCodeList(creatorId, pageIndex);
            while (reader.Read())
            {
                list.Add(LoadInviteCode(reader));
            }
            reader.Close();
            return list;
        }

        /// <summary>
        /// 获取用户拥有的邀请码个数(封闭)
        /// </summary>
        /// <param name="creatorid"></param>
        /// <returns></returns>
        public static int GetUserInviteCodeCount(int creatorId)
        {
            return DatabaseProvider.GetInstance().GetUserInviteCodeCount(creatorId);
        }

        /// <summary>
        /// 获取当日用户申请的邀请码数量
        /// </summary>
        /// <param name="creatorId"></param>
        /// <returns></returns>
        public static int GetTodayUserCreatedInviteCode(int creatorId)
        {
            return DatabaseProvider.GetInstance().GetTodayUserCreatedInviteCode(creatorId);
        }

        /// <summary>
        /// 清理用户过期的邀请码(封闭)
        /// </summary>
        /// <param name="creatorId"></param>
        /// <returns></returns>
        public static int ClearExpireInviteCode()
        {
            return DatabaseProvider.GetInstance().ClearExpireInviteCode();
        }

        #region private methods

        /// <summary>
        /// 从reader中装载邀请码信息
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private static InviteCodeInfo LoadInviteCode(IDataReader reader)
        {
            InviteCodeInfo inviteCode = new InviteCodeInfo();
            inviteCode.InviteId = TypeConverter.ObjectToInt(reader["inviteid"]);
            inviteCode.Code = reader["invitecode"].ToString();
            inviteCode.CreatorId = TypeConverter.ObjectToInt(reader["creatorid"]);
            inviteCode.Creator = reader["creator"].ToString().Trim();
            inviteCode.CreateTime = Utils.GetDate(reader["createdtime"].ToString(), "");
            inviteCode.ExpireTime = Utils.GetDate(reader["expiretime"].ToString(), "");
            inviteCode.SuccessCount = TypeConverter.ObjectToInt(reader["successcount"]);
            inviteCode.MaxCount = TypeConverter.ObjectToInt(reader["maxcount"]);
            inviteCode.InviteType = TypeConverter.ObjectToInt(reader["invitetype"]);
            return inviteCode;
        }
        #endregion
    }
}
