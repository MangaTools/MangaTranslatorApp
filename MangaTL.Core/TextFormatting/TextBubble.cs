using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MangaTL.Core.Shapes;

namespace MangaTL.Core
{
    [Serializable]
    public class TextBubble
    {
        private readonly string calculatedText = "";
        private readonly bool isTextCalculated = false;
        public Point Position;
        public float Rotation;
        public SplitSettings Settings = new SplitSettings();
        public IShape Shape;
        public TextStyle Style;
        public string TextContent;

        public string SplitToFitInTheShape()
        {
            if (isTextCalculated)
                return calculatedText;

            var sizes = TextMeasuring.GetSizes(TextContent.Split(' '), Style.Font).ToList();
            var spaceSize = TextRenderer.MeasureText(" ", Style.Font);

            var totalWidth = sizes.Sum(x => x.Width) + (sizes.Count - 1) * spaceSize.Width;

            var lines = Shape.GetLines(sizes.First().Height);
            var centralIndex = (int) Math.Ceiling((float) lines.Count / 2);

            var i = 1;
            var remainingWidth = (float) totalWidth;
            while (remainingWidth > 0 && centralIndex - i >= 0)
            {
                remainingWidth -= lines[centralIndex - i].Width * 2;
                i++;
            }

            var l = lines.Skip((lines.Count - (i * 2 + 1)) / 2).Take(i * 2 + 1).ToList();

            return TryBuild(l, sizes, spaceSize, TextContent.Split(' ').ToList());
        }

        private string TryBuild(List<RectangleF> lines, List<Size> sizes, Size spaceSize, List<string> words)
        {
            var result = new StringBuilder();
            var i = 0;
            foreach (var rectangleF in lines)
            {
                var remainingSpace = rectangleF.Width;
                var first = true;
                while (true)
                {
                    if (words.Count - 1 < i)
                        break;
                    if (first)
                    {
                        first = false;
                        if (remainingSpace - sizes[i].Width > 0)
                        {
                            result.Append(words[i]);
                            remainingSpace -= sizes[i].Width;
                            i++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        if (remainingSpace - (sizes[i].Width + spaceSize.Width) > 0)
                        {
                            result.Append(" ");
                            result.Append(words[i]);
                            remainingSpace -= sizes[i].Width + spaceSize.Width;
                            i++;
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                result.Append("\n");
            }

            result.Remove(result.Length - 1, 1);
            return result.ToString();
        }
    }
}