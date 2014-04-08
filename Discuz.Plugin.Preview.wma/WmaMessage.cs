using System;
using System.Collections.Generic;
using System.Text;
using Shell32;

namespace Discuz.Plugin.Preview.Wma
{
    public class WmaMessage
    {
        private string title, artist, album, pubyear, track, genre, size, type, bps;
        /// <summary>
        /// 文件标题
        /// </summary>
        public string Title
        {
            get { return title; }
        }
        /// <summary>
        /// 艺术家
        /// </summary>
        public string Artist
        {
            get { return artist; }
        }
        /// <summary>
        /// 专辑
        /// </summary>
        public string Album
        {
            get { return album; }
        }
        /// <summary>
        /// 发行年份
        /// </summary>
        public string PublishYear
        {
            get { return pubyear; }
        }
        /// <summary>
        /// 声道
        /// </summary>
        public string Track
        {
            get { return track; }
        }
        /// <summary>
        /// 流派
        /// </summary>
        public string Genre
        {
            get { return genre; }
        }
        /// <summary>
        /// 文件大小
        /// </summary>
        public string Size
        {
            get { return size; }
        }
        /// <summary>
        /// 文件类型
        /// </summary>
        public string Type
        {
            get { return type; }
        }
        /// <summary>
        /// 比特率
        /// </summary>
        public string BPS
        {
            get { return bps; }
        }
        public WmaMessage(string path)
        {
            //create shell instance
            Shell32.Shell shell = new Shell32.ShellClass();
            //set the namespace to file path
            Shell32.Folder folder = shell.NameSpace(path.Substring(0, path.LastIndexOf("\\")));
            //get ahandle to the file
            Shell32.FolderItem folderItem = folder.ParseName(path.Substring(path.LastIndexOf("\\") + 1));
            //did we get a handle ?

            title = folder.GetDetailsOf(folderItem, 9);
            artist = folder.GetDetailsOf(folderItem, 16);
            album = folder.GetDetailsOf(folderItem, 10);
            pubyear = folder.GetDetailsOf(folderItem, 18);
            genre = folder.GetDetailsOf(folderItem, 12);
            size = folder.GetDetailsOf(folderItem, 1);
            type = folder.GetDetailsOf(folderItem, 2);
            bps = folder.GetDetailsOf(folderItem, 22);
            track = folder.GetDetailsOf(folderItem, 34);
        }

    }
}
