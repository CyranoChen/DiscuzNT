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
    /// �༭��������
    /// </summary>
    public class Editors
    {
        public static Regex[] regexCustomTag = null;

        static Editors()
        {
            InitRegexCustomTag();
        }

        /// <summary>
        /// ��ʼ���Զ����ǩ�����������
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
        /// ���¼��ز���ʼ���Զ����ǩ�����������
        /// </summary>
        /// <param name="smiliesList">�Զ����ǩ��������</param>
        public static void ResetRegexCustomTag(CustomEditorButtonInfo[] tagList)
        {
            int tagCount = tagList.Length;

            // �����Ŀ��ͬ�����´�������, ���ⷢ������Խ��
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
        /// ��CustomEditorButtonInfo������ʽ�����Զ��尴ť
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

                // ���黺�����¼���ʱ���³�ʼ�����������������
                ResetRegexCustomTag(buttonArray);
            }
            return buttonArray;
        }
    }
}
