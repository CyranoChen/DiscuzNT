using System;
#if NET1
#else
using System.Collections.Generic;
#endif

using System.Text;

using Discuz.Entity;
using Discuz.Plugin.Preview;

namespace Discuz.Plugin.Preview.Mp3
{
    public class Viewer : IPreview
    {
        private bool useFTP;
        #region IPreview 成员
        bool IPreview.UseFTP
        {
            get
            {
                return useFTP;
            }
            set
            {
                useFTP = value;
            }
        }

        public string GetPreview(string fileName, ShowtopicPageAttachmentInfo attachment)
        {
            string Result = string.Empty;
            if (!PreviewHelper.IsFileExist(fileName))
            {
                return "";
            }
            Mp3Message mp3msg = new Mp3Message(fileName);
            Result = string.Format("<div style=\"overflow:hidden; zoom:1; border:1px solid #999; font-size:13px; width:360px;\">" +
                "<dl style=\"margin:8px;\"><dt style=\"clear:both; float:left; width:80px;\">标题：</dt><dd>{1}</dd>" +
                "<dt style=\"clear:both; float:left; width:100px;\">演唱者：</dt><dd>{2}</dd>" +
                "<dt style=\"clear:both; float:left; width:100px;\">专辑：</dt><dd>{4}</dd>" +
                "<dt style=\"clear:both; float:left; width:100px;\">流派：</dt><dd>{3}</dd>" +
                "<dt style=\"clear:both; float:left; width:100px;\">文件类型：</dt><dd>{9}</dd>" +
                "<dt style=\"clear:both; float:left; width:100px;\">比特率：</dt><dd>{6}</dd>" +
                "<dt style=\"clear:both; float:left; width:100px;\">声道：</dt><dd>{8}</dd>" +
                "<dt style=\"clear:both; float:left; width:100px;\">文件大小：</dt><dd>{7}</dd>" +
                "<dt style=\"clear:both; float:left; width:100px;\">发行年份：</dt><dd>{5}</dd></dl>" +
                "<div><object classid=\"CLSID:6BF52A52-394A-11d3-B153-00C04F79FAA6\" width=\"360\" height=\"64\"><param name=\"autostart\" value=\"1\" />" +
                "<param name=\"url\" value=\"{0}\" />" +
                "<embed src=\"{0}\" autostart=\"0\"type=\"video/x-ms-wmv\" width=\"260\"height=\"42\"></embed></object></div></div>", @"upload/" + attachment.Filename, mp3msg.Title, mp3msg.Artist, mp3msg.Genre, mp3msg.Album, mp3msg.PublishYear, mp3msg.BPS, mp3msg.Size, mp3msg.Track, mp3msg.Type);

            return Result;
        }

        public void OnSaved(string fileName)
        {

        }

        #endregion
    }
}
