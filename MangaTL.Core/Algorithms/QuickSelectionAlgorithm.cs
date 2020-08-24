using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace MangaTL.Core.Algorithms
{
    internal static class QuickSelectionAlgorithm
    {
        private static (List<Point> Points, Rectangle Bounds) GetPoints(Bitmap bitmap, Point startPoint, int threshold)
        {
            var result = new List<Point>();
            var visited = new HashSet<Point>();
            var opened = new Stack<Point>();
            opened.Push(startPoint);
            var width = bitmap.Width;
            var height = bitmap.Height;

            byte[] rgbValues;
            int horizontalLineLength;
            byte bytesPerPixel;

            lock (bitmap)
            {
                var rect = new Rectangle(0, 0, width, height);
                var bmpData = bitmap.LockBits(rect, ImageLockMode.WriteOnly, bitmap.PixelFormat);

                bytesPerPixel = (byte) (Image.GetPixelFormatSize(bitmap.PixelFormat) / 8);
                horizontalLineLength = bmpData.Stride;

                var bytes = Math.Abs(horizontalLineLength) * height;
                rgbValues = new byte[bytes];
                Marshal.Copy(bmpData.Scan0, rgbValues, 0, bytes);
                bitmap.UnlockBits(bmpData);
            }


            var basicIndex = startPoint.Y * horizontalLineLength + startPoint.X * bytesPerPixel;
            var rightValue = new[]
            {
                rgbValues[basicIndex + 2],
                rgbValues[basicIndex + 1],
                rgbValues[basicIndex]
            };

            var minX = startPoint.X;
            var maxX = startPoint.X;
            var minY = startPoint.Y;
            var maxY = startPoint.Y;
            var neighbors = new Point[4];
            var pointValue = new byte[3];

            while (opened.Count != 0)
            {
                var current = opened.Pop();
                visited.Add(current);
                basicIndex = current.Y * horizontalLineLength + current.X * bytesPerPixel;

                pointValue[0] = rgbValues[basicIndex + 2];
                pointValue[1] = rgbValues[basicIndex + 1];
                pointValue[2] = rgbValues[basicIndex];

                if (Math.Abs(rightValue[0] - pointValue[0]) >= threshold)
                    continue;

                result.Add(current);
                if (current.X < minX)
                    minX = current.X;
                if (current.X > maxX)
                    maxX = current.X;
                if (current.Y < minY)
                    minY = current.Y;
                if (current.Y > maxY)
                    maxY = current.Y;


                neighbors[0] = new Point(current.X + 1, current.Y);
                neighbors[1] = new Point(current.X, current.Y + 1);
                neighbors[2] = new Point(current.X - 1, current.Y);
                neighbors[3] = new Point(current.X, current.Y - 1);

                foreach (var t in neighbors)
                    if (!visited.Contains(t) &&
                        !opened.Contains(t) &&
                        t.X >= 0 &&
                        t.X < width &&
                        t.Y >= 0 &&
                        t.Y < height)
                        opened.Push(t);
            }

            return (result, new Rectangle(minX, minY, maxX - minX, maxY - minY));
        }

        private static Task<(List<Point> Points, Rectangle Bounds)> GetPointsAsync(Bitmap bitmap,
            Point startPoint,
            int threshold)
        {
            return Task.Run(() => GetPoints(bitmap, startPoint, threshold));
        }

        internal static async Task<Rectangle> GetRectangle(Bitmap bitmap, Point position, int threshold)
        {
            var (imagePoints, bounds) = await GetPointsAsync(bitmap, position, threshold);

            var matrix = new bool[bounds.Height + 1, bounds.Width + 1];

            foreach (var p in imagePoints)
                matrix[p.Y - bounds.Y, p.X - bounds.X] = true;
            var rect = RectangleFinder.FindRectangle(matrix);

            return new Rectangle(rect.X + bounds.X, rect.Y + bounds.Y, rect.Width, rect.Height);
        }
    }
}