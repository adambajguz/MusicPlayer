using MusicPlayer.UWP.Controllers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;


namespace MusicPlayer.UWP.Pages.Playlists
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PlaylistAddPage : Page
    {
        private readonly MainPage mainPage;
        private PlaylistController playlistController;
        private SongController songController;

        private ObservableRangeCollection<Controllers.Song.Result> AllSongs = new ObservableRangeCollection<Controllers.Song.Result>();

        public PlaylistAddPage()
        {
            this.InitializeComponent();
            playlistController = new PlaylistController(App.QueryDispatcher, App.CommandDispatcher);
            songController = new SongController(App.QueryDispatcher, App.CommandDispatcher);

            var frame = (Frame)Window.Current.Content;
            mainPage = (MainPage)frame.Content;

            LoadSongs();
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

        }
        private async void LoadSongs()
        {
            List<Controllers.Song.Result> temp = await songController.GetAll();

            AllSongs.AddRange(temp);

            SongsListView.ItemsSource = AllSongs;
            AllSongs.CollectionChanged += AllBands_CollectionChanged;
        }

        private void AllBands_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var x = e.NewItems;
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string name = NameTextBox.Text;

            string description = string.Empty;
            DescriptionRichBox.Document.GetText(Windows.UI.Text.TextGetOptions.FormatRtf, out description);

            int ID = await playlistController.Create(name, description);

            foreach (Controllers.Song.Result song in SongsListView.SelectedItems)
                await playlistController.AddSong(ID, song.Id);

            mainPage.GoBack();
        }


        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            mainPage.GoBack();
        }

    
    }
}
