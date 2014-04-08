using System;
using System.Text;
using System.Data;

namespace Discuz.Forum
{
    public class BanWords
    {
        /// <summary>
        /// 获得要过滤或转换的词条
        /// </summary>
        /// <returns></returns>
        public static DataTable GetBanWordList()
        {
            return Discuz.Data.BanWords.GetBanWordList();
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
            return (id > 0 && find != replacement) ? Data.BanWords.UpdateBanWord(id, find, replacement) : 0;
        }

        /// <summary>
        /// 删除过滤词条
        /// </summary>
        /// <param name="idList">过滤词条Id列表</param>
        /// <returns></returns>
        public static int DeleteBanWords(string idList)
        {
            if (Discuz.Common.Utils.IsNumericList(idList))
            {
                int result = Data.BanWords.DeleteBanWords(idList);
                Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/BanWordList");
                return result;
            }
            else
                return 0;
        }

        /// <summary>
        /// 检测滤词条是否存在
        /// </summary>
        /// <param name="banWord"></param>
        /// <returns></returns>
        public static bool IsExistBanWord(string banWord)
        {
            banWord = banWord.Trim('|');
            foreach (DataRow dr in GetBanWordList().Rows)
            {
                if (dr["find"].ToString().Trim() == banWord)
                    return true;
            }
            return false;
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
            if (find != replacement)
            {
                Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/BanWordList");
                return Data.BanWords.CreateBanWord(adminUserName, find.Trim('|'), replacement);
            }
            else
                return 0;
        }

        /// <summary>
        /// 更新过滤词
        /// </summary>
        /// <param name="find">要替换的词</param>
        /// <param name="replacement">被替换的词</param>
        public static void UpdateBadWords(string find, string replacement)
        {
            if (find != replacement)
                Data.BanWords.UpdateBadWords(find.Trim('|'), replacement);
        }

        /// <summary>
        /// 转换过滤词中的正则字符
        /// </summary>
        /// <param name="originalCode"></param>
        /// <returns></returns>
        public static string ConvertRegexCode(string originalCode)
        {
            //string[] code = { "\\", ".", "+", "*", "?", "[", "^", "]", "$", "(", ")", "{", "}", "=", "!", "<", ">", "|", ":" }; //该正则字符数组集要求"\\"必须在数组的第一位,否则会出问题
            string[] code = { "\\", "+", "*", "?", "[", "^", "]", "$", "(", ")", "=", "!", "<", ">", "|", ":" };//该正则字符数组集要求"\\"必须在数组的第一位,否则会出问题。由于过滤字符中有对｛｝.的支持，所以不过滤该三个字符
            foreach (string c in code)
            {
                originalCode = originalCode.Replace(c, '\\' + c);
            }
            return originalCode;
        }
    }
}
