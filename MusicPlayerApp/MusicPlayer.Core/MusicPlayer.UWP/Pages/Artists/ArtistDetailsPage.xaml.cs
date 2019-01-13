using MusicPlayer.UWP.Controllers;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
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

        private WriteOnce<int?> bandID = new WriteOnce<int?>();

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

            if(artist == null)
            {
                mainPage.GoBack();
                return;
            }


            NameTextBox.Text = artist.Name + " " + artist.Surname + " (" + artist.Pseudonym + ")";

            if(artist.BandId != null)
            {
                BandController bandController = new BandController(App.QueryDispatcher, App.CommandDispatcher);
                Controllers.Band.Result band = await bandController.Get((int)artist.BandId);
                bandID.Value = band.Id;

                BandHyperlink.Content = band.name;
            }
            else
            {
                BandHyperlink.Content = "None";
                BandHyperlink.IsEnabled = false;
            }

            BirthdayTextBox.Text = artist.Birthdate.ToLongDateString();

            DescriptionRichBox.Document.SetText(Windows.UI.Text.TextSetOptions.FormatRtf, artist.Description);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            mainPage.GoBack();
        }

        private void BandHyperlink_Click(object sender, RoutedEventArgs e)
        {
            mainPage.NavView_Navigate(MainPage.BandDetailsTag, new EntranceNavigationTransitionInfo(), bandID.Value);
        }
    }
}
