using System;
using System.Data;
using System.Data.Common;

using Discuz.Common;
using Discuz.Data;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Common.Generic;
using System.Text;
using System.Web;

namespace Discuz.Forum
{
    /// <summary>
    /// UserCreditsFactory 的摘要说明。
    /// </summary>
    public class UserCredits
    {
        /// <summary>
        /// 根据积分公式更新用户积分,并且受分数变动影响有可能会更改用户所属的用户组
        /// <param name="uid">用户ID</param>
        /// </summary>
        public static int UpdateUserCredits(int uid)
        {
            ShortUserInfo userInfo = uid > 0 ? Users.GetShortUserInfo(uid) : null;
            if (userInfo != null)
            {
                Discuz.Data.UserCredits.UpdateUserCredits(uid);
                UserGroupInfo tmpUserGroupInfo = UserGroups.GetUserGroupInfo(userInfo.Groupid);

                if (tmpUserGroupInfo != null && (UserGroups.IsCreditUserGroup(tmpUserGroupInfo) || tmpUserGroupInfo.Groupid == 7))//当用户组为积分用户组或者组ID为游客(ID=7)
                {
                    tmpUserGroupInfo = GetCreditsUserGroupId(userInfo.Credits);
                    if (tmpUserGroupInfo.Groupid != userInfo.Groupid)//当用户所属组发生变化时
                    {
                        Discuz.Data.Users.UpdateUserGroup(userInfo.Uid.ToString(), tmpUserGroupInfo.Groupid);
                        Discuz.Data.OnlineUsers.UpdateGroupid(userInfo.Uid, tmpUserGroupInfo.Groupid);
                    }
                }
                //判断操作用户是否是当前用户，如果是则更新dntusertips的cookie
                HttpCookie cookie = HttpContext.Current.Request.Cookies["dnt"];
                if (cookie != null)
                {
                    if (cookie["userid"] == uid.ToString())
                        ForumUtils.WriteUserCreditsCookie(userInfo, tmpUserGroupInfo.Grouptitle);
                }
                return 1;

            }
            else
                return 0;
        }


        /// <summary>
        /// 创建积分动画的COOKIE
        /// </summary>
        /// <param name="values">积分列表</param>
        public static void WriteUpdateUserExtCreditsCookies(float[] values)
        {
            StringBuilder creditsValue = new StringBuilder("");
            creditsValue.Append("0,");
            foreach (float s in values)
            {
                creditsValue.Append(s.ToString());
                creditsValue.Append(",");
            }

            HttpCookie cookie = HttpContext.Current.Request.Cookies["discuz_creditnotice"];
            if (cookie == null)
            {
                cookie = new HttpCookie("discuz_creditnotice");
            }
            cookie.Value = creditsValue.ToString().TrimEnd(',');
            cookie.Expires = DateTime.Now.AddMinutes(36000);
            cookie.Path = BaseConfigs.GetForumPath;
            HttpContext.Current.Response.AppendCookie(cookie);
            //Utils.WriteCookie("discuz_creditnotice", creditsValue.ToString().TrimEnd(','), 36000);
        }


