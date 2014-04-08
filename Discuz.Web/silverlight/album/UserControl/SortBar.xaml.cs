using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace DiscuzAlbum.UserControl
{
    public class SortBar : Control
    {
        public SortBar()
        {
            System.IO.Stream s = this.GetType().Assembly.GetManifestResourceStream("DiscuzAlbum.UserControl.SortBar.xaml");
            this.InitializeFromXaml(new System.IO.StreamReader(s).ReadToEnd());
        }
    }
}
