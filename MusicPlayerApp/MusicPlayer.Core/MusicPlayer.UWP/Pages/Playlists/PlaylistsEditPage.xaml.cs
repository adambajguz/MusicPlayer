using MusicPlayer.UWP.Controllers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace MusicPlayer.UWP.Pages.Playlists
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PlaylistEditPage : Page
    {
        private WriteOnce<int> elementID = new WriteOnce<int>();

        private readonly MainPage mainPage;
        private PlaylistController playlistController;

        private ObservableRangeCollection<Controllers.Song.Result> AllSongs = new ObservableRangeCollection<Controllers.Song.Result>();
        private SongController songController;

        public PlaylistEditPage()
        {
            this.InitializeComponent();

            playlistController = new PlaylistController(App.QueryDispatcher, App.CommandDispatcher);
            songController = new SongController(App.QueryDispatcher, App.CommandDispatcher);

            var frame = (Frame)Window.Current.Content;
            mainPage = (MainPage)frame.Content;

            LoadSongs();
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

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            elementID.Value = (int)e.Parameter;
            Controllers.Playlist.Result album = await playlistController.Get(elementID.Value);

            if (album == null)
            {
                mainPage.GoBack();
                return;
            }

            NameTextBox.Text = album.Name;
            DescriptionRichBox.Document.SetText(Windows.UI.Text.TextSetOptions.FormatRtf, album.Description);

          

            var selected = await playlistController.GetSongs(album.Id);

            foreach (Controllers.Song.Result item in SongsListView.Items)
                foreach (var x in selected)
                    if (x.Id == item.Id)
                    {
                        SongsListView.SelectedItems.Add(item);
                        break;
                    }
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string name = NameTextBox.Text;
            if(name == "")
            {
                DisplayNoNameDialog();
                return;
            }

            string description = string.Empty;
            DescriptionRichBox.Document.GetText(Windows.UI.Text.TextGetOptions.FormatRtf, out description);

            await playlistController.Update(elementID.Value, name, description);

            Controllers.Playlist.Result playlist = await playlistController.Get(elementID.Value);

            {
                var selected = await playlistController.GetSongs(playlist.Id);

                List<int> selectedSongsIds = new List<int>();
                List<int> DBSongsIds = new List<int>();

                foreach (Controllers.Song.Result item in SongsListView.SelectedItems)
                    selectedSongsIds.Add(item.Id);

                foreach (Controllers.Song.Result item in selected)
                    DBSongsIds.Add(item.Id);

                {
                    List<int> toRemove = new List<int>(DBSongsIds.Except(selectedSongsIds));
                    foreach (int i in toRemove)
                        await playlistController.DeleteSong(i, elementID.Value);
                }

                {
                    int s = DBSongsIds.Count;
                    List<int> toAdd = new List<int>(selectedSongsIds.Except(DBSongsIds));
                    foreach (int i in toAdd)
                        await playlistController.AddSong(elementID.Value, i);
                }
            }

            mainPage.GoBack();
        }

        private async void DisplayNoNameDialog()
        {
            ContentDialog noWifiDialog = new ContentDialog
            {
                Title = "No playlist name provided!",
                Content = "Please enter the name of the playlist and try again.",
                CloseButtonText = "Ok"
            };

            ContentDialogResult result = await noWifiDialog.ShowAsync();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            mainPage.GoBack();
        }

     
    }
}
