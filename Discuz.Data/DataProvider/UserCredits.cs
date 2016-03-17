using System;
using System.Text;
using System.Data;

using Discuz.Cache.Data;
using Discuz.Entity;

namespace Discuz.Data
{
    public class UserCredits
    {
        /// <summary>
        /// 根据积分公式更新用户积分
        /// <param name="uid">用户ID</param>
         /// </summary>
        public static void UpdateUserCredits(int uid)
        {
            DatabaseProvider.GetInstance().UpdateUserCredits(uid);

            if (Users.appDBCache)
            {
                UserInfo userInfo = null;
                IDataReader reader = DatabaseProvider.GetInstance().GetUserInfoToReader(uid);
                if (reader.Read())
                {
                    userInfo = Users.LoadSingleUserInfo(reader);
                    reader.Close();
                }
                if(userInfo != null)
                    Users.IUserService.UpdateUserProfile(userInfo);
            }
        }

        /// <summary>
        /// 更新用户扩展积分
        /// <param name="uid">用户ID</param>
        /// <param name="values">扩展积分</param>
        /// </summary>
        public static void UpdateUserExtCredits(int uid, float[] values)
        {
            DatabaseProvider.GetInstance().UpdateUserCredits(uid, values);

            if (Users.appDBCache)
            {
                UserInfo userInfo = null;
                IDataReader reader = DatabaseProvider.GetInstance().GetUserInfoToReader(uid);
                if (reader.Read())
                {
                    userInfo = Users.LoadSingleUserInfo(reader);
                    reader.Close();
                }
                if (userInfo != null)
                    Users.IUserService.UpdateUserProfile(userInfo);
            }
        }

        /// <summary>
        /// 判断扩展积分是否足够被减
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="values">扩展积分</param>
        /// <returns></returns>
        public static bool CheckUserCreditsIsEnough(int uid, float[] values)
        {
           return DatabaseProvider.GetInstance().CheckUserCreditsIsEnough(uid, values);
        }

        /// <summary>
        /// 判断扩展积分是否足够被减
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="mount">更新数量,比如由上传2个附件引发此操作,那么此参数值应为2</param>
        /// <param name="values">扩展积分</param>
        /// <param name="pos">加或减标志(正数为加,负数为减,通常被传入1或者-1)</param>
        /// <returns></returns>
        public static bool CheckUserCreditsIsEnough(int uid, float[] values, int pos, int mount)
        {
            return DatabaseProvider.GetInstance().CheckUserCreditsIsEnough(uid, values, pos, mount);
        }

        /// <summary>
        /// 更新用户扩展积分
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="values">扩展积分</param>
        /// <param name="pos">加或减标志(正数为加,负数为减,通常被传入1或者-1)</param>
        /// <param name="mount">更新数量,比如由上传2个附件引发此操作,那么此参数值应为2</param>
        /// <returns></returns>
        public static void UpdateUserExtCredits(int uid, float[] values, int pos, int mount)
        {
            DatabaseProvider.GetInstance().UpdateUserCredits(uid, values, pos, mount);

            if (Users.appDBCache)
            {
                UserInfo userInfo = null;
                IDataReader reader = DatabaseProvider.GetInstance().GetUserInfoToReader(uid);
                if (reader.Read())
                {
                    userInfo = Users.LoadSingleUserInfo(reader);
                    reader.Close();
                }
                if (userInfo != null)
                    Users.IUserService.UpdateUserProfile(userInfo);
            }
        }
    }
}
