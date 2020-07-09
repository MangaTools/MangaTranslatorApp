using System;
using System.Drawing;

namespace MangaTL.Core
{
    [Serializable]
    public class TextStyle
    {
        public Font Font;
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
            MaxFontSize = 50,
            MinFontSize = 20,
            Name = "Standart"
        };
    }
}