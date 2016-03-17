using Discuz.Common;
using Discuz.Data;
using Discuz.Entity;
using Discuz.Common.Generic;

namespace Discuz.Forum
{
    /// <summary>
    /// 悬赏操作类
    /// </summary>
    public class Bonus
    {
        /// <summary>
        /// 结束悬赏并给分
        /// </summary>
        /// <param name="topicinfo">主题信息</param>
        /// <param name="userid">当前执行此操作的用户Id</param>
        /// <param name="postIdArray">帖子Id数组</param>
        /// <param name="winerIdArray">获奖者Id数组</param>
        /// <param name="winnerNameArray">获奖者的用户名数组</param>
        /// <param name="costBonusArray">奖励积分数组</param>
        /// <param name="valuableAnswerArray">有价值答案的pid数组</param>
        /// <param name="bestAnswer">最佳答案的pid</param>
        public static void CloseBonus(TopicInfo topicinfo, int userid, int[] postIdArray, int[] winerIdArray, string[] winnerNameArray, string[] costBonusArray, string[] valuableAnswerArray, int bestAnswer)
        {
            int isbest = 0, bonus = 0;

            topicinfo.Special = 3;//标示为悬赏主题
            Topics.UpdateTopic(topicinfo);//更新标志位为已结帖状态

            //开始给分和记录
            for (int i = 0; i < winerIdArray.Length; i++)
            {
                bonus = TypeConverter.StrToInt(costBonusArray[i]);
                if (winerIdArray[i] > 0 && bonus > 0)
                    Users.UpdateUserExtCredits(winerIdArray[i], Scoresets.GetBonusCreditsTrans(), bonus);

                if (Utils.InArray(postIdArray[i].ToString(), valuableAnswerArray))
                    isbest = 1;

                if (postIdArray[i] == bestAnswer)
                    isbest = 2;

                Discuz.Data.Bonus.AddLog(topicinfo.Tid, topicinfo.Posterid, winerIdArray[i], winnerNameArray[i], postIdArray[i], bonus, Scoresets.GetBonusCreditsTrans(), isbest);
            }
        }

        /// <summary>
        /// 获取指定主题的给分记录
        /// </summary>
        /// <param name="tid">主题ID</param>
        /// <returns>悬赏日志集合</returns>
        public static List<BonusLogInfo> GetLogs(TopicInfo topic)
        {
            //已给分的悬赏帖
            return (topic.Tid > 0 && topic.Special == 3) ? Discuz.Data.Bonus.GetLogs(topic.Tid, Posts.GetPostTableId(topic.Tid)) : null;
        }
    }
}
