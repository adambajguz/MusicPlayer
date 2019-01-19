using MusicPlayer.UWP.Controllers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;


namespace MusicPlayer.UWP.Pages.PlayQueue
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PlayQueuePage : Page
    {
        private readonly MainPage mainPage;
        private PlayQueueController playQueueController;
        private SongController songController;

        private ObservableRangeCollection<PlayQueueData> songs = new ObservableRangeCollection<PlayQueueData>();
        private int? unselectID = null;

        public PlayQueuePage()
        {
            this.InitializeComponent();

            //mainPage = this.FindParent<MainPage>();

            var frame = (Frame)Window.Current.Content;
            mainPage = (MainPage)frame.Content;

            LoadingProgress.Visibility = Visibility.Visible;
            PageContent.Visibility = Visibility.Collapsed;

            playQueueController = new PlayQueueController(App.QueryDispatcher, App.CommandDispatcher);
            songController = new SongController(App.QueryDispatcher, App.CommandDispatcher);

            songs.CollectionChanged += Artists_CollectionChanged;


            var mainTask = Task.Factory.StartNew(() =>
            {
                WaitedLoad();
            });
        }

        private async void WaitedLoad()
        {
            List<Controllers.PlayQueue.Result> temp = await playQueueController.GetAll();
            List<PlayQueueData> songsList = await LoadSongs(temp);


            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                LoadingProgress.Visibility = Visibility.Collapsed;
                LoadingProgress.IsActive = false;
                PageContent.Visibility = Visibility.Visible;

                songs.Clear();
                songs.AddRange(songsList);
                PlayQueueListView.ItemsSource = songs;
            });

        }

        private async Task<List<PlayQueueData>> LoadSongs(List<Controllers.PlayQueue.Result> list)
        {
            List<PlayQueueData> songsList = new List<PlayQueueData>();

            songs.Clear();

            foreach (var pq in list)
            {
                var song = await songController.Get(pq.SongId);
                var albums = await songController.GetAlbums(song.Id);
                var artists = await songController.GetArtists(song.Id);

                songsList.Add(new PlayQueueData(song, albums, artists, pq.Id));
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


        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is AppBarButton selectedItem)
            {
                switch (selectedItem.Tag.ToString())
                {
                    case "PlayAll":
                        mainPage.PlayNextFromQueue();

                        break;
                    case "Remove":
                        DisplayDeleteListDialog(PlayQueueListView.SelectedItems);

                        break;

                }
            }
        }


        private void ArtistsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (unselectID != null)
                foreach (PlayQueueData x in PlayQueueListView.SelectedItems)
                    if (x.Song.Id == unselectID)
                    {
                        unselectID = null;
                        PlayQueueListView.SelectedItems.Remove(x);
                        break;
                    }


            if (PlayQueueListView.SelectedItems.Count > 0)
                DeleteSelected.IsEnabled = true;
            else
                DeleteSelected.IsEnabled = false;

        }


        private async void ListItemBarButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is AppBarButton selectedItem)
            {
                // DataContext with cast can be used instaed of tags!!!!!!!!
                if (int.TryParse(selectedItem.Tag.ToString(), out int id))
                {
                    // parsing successful


                    switch (selectedItem.Name.ToString())
                    {

                        case "IDetails":
                            mainPage.NavView_Navigate(MainPage.SongDetailsTag, new EntranceNavigationTransitionInfo(), id);

                            break;

                        case "IRemove":
                            Controllers.PlayQueue.Result selected = await playQueueController.Get(id);

                            DisplayDeleteSingleDialog(selected);

                            break;
                    }
                }


            }
        }

        private async void DisplayDeleteSingleDialog(Controllers.PlayQueue.Result queueItemToDelete)
        {
            var song = await songController.Get(queueItemToDelete.SongId);

            ContentDialog deleteFileDialog = new ContentDialog
            {
                Title = "Delete '" + song.Title + "' from play queue?",
                Content = "",
                PrimaryButtonText = "Delete",
                CloseButtonText = "Cancel"
            };

            ContentDialogResult result = await deleteFileDialog.ShowAsync();


            if (result == ContentDialogResult.Primary)
            {
                // Delete

                await playQueueController.Delete(queueItemToDelete.Id);


                List<Controllers.PlayQueue.Result> temp = await playQueueController.GetAll();
                List<PlayQueueData> songsList = await LoadSongs(temp);
                songs.Clear();
                songs.AddRange(songsList);
            }
            else
            {
                // The user clicked the CLoseButton, pressed ESC, Gamepad B, or the system back button.
                // Do nothing.
            }
        }

        private async void DisplayDeleteListDialog(IList<object> songsToDelete)
        {
            ContentDialog deleteFileDialog = new ContentDialog
            {
                Title = "Delete selected songs from play queue?",
                PrimaryButtonText = "Delete",
                CloseButtonText = "Cancel"
            };

            ContentDialogResult result = await deleteFileDialog.ShowAsync();


            if (result == ContentDialogResult.Primary)
            {
                // Delete
                foreach (PlayQueueData tmp in songsToDelete)
                    await playQueueController.Delete(tmp.QueueID);


                List<Controllers.PlayQueue.Result> temp = await playQueueController.GetAll();
                List<PlayQueueData> songsList = await LoadSongs(temp);
                songs.Clear();
                songs.AddRange(songsList);
            }
            else
            {
                // The user clicked the CLoseButton, pressed ESC, Gamepad B, or the system back button.
                // Do nothing.
            }
        }


        private async void RatingControl_ValueChanged(RatingControl sender, object args)
        {
            PlayQueueData songData = (PlayQueueData)sender.DataContext;
            int id = songData.Song.Id;
            unselectID = id;

            int score = Convert.ToInt32(sender.Value);

            await songController.SetScore(id, score);

            //ListViewItem lvi = DependencyObjectExtension.FindParent<ListViewItem>(sender.Parent);
            //if (lvi != null)
            //    lvi.IsSelected = false;
        }
    }
}
