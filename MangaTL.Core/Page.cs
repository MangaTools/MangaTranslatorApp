using System;
using System.Collections.Generic;
using System.Drawing;

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
                TextContent = "Test"
            };
            Bubbles.Add(bubble);

            return bubble;
        }

        private Rectangle GetRectangle(Point position, int threshold)
        {
            var points = GetPoints(position, threshold, out var minX, out var maxX, out var minY, out var maxY);
            var matrix = new bool[maxY - minY + 1, maxX - minX + 1];

            foreach (var point in points)
                matrix[point.Y - minY, point.X - minX] = true;

            return RectangleFinder.FindRectangle(matrix);
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
            var val = CleanedImage.GetPixel(position.X, position.Y);
            minX = position.X;
            maxX = position.X;
            minY = position.Y;
            maxY = position.Y;

            while (opened.Count != 0)
            {
                var current = opened.Pop();
                visited.Add(current);
                var value = CleanedImage.GetPixel(current.X, current.Y);
                if (Math.Abs(val.R - value.R) >= threshold)
                    continue;

                result.Add(current);
                if (current.X < minX)
                    minX = current.X;
                if (current.X > maxY)
                    maxY = current.X;
                if (current.Y < minY)
                    minY = current.Y;
                if (current.Y > maxY)
                    maxY = current.Y;

                for (var x = -1; x < 2; x++)
                    for (var y = -1; y < 2; y++)
                        if (Math.Abs(x + y) == 1)
                        {
                            var newPoint = new Point(current.X + x, current.Y + y);
                            if (!visited.Contains(newPoint) &&
                                !opened.Contains(newPoint) &&
                                newPoint.X >= 0 &&
                                newPoint.X < CleanedImage.Width &&
                                newPoint.Y >= 0 &&
                                newPoint.Y < CleanedImage.Height)
                            {
                            }

                            opened.Push(newPoint);
                        }
            }

            return result;
        }
    }
}