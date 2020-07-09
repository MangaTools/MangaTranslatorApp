using System;
using System.Drawing;

namespace MangaTL.Core
{
    [Serializable]
    public class TextStyle
    {
        public string FontName;
        public float FontSize;
        internal Font Font;
        public bool Bold;
        public bool Italic;
        public Color Color;
        public string Name;
        public Alignment Alignment;
        public float MinFontSize;
        public float MaxFontSize;

        public static TextStyle StandartStyle => new TextStyle
        {
            Alignment = Alignment.Center,
            Color = Color.Black,
            Font = new Font("Arial", 40),
            FontName = "Arial",
            Bold = false,
            Italic = false,
            FontSize = 40,
            MaxFontSize = 50,
            MinFontSize = 20,
            Name = "Standard"
        };
    }
}