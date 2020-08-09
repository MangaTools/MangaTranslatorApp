using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace MangaTL.Core.Algorithms
{
    internal static class FitTextAlgorithm
    {
        public static bool TryFit(List<RectangleF> lines,
            List<Size> sizes,
            Size spaceSize,
            string[] words,
            out string result)
        {
            var newLines = new List<TextLine>();
            TextLine previous = null;
            foreach (var line in lines.Select(rectangleF => new TextLine(rectangleF.Width, previous)))
            {
                newLines.Add(line);
                previous = line;
            }

            result = null;
            var currentLine = 0;
            for (var i = 0; i < sizes.Count; i++)
            {
                var added = false;
                while (currentLine < newLines.Count)
                {
                    if (!newLines[currentLine].TryAddWord(words[i], sizes[i].Width, spaceSize.Width))
                    {
                        currentLine++;
                        continue;
                    }

                    added = true;
                    break;
                }

                if (!added)
                    return false;
            }

            result = string.Join("\n", newLines.Select(x => x.Text).Where(x => !string.IsNullOrWhiteSpace(x)));
            return true;
        }

        private class TextLine
        {
            public double FullWidth;
            public TextLine NextLine;
            public TextLine PreviousLine;
            public double RemainingWidth;
            public double TakenWidth;
            public string Text;

            public TextLine(double width, TextLine previous)
            {
                FullWidth = RemainingWidth = width;
                if (previous == null)
                    return;

                PreviousLine = previous;
                previous.NextLine = this;
            }

            public bool TryAddWord(string text, double wordWidth, double spaceWidth, bool right = true)
            {
                if (string.IsNullOrWhiteSpace(Text))
                {
                    if (!(RemainingWidth - wordWidth >= 0))
                        return false;

                    AddText(wordWidth, text, right);
                    return true;
                }

                if (!(RemainingWidth - (wordWidth + spaceWidth) >= 0))
                    return false;

                AddText(wordWidth, text, right);
                return true;
            }

            private void AddText(double space, string addedText, bool right)
            {
                Text = right ? $"{Text} {addedText}" : $"{addedText} {Text}";
                RemainingWidth -= space;
                TakenWidth += space;
            }
        }
    }
}