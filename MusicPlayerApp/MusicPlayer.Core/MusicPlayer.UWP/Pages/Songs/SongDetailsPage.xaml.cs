using MusicPlayer.UWP.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;


namespace MusicPlayer.UWP.Pages.Songs
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SongDetailsPage : Page
    {
        private WriteOnce<int> elementID = new WriteOnce<int>();

        private readonly MainPage mainPage;
        private SongController songController;

        private WriteOnce<int?> bandID = new WriteOnce<int?>();

        public SongDetailsPage()
        {
            this.InitializeComponent();

            songController = new SongController(App.QueryDispatcher, App.CommandDispatcher);

            var frame = (Frame)Window.Current.Content;
            mainPage = (MainPage)frame.Content;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            elementID.Value = (int)e.Parameter;
            Controllers.Song.Result song = await songController.Get(elementID.Value);

            if (song == null)
            {
                mainPage.GoBack();
                return;
            }


            NameTextBox.Text = song.Title;

            {
                // set album string
                List<Controllers.Album.Result> albumsList = await songController.GetAlbums(song.Id);

                if (albumsList.Count == 0)
                    AlbumsTextBox.Text = "Single";
                else
                {
                    String str = albumsList.Count > 1 ? "On albums: " : "On album: ";

                    var last = albumsList.Last();
                    foreach (var album in albumsList)
                    {
                        str += album.Title;

                        if (!album.Equals(last))
                            str += ", ";
                    }

                    AlbumsTextBox.Text = str;
                }
            }


            {
                // set artists string
                List<Controllers.Artist.Result> artistList = await songController.GetArtists(song.Id);

                if (artistList.Count == 0)
                    AlbumsTextBox.Text = "No authors";
                else
                {
                    String str = "By: ";

                    var last = artistList.Last();
                    foreach (var artist in artistList)
                    {
                        str += artist.Name + " " + artist.Surname;

                        if (!artist.Equals(last))
                            str += ", ";
                    }

                    AlbumsTextBox.Text = str;
                }
            }

            {
                GenreController genreController = new GenreController(App.QueryDispatcher, App.CommandDispatcher);
                Controllers.Genre.Result genre = await genreController.Get(song.GenreId);
                GenreTextBox.Text = "Genre: " + genre.Name;
            }

            StatTextBox.Text = song.LengthText + "\t" + song.PlayTimesText +
                "\n Score: " + song.Score +
                "\n Bitrate: " + song.bitrate +
                "\n Creation date: " + song.CreationDate +
                "\n Added to library " + song.DBCreationDate;


        }

        private void BandHyperlink_Click(object sender, RoutedEventArgs e)
        {
            mainPage.NavView_Navigate(MainPage.BandDetailsTag, new EntranceNavigationTransitionInfo(), bandID.Value);
        }
    }
}
