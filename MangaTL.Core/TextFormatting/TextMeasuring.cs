using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MangaTL.Core
{
    public static class TextMeasuring
    {
        public static IEnumerable<Size> GetSizes(IEnumerable<string> strs, Font style)
        {
            //Высота линии по умолчанию = 1.20 от высоты строки
            return strs.Select(x => TextRenderer.MeasureText(x, style));
        }
    }
}