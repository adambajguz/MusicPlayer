using MusicPlayer.UWP.Controllers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;


namespace MusicPlayer.UWP.Pages.Songs
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SongAddPage : Page
    {
        private readonly MainPage mainPage;
        private SongController songController;
        private GenreController genreController;
        private AlbumController albumController;
        private ArtistController artistController;
        private ImageController imageController;

        private ObservableRangeCollection<Controllers.Album.Result> AllAlbums = new ObservableRangeCollection<Controllers.Album.Result>();
        private ObservableRangeCollection<Controllers.Genre.Result> AllGenres = new ObservableRangeCollection<Controllers.Genre.Result>();
        private ObservableRangeCollection<Controllers.Artist.Result> AllArtists = new ObservableRangeCollection<Controllers.Artist.Result>();


        public SongAddPage()
        {
            this.InitializeComponent();
            songController = new SongController(App.QueryDispatcher, App.CommandDispatcher);
            genreController = new GenreController(App.QueryDispatcher, App.CommandDispatcher);
            albumController = new AlbumController(App.QueryDispatcher, App.CommandDispatcher);
            artistController = new ArtistController(App.QueryDispatcher, App.CommandDispatcher);
            imageController = new ImageController(App.QueryDispatcher, App.CommandDispatcher);

            var frame = (Frame)Window.Current.Content;
            mainPage = (MainPage)frame.Content;

            LoadAlbums();
            LoadGenres();
            LoadArtists();
        }

        private async void LoadAlbums()
        {
            List<Controllers.Album.Result> temp = await albumController.GetAll();

            AllAlbums.AddRange(temp);

            AlbumsListView.ItemsSource = AllAlbums;
            AllAlbums.CollectionChanged += AllBands_CollectionChanged;
        }

        private async void LoadGenres()
        {
            List<Controllers.Genre.Result> temp = await genreController.GetAll();
            if(temp.Count == 0)
            {
                DisplayNoGenresDialog();
                mainPage.GoBack();
            }

            AllGenres.AddRange(temp);

            GenreComboBox.ItemsSource = AllGenres;
            AllGenres.CollectionChanged += AllBands_CollectionChanged;

            GenreComboBox.SelectedIndex = 0;
        }

        private async void LoadArtists()
        {
            List<Controllers.Artist.Result> temp = await artistController.GetAll();

            AllArtists.AddRange(temp);

            ArtistsListView.ItemsSource = AllArtists;
            AllArtists.CollectionChanged += AllBands_CollectionChanged;
        }



        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void AllBands_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var x = e.NewItems;
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string name = NameTextBox.Text;
            if (name == "")
            {
                DisplayNoNameDialog();
                return;
            }

            int score = Convert.ToInt32(ScoreRating.Value);
            string filePath = FileTextBox.Text;
            DateTime creation = CreationDateCalendar.Date.HasValue ? CreationDateCalendar.Date.Value.DateTime : DateTime.Now;

            Controllers.Genre.Result selectedGenre = (Controllers.Genre.Result)GenreComboBox.SelectedItem;

            int? image_id = null;
            if (ImageFileTextBox.Text != "")
                image_id = await imageController.Create(ImageFileTextBox.Text);

            int id = await songController.Create(score, name, creation, filePath, image_id, selectedGenre.Id);


            foreach (Controllers.Album.Result album in AlbumsListView.SelectedItems)
            {
                var selected = await albumController.GetSongs(album.Id);
                int s = selected.Count + 1;

                await albumController.AddSong(album.Id, id, s);
            }


            foreach (Controllers.Artist.Result artist in ArtistsListView.SelectedItems)
            {
                await artistController.AddSong(artist.Id, id);
            }

            mainPage.GoBack();
        }

        private async void DisplayNoNameDialog()
        {
            ContentDialog noWifiDialog = new ContentDialog
            {
                Title = "No song title provided!",
                Content = "Please enter the title of the song and try again.",
                CloseButtonText = "Ok"
            };

            ContentDialogResult result = await noWifiDialog.ShowAsync();
        }

        private async void DisplayNoGenresDialog()
        {
            ContentDialog noWifiDialog = new ContentDialog
            {
                Title = "No genres!",
                Content = "Please create a genre and try again.",
                CloseButtonText = "Ok"
            };

            ContentDialogResult result = await noWifiDialog.ShowAsync();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            mainPage.GoBack();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker
            {
                ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail,
                SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary
            };
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");

            Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                // Application now has read/write access to the picked file

                ImageFileTextBox.Text = file.Path;
            }
            else
            {
                //Operation cancelled
            }
        }

        private async void FileButton_Click(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker
            {
                ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail,
                SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.MusicLibrary
            };
            picker.FileTypeFilter.Add(".mp3");
            picker.FileTypeFilter.Add(".wav");

            Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                // Application now has read/write access to the picked file

                FileTextBox.Text = file.Path;
            }
            else
            {
                //Operation cancelled
            }
        }

    }
}
