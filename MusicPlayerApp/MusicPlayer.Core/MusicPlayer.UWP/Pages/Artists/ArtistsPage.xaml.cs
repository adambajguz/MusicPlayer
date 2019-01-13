using MusicPlayer.UWP.Controllers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;


namespace MusicPlayer.UWP.Pages.Artist
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ArtistsPage : Page
    {
        private readonly MainPage mainPage;
        private ArtistController artistController;
        private ObservableRangeCollection<Controllers.Artist.Result> artists = new ObservableRangeCollection<Controllers.Artist.Result>();

        public ArtistsPage()
        {
            this.InitializeComponent();

            //mainPage = this.FindParent<MainPage>();

            var frame = (Frame)Window.Current.Content;
            mainPage = (MainPage)frame.Content;

            LoadingProgress.Visibility = Visibility.Visible;
            PageContent.Visibility = Visibility.Collapsed;

            artistController = new ArtistController(App.QueryDispatcher, App.CommandDispatcher);

            artists.CollectionChanged += Artists_CollectionChanged;


            var mainTask = Task.Factory.StartNew(() =>
            {
                WaitedLoad();
            });
        }

        private async void WaitedLoad()
        {
            List<Controllers.Artist.Result> temp = await artistController.GetAll();
            artists.AddRange(temp);

            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                LoadingProgress.Visibility = Visibility.Collapsed;
                LoadingProgress.IsActive = false;
                PageContent.Visibility = Visibility.Visible;

                ArtistsListView.ItemsSource = artists;
            });


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
                List<Controllers.Artist.Result> temp;
                artists.Clear();

                string sortOption = selectedItem.Tag.ToString();
                switch (sortOption)
                {
                    case "az":
                        temp = await artistController.GetAll();
                        artists.AddRange(temp);

                        break;

                    case "za":
                        temp = await artistController.GetAllDescending();
                        artists.AddRange(temp);

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
                        mainPage.NavView_Navigate(MainPage.ArtistAddTag, new EntranceNavigationTransitionInfo(), null);

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

                    Controllers.Artist.Result selectedArtist = await artistController.Get(id);

                    switch (selectedItem.Name.ToString())
                    {
                        case "IDetails":
                            mainPage.NavView_Navigate(MainPage.ArtistDetailsTag, new EntranceNavigationTransitionInfo(), selectedArtist.Id);

                            break;

                        case "IEdit":
                            mainPage.NavView_Navigate(MainPage.ArtistEditTag, new EntranceNavigationTransitionInfo(), selectedArtist.Id);

                            break;

                        case "IRemove":
                            DisplayDeleteSingleDialog(selectedArtist);

                            break;
                    }
                }


            }
        }

        private async void DisplayDeleteSingleDialog(Controllers.Artist.Result bandToDelete)
        {
            ContentDialog deleteFileDialog = new ContentDialog
            {
                Title = "Delete '" + bandToDelete.Name + "' permanently?",
                Content = "If you delete this artist, you won't be able to recover him. Do you want to delete him?",
                PrimaryButtonText = "Delete",
                CloseButtonText = "Cancel"
            };

            ContentDialogResult result = await deleteFileDialog.ShowAsync();


            if (result == ContentDialogResult.Primary)
            {
                // Delete

                await artistController.Delete(bandToDelete.Id);

                artists.Clear();
                artists.AddRange(await artistController.GetAll());
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
                Title = "Delete selected artists permanently?",
                Content = "If you delete these artists, you won't be able to recover them. Do you want to delete them?",
                PrimaryButtonText = "Delete",
                CloseButtonText = "Cancel"
            };

            ContentDialogResult result = await deleteFileDialog.ShowAsync();


            if (result == ContentDialogResult.Primary)
            {
                // Delete
                foreach (Controllers.Artist.Result genre in genresToDelete)
                    await artistController.Delete(genre.Id);

                artists.Clear();
                artists.AddRange(await artistController.GetAll());
            }
            else
            {
                // The user clicked the CLoseButton, pressed ESC, Gamepad B, or the system back button.
                // Do nothing.
            }
        }

    }
}
