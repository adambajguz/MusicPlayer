using MusicPlayer.UWP.Controllers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;


namespace MusicPlayer.UWP.Pages.Songs
{


    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SongsPage : Page
    {
        private readonly MainPage mainPage;
        private SongController songController;

        private ObservableRangeCollection<SongData> songs = new ObservableRangeCollection<SongData>();

        public SongsPage()
        {
            this.InitializeComponent();

            //mainPage = this.FindParent<MainPage>();

            var frame = (Frame)Window.Current.Content;
            mainPage = (MainPage)frame.Content;

            LoadingProgress.Visibility = Visibility.Visible;
            PageContent.Visibility = Visibility.Collapsed;

            songController = new SongController(App.QueryDispatcher, App.CommandDispatcher);

            songs.CollectionChanged += Artists_CollectionChanged;


            var mainTask = Task.Factory.StartNew(() =>
            {
                WaitedLoad();
            });
        }

        private async void WaitedLoad()
        {
            List<Controllers.Song.Result> temp = await songController.GetAll();
            List<SongData> songsList = await LoadSongs(temp);


            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                LoadingProgress.Visibility = Visibility.Collapsed;
                LoadingProgress.IsActive = false;
                PageContent.Visibility = Visibility.Visible;

                songs.Clear();
                songs.AddRange(songsList);
                ArtistsListView.ItemsSource = songs;
            });

        }

        private async Task<List<SongData>> LoadSongs(List<Controllers.Song.Result> list)
        {
            List<SongData> songsList = new List<SongData>();

            songs.Clear();

            foreach (var song in list)
            {
                var albums = await songController.GetAlbums(song.Id);
                var artists = await songController.GetArtists(song.Id);

                songsList.Add(new SongData(song, albums, artists));
            }

            return songsList;
        }

        private void Artists_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var x = e.NewItems;
        }

        //private async void Callback(Controllers.Genre.Result , List<Controllers.Genre.Result> genres)
        //{
        //    LoadingProgress.Visibility = Visibility.Collapsed;
        //    LoadingProgress.IsActive = false;
        //    PageContent.Visibility = Visibility.Visible;

        //    string c = genres.Count.ToString();
        //    MessageDialog message = new MessageDialog(c + genre.Name, "OUTPUT:");
        //    await message.ShowAsync();

        //    GenresListView.ItemsSource = genres;
        //}

        private async void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuFlyoutItem selectedItem)
            {
                List<Controllers.Song.Result> temp;
                List<SongData> songsList;

                songs.Clear();

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

                songsList = await LoadSongs(temp);
                songs.AddRange(songsList);
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

                    case "Remove":
                        DisplayDeleteListDialog(ArtistsListView.SelectedItems);

                        break;

                }
            }
        }


        private void ArtistsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ArtistsListView.SelectedItems.Count > 0)
                DeleteSelected.IsEnabled = true;
            else
                DeleteSelected.IsEnabled = false;
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

                            break;

                        case "IDetails":
                            mainPage.NavView_Navigate(MainPage.SongDetailsTag, new EntranceNavigationTransitionInfo(), selectedSong.Id);

                            break;

                        case "IEdit":
                            mainPage.NavView_Navigate(MainPage.SongEditTag, new EntranceNavigationTransitionInfo(), selectedSong.Id);

                            break;

                        case "IRemove":
                            DisplayDeleteSingleDialog(selectedSong);

                            break;
                    }
                }


            }
        }

        private async void DisplayDeleteSingleDialog(Controllers.Song.Result songToDelete)
        {
            ContentDialog deleteFileDialog = new ContentDialog
            {
                Title = "Delete '" + songToDelete.Title + "' permanently?",
                Content = "If you delete this song, you won't be able to recover it. Do you want to delete it?",
                PrimaryButtonText = "Delete",
                CloseButtonText = "Cancel"
            };

            ContentDialogResult result = await deleteFileDialog.ShowAsync();


            if (result == ContentDialogResult.Primary)
            {
                // Delete

                await songController.Delete(songToDelete.Id);


                List<Controllers.Song.Result> temp = await songController.GetAll();
                List<SongData> songsList = await LoadSongs(temp);
                songs.Clear();
                songs.AddRange(songsList);
            }
            else
            {
                // The user clicked the CLoseButton, pressed ESC, Gamepad B, or the system back button.
                // Do nothing.
            }
        }

        private async void DisplayDeleteListDialog(IList<object> genresToDelete)
        {
            ContentDialog deleteFileDialog = new ContentDialog
            {
                Title = "Delete selected songs permanently?",
                Content = "If you delete these songs, you won't be able to recover them. Do you want to delete them?",
                PrimaryButtonText = "Delete",
                CloseButtonText = "Cancel"
            };

            ContentDialogResult result = await deleteFileDialog.ShowAsync();


            if (result == ContentDialogResult.Primary)
            {
                // Delete
                foreach (Controllers.Song.Result genre in genresToDelete)
                    await songController.Delete(genre.Id);


                List<Controllers.Song.Result> temp = await songController.GetAll();
                List<SongData> songsList = await LoadSongs(temp);
                songs.Clear();
                songs.AddRange(songsList);
            }
            else
            {
                // The user clicked the CLoseButton, pressed ESC, Gamepad B, or the system back button.
                // Do nothing.
            }
        }

        private async void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            String query = args.QueryText;

            List<Controllers.Song.Result> temp = await songController.Search(query);
            List<SongData> artistsList = await LoadSongs(temp);
            songs.Clear();
            songs.AddRange(artistsList);
        }

        private async void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                List<Controllers.Song.Result> temp;
                if (sender.Text == "")
                {
                    temp = await songController.GetAll();
                    List<SongData> artistsList = await LoadSongs(temp);
                    songs.Clear();
                    songs.AddRange(artistsList);
                }

                temp = await songController.Search(sender.Text);
                List<string> suggestions = new List<string>();

                foreach (var item in temp)
                    suggestions.Add(item.Title);

                sender.ItemsSource = suggestions;
            }
        }
    }
}
