using MusicPlayer.UWP.Controllers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;


namespace MusicPlayer.UWP.Pages.Albums
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AlbumsPage : Page
    {
        private readonly MainPage mainPage;
        private AlbumController albumController;
        private PlayQueueController playQueueController;

        private ObservableRangeCollection<Controllers.Album.Result> albums = new ObservableRangeCollection<Controllers.Album.Result>();

        public AlbumsPage()
        {
            this.InitializeComponent();

            //mainPage = this.FindParent<MainPage>();

            var frame = (Frame)Window.Current.Content;
            mainPage = (MainPage)frame.Content;

            LoadingProgress.Visibility = Visibility.Visible;
            PageContent.Visibility = Visibility.Collapsed;

            albumController = new AlbumController(App.QueryDispatcher, App.CommandDispatcher);
            playQueueController = new PlayQueueController(App.QueryDispatcher, App.CommandDispatcher);

            albums.CollectionChanged += Bands_CollectionChanged;


            var mainTask = Task.Factory.StartNew(() =>
            {
                WaitedLoad();
            });
        }

        private async void WaitedLoad()
        {
            List<Controllers.Album.Result> temp = await albumController.GetAll();
            albums.AddRange(temp);

            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                LoadingProgress.Visibility = Visibility.Collapsed;
                LoadingProgress.IsActive = false;
                PageContent.Visibility = Visibility.Visible;

                AlbumsListView.ItemsSource = albums;
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
                List<Controllers.Album.Result> temp;
                albums.Clear();

                string sortOption = selectedItem.Tag.ToString();
                switch (sortOption)
                {
                    case "az":
                        temp = await albumController.GetAll();
                        albums.AddRange(temp);

                        break;

                    case "za":
                        temp = await albumController.GetAllDescending();
                        albums.AddRange(temp);

                        break;

                    case "pub_az":
                        temp = await albumController.GetAllByPublicationDate();
                        albums.AddRange(temp);

                        break;

                    case "pub_za":
                        temp = await albumController.GetAllByPublicationDateDescending();
                        albums.AddRange(temp);

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
                        mainPage.NavView_Navigate(MainPage.AlbumAddTag, new EntranceNavigationTransitionInfo(), null);

                        break;

                    case "Remove":
                        DisplayDeleteListDialog(AlbumsListView.SelectedItems);

                        break;

                }
            }
        }


        private void BandsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AlbumsListView.SelectedItems.Count > 0)
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

                    Controllers.Album.Result selectedAlbum = await albumController.Get(id);

                    switch (selectedItem.Name.ToString())
                    {
                        case "IPlay":
                            {
                                List<Controllers.Song.Result> albumSongs = await albumController.GetSongs(selectedAlbum.Id);

                                bool first = true;
                                foreach (Controllers.Song.Result x in albumSongs)
                                {
                                    if (first)
                                    {
                                        mainPage.SetAudio(x.FilePath, x);
                                        first = false;
                                    }
                                    else
                                        await playQueueController.Create(x.Id);

                                }
                            }
                            break;
                        case "IDetails":
                            mainPage.NavView_Navigate(MainPage.AlbumDetailsTag, new EntranceNavigationTransitionInfo(), selectedAlbum.Id);

                            break;


                        case "IAddToQueue":
                            {
                                List<Controllers.Song.Result> albumSongs = await albumController.GetSongs(selectedAlbum.Id);
                                albumSongs.Reverse();

                                foreach (Controllers.Song.Result x in albumSongs)
                                    await playQueueController.Create(x.Id);
                            }
                            break;

                        case "IEdit":
                            mainPage.NavView_Navigate(MainPage.AlbumEditTag, new EntranceNavigationTransitionInfo(), selectedAlbum.Id);

                            break;

                        case "IRemove":
                            DisplayDeleteSingleDialog(selectedAlbum);

                            break;
                    }
                }


            }
        }

        private async void DisplayDeleteSingleDialog(Controllers.Album.Result albumToDelete)
        {
            ContentDialog deleteFileDialog = new ContentDialog
            {
                Title = "Delete '" + albumToDelete.Title + "' permanently?",
                Content = "If you delete this album, you won't be able to recover it. Do you want to delete it?",
                PrimaryButtonText = "Delete",
                CloseButtonText = "Cancel"
            };

            ContentDialogResult result = await deleteFileDialog.ShowAsync();


            if (result == ContentDialogResult.Primary)
            {
                // Delete
                ImageController imageController = new ImageController(App.QueryDispatcher, App.CommandDispatcher);
                await imageController.Delete(albumToDelete.ImageId);

                await albumController.Delete(albumToDelete.Id);

                List<Controllers.Album.Result> temp = await albumController.GetAll();
                albums.Clear();
                albums.AddRange(temp);

            }
            else
            {
                // The user clicked the CLoseButton, pressed ESC, Gamepad B, or the system back button.
                // Do nothing.
            }
        }

        private async void DisplayDeleteListDialog(IList<object> albumsToDelete)
        {
            ContentDialog deleteFileDialog = new ContentDialog
            {
                Title = "Delete selected albums permanently?",
                Content = "If you delete these albums, you won't be able to recover them. Do you want to delete them?",
                PrimaryButtonText = "Delete",
                CloseButtonText = "Cancel"
            };

            ContentDialogResult result = await deleteFileDialog.ShowAsync();


            if (result == ContentDialogResult.Primary)
            {
                ImageController imageController = new ImageController(App.QueryDispatcher, App.CommandDispatcher);

                // Delete
                foreach (Controllers.Album.Result album in albumsToDelete)
                {
                    await imageController.Delete(album.ImageId);
                    await albumController.Delete(album.Id);
                }

                List<Controllers.Album.Result> temp = await albumController.GetAll();
                albums.Clear();
                albums.AddRange(temp);
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

            List<Controllers.Album.Result> temp = await albumController.Search(query);
            albums.Clear();
            albums.AddRange(temp);
        }

        private async void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                List<Controllers.Album.Result> temp;
                if (sender.Text == "")
                {
                    temp = await albumController.GetAll();
                    albums.Clear();
                    albums.AddRange(temp);
                }

                temp = await albumController.Search(sender.Text);
                List<string> suggestions = new List<string>();

                foreach (var item in temp)
                    suggestions.Add(item.Title);

                sender.ItemsSource = suggestions;
            }
        }
    }
}
