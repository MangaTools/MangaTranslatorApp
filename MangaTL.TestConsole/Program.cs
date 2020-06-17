using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MangaTL.Core;
using MangaTL.Core.Shapes;

namespace MangaTL.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {

            var font = new Font("Anime Ace v05", 1.6667f);
            var text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.";

            var bubble = new TextBubble();
            bubble.Style = new TextStyle()
            {
                Alignment = Alignment.Center,
                Color = Color.Black,
                Font = font,
                MaxFontSize = 24 / 12f,
                MinFontSize = 14 / 12f,
                Name = "Supa Style"
            };
            bubble.TextContent = text;
            bubble.Shape = new Ellipse()
            {
                Size = new SizeF(20, 20)
            };
            var result = bubble.SplitToFitInTheShape();
            Console.WriteLine(result);

            Console.ReadKey();
        }
    }
}
