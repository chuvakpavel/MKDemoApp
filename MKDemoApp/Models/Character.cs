using MKDemoApp.Abstractions;

namespace MKDemoApp.Models
{
    public sealed class Character : BaseParallaxCarouselItem
    {
        public string Name { get; }

        public string Info { get; }

        public string Image { get; }

        public Character(string name, string info, string image)
        {
            Name = name;
            Info = info;
            Image = image;
        }
    }
}