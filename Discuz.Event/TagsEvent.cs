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
    /// �йر�ǩ�ļƻ�����
    /// </summary>
    public class TagsEvent : IEvent
    {
        #region IEvent ��Ա

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
