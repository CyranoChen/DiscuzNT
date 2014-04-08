using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Discuz.Silverlight
{
    public struct Speed
    {
        public string BackgroundUri;
        public string PhotoUri;
        public string TextUri;
    }
    public class Screen
    {
        public System.Collections.Generic.List<Title> Titles = new System.Collections.Generic.List<Title>();
    }
    public class Title
    {
        public int Width;
        public int Height;
        public String TextImage;
    }
    public struct Play
    {
        public int PlayModel;
        public string ImageUri;
        public string MediaUri;
    }
    public sealed class ADMediaHelper
    {
        public Screen Screen
        {
            get { return _screen; }
        }
        public Speed Speed
        {
            get { return _speed; }
        }
        public Play Play
        {
            get { return _play; }
        }
        Screen _screen = new Screen();
        Speed _speed = new Speed();
        Play _play = new Play();

        public ADMediaHelper(string source)
        {
            string temp = source.ToLower();

            string spattern = "";
            string epattern = "";
            string result = "";
            int start = 0;
            int end = 0;


            spattern = "<playmodel>";
            epattern = "</playmodel>";
            start = temp.IndexOf(spattern) + spattern.Length;
            end = temp.IndexOf(epattern);
            result = temp.Substring(start, end - start).Trim();
            _play.PlayModel = int.Parse(result);

            spattern = "<backgrounduri>";
            epattern = "</backgrounduri>";
            start = temp.IndexOf(spattern) + spattern.Length;
            end = temp.IndexOf(epattern);
            result = temp.Substring(start, end - start).Trim();
            _play.ImageUri = result;

            spattern = "<mediauri>";
            epattern = "</mediauri>";
            start = temp.IndexOf(spattern) + spattern.Length;
            end = temp.IndexOf(epattern);
            result = temp.Substring(start, end - start).Trim();
            _play.MediaUri = result;
            {
                spattern = "<speed>";
                epattern = "</speed>";
                start = temp.IndexOf(spattern) + spattern.Length;
                end = temp.IndexOf(epattern);
                string speed = temp.Substring(start, end - start).Trim();
                spattern = "";

                spattern = "<speedbackground>";
                epattern = "</speedbackground>";
                start = temp.IndexOf(spattern) + spattern.Length;
                end = temp.IndexOf(epattern);
                result = temp.Substring(start, end - start).Trim();
                _speed.BackgroundUri = result;

                spattern = "<speedphoto>";
                epattern = "</speedphoto>";
                start = temp.IndexOf(spattern) + spattern.Length;
                end = temp.IndexOf(epattern);
                result = temp.Substring(start, end - start).Trim();
                _speed.PhotoUri = result;

                spattern = "<speedtext>";
                epattern = "</speedtext>";
                start = temp.IndexOf(spattern) + spattern.Length;
                end = temp.IndexOf(epattern);
                result = temp.Substring(start, end - start);
                _speed.TextUri = result;
            }
            spattern = "<screen>";
            epattern = "</screen>";
            start = temp.IndexOf(spattern) + spattern.Length;
            end = temp.IndexOf(epattern);
            string screen = temp.Substring(start, end - start).Trim();
            spattern = "";

            spattern = "<group>";
            epattern = "</group>";
            start = screen.IndexOf(spattern) + spattern.Length;
            end = screen.IndexOf(epattern);
            temp = screen.Substring(start, end - start);
            spattern = "";
            Title _title = null;
            while (temp.IndexOf(spattern) >= 0)
            {
                _title = new Title();
                spattern = "<text>";
                epattern = "</text>";
                start = temp.IndexOf(spattern) + spattern.Length;
                end = temp.IndexOf(epattern);
                result = temp.Substring(start, end - start);
                _title.TextImage = result;

                spattern = "<textwidth>";
                epattern = "</textwidth>";
                start = temp.IndexOf(spattern) + spattern.Length;
                end = temp.IndexOf(epattern);
                result = temp.Substring(start, end - start).Trim();
                _title.Width = int.Parse(result);

                spattern = "<textheight>";
                epattern = "</textheight>";
                start = temp.IndexOf(spattern) + spattern.Length;
                end = temp.IndexOf(epattern);
                result = temp.Substring(start, end - start).Trim();
                _title.Height = int.Parse(result);

                _screen.Titles.Add(_title);

                _title = new Title();

                spattern = "<image>";
                epattern = "</image>";
                start = temp.IndexOf(spattern) + spattern.Length;
                end = temp.IndexOf(epattern);
                result = temp.Substring(start, end - start).Trim();
                _title.TextImage = result;

                spattern = "<imagewidth>";
                epattern = "</imagewidth>";
                start = temp.IndexOf(spattern) + spattern.Length;
                end = temp.IndexOf(epattern);
                result = temp.Substring(start, end - start).Trim();
                _title.Width = int.Parse(result);

                spattern = "<imageheight>";
                epattern = "</imageheight>";
                start = temp.IndexOf(spattern) + spattern.Length;
                end = temp.IndexOf(epattern);
                result = temp.Substring(start, end - start).Trim();
                _title.Height = int.Parse(result);

                _screen.Titles.Add(_title);

                temp = temp.Substring(end + epattern.Length);
            }
        }
    }
}
