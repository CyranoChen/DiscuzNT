using System.Text.RegularExpressions;

namespace Discuz.Forum
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
            Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/ForumLinkList");

            return Data.ForumLinks.CreateForumLink(displayOrder, name, url, note, logo);
        }

        /// <summary>
        /// 获取全部链接
        /// </summary>
        /// <returns></returns>
        public static System.Data.DataTable GetForumLinks()
        {
            return Data.ForumLinks.GetForumLinks();
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
        public static int UpdateForumLink(int id, int displayorder, string name, string url, string note, string logo)
        {
            Regex r = new Regex("(http|https)://([\\w-]+\\.)+[\\w-]+(/[\\w-./?%&=]*)?");
            if (name == "" || !r.IsMatch(url.Replace("'", "''")))
            {
                return -1;
            }
            return Data.ForumLinks.UpdateForumLink(id, displayorder, name, url, note, logo);
        }


        /// <summary>
        /// 删除友情链接
        /// </summary>
        /// <param name="forumlinkidlist">链接ID列表</param>
        /// <returns></returns>
        public static int DeleteForumLink(string forumlinkidlist)
        {
            Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/ForumLinkList");

            return Data.ForumLinks.DeleteForumLink(forumlinkidlist);
        }
    }
}
