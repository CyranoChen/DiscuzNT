using System.Windows.Media;

namespace DiscuzAlbum
{
    /// <summary>
    /// This class provides the common static values in this app.
    /// </summary>
    public class CommonValues
    {
        public static int RandomSliceCount = 12;        // 4x3 slice matrix.
        public static int FixRows = 3;
        public static int FixCols = 4;
        public static double minRotation = -30;         // minimum rotation for random slices.
        public static double maxRotation = 30;          // maximum rotation for random slices.
        public static double FreespaceRatio = 0.8;      // recommended: 0.8 ~ 0.9
        public static double ZoomOutScale = 0.4;        // Once the holder canvas be zoomed out, it will be shrinked into this ratio.
        public static double ZoomInScale = 1;           // Once the holder canvas be zoomed in, it will be shrinked into this ratio.
        public static double SortedMargin = 90;        // Margin between sorted slices, in pixel(point by pdi).
        public static double SortBufferCount = 1;       // Buffer the left & right slices for specified count, recommened value: 1~2
        public static double GenuinePadding = 30;
    }
}
