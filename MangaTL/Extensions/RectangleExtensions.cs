using System.Drawing;
using System.Runtime.CompilerServices;
using Point = System.Windows.Point;

namespace MangaTL
{
    public static class RectangleExtensions
    {
        public static bool Intersect(this Rectangle rect, Point point)
        {
            var usualPoint = new System.Drawing.Point((int)point.X, (int)point.Y);
            return rect.X <= usualPoint.X &&
                   rect.Right >= usualPoint.X &&
                   rect.Y <= usualPoint.Y &&
                   rect.Bottom >= usualPoint.Y;
        }
    }
}