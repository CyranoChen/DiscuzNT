using System;
using System.Text;
using Discuz.Config;

namespace Discuz.Album.Config
{
    /// <summary>
    /// 相册配置信息类
    /// </summary>
    [Serializable]
    public class AlbumConfigInfo : IConfigInfo
    {
        private string _maxalbumcount;

        /// <summary>
        /// 单个用户最大允许的相册数
        /// </summary>
        public string MaxAlbumCount
        {
            get { return _maxalbumcount; }
            set { _maxalbumcount = value; }
        }
    }
}
