using System;
using System.Collections.Generic;
using System.Drawing;

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

        public TextBubble CreateBubble(Rectangle rect)
        {
            var bubble = new TextBubble
            {
                Rect = rect,
                TextContent =
                    "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed nec turpis ultricies nulla posuere consequat."
            };
            Bubbles.Add(bubble);

            return bubble;
        }
    }
}