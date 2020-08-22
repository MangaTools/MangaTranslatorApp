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

        public int GetBubbleIndex(TextBubble bubble)
        {
            return bubbles.IndexOf(bubble);
        }

        public void ChangeBubbleOrder(TextBubble bubble, bool up)
        {
            var index = GetBubbleIndex(bubble);
            if (up && index + 1 == bubbles.Count || !up && index == 0)
                return;
            bubbles.RemoveAt(index);
            var newIndex = index + (up ? 1 : -1);
            bubbles.Insert(newIndex, bubble);
        }

        private void OnBubbleChanged()
        {
            PageChanged?.Invoke();
        }

        public event Action PageChanged;
    }
}