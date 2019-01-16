using MusicPlayer.UWP.Controllers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
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

        private ObservableRangeCollection<Controllers.Song.Result> songs = new ObservableRangeCollection<Controllers.Song.Result>();


        public AlbumDetailsPage()
        {
            this.InitializeComponent();

            albumController = new AlbumController(App.QueryDispatcher, App.CommandDispatcher);
            songController = new SongController(App.QueryDispatcher, App.CommandDispatcher);

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
                        case "IDetails":
                            mainPage.NavView_Navigate(MainPage.SongDetailsTag, new EntranceNavigationTransitionInfo(), selectedSong.Id);

                            break;
                    }
                }


            }
        }
    }
}
