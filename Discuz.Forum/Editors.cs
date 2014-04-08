using System;
using System.Text;
using System.Text.RegularExpressions;

using Discuz.Common;
using Discuz.Data;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Entity;

namespace Discuz.Forum
{
    /// <summary>
    /// 编辑器操作类
    /// </summary>
    public class Editors
    {
        public static Regex[] regexCustomTag = null;

        static Editors()
        {
            InitRegexCustomTag();
        }

        /// <summary>
        /// 初始化自定义标签正则对象数组
        /// </summary>
        public static void InitRegexCustomTag()
        {
            CustomEditorButtonInfo[] tagList = Editors.GetCustomEditButtonListWithInfo();
            if (tagList != null)
            {
                ResetRegexCustomTag(tagList);
            }
        }

        /// <summary>
        /// 重新加载并初始化自定义标签正则对象数组
        /// </summary>
        /// <param name="smiliesList">自定义标签对象数组</param>
        public static void ResetRegexCustomTag(CustomEditorButtonInfo[] tagList)
        {
            int tagCount = tagList.Length;

            // 如果数目不同则重新创建数组, 以免发生数组越界
            if (regexCustomTag == null || tagCount != regexCustomTag.Length)
                regexCustomTag = new Regex[tagCount];

            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < tagCount; i++)
            {
                if (builder.Length > 0)
                    builder.Remove(0, builder.Length);

                builder.Append(@"(\[");
                builder.Append(tagList[i].Tag);
                if (tagList[i].Params > 1)
                {
                    builder.Append("=");
                    for (int j = 2; j <= tagList[i].Params; j++)
                    {
                        builder.Append(@"(.*?)");
                        if (j < tagList[i].Params)
                            builder.Append(","); 
                    }
                }

                builder.Append(@"\])([\s\S]+?)\[\/");
                builder.Append(tagList[i].Tag);
                builder.Append(@"\]");

                regexCustomTag[i] = new Regex(builder.ToString(), RegexOptions.IgnoreCase);
            }
        }


        /// <summary>
        /// 以CustomEditorButtonInfo数组形式返回自定义按钮
        /// </summary>
        /// <returns></returns>
        public static CustomEditorButtonInfo[] GetCustomEditButtonListWithInfo()
        {
            Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();
            CustomEditorButtonInfo[] buttonArray = cache.RetrieveObject("/Forum/UI/CustomEditButtonInfo") as CustomEditorButtonInfo[];
            if (buttonArray == null)
            {
                buttonArray = Discuz.Data.Editors.GetCustomEditButtonListWithInfo();
                cache.AddObject("/Forum/UI/CustomEditButtonInfo", buttonArray);

                // 表情缓存重新加载时重新初始化表情正则对象数组
                ResetRegexCustomTag(buttonArray);
            }
            return buttonArray;
        }
    }
}
