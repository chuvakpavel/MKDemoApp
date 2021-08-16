using MKDemoApp.ViewModels;

namespace MKDemoApp.Views
{
    public partial class CharactersCarouselPage
    {
        public CharactersCarouselPage()
        {
            BindingContext = new CharactersCarouselViewModel(Navigation);

            InitializeComponent();
        }
    }
}