using MKDemoApp.ViewModels;
using Xamarin.Forms;

namespace MKDemoApp.Views
{
    public partial class MainPage
    {
        private const string PulseAnimationKey = "pulse";

        public MainPage()
        {
            BindingContext = new MainViewModel(Navigation);

            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            StartButtonAnimation();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            AbortButtonAnimation();
        }

        private void StartButtonAnimation()
        {
            var finalAnimation = new Animation();

            var scaleUpAnimation = new Animation(scale => { StartButton.Scale = scale; }, 0.98d, 1d, Easing.Linear);
            var scaleDownAnimation = new Animation(scale => { StartButton.Scale = scale; }, 1d, 0.98d, Easing.Linear);

            finalAnimation.Add(0.00d, 0.05d, scaleUpAnimation);
            finalAnimation.Add(0.05d, 0.10d, scaleDownAnimation);
            finalAnimation.Add(0.02d, 1d, new Animation());

            finalAnimation.Commit(StartButton, PulseAnimationKey, length: 800u, repeat: () => true);
        }

        private void AbortButtonAnimation() => StartButton.AbortAnimation(PulseAnimationKey);
    }
}