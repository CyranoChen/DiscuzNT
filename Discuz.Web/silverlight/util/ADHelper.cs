using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Xml;
using System.Linq;
using System.Xml.Schema;
namespace Discuz.Silverlight
{
    public class adimage
    {
        public System.Collections.Generic.List<string> Files = new System.Collections.Generic.List<string>();
        public int Interval;
    }
    public struct frame
    {
        public bool ShowFrame;
    }
    public struct storyboard
    {
        public int Action;
    }
    public struct background
    {
        public string Uri;
        public int action;
        public int interval;
    }
    public sealed class ADHelper
    {
        public adimage Adimage
        {
            get { return _image; }
        }
        public frame Frame
        {
            get { return _frame; }
        }
        public storyboard Story
        {
            get { return _story; }
        }
        public background Back
        {
            get { return _back; }
        }
        public ADHelper(string source)
        {
            string temp = source.Replace(" ", "").ToLower();

            string spattern = "";
            string epattern = "";
            string result = "";
            int start = 0;
            int end = 0;
            while (temp.IndexOf(spattern) >= 0)
            {
                spattern = "<file>";
                epattern = "</file>";
                start = temp.IndexOf(spattern) + spattern.Length;
                end = temp.IndexOf(epattern);
                result = temp.Substring(start, end - start).Trim();
                _image.Files.Add(result);
                temp = temp.Substring(end + epattern.Length);
            }

            spattern = "<imageinterval>";
            epattern = "</imageinterval>";
            start = temp.IndexOf(spattern) + spattern.Length;
            end = temp.IndexOf(epattern);
            result = temp.Substring(start, end - start).Trim();
            _image.Interval = int.Parse(result);


            spattern = "<showframe>";
            epattern = "</showframe>";
            start = temp.IndexOf(spattern) + spattern.Length;
            end = temp.IndexOf(epattern);
            result = temp.Substring(start, end - start).Trim();
            _frame.ShowFrame = result.ToLower().Equals("true") ? true : false;

            spattern = "<action>";
            epattern = "</action>";
            start = temp.IndexOf(spattern) + spattern.Length;
            end = temp.IndexOf(epattern);
            result = temp.Substring(start, end - start).Trim();
            _story.Action = int.Parse(result);

            spattern = "<uri>";
            epattern = "</uri>";
            start = temp.IndexOf(spattern) + spattern.Length;
            end = temp.IndexOf(epattern);
            result = temp.Substring(start, end - start).Trim();
            _back.Uri = result;

            spattern = "<backaction>";
            epattern = "</backaction>";
            start = temp.IndexOf(spattern) + spattern.Length;
            end = temp.IndexOf(epattern);
            result = temp.Substring(start, end - start).Trim();
            _back.action = int.Parse(result);

            spattern = "<interval>";
            epattern = "</interval>";
            start = temp.IndexOf(spattern) + spattern.Length;
            end = temp.IndexOf(epattern);
            result = temp.Substring(start, end - start).Trim();
            _back.interval = int.Parse(result);

            Console.WriteLine(start);
            Console.WriteLine(end);
            Console.WriteLine(result);
            Console.ReadLine();
            //temp.Substring(spattern + spattern.Length);

        }
        adimage _image = new adimage();
        frame _frame = new frame();
        storyboard _story = new storyboard();
        background _back = new background();

    }
}
