using MusicPlayer.UWP.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Storage;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
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

            {
                var img = new Core.NullObjects.ImageNullObject();
                PhotoImage.Source = new BitmapImage(new Uri(img.FilePath));

                ImageController imageController = new ImageController(App.QueryDispatcher, App.CommandDispatcher);
                {
                    Controllers.Image.Result DBimage = null;
                    if (song.ImageId != null)
                        DBimage = await imageController.Get((int)song.ImageId);
                    else
                    {
                        List<Controllers.Album.Result> albums = await songController.GetAlbums(song.Id);
                        if (albums.Count > 0)
                            DBimage = await imageController.Get(albums.ElementAt(0).ImageId);
                    }

                    if (DBimage != null && DBimage.FilePath != img.FilePath)
                    {
                        try
                        {
                            var file = await StorageFile.GetFileFromPathAsync(DBimage.FilePath);
                            var stream = await file.OpenReadAsync();
                            var imageSource = new BitmapImage();
                            await imageSource.SetSourceAsync(stream);

                            PhotoImage.Source = imageSource;
                        }
                        catch (Exception)
                        {
                            // prompt user for what action they should do then launch below
                            // suggestion could be a message prompt
                            await Launcher.LaunchUriAsync(new Uri("ms-settings:appsfeatures-app"));
                        }


                    }
                }

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

            SongRating.Value = song.Score;

            {
                GenreController genreController = new GenreController(App.QueryDispatcher, App.CommandDispatcher);
                Controllers.Genre.Result genre = await genreController.Get(song.GenreId);
                GenreTextBox.Text = "Genre: " + genre.Name;
            }

            StatTextBox.Text = song.LengthText + "\t" + song.PlayTimesText +
                "\n\nBitrate: " + song.bitrate +
                "\nFile path: " + song.FilePath +
                "\n\nCreation date: " + song.CreationDate.ToLongDateString() +
                "\nAdded to library: " + song.DBCreationDate.ToLongDateString();


        }

        private void BandHyperlink_Click(object sender, RoutedEventArgs e)
        {
            mainPage.NavView_Navigate(MainPage.BandDetailsTag, new EntranceNavigationTransitionInfo(), bandID.Value);
        }
    }
}
