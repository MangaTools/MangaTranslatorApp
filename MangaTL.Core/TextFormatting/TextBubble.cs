using System;
using System.Drawing;

namespace MangaTL.Core
{
    [Serializable]
    public class TextBubble
    {
        private Rectangle rect;
        private string textContent;

        public Rectangle Rect
        {
            get => rect;
            set
            {
                if (rect == value)
                    return;
                rect = value;
                BubbleChanged?.Invoke();
            }
        }
        public string TextContent
        {
            get => textContent;
            set
            {
                if(textContent == value)
                    return;
                textContent = value;
                BubbleChanged?.Invoke();
            }
        }

        public event Action BubbleChanged;
    }
}