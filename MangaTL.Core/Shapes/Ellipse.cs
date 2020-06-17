using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace MangaTL.Core.Shapes
{
    public class Ellipse : IShape
    {
        public SizeF Size { get; set; }

        public List<RectangleF> GetLines(float height)
        {
            var result = new List<RectangleF>();
            var remainingHeight = Size.Height / 2;
            var y = (float) 0;
            while (remainingHeight > 0)
            {
                var width = Size.Width /
                            (Size.Height / 2) *
                            (float) Math.Sqrt(Size.Height * Size.Height / 4 - y * y);
                result.Add(new RectangleF(-width / 2, y - height / 2, width, height));
                y += height;
                remainingHeight -= height;
            }

            var bottomList = result.Skip(1).Select(x => new RectangleF(x.Location, x.Size)).ToList();
            result.Reverse();
            result.AddRange(bottomList);

            return result;
        }
    }
}