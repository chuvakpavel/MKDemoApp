using System.ComponentModel;

namespace MKDemoApp.Abstractions
{
    public abstract class BaseParallaxCarouselItem : INotifyPropertyChanged
    {
        public double ParallaxTranslation { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}