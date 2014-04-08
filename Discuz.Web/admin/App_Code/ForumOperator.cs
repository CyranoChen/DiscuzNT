using System;
using System.Collections.Generic;
using System.Text;
using Discuz.Cache;

namespace Discuz.Web.Admin
{
    public class ForumOperator
    {
        /// <summary>
        /// 刷新论坛缓存信息
        /// </summary>
        public static void RefreshForumCache()
        {
            DNTCache.GetCacheService().RemoveObject("/Forum/DropdownOptions");
            DNTCache.GetCacheService().RemoveObject("/Forum/ForumList");
            DNTCache.GetCacheService().RemoveObject("/Forum/ForumListMenuDiv");
        }
    }
}
