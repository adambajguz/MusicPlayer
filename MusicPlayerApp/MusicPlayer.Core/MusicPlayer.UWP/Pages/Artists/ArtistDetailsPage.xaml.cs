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
    public sealed partial class ArtistDetailsPage : Page
    {
        private WriteOnce<int> elementID = new WriteOnce<int>();

        private readonly MainPage mainPage;
        private ArtistController artistController;

        public ArtistDetailsPage()
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

            //string end = artist.EndDate == null ? "..." : artist.EndDate.ToString();
            //CreationEndTextBox.Text = "(" + artist.CreationData.ToLongDateString() + " - " + end + ")";


            DescriptionRichBox.Document.SetText(Windows.UI.Text.TextSetOptions.FormatRtf, artist.Description);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            mainPage.GoBack();
        }
    }
}
