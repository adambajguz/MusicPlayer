using MusicPlayer.UWP.Controllers;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;


namespace MusicPlayer.UWP.Pages.Genre
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ArtistDetailsPage : Page
    {
        private WriteOnce<int> elementID = new WriteOnce<int>();

        private readonly MainPage mainPage;
        private BandController bandController;

        public ArtistDetailsPage()
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

            string end = band.EndDate == null ? "..." : band.EndDate.ToString();
            CreationEndTextBox.Text = "(" + band.CreationData.ToLongDateString() + " - " + end + ")";


            DescriptionRichBox.Document.SetText(Windows.UI.Text.TextSetOptions.FormatRtf, band.Description);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            mainPage.GoBack();
        }
    }
}
