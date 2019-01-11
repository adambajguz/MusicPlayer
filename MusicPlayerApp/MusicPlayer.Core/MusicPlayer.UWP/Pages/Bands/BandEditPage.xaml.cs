using MusicPlayer.UWP.Controllers;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace MusicPlayer.UWP.Pages.Band
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BandEditPage : Page
    {
        private WriteOnce<int> elementID = new WriteOnce<int>();

        private readonly MainPage mainPage;
        private BandController bandController;

        public BandEditPage()
        {
            this.InitializeComponent();

            bandController = new BandController(App.QueryDispatcher, App.CommandDispatcher);

            var frame = (Frame)Window.Current.Content;
            mainPage = (MainPage)frame.Content;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            elementID.Value = (int)e.Parameter;
            Controllers.Band.Result band = await bandController.Get(elementID.Value);
            NameTextBox.Text = band.name;
            DescriptionRichBox.Document.SetText(Windows.UI.Text.TextSetOptions.FormatRtf, band.Description);

            CreationDateCalendar.Date = band.CreationData;

            if (band.EndDate != null)
            {
                EndDateCalendar.Date = band.EndDate;
                EndDateToggle.IsOn = true;
            }
            else
                EndDateCalendar.Visibility = Visibility.Collapsed;
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string name = NameTextBox.Text;

            DateTime creation = CreationDateCalendar.Date.HasValue ? CreationDateCalendar.Date.Value.DateTime : DateTime.Now;
            DateTime? end = EndDateToggle.IsOn && EndDateCalendar.Date.HasValue ? (DateTime?)EndDateCalendar.Date.Value.DateTime : (DateTime?)null;

            string description = string.Empty;
            DescriptionRichBox.Document.GetText(Windows.UI.Text.TextGetOptions.FormatRtf, out description);

            await bandController.Update(elementID.Value, name, creation, end, description);

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
