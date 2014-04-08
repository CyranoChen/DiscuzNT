﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Browser;
namespace Discuz.Silverlight
{
    public sealed class MediaDownloaderHelper : BaseDownloader
    {
        public MediaDownloaderHelper(MediaElement element, string urlmedia):base(urlmedia)
        {
            this.element = element;
            //Downloader downloader = new Downloader();
            //downloader.Completed += new EventHandler(download_Completed);
            //downloader.DownloadFailed += new System.Windows.ErrorEventHandler(this.downloader_DownloadFailed);
            //downloader.Open("GET", new Uri(HtmlPage.DocumentUri, urlimage), true);
            //downloader.Send();
        }
        protected override void download_Completed(object sender, EventArgs e)
        {
            Downloader downloader = sender as Downloader;
            element.SetSource(downloader, null);
            base.download_Completed(sender, e);
        }

        protected override void downloader_DownloadFailed(object sender, System.Windows.ErrorEventArgs e)
        {
            base.downloader_DownloadFailed(sender, e);
        }
       
        MediaElement element;

    }
}
