using System.Text;

using Discuz.Common;
using Discuz.Config;
using Discuz.Cache;
using Discuz.Entity;

namespace Discuz.Forum
{
    /// <summary>
    /// Feed操作类
    /// </summary>
    public class Feeds
    {     
        /// <summary>
        /// 获得论坛最新的20个主题的Rss描述
        /// </summary>
        /// <param name="ttl">TTL数值</param>
        /// <returns>Rss描述</returns>
        public static string GetRssXml(int ttl)
        {
            DNTCache cache = DNTCache.GetCacheService();
            string rssContent = cache.RetrieveObject("/Forum/RSS/Index") as string;

            if (rssContent == null)
            {
                UserGroupInfo guestinfo = UserGroups.GetUserGroupInfo(7);
                StringBuilder forumlist = new StringBuilder();//允许访问的板块Id列表

                foreach (ForumInfo f in Forums.GetForumList())
                {
                    //当前版块允许rss订阅
                    if (f.Allowrss == 1)
                    {
                        //板块有权限设置信息时
                        if (!Utils.StrIsNullOrEmpty(f.Viewperm))
                        {
                            if (Utils.InArray("7", f.Viewperm, ","))
                                forumlist.AppendFormat(",{0}", f.Fid);
                        }
                        else
                        {
                            if (guestinfo.Allowvisit == 1)
                                forumlist.AppendFormat(",{0}", f.Fid);
                        }
                    }

                    //if (f.Allowrss == 0)
                    //    forumlist.AppendFormat(",{0}", f.Fid);
                    //else
                    //{
                    //    //板块权限设置为空，按照用户组权限走，RSS仅检查游客权限
                    //    if (Utils.StrIsNullOrEmpty(f.Viewperm))
                    //    {
                    //        if (guestinfo.Allowvisit == 0)
                    //            forumlist.AppendFormat(",{0}", f.Fid);
                    //    }
                    //    else
                    //    {
                    //        if (!Utils.InArray("7", f.Viewperm, ","))
                    //            forumlist.AppendFormat(",{0}", f.Fid);
                    //    }
                    //}
                }
                rssContent = Discuz.Data.Feeds.BuildRssOutput(ttl, forumlist.ToString().Trim(','), "");
                cache.AddObject("/Forum/RSS/Index", rssContent, ttl * 60);
            }
            return rssContent;
        }

        /// <summary>
        /// 获得指定版块最新的20个主题的Rss描述
        /// </summary>
        /// <param name="ttl">TTL数值</param>
        /// <param name="fid">版块id</param>
        /// <returns>Rss描述</returns>
        public static string GetForumRssXml(int ttl, int fid)
        {
            DNTCache cache = DNTCache.GetCacheService();
            string rssContent = cache.RetrieveObject("/Forum/RSS/Forum" + fid) as string;

            if (rssContent == null)
            {
                ForumInfo forum = Forums.GetForumInfo(fid);
                if (forum == null)
                    return "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n<Rss>Specified forum not found</Rss>\r\n";

                //板块d有权限设置，按照用户组权限走，RSS仅检查游客权限
                if (!Utils.StrIsNullOrEmpty(forum.Viewperm) && !Utils.InArray("7", forum.Viewperm, ","))
                    return "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n<Rss>Guest Denied</Rss>\r\n";
                else if (UserGroups.GetUserGroupInfo(7).Allowvisit == 0)
                    return "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n<Rss>Guest Denied</Rss>\r\n";

                rssContent = Discuz.Data.Feeds.BuildRssOutput(ttl, fid.ToString(), forum.Name);
                cache.AddObject("/Forum/RSS/Forum" + fid, rssContent, ttl * 60);
            }
            return rssContent;
        }


        /// <summary>
        /// 获得百度论坛收录协议xml
        /// </summary>
        /// <param name="ttl">TTL数值</param>
        /// <returns></returns>
        public static string GetBaiduSitemap(int ttl)
        {
            DNTCache cache = DNTCache.GetCacheService();
            string sitemap = cache.RetrieveObject("/Forum/Sitemap/Baidu") as string;

            if (sitemap == null)
            {
                UserGroupInfo guestinfo = UserGroups.GetUserGroupInfo(7);
                StringBuilder sbforumlist = new StringBuilder();//不允许游客访问的板块Id列表                

                foreach (ForumInfo f in Forums.GetForumList())
                {
                    if (f.Allowrss == 0)
                        sbforumlist.AppendFormat(",{0}", f.Fid);
                    else
                    {
                        //板块权限设置为空，按照用户组权限走，RSS仅检查游客权限
                        //if (Utils.StrIsNullOrEmpty(f.Viewperm) && guestinfo.Allowvisit == 0)
                        //    sbforumlist.AppendFormat(",{0}", f.Fid);
                        //else
                        //{
                        //    if (!Utils.InArray("7", f.Viewperm, ","))
                        //        sbforumlist.AppendFormat(",{0}", f.Fid);
                        //}
                        if (string.IsNullOrEmpty(f.Viewperm))
                        {
                            if (guestinfo.Allowvisit == 0)
                                sbforumlist.AppendFormat(",{0}", f.Fid);
                        }
                        else if (!Utils.InArray("7", f.Viewperm, ","))
                        {
                            sbforumlist.AppendFormat(",{0}", f.Fid);
                        }
                    }
                }
                sbforumlist = sbforumlist.Length > 0 ? sbforumlist.Remove(0, 1) : sbforumlist;
                sitemap = Discuz.Data.Feeds.GetBaiduSitemap(sbforumlist.ToString(), Users.GetShortUserInfo(BaseConfigs.GetFounderUid));

                cache.AddObject("/Forum/Sitemap/Baidu", sitemap, ttl * 60);
            }
            return sitemap;
        }
    }
}