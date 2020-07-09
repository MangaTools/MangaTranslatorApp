using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

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

        public async Task<TextBubble> CreateBubble(TextStyle style, Point position, int threshold)
        {
            threshold /= 2;
            var rectangle = await QuickSelectionAlgorithm.GetRectangle(CleanedImage, position, threshold);
            var bubble = new TextBubble
            {
                Position = new Point(rectangle.X, rectangle.Y),
                Rotation = 0,
                Shape = new Shapes.Rectangle(rectangle.Width, rectangle.Height),
                Style = style,
                TextContent = "Test\nTest\nTest\nTest\nTest\nTest\nTest"
            };
            Bubbles.Add(bubble);

            return bubble;
        }
    }
}