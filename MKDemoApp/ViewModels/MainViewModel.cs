using MKDemoApp.Abstractions;
using MKDemoApp.Views;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace MKDemoApp.ViewModels
{
    public sealed class MainViewModel : BaseViewModel
    {
        public IAsyncCommand GoToCarouselCommand { get; }

        private readonly INavigation _navigation;

        public MainViewModel(INavigation navigation)
        {
            _navigation = navigation;

            GoToCarouselCommand = new AsyncCommand(GoToCarouselAsync, allowsMultipleExecutions: false);
        }

        private async Task GoToCarouselAsync() => await _navigation.PushAsync(new CharactersCarouselPage());
    }
}