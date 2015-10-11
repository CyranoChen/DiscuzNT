using System;
using System.Text;
using System.Data;

using Discuz.Entity;
using Discuz.Common;
using Discuz.Common.Generic;

namespace Discuz.Data
{
    public class Polls
    {
        /// <summary>
        /// 创建一个投票
        /// </summary>
        /// <param name="pollinfo">投票信息</param>
        /// <returns></returns>
        public static int CreatePoll(PollInfo pollinfo)
        {
            return DatabaseProvider.GetInstance().CreatePoll(pollinfo);
        }

        /// <summary>
        /// 创建投票项
        /// </summary>
        /// <param name="polloptioninfocoll">投票项</param>
        /// <returns></returns>
        public static void CreatePollOption(PollOptionInfo polloptioninfo)
        {
            DatabaseProvider.GetInstance().CreatePollOption(polloptioninfo);
        }

        /// <summary>
        /// 更新投票
        /// </summary>
        /// <param name="pollinfo">更新投票</param>
        /// <returns></returns>
        public static bool UpdatePoll(PollInfo pollinfo)
        {
            return DatabaseProvider.GetInstance().UpdatePoll(pollinfo);
        }

        /// <summary>
        /// 通过主题ID获取相应的投票信息
        /// </summary>
        /// <param name="tid">主题id</param>
        /// <returns>投票选项集合</returns>
        public static List<PollOptionInfo> GetPollOptionInfoCollection(int tid)
        {
            List<PollOptionInfo> pollOptionInfoList = new List<PollOptionInfo>();
            IDataReader idatareader = DatabaseProvider.GetInstance().GetPollOptionList(tid);
            PollOptionInfo polloptioninfo;
            while (idatareader.Read())
            {
                polloptioninfo = new PollOptionInfo();
                polloptioninfo.Polloptionid = TypeConverter.ObjectToInt(idatareader["polloptionid"], 0);
                polloptioninfo.Displayorder = TypeConverter.ObjectToInt(idatareader["displayorder"], 0);
                polloptioninfo.Pollid = TypeConverter.ObjectToInt(idatareader["pollid"], 0);
                polloptioninfo.Polloption = idatareader["polloption"].ToString().Trim();
                polloptioninfo.Tid = TypeConverter.ObjectToInt(idatareader["tid"], 0);
                polloptioninfo.Voternames = idatareader["voternames"].ToString().Trim();
                polloptioninfo.Votes = TypeConverter.ObjectToInt(idatareader["votes"], 0);
                pollOptionInfoList.Add(polloptioninfo);
            }
            idatareader.Close();
            return pollOptionInfoList;
        }

         /// <summary>
        /// 更新投票项
        /// </summary>
        /// <param name="PollOptionInfo">投票项</param>
        /// <returns></returns>
        public static bool UpdatePollOption(PollOptionInfo pollInfoOption)
        {
            return DatabaseProvider.GetInstance().UpdatePollOption(pollInfoOption);
        }

        /// <summary>
        /// 删除指定的投票项
        /// </summary>
        /// <param name="pollInfoOption">投票项</param>
        /// <returns></returns>
        public static bool DeletePollOption(PollOptionInfo pollInfoOption)
        {
            return DatabaseProvider.GetInstance().DeletePollOption(pollInfoOption);
        }


        /// <summary>
        /// 通过主题ID获取相应的投票信息
        /// </summary>
        /// <param name="tid">投票主题的id</param>
        /// <returns>投票信息</returns>
        public static PollInfo GetPollInfo(int tid)
        {
            PollInfo pollinfo = new PollInfo();
            IDataReader idatareader = DatabaseProvider.GetInstance().GetPollList(tid);
            while (idatareader.Read())
            {
                pollinfo.Pollid = TypeConverter.ObjectToInt(idatareader["pollid"], 0);
                pollinfo.Displayorder = TypeConverter.ObjectToInt(idatareader["displayorder"], 0);
                pollinfo.Expiration = Utils.GetStandardDate(idatareader["expiration"].ToString());
                pollinfo.Maxchoices = TypeConverter.ObjectToInt(idatareader["maxchoices"], 0);
                pollinfo.Multiple = TypeConverter.ObjectToInt(idatareader["multiple"], 0);
                pollinfo.Tid = TypeConverter.ObjectToInt(idatareader["tid"], 0);
                pollinfo.Visible = TypeConverter.ObjectToInt(idatareader["visible"], 0);
                pollinfo.Allowview = TypeConverter.ObjectToInt(idatareader["allowview"], 0);
                pollinfo.Voternames = idatareader["voternames"].ToString().Trim();
                pollinfo.Uid = TypeConverter.ObjectToInt(idatareader["uid"], 0);
                break; //目前一个主题只有一个投票，因此在绑定了第一条投票信息后退出
            }
            idatareader.Close();
            return pollinfo;
        }

        /// <summary>
        /// 获取指定tid投票人列表
        /// </summary>
        /// <param name="tid">投票tid</param>
        /// <returns></returns>
        public static string GetPollUserNameList(int tid)
        {
            return DatabaseProvider.GetInstance().GetPollUserNameList(tid);
        }

        /// <summary>
        /// 得到投票帖的投票类型
        /// </summary>
        /// <param name="tid">主题ID</param>
        /// <returns>投票类型</returns>
        public static int GetPollType(int tid)
        {
            return DatabaseProvider.GetInstance().GetPollType(tid);
        }

        /// <summary>
        /// 得到投票帖的结束时间
        /// </summary>
        /// <param name="tid">主题id</param>
        /// <returns>结束时间</returns>
        public static string GetPollEnddatetime(int tid)
        {
            return DatabaseProvider.GetInstance().GetPollEnddatetime(tid);
        }
    }
}
