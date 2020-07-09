using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

using MangaTL.Core.Algorithms;

namespace MangaTL.Core
{
    [Serializable]
    public class Page
    {
        public List<TextBubble> Bubbles;
        public Bitmap CleanedImage;
        public Bitmap TranslateImage;

        public Page(string cleanedImagePath, string translateImagePath)
        {
            CleanedImage = new Bitmap(cleanedImagePath);
            TranslateImage = new Bitmap(translateImagePath);
            Bubbles = new List<TextBubble>();
        }

        public TextBubble CreateBubble(TextStyle style, Point position, int threshold)
        {
            threshold /= 2;
            var rectangle = GetRectangle(position, threshold);
            var bubble = new TextBubble
            {
                Position = new Point(rectangle.X, rectangle.Y),
                Rotation = 0,
                Shape = new Shapes.Rectangle(rectangle.Width, rectangle.Height),
                Style = style,
                TextContent = "TestTestTestTestTestTestTest"
            };
            Bubbles.Add(bubble);

            return bubble;
        }

        private Rectangle GetRectangle(Point position, int threshold)
        {
            var imagePoints =
                GetPoints(position, threshold, out var minX, out var maxX, out var minY, out var maxY);
            var matrix = new bool[maxY - minY + 1, maxX - minX + 1];

            for (var i = 0; i < imagePoints.Count; i++)
            {
                var p = imagePoints[i];
                matrix[p.Y - minY, p.X - minX] = true;
            }
            var rect = RectangleFinder.FindRectangle(matrix);

            return new Rectangle(rect.X + minX, rect.Y + minY, rect.Width, rect.Height);
        }

        private List<Point> GetPoints(Point position,
            int threshold,
            out int minX,
            out int maxX,
            out int minY,
            out int maxY)
        {
            var result = new List<Point>();
            var visited = new HashSet<Point>();
            var opened = new Stack<Point>();
            opened.Push(position);

            var rect = new Rectangle(0, 0, CleanedImage.Width, CleanedImage.Height);
            var bmpData = CleanedImage.LockBits(rect, ImageLockMode.ReadWrite, CleanedImage.PixelFormat);
            var ptr = bmpData.Scan0;

            var bitsPerPixel = Image.GetPixelFormatSize(CleanedImage.PixelFormat) / 8;

            var bytes = Math.Abs(bmpData.Stride) * CleanedImage.Height;
            var rgbValues = new byte[bytes];
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            var rightValue = new[]
            {
                rgbValues[position.Y * bmpData.Stride + position.X * bitsPerPixel + 2],
                rgbValues[position.Y * bmpData.Stride + position.X * bitsPerPixel + 1],
                rgbValues[position.Y * bmpData.Stride + position.X * bitsPerPixel]
            };
            minX = position.X;
            maxX = position.X;
            minY = position.Y;
            maxY = position.Y;
            var neighbors = new Point[4];
            var pointValue = new byte[3];

            while (opened.Count != 0)
            {
                var current = opened.Pop();
                visited.Add(current);

                var basicIndex = current.Y * bmpData.Stride + current.X * bitsPerPixel;
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

                for (var i = 0; i < neighbors.Length; i++)
                    if (!visited.Contains(neighbors[i]) &&
                        !opened.Contains(neighbors[i]) &&
                        neighbors[i].X >= 0 &&
                        neighbors[i].X < CleanedImage.Width &&
                        neighbors[i].Y >= 0 &&
                        neighbors[i].Y < CleanedImage.Height)
                        opened.Push(neighbors[i]);
            }

            CleanedImage.UnlockBits(bmpData);

            return result;
        }
    }
}