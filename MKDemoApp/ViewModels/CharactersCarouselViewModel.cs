using MKDemoApp.Abstractions;
using MKDemoApp.Helpers;
using MKDemoApp.Models;
using MKDemoApp.Views;
using Rg.Plugins.Popup.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace MKDemoApp.ViewModels
{
    public class CharactersCarouselViewModel : BaseViewModel
    {
        public IAsyncCommand GoBackCommand { get; }
        public IAsyncCommand SelectCharacterCommand { get; }

        public ObservableCollection<Character> Characters { get; }
        public Character CurrentCharacter { get; set; }

        private readonly INavigation _navigation;

        public CharactersCarouselViewModel(INavigation navigation)
        {
            _navigation = navigation;

            GoBackCommand = new AsyncCommand(GoBackAsync, allowsMultipleExecutions: false);
            SelectCharacterCommand = new AsyncCommand(SelectCharacterAsync, allowsMultipleExecutions: false);

            Characters = new ObservableCollection<Character>
            {
                new(Constants.Texts.Scorpion, Constants.Texts.ScorpionInfo, Constants.Images.Scorpion),
                new(Constants.Texts.SubZero, Constants.Texts.SubZeroInfo, Constants.Images.SubZero),
                new(Constants.Texts.JohnnyCage, Constants.Texts.JohnnyCageInfo, Constants.Images.JohnnyCage),
                new(Constants.Texts.ErronBlack, Constants.Texts.ErronBlackInfo, Constants.Images.ErronBlack),
                new(Constants.Texts.Raiden, Constants.Texts.RaidenInfo, Constants.Images.Raiden),
                new(Constants.Texts.QuanChi, Constants.Texts.QuanChiInfo, Constants.Images.QuanChi),
                new(Constants.Texts.LiuKang, Constants.Texts.LiuKangInfo, Constants.Images.LiuKang),
                new(Constants.Texts.KungLao, Constants.Texts.KungLaoInfo, Constants.Images.KungLao),
                new(Constants.Texts.Kitana, Constants.Texts.KitanaInfo, Constants.Images.Kitana),
                new(Constants.Texts.Kenshi, Constants.Texts.KenshiInfo, Constants.Images.Kenshi),
                new(Constants.Texts.Kano, Constants.Texts.KanoInfo, Constants.Images.Kano),
                new(Constants.Texts.Ermak, Constants.Texts.ErmakInfo, Constants.Images.Ermak)
            };
        }

        private Task GoBackAsync() => _navigation.PopAsync();

        private async Task SelectCharacterAsync()
        {
            if (CurrentCharacter == null)
            {
                return;
            }

            await PopupNavigation.Instance.PushAsync(new SelectionInfoPopup(CurrentCharacter.Name));
        }
    }
}