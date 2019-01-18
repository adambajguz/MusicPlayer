using MusicPlayer.UWP.Controllers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.Storage;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;


namespace MusicPlayer.UWP.Pages.Albums
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AlbumDetailsPage : Page
    {
        private WriteOnce<int> elementID = new WriteOnce<int>();

        private readonly MainPage mainPage;
        private AlbumController albumController;
        private SongController songController;
        private PlayQueueController playQueueController;

        private ObservableRangeCollection<Controllers.Song.Result> songs = new ObservableRangeCollection<Controllers.Song.Result>();


        public AlbumDetailsPage()
        {
            this.InitializeComponent();

            albumController = new AlbumController(App.QueryDispatcher, App.CommandDispatcher);
            songController = new SongController(App.QueryDispatcher, App.CommandDispatcher);
            playQueueController = new PlayQueueController(App.QueryDispatcher, App.CommandDispatcher);

            songs.CollectionChanged += Genres_CollectionChanged;


            var frame = (Frame)Window.Current.Content;
            mainPage = (MainPage)frame.Content;
        }


        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            elementID.Value = (int)e.Parameter;
            Controllers.Album.Result album = await albumController.Get(elementID.Value);

            if (album == null)
            {
                mainPage.GoBack();
                return;
            }

            {
                var img = new Core.NullObjects.ImageNullObject();
                PhotoImage.Source = new BitmapImage(new Uri(img.FilePath));

                ImageController imageController = new ImageController(App.QueryDispatcher, App.CommandDispatcher);

                Controllers.Image.Result DBimage = await imageController.Get(album.ImageId);
                if (DBimage.FilePath != img.FilePath)
                {
                    try
                    {
                        StorageFile file = await StorageFile.GetFileFromPathAsync(DBimage.FilePath);
                        var stream = await file.OpenReadAsync();
                        BitmapImage imageSource = new BitmapImage();
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


            NameTextBox.Text = album.Title;

            CreationEndTextBox.Text = "Publication date: " + album.PublicationDate.ToLongDateString() + "\nCreation date: " + album.DBCreationDate.ToLongDateString();

            DescriptionRichBox.Document.SetText(Windows.UI.Text.TextSetOptions.FormatRtf, album.Description);


            LoadSongs();
        }


        private async void LoadSongs()
        {
            List<Controllers.Song.Result> temp = await albumController.GetSongs(elementID.Value);

            songs.AddRange(temp);

            SongsListView.ItemsSource = songs;
            songs.CollectionChanged += Genres_CollectionChanged;
        }


        private void Genres_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var x = e.NewItems;
        }

        private async void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuFlyoutItem selectedItem)
            {
                List<Controllers.Song.Result> temp;


                string sortOption = selectedItem.Tag.ToString();
                switch (sortOption)
                {
                    case "az":
                        temp = await songController.GetAll();
                        break;

                    case "za":
                        temp = await songController.GetAllDescending();
                        break;

                    case "len_az":
                        temp = await songController.GetAllByLength();
                        break;

                    case "len_za":
                        temp = await songController.GetAllByLengthDescending();
                        break;

                    case "play_az":
                        temp = await songController.GetAllByPlayTimes();
                        break;

                    case "play_za":
                        temp = await songController.GetAllByPlayTimesDescending();
                        break;

                    case "score_az":
                        temp = await songController.GetAllByScore();
                        break;

                    case "score_za":
                        temp = await songController.GetAllByScoreDescending();
                        break;

                    case "date_az":
                        temp = await songController.GetAllByDate();
                        break;

                    case "date_za":
                        temp = await songController.GetAllByDateDescending();
                        break;

                    default:
                        return;
                }

                List<Controllers.Song.Result> albumSongs = await albumController.GetSongs(elementID.Value);
                List<int> albumSongsIds = new List<int>();

                foreach (Controllers.Song.Result item in albumSongs)
                    albumSongsIds.Add(item.Id);

                List<Controllers.Song.Result> toDisplay = new List<Controllers.Song.Result>();
                foreach (Controllers.Song.Result item in temp)
                    if (albumSongsIds.Contains(item.Id))
                        toDisplay.Add(item);

                songs.Clear();
                songs.AddRange(toDisplay);
            }
        }



        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is AppBarButton selectedItem)
            {
                switch (selectedItem.Tag.ToString())
                {
                    case "Add":
                        mainPage.NavView_Navigate(MainPage.SongAddTag, new EntranceNavigationTransitionInfo(), null);

                        break;
                }
            }
        }


        private async void ListItemBarButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is AppBarButton selectedItem)
            {
                if (int.TryParse(selectedItem.Tag.ToString(), out int id))
                {
                    // parsing successful

                    Controllers.Song.Result selectedSong = await songController.Get(id);

                    switch (selectedItem.Name.ToString())
                    {
                        case "IPlay":
                            mainPage.SetAudio(selectedSong.FilePath, selectedSong);
                            break;

                        case "IAddToQueue":
                            await playQueueController.Create(selectedSong.Id);
                            break;

                        case "IDetails":
                            mainPage.NavView_Navigate(MainPage.SongDetailsTag, new EntranceNavigationTransitionInfo(), selectedSong.Id);

                            break;

                        case "IEdit":
                            mainPage.NavView_Navigate(MainPage.SongEditTag, new EntranceNavigationTransitionInfo(), selectedSong.Id);

                            break;
                    }
                }


            }
        }

    }

}
