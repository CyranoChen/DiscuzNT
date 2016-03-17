using System;
using System.Text;
using System.Data;
using System.Data.Common;

using Discuz.Common;
using Discuz.Data;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Common.Generic;

namespace Discuz.Forum
{
    /// <summary>
    /// 投票操作类
    /// </summary>
    public class Polls
    {
        /// <summary>
        /// 创建一个投票
        /// </summary>
        /// <param name="tid">关联的主题id</param>
        /// <param name="multiple">投票类型, 0为单选, 1为多选</param>
        /// <param name="itemcount">投票项总数</param>
        /// <param name="itemnamelist">投票项目列表</param>
        /// <param name="itemvaluelist">投票项目结果列表</param>
        /// <param name="usernamelist">用户名列表</param>
        /// <param name="enddatetime">截止日期</param>
        /// <param name="userid">用户id</param>
        /// <param name="maxchoices">最多可选项数</param>
        /// <param name="visible">提交投票后结果才可见, 0为可见, 1为投票后可见</param>
        /// <param name="allowview">是否允许公开投票参与人</param>
        /// <returns>成功则返回true, 否则返回false</returns>
        public static bool CreatePoll(int tid, int multiple, int itemcount, string itemnamelist, string itemvaluelist, string enddatetime, int userid, int maxchoices, int visible, int allowview)
        {
            string[] itemname = Utils.SplitString(itemnamelist, "\r\n");

            if ((itemname.Length != itemcount) || (Utils.SplitString(itemvaluelist, "\r\n").Length != itemcount))
                return false;

            PollInfo pollinfo = new PollInfo();
            pollinfo.Displayorder = 0;
            pollinfo.Expiration = Utils.GetStandardDateTime(enddatetime);
            pollinfo.Maxchoices = maxchoices;
            pollinfo.Multiple = multiple;
            pollinfo.Tid = tid;
            pollinfo.Uid = userid;
            pollinfo.Visible = visible;
            pollinfo.Allowview = allowview;

            int pollid = Discuz.Data.Polls.CreatePoll(pollinfo);

            if (pollid > 0)
            {
                for (int i = 0; i < itemcount; i++)
                {
                    PollOptionInfo polloptioninfo = new PollOptionInfo();
                    polloptioninfo.Displayorder = i + 1;
                    polloptioninfo.Pollid = pollid;
                    polloptioninfo.Polloption = Utils.GetSubString(itemname[i], 80, "");
                    polloptioninfo.Tid = tid;
                    polloptioninfo.Voternames = "";
                    polloptioninfo.Votes = 0;
                    Discuz.Data.Polls.CreatePollOption(polloptioninfo);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 更新一个投票
        /// </summary>
        /// <param name="tid">关联的主题id</param>
        /// <param name="multiple">投票类型, 0为单选, 1为多选</param>
        /// <param name="itemcount">投票项总数</param>
        /// <param name="polloptionidlist">投票项id列表</param> 
        /// <param name="itemnamelist">投票项目列表</param>
        /// <param name="itemdisplayorderlist">投票项目排列顺序列表</param>
        /// <param name="enddatetime">截止日期</param>
        /// <param name="maxchoices">最多可选项数</param>
        /// <param name="visible">提交投票后结果才可见, 0为可见, 1为投票后可见</param>
        /// <param name="allowview">是否允许公开投票参与人</param>
        /// <returns>成功则返回true, 否则返回false</returns>
        public static bool UpdatePoll(int tid, int multiple, int itemcount, string polloptionidlist, string itemnamelist, string itemdisplayorderlist, string enddatetime, int maxchoices, int visible, int allowview)
        {
            string[] itemname = Utils.SplitString(itemnamelist, "\r\n");
            string[] itemdisplayorder = Utils.SplitString(itemdisplayorderlist, "\r\n");
            string[] polloptionid = Utils.SplitString(polloptionidlist, "\r\n");

            if ((itemname.Length != itemcount) || (itemdisplayorder.Length != itemcount))
                return false;

            PollInfo pollinfo = Discuz.Data.Polls.GetPollInfo(tid);
            pollinfo.Expiration = Utils.GetStandardDateTime(enddatetime);
            pollinfo.Maxchoices = maxchoices;
            pollinfo.Multiple = multiple;
            pollinfo.Tid = tid;
            pollinfo.Visible = visible;
            pollinfo.Allowview = allowview;

            bool result = false;
            if (pollinfo.Pollid > 0)
                result = Discuz.Data.Polls.UpdatePoll(pollinfo);

            if (result)
            {
                List<PollOptionInfo> pollOptionInfoList = Discuz.Data.Polls.GetPollOptionInfoCollection(pollinfo.Tid);
                int i = 0;

                //先作已存在的投票选项更新及新添加选项的添加操作
                bool optionexist;
                foreach (string optionid in polloptionid)
                {
                    optionexist = false;
                    foreach (PollOptionInfo polloptioninfo in pollOptionInfoList)
                    {
                        if (optionid == polloptioninfo.Polloptionid.ToString())
                        {
                            polloptioninfo.Pollid = pollinfo.Pollid;
                            polloptioninfo.Polloption = Utils.GetSubString(itemname[i], 80, "");
                            polloptioninfo.Displayorder = Utils.StrIsNullOrEmpty(itemdisplayorder[i]) ? i + 1 : Utils.StrToInt(itemdisplayorder[i], 0);
                            Discuz.Data.Polls.UpdatePollOption(polloptioninfo);
                            optionexist = true;
                            break;
                        }
                    }
                    if (!optionexist) //如果当前选项不存在，表示该选项为新添选项
                    {
                        PollOptionInfo polloptioninfo = new PollOptionInfo();
                        polloptioninfo.Displayorder = Utils.StrIsNullOrEmpty(itemdisplayorder[i]) ? i + 1 : Utils.StrToInt(itemdisplayorder[i], 0);
                        polloptioninfo.Pollid = pollinfo.Pollid;
                        polloptioninfo.Polloption = Utils.GetSubString(itemname[i], 80, "");
                        polloptioninfo.Tid = tid;
                        polloptioninfo.Voternames = "";
                        polloptioninfo.Votes = 0;
                        Discuz.Data.Polls.CreatePollOption(polloptioninfo);
                    }
                    i++;
                }

                foreach (PollOptionInfo polloptioninfo in pollOptionInfoList)
                {
                    //下面代码用于删除已去除的投票项
                    if (("\r\n" + polloptionidlist + "\r\n").IndexOf("\r\n" + polloptioninfo.Polloptionid + "\r\n") < 0)
                        Discuz.Data.Polls.DeletePollOption(polloptioninfo);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

     
      
        /// <summary>
        /// 根据投票信息更新数据库中的记录
        /// </summary>
        /// <param name="tid">主题id</param>
        /// <param name="selitemidlist">选择的投票项id列表</param>
        /// <param name="username">用户名</param>
        /// <returns>如果执行成功则返回0, 非法提交返回负值</returns>
        public static int UpdatePoll(int tid, string selitemidlist, string username)
        {
            if (Utils.StrIsNullOrEmpty(username))
                return -1;

            string[] selitemid = Utils.SplitString(selitemidlist, ",");
            foreach (string optionid in selitemid)
            {
                // 如果id结果集合中出现非数字类型字符则认为是非法
                if (Utils.StrToInt(optionid, -1) == -1)
                    return -1;
            }

            PollInfo pollinfo = Discuz.Data.Polls.GetPollInfo(tid);

            if (pollinfo.Pollid < 1)
                return -3;

            if (Utils.StrIsNullOrEmpty(pollinfo.Voternames))
                pollinfo.Voternames = username;
            else
                pollinfo.Voternames = pollinfo.Voternames + "\r\n" + username;

            Discuz.Data.Polls.UpdatePoll(pollinfo);
            List<PollOptionInfo> pollOptionInfoList = Discuz.Data.Polls.GetPollOptionInfoCollection(pollinfo.Tid);

            foreach (string optionid in selitemid)
            {
                foreach (PollOptionInfo polloptioninfo in pollOptionInfoList)
                {
                    if (optionid == polloptioninfo.Polloptionid.ToString())
                    {
                        if (Utils.StrIsNullOrEmpty(polloptioninfo.Voternames))
                            polloptioninfo.Voternames = username;
                        else
                            polloptioninfo.Voternames = polloptioninfo.Voternames + "\r\n" + username;

                        polloptioninfo.Votes += 1;
                        DatabaseProvider.GetInstance().UpdatePollOption(polloptioninfo);
                    }
                }
            }
            return 0;
        }




        /// <summary>
        /// 获得与指定主题id关联的投票数据
        /// </summary>
        /// <param name="tid">主题id</param>
        /// <returns>投票数据</returns>
        public static DataTable GetPollOptionList(int tid)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("name", System.Type.GetType("System.String"));//投票项名称
            dt.Columns.Add("value", System.Type.GetType("System.String"));//票数
            dt.Columns.Add("barid", System.Type.GetType("System.Int32"));
            dt.Columns.Add("barwidth", System.Type.GetType("System.Double"));//显示宽度
            dt.Columns.Add("percent", System.Type.GetType("System.String"));//投票百分比
            dt.Columns.Add("multiple", System.Type.GetType("System.String"));//是否多选　
            dt.Columns.Add("polloptionid", System.Type.GetType("System.Int32"));//投票项ID
            dt.Columns.Add("displayorder", System.Type.GetType("System.Int32"));//排序位置
            dt.Columns.Add("votername", System.Type.GetType("System.String"));//投票人名称
            dt.Columns.Add("percentwidth", System.Type.GetType("System.String"));//投票进度条宽度

            List<PollOptionInfo> pollOptionInfoList = Discuz.Data.Polls.GetPollOptionInfoCollection(tid);
            object[] rowVals;

            int votesum = 0 , maxVote = 0, multiple = Discuz.Data.Polls.GetPollInfo(tid).Multiple;;

            foreach (PollOptionInfo polloptioninfo in pollOptionInfoList)
            {
                votesum += polloptioninfo.Votes;
                maxVote = polloptioninfo.Votes > maxVote ? polloptioninfo.Votes : maxVote;
            }

            if (votesum == 0)
                votesum = 1;

            int i = 0;
            string votername = "";
            //组定数据项
            foreach (PollOptionInfo polloptioninfo in pollOptionInfoList)
            {
                rowVals = new object[10];
                rowVals[0] = polloptioninfo.Polloption;
                rowVals[1] = polloptioninfo.Votes;
                rowVals[2] = i % 10;
                rowVals[3] = (((double)(Utils.StrToFloat(polloptioninfo.Votes, 0) * 100 / votesum) / 100) * 200 + 3).ToString("0.00");
                rowVals[4] = ((double)(Utils.StrToFloat(polloptioninfo.Votes, 0) * 100 / votesum) / 100).ToString("0.00%");
                rowVals[5] = multiple;
                rowVals[6] = polloptioninfo.Polloptionid;
                rowVals[7] = polloptioninfo.Displayorder;

                if (!Utils.StrIsNullOrEmpty(polloptioninfo.Voternames))
                {
                    //将投票人字段的数据重新组合成链接字符串
                    foreach (string username in Utils.SplitString(polloptioninfo.Voternames, "\r\n"))
                    {
                        votername += "<a href=\"userinfo.aspx?username=" + Utils.UrlEncode(username.Trim()) + "\">" + username.Trim() + "</a> ";
                    }
                    rowVals[8] = votername;
                    votername = "";
                }
                else
                    rowVals[8] = "";

                rowVals[9] = (420*((double) (Utils.StrToFloat(polloptioninfo.Votes, 0)/maxVote))).ToString();
                dt.Rows.Add(rowVals);
                i++;
            }
            return dt;
        }



        /// <summary>
        /// 判断是否允许指定用户投票
        /// </summary>
        /// <param name="tid">主题id</param>
        /// <param name="username">用户名</param>
        /// <returns>判断结果</returns>
        public static bool AllowVote(int tid, string username)
        {
            if (Utils.StrIsNullOrEmpty(username))
                return false;

            string strUsernamelist = Discuz.Data.Polls.GetPollUserNameList(tid);

            // 如果已投票用户uid列表为"" (尚无人投票), 则立即返回true
            if (Utils.StrIsNullOrEmpty(strUsernamelist))
                return true;

            // 检查已投票用户uid列表中是否存在指定用户的uid
            foreach(string userName in Utils.SplitString(strUsernamelist.Trim(), "\r\n"))
            {
                if (userName.Trim() == username)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 获取参与投票者名单
        /// </summary>
        /// <param name="tid">投票的主题id</param>
        /// <param name="userid">当前投票用户id</param>
        /// <param name="username">当前投票用户名</param>
        /// <param name="allowvote">是否允许投票</param>
        /// <returns>参与投票者名单</returns>
        public static string GetVoters(int tid, int userid, string username, out bool allowvote)
        {
            string strUsernamelist = Discuz.Data.Polls.GetPollUserNameList(tid);
            // 如果已投票用户uid列表为"" (尚无人投票), 则立即返回true
            allowvote = true;

            if (Utils.StrIsNullOrEmpty(strUsernamelist))
                return "<li>暂无人投票</li>";

            string[] votername = Utils.SplitString(strUsernamelist.Trim(), "\r\n");
            StringBuilder sbVoters = new StringBuilder();
            for (int i = 0; i < votername.Length; i++)
            {
                if (votername[i].Trim() == username)
                    allowvote = false;

                if (userid == -1 && Utils.InArray(tid.ToString(), ForumUtils.GetCookie("dnt_polled")))
                    allowvote = false;

                if (votername[i].IndexOf(' ') == -1)
                    sbVoters.AppendFormat("<li><a href=\"userinfo.aspx?username={0}\">{1}</a></li>", Utils.UrlEncode(votername[i].Trim()), votername[i]);
                else
                    sbVoters.Append(votername[i].Substring(0, votername[i].LastIndexOf(".") + 1).Trim().Replace(" ", string.Empty) + "]");

                sbVoters.Append("&nbsp; ");
            }

            return sbVoters.ToString();
        }


        /// <summary>
        /// 得到投票帖的结束时间
        /// </summary>
        /// <param name="tid">主题id</param>
        /// <returns>结束时间</returns>
        public static string GetPollEnddatetime(int tid)
        {
            return Discuz.Data.Polls.GetPollEnddatetime(tid);
        }

        /// <summary>
        /// 通过主题ID获取相应的投票信息
        /// </summary>
        /// <param name="tid">投票主题的id</param>
        /// <returns>投票信息</returns>
        public static PollInfo GetPollInfo(int tid)
        {
            return Discuz.Data.Polls.GetPollInfo(tid);
        }


        /// <summary>
        /// 创建投票
        /// </summary>
        /// <param name="pollItemName"></param>
        /// <param name="multiple"></param>
        /// <param name="maxchoices"></param>
        /// <param name="visiblepoll"></param>
        /// <param name="enddatetime"></param>
        /// <param name="tid"></param>
        /// <param name="pollitem"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static string CreatePoll(string pollItemName, int multiple, int maxchoices, int visiblepoll, int allowview, string enddatetime, int tid, string[] pollitem, int userid)
        {
            string msg = null;
            StringBuilder itemvaluelist = new StringBuilder("");

            // 生成以回车换行符为分割的项目与结果列
            for (int i = 0; i < pollitem.Length; i++)
            {
                itemvaluelist.Append("0\r\n");
            }

            string PollItemname = Utils.HtmlEncode(pollItemName);
            if (PollItemname != "")
            {
                if (multiple <= 0)
                    multiple = 0;
                
                if (multiple == 1 && maxchoices > pollitem.Length)
                    maxchoices = pollitem.Length;

                if (!Polls.CreatePoll(tid, multiple, pollitem.Length, PollItemname.Trim(), itemvaluelist.ToString().Trim(), enddatetime, userid, maxchoices, visiblepoll, allowview))
                    msg = "投票错误";
            }
            else
                msg = "投票项为空";

            return msg;
        }
    }
}
