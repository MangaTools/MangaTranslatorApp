using System;
using System.Collections.Generic;
using System.Drawing;

namespace MangaTL.Core
{
    [Serializable]
    public class Page
    {
        public List<TextBubble> Bubbles;
        public Bitmap TranslateImage;

        public Page(string translateImagePath)
        {
            TranslateImage = new Bitmap(translateImagePath);
            Bubbles = new List<TextBubble>();
        }

        public TextBubble CreateBubble(Rectangle rect)
        {
            var bubble = new TextBubble
            {
                Rect = rect,
                TextContent = ""
            };
            Bubbles.Add(bubble);

            return bubble;
        }
    }
}