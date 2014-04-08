using System;
using System.IO;
using System.Text;
using System.Data;

using Discuz.Common;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Common.Generic;
using Discuz.Data;
using Discuz.Forum.ScheduledEvents;

namespace Discuz.Forum
{
    public class Tags
    {
        /// <summary>
        /// 获取标签信息(不存在返回null)
        /// </summary>
        /// <param name="tagid">标签id</param>
        /// <returns></returns>
        public static TagInfo GetTagInfo(int tagid)
        {
            return tagid > 0 ? Discuz.Data.Tags.GetTagInfo(tagid) : null;
        }

        /// <summary>
        /// 写入标签缓存文件
        /// </summary>
        /// <param name="filename">文件绝对路径(mappath之后的)</param>
        /// <param name="tags">标签集合</param>
        /// <param name="jsonp_callback">jsonp的回调函数名, 如不使用, 请传入string.Empty</param>
        /// <param name="outputcountfield">是否输出计数统计字段</param>
        public static void WriteTagsCacheFile(string filename, List<TagInfo> tags, string jsonp_callback, bool outputcountfield)
        {
            if (tags.Count > 0)
            {
                if (!Directory.Exists(Path.GetDirectoryName(filename)))
                    Directory.CreateDirectory(Path.GetDirectoryName(filename));

                StringBuilder builder = new StringBuilder();

                if (!Utils.StrIsNullOrEmpty(jsonp_callback))
                {
                    builder.Append(jsonp_callback);
                    builder.Append("(");
                }

                builder.Append("[\r\n  ");
                foreach (TagInfo tag in tags)
                {
                    if (outputcountfield)
                        builder.Append(string.Format("{{'tagid' : '{0}', 'tagname' : '{1}', 'fcount' : '{2}', 'pcount' : '{3}', 'scount' : '{4}', 'vcount' : '{5}', 'gcount' : '{6}'}}, ",
                                tag.Tagid, tag.Tagname, tag.Fcount, tag.Pcount, tag.Scount, tag.Vcount, tag.Gcount));
                    else
                            builder.Append(string.Format("{{'tagid' : '{0}', 'tagname' : '{1}'}}, ", tag.Tagid, tag.Tagname));
                }

                builder.Append("\r\n]");
                if (!Utils.StrIsNullOrEmpty(jsonp_callback))
                    builder.Append(")");

                try
                {
                    using (FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                    {
                        Byte[] info = System.Text.Encoding.UTF8.GetBytes(builder.ToString());
                        fs.Write(info, 0, info.Length);
                        fs.Close();
                    }
                }
                catch
                { }
            }
        }

        /// <summary>
        /// 更新TAG
        /// </summary>
        /// <param name="tagid">标签ID</param>
        /// <param name="orderid">排序,-1为不可用</param>
        /// <param name="color">颜色</param>
        /// <returns></returns>
        public static bool UpdateForumTags(int tagid, int orderid, string color)
        {
            System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex("^#?([0-9|A-F]){6}$");
            if (color != "" && !r.IsMatch(color))
                return false;
            Data.Tags.UpdateForumTags(tagid, orderid, color.Replace("#", ""));
            return true;
        }

        /// <summary>
        /// 返回论坛Tag列表
        /// </summary>
        /// <param name="tagname">查询关键字</param>
        /// <param name="type">全部0 锁定1 开放2</param>
        /// <returns></returns>
        public static DataTable GetForumTags(string tagName, int type)
        {
            return Discuz.Data.Tags.GetForumTags(tagName, type);
        }
    }
}
