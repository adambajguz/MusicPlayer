using MusicPlayer.UWP.Controllers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;


namespace MusicPlayer.UWP.Pages.Songs
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SongEditPage : Page
    {
        private WriteOnce<int> elementID = new WriteOnce<int>();

        private readonly MainPage mainPage;
        private SongController songController;
        private GenreController genreController;
        private AlbumController albumController;
        private ArtistController artistController;
        private ImageController imageController;

        private ObservableRangeCollection<Controllers.Album.Result> AllAlbums = new ObservableRangeCollection<Controllers.Album.Result>();
        private ObservableRangeCollection<Controllers.Genre.Result> AllGenres = new ObservableRangeCollection<Controllers.Genre.Result>();
        private ObservableRangeCollection<Controllers.Artist.Result> AllArtists = new ObservableRangeCollection<Controllers.Artist.Result>();


        public SongEditPage()
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



        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            elementID.Value = (int)e.Parameter;
            Controllers.Song.Result song = await songController.Get(elementID.Value);

            if (song == null)
            {
                mainPage.GoBack();
                return;
            }

            if (song.ImageId != null)
            {
                Controllers.Image.Result image = await imageController.Get((int)song.ImageId);
                ImageFileTextBox.Text = image.FilePath;
            }

            NameTextBox.Text = song.Title;
            ScoreRating.Value = song.Score;
            FileTextBox.Text = song.FilePath;
            CreationDateCalendar.Date = song.CreationDate;

            foreach (Controllers.Genre.Result item in GenreComboBox.Items)
                if (item.Id == elementID.Value)
                {
                    GenreComboBox.SelectedItem = item;
                    break;
                }

            // select albums
            {
                var selected = await songController.GetAlbums(elementID.Value);

                foreach (Controllers.Album.Result item in AlbumsListView.Items)
                    foreach (var x in selected)
                        if (x.Id == item.Id)
                            AlbumsListView.SelectedItems.Add(item);
            }

            // select artists
            {
                var selected = await songController.GetArtists(elementID.Value);

                foreach (Controllers.Artist.Result item in ArtistsListView.Items)
                    foreach (var x in selected)
                        if (x.Id == item.Id)
                            ArtistsListView.SelectedItems.Add(item);
            }
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
            {
                Controllers.Song.Result song = await songController.Get(elementID.Value);
                image_id = song.ImageId;

                if(image_id != null && ImageFileTextBox.Text != "")
                    await imageController.Update((int)image_id, ImageFileTextBox.Text);
            }

            await songController.Update(elementID.Value, score, name, creation, filePath, image_id, selectedGenre.Id);

            //update albums
            {
                List<Controllers.Album.Result> selected = await songController.GetAlbums(elementID.Value);

                List<int> selectedAlbumsIds = new List<int>();
                List<int> DBAlbumsIds = new List<int>();

                foreach (Controllers.Album.Result item in AlbumsListView.SelectedItems)
                    selectedAlbumsIds.Add(item.Id);

                foreach (Controllers.Album.Result item in selected)
                    DBAlbumsIds.Add(item.Id);

                {
                    List<int> toRemove = new List<int>(DBAlbumsIds.Except(selectedAlbumsIds));
                    foreach (int i in toRemove)
                        await albumController.DeleteSong(elementID.Value, i);
                }

                {
                    List<int> toAdd = new List<int>(selectedAlbumsIds.Except(DBAlbumsIds));
                    foreach (int i in toAdd)
                    {
                        List<Controllers.Song.Result> albumSongs = await albumController.GetSongs(i);
                        int track = albumSongs.Count + 1;
                        await albumController.AddSong(i, elementID.Value, track);
                    }
                }
            }


            //update artists
            {
                List<Controllers.Artist.Result> selected = await songController.GetArtists(elementID.Value);

                List<int> selectedArtistsIds = new List<int>();
                List<int> DBArtistsIds = new List<int>();

                foreach (Controllers.Artist.Result item in ArtistsListView.SelectedItems)
                    selectedArtistsIds.Add(item.Id);

                foreach (Controllers.Artist.Result item in selected)
                    DBArtistsIds.Add(item.Id);

                {
                    List<int> toRemove = new List<int>(DBArtistsIds.Except(selectedArtistsIds));
                    foreach (int i in toRemove)
                        await artistController.DeleteSong(elementID.Value, i);
                }

                {
                    List<int> toAdd = new List<int>(selectedArtistsIds.Except(DBArtistsIds));
                    foreach (int i in toAdd)
                    {
                        await artistController.AddSong(i, elementID.Value);
                    }
                }
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
