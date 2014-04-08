using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web;
using Shell32;

namespace Discuz.Plugin.Preview.Mp3
{
    public class Mp3Message
    {
        private string title, artist, album, pubyear, track, genre, size, type, bps, url;
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
        ///// <summary>
        ///// 文件地址
        ///// </summary>
        //public string Url
        //{
        //    get { return url; }
        //}
        public Mp3Message(string FilePath)
        {
            //url = GetRootURI()+FilePath.Remove(0,FilePath.IndexOf("\\upload\\")).Replace("\\","/");
            Shell32.Shell shell = new Shell32.ShellClass();
            //set the namespace to file path
            Shell32.Folder folder = shell.NameSpace(FilePath.Substring(0, FilePath.LastIndexOf("\\")));
            //get ahandle to the file
            Shell32.FolderItem folderItem = folder.ParseName(FilePath.Substring(FilePath.LastIndexOf("\\") + 1));
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
        public static string GetRootURI()
        {
            string AppPath = "";
            HttpContext HttpCurrent = HttpContext.Current;
            HttpRequest Req;
            if (HttpCurrent != null)
            {
                Req = HttpCurrent.Request;

                string UrlAuthority = Req.Url.GetLeftPart(UriPartial.Authority);
                if (Req.ApplicationPath == null || Req.ApplicationPath == "/")
                    //直接安装在   Web   站点   
                    AppPath = UrlAuthority;
                else
                    //安装在虚拟子目录下   
                    AppPath = UrlAuthority + Req.ApplicationPath;
            }
            return AppPath;
        }

    }
}
//readonly string[] GENRE = {"Blues","Classic Rock","Country","Dance","Disco","Funk","Grunge","Hip-Hop",
//    "Jazz","Metal","New Age","Oldies","Other", "Pop", "R&B", "Rap", "Reggae", "Rock", "Techno", 
//    "Industrial", "Alternative","Ska","Death Metal","Pranks","Soundtrack","Euro-Techno","Ambient","Trip-Hop",
//     "Vocal","Jazz+Funk","Fusion","Trance","Classical","Instrumental","Acid","House","Game","Sound Clip",
//     "Gospel","Noise","AlternRock","Bass","Soul","Punk","Space","Meditative","Instrumental Pop","Instrumental Rock",
//     "Ethnic","Gothic","Darkwave","Techno-Industrial","Electronic","Pop-Folk","Eurodance","Dream","Southern Rock",
//     "Comedy","Cult","Gangsta","Top 40","Christian Rap","Pop/Funk","Jungle","Native American","Cabaret","New Wave",
//     "Psychadelic","Rave","Showtunes","Trailer","Lo-Fi","Tribal","Acid Punk","Acid Jazz","Polka","Retro","Musical",
//     "Rock & Roll","Hard Rock"}; 原MP3流派信息
//byte[] tagBody = new byte[128];
//string tagFlag;

//if (!File.Exists(FilePath))
//    return;

////读取MP3文件的最后128个字节的内容
//using (FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
//{
//    fs.Seek(-128, SeekOrigin.End);
//    fs.Read(tagBody, 0, 128);
//    fs.Close();
//}

////取TAG段的前三个字节
//tagFlag = Encoding.Default.GetString(tagBody, 0, 3);

////如果没有TAG信息，则直接返回
//if (!"TAG".Equals(tagFlag, StringComparison.InvariantCultureIgnoreCase))
//{
//    return;
//    //throw new InvalidDataException("指定的MP3文件没有TAG信息！");
//}

////按照MP3 ID3 V1 的tag定义，依次读取相关的信息
//this.title = Encoding.Default.GetString(tagBody, 3, 30).TrimEnd();
//this.artist = Encoding.Default.GetString(tagBody, 33, 30).TrimEnd();
//this.album = Encoding.Default.GetString(tagBody, 62, 30).TrimEnd();
//this.pubyear = Encoding.Default.GetString(tagBody, 93, 4).TrimEnd();
//this.comment = Encoding.Default.GetString(tagBody, 97, 30);
//Int16 g = (Int16)tagBody[127];
//this.genre = (g >= GENRE.Length ? "未知" : GENRE[g]);