using System;
using System.Text;
using System.Data;

namespace Discuz.Data
{
    public class ForumLinks
    {
        /// <summary>
        /// 添加友情链接
        /// </summary>
        /// <param name="displayOrder">序号</param>
        /// <param name="name">链接名称</param>
        /// <param name="url">链接地址</param>
        /// <param name="note">注释</param>
        /// <param name="logo">图片地址</param>
        /// <returns></returns>
        public static int CreateForumLink(int displayOrder, string name, string url, string note, string logo)
        {
            return DatabaseProvider.GetInstance().AddForumLink(displayOrder, name, url, note, logo);
        }

        /// <summary>
        /// 获取全部链接
        /// </summary>
        /// <returns></returns>
        public static DataTable GetForumLinks()
        {
            return DatabaseProvider.GetInstance().GetForumLinks();
        }

        /// <summary>
        /// 更新友情链接
        /// </summary>
        /// <param name="displayOrder">序号</param>
        /// <param name="name">链接名称</param>
        /// <param name="url">链接地址</param>
        /// <param name="note">注释</param>
        /// <param name="logo">图片地址</param>
        /// <returns></returns>
        public static  int UpdateForumLink(int id, int displayorder, string name, string url, string note, string logo)
        {
            return DatabaseProvider.GetInstance().UpdateForumLink(id, displayorder, name, url, note, logo);
        }

        /// <summary>
        /// 删除友情链接
        /// </summary>
        /// <param name="forumlinkidlist">链接ID列表</param>
        /// <returns></returns>
        public static int DeleteForumLink(string forumlinkidlist)
        {
            return DatabaseProvider.GetInstance().DeleteForumLink(forumlinkidlist);
        }
    }
}
