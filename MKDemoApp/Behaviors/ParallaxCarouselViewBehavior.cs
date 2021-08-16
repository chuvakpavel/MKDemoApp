using MKDemoApp.Abstractions;
using System;
using System.Linq;
using Xamarin.Forms;

namespace MKDemoApp.Behaviors
{
    public class ParallaxCarouselViewBehavior : Behavior<CarouselView>
    {
        #region Bindable Properties

        #region ParallaxOffset Property

        public static readonly BindableProperty ParallaxOffsetProperty = BindableProperty.Create(
            nameof(ParallaxOffset),
            typeof(double),
            typeof(ParallaxCarouselViewBehavior),
            500d,
            BindingMode.TwoWay);

        public double ParallaxOffset
        {
            get => (double) GetValue(ParallaxOffsetProperty);
            set => SetValue(ParallaxOffsetProperty, value);
        }

        #endregion ParallaxOffset Property

        #endregion Bindable Properties

        protected override void OnAttachedTo(CarouselView bindable)
        {
            base.OnAttachedTo(bindable);

            bindable.Scrolled += OnScrolled;
        }

        protected override void OnDetachingFrom(CarouselView bindable)
        {
            base.OnDetachingFrom(bindable);

            bindable.Scrolled -= OnScrolled;
        }

        private void OnScrolled(object sender, ItemsViewScrolledEventArgs e)
        {
            if (!(sender is CarouselView carouselView) || !(carouselView.ItemsLayout is ItemsLayout itemsLayout))
            {
                return;
            }

            var carouselItems = carouselView.ItemsSource.Cast<BaseParallaxCarouselItem>().ToList();

            var centerItemIndex = e.CenterItemIndex;
            var lastItemIndex = e.LastVisibleItemIndex;

            switch (itemsLayout.Orientation)
            {
                case ItemsLayoutOrientation.Horizontal:
                {
                    var carouselWidth = carouselView.Width;

                    var offset = carouselWidth * (centerItemIndex + 1) - e.HorizontalOffset;
                    var position = offset * ParallaxOffset / carouselWidth - ParallaxOffset;

                    var lastItem = carouselItems[lastItemIndex];
                    lastItem.ParallaxTranslation = position + ParallaxOffset;

                    var currentItem = carouselItems[centerItemIndex];
                    currentItem.ParallaxTranslation = position;

                    break;
                }
                case ItemsLayoutOrientation.Vertical:
                {
                    var carouselHeight = carouselView.Height;

                    var offset = carouselHeight * (centerItemIndex + 1) - e.VerticalOffset;
                    var position = offset * ParallaxOffset / carouselHeight - ParallaxOffset;

                    var lastItem = carouselItems[lastItemIndex];
                    lastItem.ParallaxTranslation = position + ParallaxOffset;

                    var currentItem = carouselItems[centerItemIndex];
                    currentItem.ParallaxTranslation = position;

                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}