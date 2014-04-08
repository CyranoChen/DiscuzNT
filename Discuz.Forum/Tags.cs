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
        /// ��ȡ��ǩ��Ϣ(�����ڷ���null)
        /// </summary>
        /// <param name="tagid">��ǩid</param>
        /// <returns></returns>
        public static TagInfo GetTagInfo(int tagid)
        {
            return tagid > 0 ? Discuz.Data.Tags.GetTagInfo(tagid) : null;
        }

        /// <summary>
        /// д���ǩ�����ļ�
        /// </summary>
        /// <param name="filename">�ļ�����·��(mappath֮���)</param>
        /// <param name="tags">��ǩ����</param>
        /// <param name="jsonp_callback">jsonp�Ļص�������, �粻ʹ��, �봫��string.Empty</param>
        /// <param name="outputcountfield">�Ƿ��������ͳ���ֶ�</param>
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
        /// ����TAG
        /// </summary>
        /// <param name="tagid">��ǩID</param>
        /// <param name="orderid">����,-1Ϊ������</param>
        /// <param name="color">��ɫ</param>
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
        /// ������̳Tag�б�
        /// </summary>
        /// <param name="tagname">��ѯ�ؼ���</param>
        /// <param name="type">ȫ��0 ����1 ����2</param>
        /// <returns></returns>
        public static DataTable GetForumTags(string tagName, int type)
        {
            return Discuz.Data.Tags.GetForumTags(tagName, type);
        }
    }
}
