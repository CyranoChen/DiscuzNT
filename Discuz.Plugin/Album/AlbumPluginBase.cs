using System;
using System.Collections;
using Discuz.Common.Generic;
using System.Text;
using Discuz.Entity;
using System.IO;
using System.Data;

namespace Discuz.Plugin.Album
{
    public abstract class AlbumPluginBase : PluginBase
    {
        public abstract string PHOTO_HOT_TAG_CACHE_FILENAME
        {
            get;
        }

        public abstract void WritePhotoTagsCacheFile(int photoid);

        public abstract void WriteHotTagsListForPhotoJSONPCacheFile(int count);

        public abstract AlbumInfo GetAlbumInfo(int albumid);

        public abstract void CreateAlbumJsonData(int albumid);

        public abstract string GetAlbumJsonData(int albumid);

        public abstract int GetPhotoCountWithSameTag(int tagid);

        public abstract List<PhotoInfo> GetPhotosWithSameTag(int tagid, int pageid, int tpp);

        public abstract IDataReader GetFocusPhotoList(int type, int focusphotocount, int vaildDays);

        public abstract IDataReader GetAlbumListByCondition(int type, int focusphotocount, int vaildDays);

        public abstract PhotoInfo GetPhotoEntity(IDataReader reader);

        public abstract AlbumInfo GetAlbumEntity(IDataReader reader);

        public abstract int GetPhotoSizeByUserid(int userid);

        public abstract DataTable GetSpaceAlbumByUserId(int userid);

        public abstract Discuz.Common.Generic.List<AlbumCategoryInfo> GetAlbumCategory();

        public abstract string GetThumbnailImage(string filename);
    }
}
