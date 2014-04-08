using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Browser;
using System.IO;
using System.Net;
using System.Xml;
using System.Windows.Interop;
using System.Windows.Browser.Net;

namespace Discuz.Silverlight
{
    public partial class Ad : Canvas
    {
        public int Action
        {
            set
            {
                action = value;
                switch (action)
                {
                    case 0:
                        this.tickerAnim.Begin();
                        this.newsAnim.Begin();
                        break;
                    case 1:
                    case 2:
                        break;
                    default:
                        this.tickerAnim.Begin();
                        this.newsAnim.Begin();
                        break;
                }
            }
        }
        bool screenOpen = true;
        int action = 1;
        bool finish = false;
        //MediaDownloaderHelper _image;
        //BrowserHttpWebRequest _request;
        ADMediaHelper _helper;
        ADRemote ad = null;
        public void Page_Loaded(object o, EventArgs e)
        {
            try
            {
                InitializeComponent();
                ProgressAnim.Begin();
                ad = new ADRemote();
                ad.OnCompleted += new ADCompleted(OnCompleted);
            }
            catch (Exception ex)
            {
                string msg = ex.ToString();
            }
            //this.msg.Text = ad.Caption;
        }
        void OnCompleted(object sender, ADMediaHelper helper)
        {
            this._helper = helper;
            this.init();
        }
        void newsAnim_Completed(object sender, EventArgs e)
        {
            finish = true;
        }
        void Completed(object sender, EventArgs e)
        {
            finish = true;
        }
        void onMouseAction(object sender, MouseEventArgs e)
        {
            showscreen();
        }
        void showscreen()
        {
            if (screenOpen)
            {
                finish = false;
                switch (action)
                {
                    case 0:
                        this.openPip.Begin();
                        break;
                    case 1:
                        this.openAnim.Begin();
                        break;
                    case 2:
                        this.openAnim2.Begin();
                        break;
                    default:
                        this.openPip.Begin();
                        break;
                }
                screenOpen = false;
            }
            else if (!screenOpen)
            {
                switch (action )
                {
                    case 0:
                        this.closePip.Begin();
                        break;
                    case 1:
                        this.closeAnim.Begin();
                        break;
                    case 2:
                        this.closeAnim2.Begin();
                        break;
                    default:
                        this.closePip.Begin();
                        break;
                }
                screenOpen = true;
            }
        }

        
        void init()
        {
            new ImageDownloaderHelper(this.speedbackground, _helper.Speed.BackgroundUri);
            new ImageDownloaderHelper(this.speedphoto, _helper.Speed.PhotoUri);
            new ImageDownloaderHelper(this.speedtext, _helper.Speed.TextUri);
            new ImageDownloaderHelper(this.image, _helper.Play.ImageUri);
            new MediaDownloaderHelper(this.media, _helper.Play.MediaUri);
            
            this.Action = _helper.Play.PlayModel;

            TextBlock _text = null;
            Image _image = null;
            int _left = 0;
            for (int i = 0; i < _helper.Screen.Titles.Count; i += 2)
            {


                _text = new TextBlock();
                _text.FontFamily = "Arial";
                _text.FontSize = 12;
                _text.TextWrapping = TextWrapping.Wrap;
                _text.Foreground = new SolidColorBrush(Color.FromArgb(0xff, 0xca, 0xca, 0xca));
                _text.Width = _helper.Screen.Titles[i].Width;
                _text.Height = _helper.Screen.Titles[i].Height;
                _text.Text = _helper.Screen.Titles[i].TextImage;
                //_text.SetValue(Canvas.LeftProperty, _left);
                _image = new Image();

                _image.Width = _helper.Screen.Titles[i + 1].Width;
                _image.Height = _helper.Screen.Titles[i + 1].Height;
                _image.Stretch = Stretch.Fill;
                //_left += _helper.Screen.Titles[i].Width;
                //_image.SetValue(Canvas.LeftProperty, _left);

                _image.SetValue(Canvas.LeftProperty, _left);
                _left += _helper.Screen.Titles[i + 1].Width;
                _text.SetValue(Canvas.LeftProperty, _left);
                _left += _helper.Screen.Titles[i].Width;
                new ImageDownloaderHelper(_image, _helper.Screen.Titles[i + 1].TextImage);
                this.news.Children.Add(_image);
                this.news.Children.Add(_text);

            }
            this.step.Value = -(_left + 568);
            flash();
        }

        void flash()
        {
            this.ProgressAnim.Stop();
            flashCanvas.SetValue<int>(Canvas.ZIndexProperty, -1);
        }
    }
}
