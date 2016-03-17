using System;
using System.Text;
using System.Data;

using Discuz.Entity;
using Discuz.Common;
using Discuz.Common.Generic;

namespace Discuz.Data
{
    public class PrivateMessages
    {
        /// <summary>
        /// 获得指定ID的短消息的内容
        /// </summary>
        /// <param name="pmid">短消息pmid</param>
        /// <returns>短消息内容</returns>
        public static PrivateMessageInfo GetPrivateMessageInfo(int pmid)
        {
            PrivateMessageInfo privatemessageinfo = null;
            IDataReader reader = DatabaseProvider.GetInstance().GetPrivateMessageInfo(pmid);

            if (reader.Read())
                privatemessageinfo = LoadSinglePrivateMessage(reader);

            reader.Close();
            return privatemessageinfo;
        }

        /// <summary>
        /// 得到当用户的短消息数量
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="folder">所属文件夹(0:收件箱,1:发件箱,2:草稿箱)</param>
        /// <param name="state">短消息状态(0:已读短消息、1:未读短消息、2:最近消息（7天内）、-1:全部短消息)</param>
        /// <returns>短消息数量</returns>
        public static int GetPrivateMessageCount(int userId, int folder, int state)
        {
            return DatabaseProvider.GetInstance().GetPrivateMessageCount(userId, folder, state);
        }

        /// <summary>
        /// 得到公共消息数量
        /// </summary>
        /// <returns>公共消息数量</returns>
        public static int GetAnnouncePrivateMessageCount()
        {
            return DatabaseProvider.GetInstance().GetAnnouncePrivateMessageCount();
        }

    
        /// <summary>
        /// 创建短消息
        /// </summary>
        /// <param name="privatemessageinfo">短消息内容</param>
        /// <param name="savetosentbox">设置短消息是否在发件箱保留(0为不保留, 1为保留)</param>
        /// <returns>短消息在数据库中的pmid</returns>
        public static int CreatePrivateMessage(PrivateMessageInfo privatemessageinfo, int savetosentbox)
        {
            int pmid = DatabaseProvider.GetInstance().CreatePrivateMessage(privatemessageinfo, savetosentbox);

            if (Users.appDBCache && Users.IUserService != null)
            {
                UserInfo userInfo = Users.IUserService.GetUserInfo(privatemessageinfo.Msgtoid);
                if (userInfo != null)
                {
                    userInfo.Newpmcount = userInfo.Newpmcount + 1;
                    userInfo.Newpm = 1;
                    Users.IUserService.UpdateUser(userInfo);
                }
            }
            return pmid;
        }

        /// <summary>
        /// 删除指定用户的短信息
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="pmitemidList">要删除的短信息列表</param>
        /// <returns>删除记录数</returns>
        public static int DeletePrivateMessages(int userId, string pmitemidList)
        {
            return DatabaseProvider.GetInstance().DeletePrivateMessages(userId, pmitemidList);
        }

        /// <summary>
        /// 获取新短消息数
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public static int GetNewPMCount(int userId)
        {
            return DatabaseProvider.GetInstance().GetNewPMCount(userId);
        }

        /// <summary>
        /// 设置短信息状态
        /// </summary>
        /// <param name="pmid">短信息ID</param>
        /// <param name="state">状态值</param>
        /// <returns>更新记录数</returns>
        public static int SetPrivateMessageState(int pmid, byte state)
        {
            return DatabaseProvider.GetInstance().SetPrivateMessageState(pmid, state);
        }

        /// <summary>
        /// 获得指定用户的短信息列表
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="folder">短信息类型(0:收件箱,1:发件箱,2:草稿箱)</param>
        /// <param name="pagesize">每页显示短信息数</param>
        /// <param name="pageindex">当前要显示的页数</param>
        /// <param name="strwhere">筛选条件1为未读</param>
        /// <returns>短信息列表</returns>
        public static List<PrivateMessageInfo> GetPrivateMessageCollection(int userId, int folder, int pagesize, int pageindex, int readStatus)
        {
            List<PrivateMessageInfo> coll = new List<PrivateMessageInfo>();
            IDataReader reader = DatabaseProvider.GetInstance().GetPrivateMessageList(userId, folder, pagesize, pageindex, readStatus);
            if (reader != null)
            {
                while (reader.Read())
                {
                    coll.Add(LoadSinglePrivateMessage(reader));
                }
                reader.Close();
            }
            return coll;
        }

        /// <summary>
        /// 获得公共消息列表
        /// </summary>
        /// <param name="pagesize">每页显示短信息数</param>
        /// <param name="pageindex">当前要显示的页数</param>
        /// <returns>公共消息列表</returns>
        public static List<PrivateMessageInfo> GetAnnouncePrivateMessageCollection(int pagesize, int pageindex)
        {
            List<PrivateMessageInfo> coll = new List<PrivateMessageInfo>();
            IDataReader reader = DatabaseProvider.GetInstance().GetAnnouncePrivateMessageList(pagesize, pageindex);
            if (reader != null)
            {
                while (reader.Read())
                {
                    coll.Add(LoadSinglePrivateMessage(reader));
                }
                reader.Close();
            }
            return coll;
        }

        /// <summary>
        /// 更新短信发送和接收者的用户名
        /// </summary>
        /// <param name="uid">Uid</param>
        /// <param name="newUserName">新用户名</param>
        public static void UpdatePMSenderAndReceiver(int uid, string newUserName)
        {
            DatabaseProvider.GetInstance().UpdatePMSenderAndReceiver(uid, newUserName);
        }

        /// <summary>
        /// 加载单个短消息对象
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private static PrivateMessageInfo LoadSinglePrivateMessage(IDataReader reader)
        {
            PrivateMessageInfo info = new PrivateMessageInfo();
            info.Pmid = TypeConverter.StrToInt(reader["pmid"].ToString());
            info.Msgfrom = reader["msgfrom"].ToString();
            info.Msgfromid = TypeConverter.StrToInt(reader["msgfromid"].ToString());
            info.Msgto = reader["msgto"].ToString();
            info.Msgtoid = TypeConverter.StrToInt(reader["msgtoid"].ToString());
            info.Folder = TypeConverter.StrToInt(reader["folder"].ToString());
            info.New = TypeConverter.StrToInt(reader["new"].ToString());
            info.Subject = reader["subject"].ToString();
            info.Postdatetime = reader["postdatetime"].ToString();
            info.Message = reader["message"].ToString();
            return info;
        }

        /// <summary>
        /// 删除短消息
        /// </summary>
        /// <param name="isNew">是否删除新短消息</param>
        /// <param name="postDateTime">发送日期</param>
        /// <param name="msgFromList">发送者列表</param>
        /// <param name="lowerUpper">是否区分大小写</param>
        /// <param name="subject">主题</param>
        /// <param name="message">内容</param>
        /// <param name="isUpdateUserNewPm">是否更新用户短消息数</param>
        /// <returns></returns>
        public static string DeletePrivateMessages(bool isNew, string postDateTime, string msgFromList, bool lowerUpper, string subject,
            string message, bool isUpdateUserNewPm)
        {
            return DatabaseProvider.GetInstance().DeletePrivateMessages(isNew, postDateTime, msgFromList, lowerUpper,
                subject, message, isUpdateUserNewPm).ToString();
        }

    }
}
