using MKDemoApp.Helpers;
using MKDemoApp.Services;
using MKDemoApp.Views;
using System.IO;
using System.Reflection;
using Xamarin.Forms;

namespace MKDemoApp
{
    public partial class App
    {
        private readonly IAudioPlayerService _audioPlayerService;

        public App()
        {
            _audioPlayerService = DependencyService.Get<IAudioPlayerService>();
            _audioPlayerService.Init(GetStreamFromFile(Constants.Audio.MkBackground));
            _audioPlayerService.Volume = 0.75f;
            _audioPlayerService.IsLooped = true;

            DependencyService.Get<IStatusBarService>().SetColor(Color.FromHex("#1C1414"));

            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
        _audioPlayerService.Play();
        }

        protected override void OnSleep()
        {
            _audioPlayerService.Pause();
        }

        protected override void OnResume()
        {
            _audioPlayerService.Play();
        }

        private static Stream GetStreamFromFile(string filename) =>
            typeof(App).GetTypeInfo().Assembly.GetManifestResourceStream(filename);
    }
}