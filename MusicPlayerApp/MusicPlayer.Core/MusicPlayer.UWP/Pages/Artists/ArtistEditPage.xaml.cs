using MusicPlayer.UWP.Controllers;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace MusicPlayer.UWP.Pages.Artist
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ArtistEditPage : Page
    {
        private WriteOnce<int> elementID = new WriteOnce<int>();

        private readonly MainPage mainPage;
        private ArtistController artistController;

        public ArtistEditPage()
        {
            this.InitializeComponent();

            artistController = new ArtistController(App.QueryDispatcher, App.CommandDispatcher);

            var frame = (Frame)Window.Current.Content;
            mainPage = (MainPage)frame.Content;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            elementID.Value = (int)e.Parameter;
            Controllers.Artist.Result artist = await artistController.Get(elementID.Value);
            NameTextBox.Text = artist.Name;
            DescriptionRichBox.Document.SetText(Windows.UI.Text.TextSetOptions.FormatRtf, artist.Description);

            //CreationDateCalendar.Date = artist.CreationData;

            //if (artist.EndDate != null)
            //{
            //    EndDateCalendar.Date = artist.EndDate;
            //    EndDateToggle.IsOn = true;
            //}
            //else
            //    EndDateCalendar.Visibility = Visibility.Collapsed;
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string name = NameTextBox.Text;

            DateTime creation = CreationDateCalendar.Date.HasValue ? CreationDateCalendar.Date.Value.DateTime : DateTime.Now;
            DateTime? end = EndDateToggle.IsOn && EndDateCalendar.Date.HasValue ? (DateTime?)EndDateCalendar.Date.Value.DateTime : (DateTime?)null;

            string description = string.Empty;
            DescriptionRichBox.Document.GetText(Windows.UI.Text.TextGetOptions.FormatRtf, out description);

            //await artistController.Update(elementID.Value, name, creation, end, description);

            mainPage.GoBack();
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
