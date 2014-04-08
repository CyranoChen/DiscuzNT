using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Browser;

namespace Discuz.Silverlight
{
    public class DownloaderEventArgs : EventArgs
    {
        public int Action
        {
            get
            {
                return action;
            }
        }
        public DownloaderEventArgs(int action)
            : base()
        {
            this.action = action;
        }
        int action = 0;
    }
    public delegate void DownloaderEventHandle(object sender,DownloaderEventArgs args);
    public abstract class BaseDownloader
    {
        public static int ACTION = 0;
        public BaseDownloader(string url)
        {
            Downloader downloader = new Downloader();
            downloader.Completed += new EventHandler(download_Completed);
            downloader.DownloadFailed += new System.Windows.ErrorEventHandler(this.downloader_DownloadFailed);
            downloader.Open("GET", new Uri(HtmlPage.DocumentUri, url));
            downloader.Send();
        }
        protected virtual void download_Completed(object sender, EventArgs e)
        {            
            completed = true;
            if (OnDownloaderEventHandle != null)
                OnDownloaderEventHandle(this, new DownloaderEventArgs(ACTION++));
        }
        protected virtual void downloader_DownloadFailed(object sender, System.Windows.ErrorEventArgs e)
        {
            caption = e.ErrorMessage;
        }
        public bool Completed
        {
            get
            {
                return completed;
            }
        }
        public string Information
        {
            get
            {
                return caption;
            }
        }
        public event DownloaderEventHandle OnDownloaderEventHandle;
        string caption = string.Empty;
        bool completed = false;
    }
}
