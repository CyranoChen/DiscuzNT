using System;
using System.Text;
using Discuz.Forum.ScheduledEvents;
using Discuz.Plugin.Space;
using Discuz.Plugin.Album;
using Discuz.Plugin.Mall;
using Discuz.Forum;

namespace Discuz.Event
{
    /// <summary>
    /// 有关标签的计划任务
    /// </summary>
    public class TagsEvent : IEvent
    {
        #region IEvent 成员

        public void Execute(object state)
        {
            SpacePluginBase spb = SpacePluginProvider.GetInstance();

            AlbumPluginBase apb = AlbumPluginProvider.GetInstance();

            ForumTags.WriteHotTagsListForForumCacheFile(60);
            ForumTags.WriteHotTagsListForForumJSONPCacheFile(60);
            if (spb != null)
                spb.WriteHotTagsListForSpaceJSONPCacheFile(60);
            if (apb != null)
                apb.WriteHotTagsListForPhotoJSONPCacheFile(60);

            MallPluginBase imp = MallPluginProvider.GetInstance();
            if (imp != null)
            {
                imp.WriteHotTagsListForGoodsJSONPCacheFile(60);
            }
        }

        #endregion
    }
}