        /// <summary>
        /// 通过指定值更新用户积分
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="values">积分变动值,应保证是一个长度为8的数组,对应8种扩展积分的变动值</param>
        /// <param name="allowMinus">是否允许被扣成负分,true允许,false不允许并且不进行扣分返回-1</param>
        /// <returns></returns>
        public static int UpdateUserExtCredits(int uid, float[] values, bool allowMinus)
        {
            if (uid < 1 || Discuz.Data.Users.GetUserInfo(uid) == null)
                return 0;

            if (values.Length < 8)
                return -1;

            if (!allowMinus)//不允许扣成负分时要进行判断积分是否足够被减
            {
                // 如果要减扩展积分, 首先判断扩展积分是否足够被减
                if (!Discuz.Data.UserCredits.CheckUserCreditsIsEnough(uid, values))
                    return -1;
            }

            Discuz.Data.UserCredits.UpdateUserExtCredits(uid, values);

            UpdateUserCredits(uid);

            //向应用同步扩展积分
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] != 0.0)
                {
                    Sync.UpdateCredits(uid, i + 1, values[i].ToString(), "");
                }
            }
            ///更新用户积分
            return 1;
        }


        /// <summary>
        /// 检查用户积分是否足够被减(适用于单用户, 单个或多个积分)
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="mount">更新数量,比如由上传2个附件引发此操作,那么此参数值应为2</param>
        /// <param name="creditsOperationType">积分操作类型,如发帖等</param>
        /// <param name="pos">加或减标志(正数为加,负数为减,通常被传入1或者-1)</param>
        /// <returns></returns>
        public static bool CheckUserCreditsIsEnough(int uid, int mount, CreditsOperationType creditsOperationType, int pos)
        {
            DataTable dt = Scoresets.GetScoreSet();

            dt.PrimaryKey = new DataColumn[1] { dt.Columns["id"] };

            float[] extCredits = new float[8];
            for (int i = 0; i < 8; i++)
            {
                extCredits[i] = TypeConverter.ObjectToFloat(dt.Rows[(int)creditsOperationType]["extcredits" + (i + 1)]);
            }

            if (pos < 0)
            {
                for (int i = 0; i < 8; i++)
                {
                    if (Utils.StrToFloat(extCredits[i], 0) < 0)//只要任何一项要求减分,就去数据库检查
                        return Discuz.Data.UserCredits.CheckUserCreditsIsEnough(uid, extCredits, pos, mount);
                }
            }
            return true;
        }


        /// <summary>
        /// 更新用户积分(适用于单用户,单个或多个操作)
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="extCredits">使用的积分规则</param>
        /// <param name="mount">更新数量,比如由上传2个附件引发此操作,那么此参数值应为2</param>
        /// <param name="creditsOperationType">积分操作类型,如发帖等</param>
        /// <param name="pos">加或减标志(正数为加,负数为减,通常被传入1或者-1)</param>
        /// <param name="allowMinus">是否允许被扣成负分,true允许,false不允许并且不进行扣分返回-1</param>
        /// <returns></returns>
        public static int UpdateUserExtCredits(int uid, float[] extCredits, int mount, CreditsOperationType creditsOperationType, int pos, bool allowMinus)
        {
            //float[] extCredits = Scoresets.GetUserExtCredits(creditsOperationType);
            float extCredit = 0;

            foreach (float e in extCredits)//此循环用于校验当前积分操作是否需要更新用户积分
            {
                if (e != 0)
                {
                    extCredit = e;
                    break;
                }
            }

            if (extCredit == 0)//如果搜索积分设置中全部为0，即不操作积分，则直接返回1
                return 1;

            if (uid == -1)//如果当前用户为游客，则直接返回-1
                return -1;

            // 如果要减扩展积分, 首先判断扩展积分是否足够被减
            if (pos < 0)
            {
                //当不是删除主题或回复时
                if (creditsOperationType != CreditsOperationType.PostTopic && creditsOperationType != CreditsOperationType.PostReply)
                {
                    if (!allowMinus && !Discuz.Data.UserCredits.CheckUserCreditsIsEnough(uid, extCredits, pos, mount))
                        return -1;
                }
            }
            else
            {
                if (creditsOperationType == CreditsOperationType.DownloadAttachment || creditsOperationType == CreditsOperationType.Search)//临时性解决用户搜索扣分可以为负数的问题，当积分系统被重新开发时，该判断代码可根据实际情况拿掉
                {
                    if (!allowMinus && !Discuz.Data.UserCredits.CheckUserCreditsIsEnough(uid, extCredits, -1, mount))
                        return -1;
                }
            }

            Discuz.Data.UserCredits.UpdateUserExtCredits(uid, extCredits, pos, mount);

            for (int i = 0; i < extCredits.Length; i++)
            {
                if (extCredits[i] != 0.0)
                {
                    Sync.UpdateCredits(uid, i + 1, extCredits[i].ToString(), "");
                }
            }

            ///更新用户积分
            return 1;
        }

        /// <summary>
        /// 更新用户积分(适用于单用户,单个或多个操作)
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="mount">更新数量,比如由上传2个附件引发此操作,那么此参数值应为2</param>
        /// <param name="creditsOperationType">积分操作类型,如发帖等</param>
        /// <param name="pos">加或减标志(正数为加,负数为减,通常被传入1或者-1)</param>
        /// <param name="allowMinus">是否允许被扣成负分,true允许,false不允许并且不进行扣分返回-1</param>
        /// <returns></returns>
        public static int UpdateUserExtCredits(int uid, int mount, CreditsOperationType creditsOperationType, int pos, bool allowMinus)
        {
            return UpdateUserExtCredits(uid, Scoresets.GetUserExtCredits(creditsOperationType), mount, creditsOperationType, pos, allowMinus);
        }

        /// <summary>
        /// 根据用户列表,一次更新多个用户的积分
        /// </summary>
        /// <param name="uidlist">用户ID列表</param>
        /// <param name="values">扩展积分值</param>
        public static int UpdateUserExtCredits(string uidlist, float[] values)
        {
            int reval = -1;
            if (Utils.IsNumericList(uidlist))
            {
                reval = 0;
                ///根据公式计算用户的总积分,并更新	
                foreach (string uid in Utils.SplitString(uidlist, ","))
                {
                    if (TypeConverter.StrToInt(uid, 0) > 0)
                        reval = reval + UpdateUserExtCredits(TypeConverter.StrToInt(uid, 0), values, true);
                }
            }
            return reval;
        }



        /// <summary>
        /// 根据用户列表,一次更新多个用户的积分
        /// </summary>
        /// <param name="uidlist">用户ID列表</param>
        /// <param name="creditsOperationType">积分操作类型,如发帖等</param>
        /// <param name="pos">加或减标志(正数为加,负数为减,通常被传入1或者-1)</param>
        public static int UpdateUserCreditsAndExtCredits(string uidlist, CreditsOperationType creditsOperationType, int pos)
        {
            int reval = -1;
            if (Utils.IsNumericList(uidlist))
            {
                reval = 0;
                ///根据公式计算用户的总积分,并更新	
                foreach (string uid in Utils.SplitString(uidlist, ","))
                {
                    if (TypeConverter.StrToInt(uid, 0) > 0)
                        reval = reval + UpdateUserExtCredits(TypeConverter.StrToInt(uid), 1, creditsOperationType, pos, false);

                    UserCredits.UpdateUserCredits(TypeConverter.StrToInt(uid));
                }
            }
            return reval;
        }


        /// <summary>
        /// 根据用户列表,一次更新多个用户的积分(此方法只在删除主题时使用过)
        /// </summary>
        /// <param name="uidlist">用户ID列表</param>
        /// <param name="creditsOperationType">积分操作类型,如发帖等</param>
        /// <param name="pos">加或减标志(正数为加,负数为减,通常被传入1或者-1)</param>
        private static int UpdateUserCredits(int[] uidlist, CreditsOperationType creditsOperationType, int pos)
        {
            ///根据公式计算用户的总积分,并更新
            int[] mountlist = new int[uidlist.Length];
            for (int i = 0; i < mountlist.Length; i++)
            {
                mountlist[i] = 1;
            }
            return UpdateUserCredits(uidlist, mountlist, creditsOperationType, pos);
        }

        /// <summary>
        /// 根据用户列表,一次更新多个用户的积分(此方法只在删除主题时使用过)
        /// </summary>
        /// <param name="uidlist">用户ID列表</param>
        /// <param name="mountlist">操作的次数，用来记录增减积分的次数；例如删帖：删一个帖子（也就是一个操作），mountlist为1;删除两个帖子，两个帖子分别都有两个附件，也就是两次操作，mountlist值就为2</param>
        /// <param name="creditsOperationType">积分操作类型,如发帖等</param>
        /// <param name="pos">加或减标志(正数为加,负数为减,通常被传入1或者-1)</param>
        public static int UpdateUserCredits(int[] uidlist, int[] mountlist, CreditsOperationType creditsOperationType, int pos)
        {
            ///根据公式计算用户的总积分,并更新
            int reval = 0;
            for (int i = 0; i < uidlist.Length; i++)
            {
                if (uidlist[i] > 0)
                    reval = reval + UpdateUserExtCredits(uidlist[i], mountlist[i], creditsOperationType, pos, true);

                UserCredits.UpdateUserCredits(uidlist[i]);
            }
            return reval;
        }


        /// <summary>
        /// 根据积分获得积分用户组所应该匹配的用户组描述 (如果没有匹配项或用户非积分用户组则返回null)
        /// </summary>
        /// <param name="Credits">积分</param>
        /// <returns>用户组描述</returns>
        public static UserGroupInfo GetCreditsUserGroupId(float Credits)
        {
            List<UserGroupInfo> usergroupinfo = UserGroups.GetUserGroupList();
            UserGroupInfo tmpitem = null;

            UserGroupInfo maxCreditGroup = null;
            foreach (UserGroupInfo infoitem in usergroupinfo)
            {
                // 积分用户组的特征是radminid等于0
                if (infoitem.Radminid == 0 && infoitem.System == 0 && (Credits >= infoitem.Creditshigher && Credits <= infoitem.Creditslower))
                {
                    if (tmpitem == null || infoitem.Creditshigher > tmpitem.Creditshigher)
                        tmpitem = infoitem;
                }
                //更新积分上线最高的用户组
                if (maxCreditGroup == null || maxCreditGroup.Creditshigher < infoitem.Creditshigher)
                    maxCreditGroup = infoitem;
            }

            if (maxCreditGroup != null && maxCreditGroup.Creditshigher < Credits)
                tmpitem = maxCreditGroup;

            return tmpitem == null ? new UserGroupInfo() : tmpitem;
        }



        /// <summary>
        /// 用户发表主题时更新用户的积分
        /// </summary>
        /// <param name="uid">用户id</param>
        public static void UpdateUserCreditsByPostTopic(int uid)
        {
            UpdateUserExtCredits(uid, 1, CreditsOperationType.PostTopic, 1, false);
            UserCredits.UpdateUserCredits(uid);
        }

        /// <summary>
        /// 用户发表主题时更新用户的积分
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="values">积分变动值,应保证是一个长度为8的数组,对应8种扩展积分的变动值</param>
        public static void UpdateUserCreditsByPostTopic(int uid, float[] values)
        {
            UpdateUserExtCredits(uid, values, false);
            UserCredits.UpdateUserCredits(uid);
        }

        /// <summary>
        /// 用户发表回复时更新用户的积分
        /// </summary>
        /// <param name="uid">用户id</param>
        public static void UpdateUserCreditsByPosts(int uid)
        {
            UpdateUserExtCredits(uid, 1, CreditsOperationType.PostReply, 1, false);
            UserCredits.UpdateUserCredits(uid);
        }

        /// <summary>
        /// 用户发表回复时更新用户的积分
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="values">积分变动值,应保证是一个长度为8的数组,对应8种扩展积分的变动值</param>
        public static void UpdateUserCreditsByPosts(int uid, float[] values)
        {
            UpdateUserExtCredits(uid, values, false);
            UserCredits.UpdateUserCredits(uid);
        }

        /// <summary>
        /// 用户发表回复时更新用户的积分
        /// </summary>
        /// <param name="uid">用户id</param>
        public static void UpdateUserCreditsByDeletePosts(int uid)
        {
            UpdateUserExtCredits(uid, 1, CreditsOperationType.DeletePost, -1, false);
            UserCredits.UpdateUserCredits(uid);
        }


        /// <summary>
        /// 用户上传附件时更新用户的积分
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="mount">上传附件数量</param>
        public static int UpdateUserExtCreditsByUploadAttachment(int uid, int mount)
        {
            if (uid > 0 && mount > 0)
                return UpdateUserExtCredits(uid, mount, CreditsOperationType.UploadAttachment, 1, false);
            else
                return 0;
        }

        /// <summary>
        /// 用户下载附件时更新用户的积分
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="mount">下载附件数量,比如下载2个附件引发此操作,那么此参数值应为2</param>
        public static int UpdateUserExtCreditsByDownloadAttachment(int uid, int mount)
        {
            if (uid > 0 && mount > 0)
                return UpdateUserExtCredits(uid, mount, CreditsOperationType.DownloadAttachment, 1, false);
            else
                return -1;
        }

        /// <summary>
        /// 用户发送短消息时更新用户的积分
        /// </summary>
        /// <param name="uid">用户id</param>
        public static int UpdateUserCreditsBySendpms(int uid)
        {
            if (uid > 0)
            {
                int result = UpdateUserExtCredits(uid, 1, CreditsOperationType.SendMessage, 1, false);
                UserCredits.UpdateUserCredits(uid);
                return result;
            }
            else
                return -1;
        }


        /// <summary>
        /// 用户搜索时更新用户的积分
        /// </summary>
        /// <param name="uid">用户id</param>
        public static int UpdateUserCreditsBySearch(int uid)
        {
            int result = UpdateUserExtCredits(uid, 1, CreditsOperationType.Search, 1, false);
            UserCredits.UpdateUserCredits(uid);
            return result;
        }



        /// <summary>
        /// 用户交易成功时更新用户的积分
        /// </summary>
        /// <param name="uid">用户id</param>
        public static int UpdateUserCreditsByTradefinished(int userid)
        {
            if (userid > 0)
            {
                UpdateUserExtCredits(userid, 1, CreditsOperationType.TradeSucceed, 1, false);
                return UserCredits.UpdateUserCredits(userid);
            }
            else
                return 0;
        }

        /// <summary>
        /// 用户参与投票时更新用户的积分
        /// </summary>
        /// <param name="uid">用户id</param>
        public static void UpdateUserCreditsByVotepoll(int userid)
        {
            if (userid > 0)
            {
                UpdateUserExtCredits(userid, 1, CreditsOperationType.Vote, 1, false);
                UpdateUserCredits(userid);
            }
        }

        /// <summary>
        /// 用户邀请注册更新用户的积分
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="mount"></param>
        public static void UpdateUserCreditsByInvite(int userid, int mount)
        {
            if (userid > 0)
            {
                UpdateUserExtCredits(userid, mount, CreditsOperationType.Invite, 1, false);
                UpdateUserCredits(userid);
            }
        }

        /// <summary>
        /// 根据用户Id重新计算用户积分
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns>用户积分</returns>
        public static int GetUserCreditsByUid(int uid)
        {
            ShortUserInfo shortUserInfo = Discuz.Data.Users.GetShortUserInfo(uid);
            if (shortUserInfo != null)
            {
                return GetUserCreditsByUserInfo(shortUserInfo);
            }
            return 0;
        }

        /// <summary>
        /// 根据用户信息重新计算用户积分
        /// </summary>
        /// <param name="shortUserInfo">用户信息</param>
        /// <returns>用户积分</returns>
        public static int GetUserCreditsByUserInfo(ShortUserInfo shortUserInfo)
        {
            string ArithmeticStr = Scoresets.GetScoreCalFormula();

            if (Utils.StrIsNullOrEmpty(ArithmeticStr))
                return 0;

            ArithmeticStr = ArithmeticStr.Replace("digestposts", shortUserInfo.Digestposts.ToString());
            ArithmeticStr = ArithmeticStr.Replace("posts", shortUserInfo.Posts.ToString());
            ArithmeticStr = ArithmeticStr.Replace("oltime", shortUserInfo.Oltime.ToString());
            ArithmeticStr = ArithmeticStr.Replace("pageviews", shortUserInfo.Pageviews.ToString());
            ArithmeticStr = ArithmeticStr.Replace("extcredits1", shortUserInfo.Extcredits1.ToString());
            ArithmeticStr = ArithmeticStr.Replace("extcredits2", shortUserInfo.Extcredits2.ToString());
            ArithmeticStr = ArithmeticStr.Replace("extcredits3", shortUserInfo.Extcredits3.ToString());
            ArithmeticStr = ArithmeticStr.Replace("extcredits4", shortUserInfo.Extcredits4.ToString());
            ArithmeticStr = ArithmeticStr.Replace("extcredits5", shortUserInfo.Extcredits5.ToString());
            ArithmeticStr = ArithmeticStr.Replace("extcredits6", shortUserInfo.Extcredits6.ToString());
            ArithmeticStr = ArithmeticStr.Replace("extcredits7", shortUserInfo.Extcredits7.ToString());
            ArithmeticStr = ArithmeticStr.Replace("extcredits8", shortUserInfo.Extcredits8.ToString());

            object expression = Arithmetic.ComputeExpression(ArithmeticStr);
            return Utils.StrToInt(Math.Floor(Utils.StrToFloat(expression, 0)), 0);
        }
    } // end class
}
