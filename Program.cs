using System.IO;
using Jil;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;

namespace CardGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            FontCollection fonts = new FontCollection();
            var titleFontFamily = fonts.Install("OpenSans-Regular.ttf");
            var contentFontFamily = fonts.Install("NeoSansSpacing-Medium.ttf");

            var titleFont = fonts.CreateFont(titleFontFamily.Name, 84);
            var titleColor = new Rgba32(209, 57, 111);
            var titleOptions = new TextGraphicsOptions(true)
            {
                HorizontalAlignment = HorizontalAlignment.Center
            };

            var contentFont = fonts.CreateFont(contentFontFamily.Name, 65);
            var contentColor = new Rgba32(12, 13, 14);
            var contentOptions = new TextGraphicsOptions(true)
            {
                WrapTextWidth = 944
            };

            CardRequest[] cards;
            using (var reader = File.OpenText(args[0]))
            {
                cards = JSON.Deserialize<CardRequest[]>(reader);
            }

            foreach (var card in cards)
            {
                using (Image<Rgba32> image = SixLabors.ImageSharp.Image.Load("background.png"))
                {
                    image.Mutate(x => x
                        .DrawText(titleOptions, card.Title, titleFont, titleColor, new PointF(512, 40))
                        .DrawText(contentOptions, card.Content, contentFont, contentColor, new PointF(40, 130))
                    );

                    var directory = Path.GetDirectoryName(card.DestinationPath);
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }
                    image.Save(card.DestinationPath);
                }
            }
        }
    }
}
