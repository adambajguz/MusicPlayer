using MusicPlayer.UWP.Controllers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;


namespace MusicPlayer.UWP.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BandsPage : Page
    {
        private readonly MainPage mainPage;
        private BandController bandController;
        private ObservableRangeCollection<Controllers.Band.Result> bands = new ObservableRangeCollection<Controllers.Band.Result>();

        public BandsPage()
        {
            this.InitializeComponent();

            //mainPage = this.FindParent<MainPage>();

            var frame = (Frame)Window.Current.Content;
            mainPage = (MainPage)frame.Content;

            LoadingProgress.Visibility = Visibility.Visible;
            PageContent.Visibility = Visibility.Collapsed;

            bandController = new BandController(App.QueryDispatcher, App.CommandDispatcher);

            bands.CollectionChanged += Genres_CollectionChanged;


            var mainTask = Task.Factory.StartNew(() =>
            {
                WaitedLoad();
            });
        }

        private async void WaitedLoad()
        {
            List<Controllers.Band.Result> temp = await bandController.GetAll();
            bands.AddRange(temp);

            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                LoadingProgress.Visibility = Visibility.Collapsed;
                LoadingProgress.IsActive = false;
                PageContent.Visibility = Visibility.Visible;

                GenresListView.ItemsSource = bands;
            });


        }

        private void Genres_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
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
                List<Controllers.Band.Result> temp;
                bands.Clear();

                string sortOption = selectedItem.Tag.ToString();
                switch (sortOption)
                {
                    case "az":
                        temp = await bandController.GetAll();
                        bands.AddRange(temp);

                        break;

                    case "za":
                        temp = await bandController.GetAllDescending();
                        bands.AddRange(temp);

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
                        mainPage.NavView_Navigate(MainPage.BandAddTag, new EntranceNavigationTransitionInfo(), null);

                        break;

                    case "Remove":
                        DisplayDeleteListDialog(GenresListView.SelectedItems);

                        break;

                }
            }
        }


        private void GenresListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GenresListView.SelectedItems.Count > 0)
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

                    Controllers.Band.Result selectedBand = await bandController.Get(id);

                    switch (selectedItem.Name.ToString())
                    {
                        case "IDetails":
                            mainPage.NavView_Navigate(MainPage.BandDetailsTag, new EntranceNavigationTransitionInfo(), selectedBand.Id);

                            break;

                        case "IEdit":
                            mainPage.NavView_Navigate(MainPage.BandEditTag, new EntranceNavigationTransitionInfo(), selectedBand.Id);

                            break;

                        case "IRemove":
                            DisplayDeleteSingleDialog(selectedBand);

                            break;
                    }
                }


            }
        }

        private async void DisplayDeleteSingleDialog(Controllers.Band.Result bandToDelete)
        {
            ContentDialog deleteFileDialog = new ContentDialog
            {
                Title = "Delete '" + bandToDelete.name + "' permanently?",
                Content = "If you delete this band, you won't be able to recover it. Do you want to delete it?",
                PrimaryButtonText = "Delete",
                CloseButtonText = "Cancel"
            };

            ContentDialogResult result = await deleteFileDialog.ShowAsync();


            if (result == ContentDialogResult.Primary)
            {
                // Delete

                await bandController.Delete(bandToDelete.Id);

                List<Controllers.Band.Result> temp = await bandController.GetAll();
                bands.Clear();
                bands.AddRange(temp);

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
                Title = "Delete selected bands permanently?",
                Content = "If you delete these bands, you won't be able to recover them. Do you want to delete them?",
                PrimaryButtonText = "Delete",
                CloseButtonText = "Cancel"
            };

            ContentDialogResult result = await deleteFileDialog.ShowAsync();


            if (result == ContentDialogResult.Primary)
            {
                // Delete
                foreach (Controllers.Band.Result genre in genresToDelete)
                    await bandController.Delete(genre.Id);

                List<Controllers.Band.Result> temp = await bandController.GetAll();
                bands.Clear();
                bands.AddRange(temp);
            }
            else
            {
                // The user clicked the CLoseButton, pressed ESC, Gamepad B, or the system back button.
                // Do nothing.
            }
        }

    }
}
