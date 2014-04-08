using System;

using Discuz.Common;
using Discuz.Config;
using Discuz.Entity;

namespace Discuz.Forum
{
    public class Invitation
    {


        #region private methods

        private static string BuildInviteCode()
        {
            string[] sourceCode = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            string code = "";
            Random inviteCodeRandom = new Random();
            int i = 0;
            while (i++ < 7)
            {
                code += sourceCode[inviteCodeRandom.Next(0, 25)];
            }
            return code;
        }

        private static bool IsInviteCodeExist(string code)
        {
            return Data.Invitation.IsInviteCodeExist(code);
        }

        #endregion

        /// <summary>
        /// 创建邀请码信息
        /// </summary>
        /// <param name="userInfo">创建用户信息</param>
        /// <returns></returns>
        public static int CreateInviteCode(ShortUserInfo userInfo)
        {
            InvitationConfigInfo configInfo = InvitationConfigs.GetConfig();
            InviteCodeInfo inviteCode = new InviteCodeInfo();
            inviteCode.CreatorId = userInfo.Uid;
            inviteCode.Creator = userInfo.Username;

            inviteCode.Code = BuildInviteCode();
            while (IsInviteCodeExist(inviteCode.Code))//生成的邀请码code是否存在于数据库中
            {
                inviteCode.Code = BuildInviteCode();
            }

            inviteCode.CreateTime = Utils.GetDateTime();
            inviteCode.InviteType = GeneralConfigs.GetConfig().Regstatus;
            inviteCode.ExpireTime = Utils.GetDateTime(configInfo.InviteCodeExpireTime);
            if (inviteCode.InviteType == 3)
                inviteCode.MaxCount = configInfo.InviteCodeMaxCount > 1 ? configInfo.InviteCodeMaxCount : 1;
            else
                inviteCode.MaxCount = configInfo.InviteCodeMaxCount;
            return Data.Invitation.CreateInviteCode(inviteCode);
        }

        /// <summary>
        /// 通过创建人ID获取邀请码
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns></returns>
        public static InviteCodeInfo GetInviteCodeByUid(int userId)
        {
            if (userId > 0)
                return Data.Invitation.GetInviteCodeByUid(userId);
            else
                return null;
        }

        /// <summary>
        /// 通过ID获取邀请码
        /// </summary>
        /// <param name="inviteid">邀请码id</param>
        /// <returns></returns>
        public static InviteCodeInfo GetInviteCodeById(int inviteId)
        {
            return Data.Invitation.GetInviteCodeById(inviteId);
        }

        /// <summary>
        /// 通过字符码获取邀请码
        /// </summary>
        /// <param name="code">code</param>
        /// <returns></returns>
        public static InviteCodeInfo GetInviteCodeByCode(string code)
        {
            return Data.Invitation.GetInviteCodeByCode(code);
        }

        /// <summary>
        /// 删除邀请码
        /// </summary>
        /// <param name="inviteid">邀请码id</param>
        public static void DeleteInviteCode(int inviteId)
        {
            Data.Invitation.DeleteInviteCode(inviteId);
        }

        /// <summary>
        /// 更新邀请码的成功注册次数
        /// </summary>
        /// <param name="inviteid">邀请码id</param>
        public static void UpdateInviteCodeSuccessCount(int inviteId)
        {
            Data.Invitation.UpdateInviteCodeSuccessCount(inviteId);
        }

        /// <summary>
        /// 验证邀请码是否可用
        /// </summary>
        /// <param name="inviteCode">邀请码信息</param>
        /// <returns></returns>
        public static bool CheckInviteCode(InviteCodeInfo inviteCode)
        {
            GeneralConfigInfo configInfo = GeneralConfigs.GetConfig();
            if (inviteCode == null)
                return false;

            if (inviteCode.InviteType != configInfo.Regstatus)
                return false;

            if (inviteCode.CreateTime != inviteCode.ExpireTime)  //如果创建时间等于失效时间，则表示该邀请码永不失效
                if (Utils.StrDateDiffHours(inviteCode.ExpireTime, 0) > 0)
                    return false;

            //如果邀请类型为邀请链接式，且全局邀请设置中链接最大使用次数不是0（无数次），则校验该邀请链接的次数合法性
            //否则如果邀请码的最大使用次数不是0，则校验该邀请码的次数合法性
            int maxCount = inviteCode.InviteType == 2 ? InvitationConfigs.GetConfig().InviteCodeMaxCount : inviteCode.MaxCount;

            if (maxCount > 0 && inviteCode.SuccessCount >= maxCount)
                return false;

            return true;
        }

        /// <summary>
        /// 将邀请码信息兑换为用户积分
        /// </summary>
        /// <param name="inviteCode">邀请码信息</param>
        /// <param name="addScoreLine"></param>
        public static void ConvertInviteCodeToCredits(InviteCodeInfo inviteCode, int inviteCodePayCount)
        {
            int mount = inviteCode.SuccessCount - inviteCodePayCount;
            if (mount > -1)//如果邀请码使用次数超过了加分线，则做加分操作
            {
                UserCredits.UpdateUserCreditsByInvite(inviteCode.CreatorId, inviteCode.SuccessCount);
            }
        }

        /// <summary>
        /// 获取用户拥有的邀请码个数(封闭式)
        /// </summary>
        /// <param name="creatorid">用户id</param>
        /// <returns></returns>
        public static int GetUserInviteCodeCount(int creatorId)
        {
            if (creatorId > 0)
            {
                return Data.Invitation.GetUserInviteCodeCount(creatorId);
            }
            return 0;
        }

        /// <summary>
        /// 获取用户邀请码列表(封闭式)
        /// </summary>
        /// <param name="creatorid">用户id</param>
        /// <param name="pageindex">页码</param>
        /// <returns></returns>
        public static Common.Generic.List<InviteCodeInfo> GetUserInviteCodeList(int creatorId, int pageIndex)
        {
            if (creatorId > 0)
            {
                if (pageIndex == 0)
                    pageIndex = 1;
                return Data.Invitation.GetUserInviteCodeList(creatorId, pageIndex);
            }
            return null;
        }

        /// <summary>
        /// 获取用户当日申请的邀请码个数
        /// </summary>
        /// <param name="creatorId">用户id</param>
        /// <returns></returns>
        public static int GetTodayUserCreatedInviteCode(int creatorId)
        {
            if (creatorId > 0)
                return Data.Invitation.GetTodayUserCreatedInviteCode(creatorId);
            return -1;
        }

        /// <summary>
        /// 清除用户已过期的邀请码(封闭式)
        /// </summary>
        /// <param name="creatorId">用户id</param>
        /// <returns></returns>
        public static int ClearExpireInviteCode()
        {
            return Data.Invitation.ClearExpireInviteCode();
        }
    }
}
