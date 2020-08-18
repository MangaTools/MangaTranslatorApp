using System;
using System.Collections.Generic;
using System.Drawing;

namespace MangaTL.Core
{
    [Serializable]
    public class Page
    {
        private List<TextBubble> bubbles;
        public Bitmap TranslateImage;

        public IReadOnlyList<TextBubble> Bubbles => bubbles;

        public Page(string translateImagePath)
        {
            TranslateImage = new Bitmap(translateImagePath);
            bubbles = new List<TextBubble>();
        }

        public TextBubble CreateBubble(Rectangle rect)
        {
            var bubble = new TextBubble
            {
                Rect = rect,
                TextContent = ""
            };
            bubbles.Add(bubble);
            bubble.BubbleChanged += OnBubbleChanged;

            return bubble;
        }

        public void RemoveBubble(TextBubble bubble)
        {
            if (!bubbles.Contains(bubble))
                return;

            bubbles.Remove(bubble);
            bubble.BubbleChanged -= OnBubbleChanged;
        }

        private void OnBubbleChanged()
        {
            PageChanged?.Invoke();
        }

        public event Action PageChanged;
    }
}