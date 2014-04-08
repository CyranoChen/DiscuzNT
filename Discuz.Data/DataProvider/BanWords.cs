using System;
using System.Text;
using System.Data;

namespace Discuz.Data
{
    public class BanWords
    {
        /// <summary>
        /// 获得要过滤或转换的词条
        /// </summary>
        /// <returns></returns>
        public static DataTable GetBanWordList()
        {
           return  DatabaseProvider.GetInstance().GetBanWordList();
        }

        /// <summary>
        /// 更新过滤词条
        /// </summary>
        /// <param name="id">词条Id</param>
        /// <param name="find">查找词</param>
        /// <param name="replacement">替换内容</param>
        /// <returns></returns>
        public static int UpdateBanWord(int id, string find, string replacement)
        {
            return DatabaseProvider.GetInstance().UpdateWord(id, find, replacement);
        }

        /// <summary>
        /// 删除过滤词条
        /// </summary>
        /// <param name="idList">过滤词条Id列表</param>
        /// <returns></returns>
        public static int DeleteBanWords(string idList)
        {
            return DatabaseProvider.GetInstance().DeleteWords(idList);
        }

        /// <summary>
        /// 创建过滤词条
        /// </summary>
        /// <param name="adminUserName">创建管理员用户名</param>
        /// <param name="find">查找词</param>
        /// <param name="replacement">替换内容</param>
        /// <returns></returns>
        public static int CreateBanWord(string adminUserName, string find, string replacement)
        {
            return DatabaseProvider.GetInstance().AddWord(adminUserName, find, replacement);
        }

        /// <summary>
        /// 更新过滤词
        /// </summary>
        /// <param name="find">要替换的词</param>
        /// <param name="replacement">被替换的词</param>
        public static void UpdateBadWords(string find, string replacement)
        {
            DatabaseProvider.GetInstance().UpdateBadWords(find, replacement);
        }
    }
}
