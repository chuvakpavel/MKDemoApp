using Rg.Plugins.Popup.Services;
using System;

namespace MKDemoApp.Views
{
    public partial class SelectionInfoPopup
    {
        public string CharacterName { get; }

        public SelectionInfoPopup(string characterName)
        {
            CharacterName = characterName;

            InitializeComponent();
        }

        private void OnOkButtonClicked(object sender, EventArgs e) => PopupNavigation.Instance.PopAsync();
    }
}