using MusicPlayer.UWP.Controllers;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;


namespace MusicPlayer.UWP.Pages.Bands
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BandAddPage : Page
    {
        private readonly MainPage mainPage;
        private BandController bandController;

        public BandAddPage()
        {
            this.InitializeComponent();
            bandController = new BandController(App.QueryDispatcher, App.CommandDispatcher);

            var frame = (Frame)Window.Current.Content;
            mainPage = (MainPage)frame.Content;

            EndDateCalendar.Visibility = Visibility.Collapsed;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

        }


        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string name = NameTextBox.Text;
            if(name == "")
            {
                DisplayNoNameDialog();
                return;
            }

            DateTime creation = CreationDateCalendar.Date.HasValue ? CreationDateCalendar.Date.Value.DateTime : DateTime.Now;
            DateTime? end = EndDateToggle.IsOn && EndDateCalendar.Date.HasValue ? (DateTime?)EndDateCalendar.Date.Value.DateTime : (DateTime?)null;

            string description = string.Empty;
            DescriptionRichBox.Document.GetText(Windows.UI.Text.TextGetOptions.FormatRtf, out description);

            await bandController.Create(name, creation, end, description);

            mainPage.GoBack();
        }

        private async void DisplayNoNameDialog()
        {
            ContentDialog noWifiDialog = new ContentDialog
            {
                Title = "No band name provided!",
                Content = "Please enter the name of the band and try again.",
                CloseButtonText = "Ok"
            };

            ContentDialogResult result = await noWifiDialog.ShowAsync();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            mainPage.GoBack();
        }

        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            if (sender is ToggleSwitch toggleSwitch)
            {
                if (toggleSwitch.IsOn == true)
                    EndDateCalendar.Visibility = Visibility.Visible;
                else
                    EndDateCalendar.Visibility = Visibility.Collapsed;
            }
        }
    }
}
