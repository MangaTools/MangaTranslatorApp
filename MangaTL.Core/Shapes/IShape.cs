using System.Collections.Generic;
using System.Drawing;

namespace MangaTL.Core.Shapes
{
    public interface IShape
    {
        SizeF Size { get; set; }

        List<RectangleF> GetLines(float height);
    }
}