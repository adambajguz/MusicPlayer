using MusicPlayer.UWP.Controllers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;


namespace MusicPlayer.UWP.Pages.Playlists
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PlaylistsPage : Page
    {
        private readonly MainPage mainPage;
        private PlaylistController playlistController;
        private ObservableRangeCollection<Controllers.Playlist.Result> playlists = new ObservableRangeCollection<Controllers.Playlist.Result>();

        public PlaylistsPage()
        {
            this.InitializeComponent();

            //mainPage = this.FindParent<MainPage>();

            var frame = (Frame)Window.Current.Content;
            mainPage = (MainPage)frame.Content;

            LoadingProgress.Visibility = Visibility.Visible;
            PageContent.Visibility = Visibility.Collapsed;

            playlistController = new PlaylistController(App.QueryDispatcher, App.CommandDispatcher);

            playlists.CollectionChanged += Bands_CollectionChanged;


            var mainTask = Task.Factory.StartNew(() =>
            {
                WaitedLoad();
            });
        }

        private async void WaitedLoad()
        {
            List<Controllers.Playlist.Result> temp = await playlistController.GetAll();
            playlists.AddRange(temp);

            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                LoadingProgress.Visibility = Visibility.Collapsed;
                LoadingProgress.IsActive = false;
                PageContent.Visibility = Visibility.Visible;

                PlaylistListView.ItemsSource = playlists;
            });


        }

        private void Bands_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
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
                List<Controllers.Playlist.Result> temp;
                playlists.Clear();

                string sortOption = selectedItem.Tag.ToString();
                switch (sortOption)
                {
                    case "az":
                        temp = await playlistController.GetAll();
                        playlists.AddRange(temp);

                        break;

                    case "za":
                        temp = await playlistController.GetAllDescending();
                        playlists.AddRange(temp);

                        break;

                    case "pub_az":
                        temp = await playlistController.GetAllByDate();
                        playlists.AddRange(temp);

                        break;

                    case "pub_za":
                        temp = await playlistController.GetAllByDateDescending();
                        playlists.AddRange(temp);

                        break;
                }
            }
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is AppBarButton selectedItem)
            {
                switch (selectedItem.Tag.ToString())
                {
                    case "Add":
                        mainPage.NavView_Navigate(MainPage.PlaylistAddTag, new EntranceNavigationTransitionInfo(), null);

                        break;

                    case "Remove":
                        DisplayDeleteListDialog(PlaylistListView.SelectedItems);

                        break;

                }
            }
        }


        private void BandsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PlaylistListView.SelectedItems.Count > 0)
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

                    Controllers.Playlist.Result selectedAlbum = await playlistController.Get(id);

                    switch (selectedItem.Name.ToString())
                    {

                        case "IPlay":

                            break;

                        case "IDetails":
                            mainPage.NavView_Navigate(MainPage.PlaylistDetailsTag, new EntranceNavigationTransitionInfo(), selectedAlbum.Id);

                            break;


                        case "IExport":

                            break;

                        case "IEdit":
                            mainPage.NavView_Navigate(MainPage.PlaylistEditTag, new EntranceNavigationTransitionInfo(), selectedAlbum.Id);

                            break;

                        case "IRemove":
                            DisplayDeleteSingleDialog(selectedAlbum);

                            break;
                    }
                }


            }
        }

        private async void DisplayDeleteSingleDialog(Controllers.Playlist.Result playlistToDelete)
        {
            ContentDialog deleteFileDialog = new ContentDialog
            {
                Title = "Delete '" + playlistToDelete.Name + "' permanently?",
                Content = "If you delete this playlist, you won't be able to recover it. Do you want to delete it?",
                PrimaryButtonText = "Delete",
                CloseButtonText = "Cancel"
            };

            ContentDialogResult result = await deleteFileDialog.ShowAsync();


            if (result == ContentDialogResult.Primary)
            {
                // Delete

                await playlistController.Delete(playlistToDelete.Id);

                List<Controllers.Playlist.Result> temp = await playlistController.GetAll();
                playlists.Clear();
                playlists.AddRange(temp);

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
                Title = "Delete selected playlists permanently?",
                Content = "If you delete these playlists, you won't be able to recover them. Do you want to delete them?",
                PrimaryButtonText = "Delete",
                CloseButtonText = "Cancel"
            };

            ContentDialogResult result = await deleteFileDialog.ShowAsync();


            if (result == ContentDialogResult.Primary)
            {
                // Delete
                foreach (Controllers.Playlist.Result genre in genresToDelete)
                    await playlistController.Delete(genre.Id);

                List<Controllers.Playlist.Result> temp = await playlistController.GetAll();
                playlists.Clear();
                playlists.AddRange(temp);
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

            List<Controllers.Playlist.Result> temp = await playlistController.Search(query);
            playlists.Clear();
            playlists.AddRange(temp);
        }

        private async void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                List<Controllers.Playlist.Result> temp;
                if (sender.Text == "")
                {
                    temp = await playlistController.GetAll();
                    playlists.Clear();
                    playlists.AddRange(temp);
                }

                temp = await playlistController.Search(sender.Text);
                List<string> suggestions = new List<string>();

                foreach (var item in temp)
                    suggestions.Add(item.Name);

                sender.ItemsSource = suggestions;
            }
        }
    }
}
