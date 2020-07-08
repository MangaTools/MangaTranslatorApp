using System.Collections.Generic;
using System.Drawing;

namespace MangaTL.Core.Shapes
{
    public class Rectangle : IShape
    {
        public SizeF Size { get; set; }

        public Rectangle(int width, int height)
        {
            Size = new SizeF(width, height);
        }

        public List<RectangleF> GetLines(float height)
        {
            var result = new List<RectangleF>();
            var startY = -Size.Height / 2;
            var x = -Size.Width / 2;


            for (var i = 0; i < Size.Height / height; i++)
            {
                result.Add(new RectangleF(x, startY + i * height, Size.Width, height));
            }

            return result;
        }
    }
}