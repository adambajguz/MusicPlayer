using MusicPlayer.UWP.Controllers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;


namespace MusicPlayer.UWP.Pages.Genres
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GenresPage : Page
    {
        private readonly MainPage mainPage;
        private GenreController genreController;
        private ObservableRangeCollection<Controllers.Genre.Result> genres = new ObservableRangeCollection<Controllers.Genre.Result>();

        public GenresPage()
        {
            this.InitializeComponent();

            //mainPage = this.FindParent<MainPage>();

            var frame = (Frame)Window.Current.Content;
            mainPage = (MainPage)frame.Content;

            LoadingProgress.Visibility = Visibility.Visible;
            PageContent.Visibility = Visibility.Collapsed;

            genreController = new GenreController(App.QueryDispatcher, App.CommandDispatcher);

            genres.CollectionChanged += Genres_CollectionChanged;


            var mainTask = Task.Factory.StartNew(() =>
            {
                WaitedLoad();
            });
        }

        private async void WaitedLoad()
        {
            List<Controllers.Genre.Result> temp = await genreController.GetAll();
            genres.AddRange(temp);

            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                LoadingProgress.Visibility = Visibility.Collapsed;
                LoadingProgress.IsActive = false;
                PageContent.Visibility = Visibility.Visible;

                GenresListView.ItemsSource = genres;
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
                List<Controllers.Genre.Result> temp;
                genres.Clear();

                string sortOption = selectedItem.Tag.ToString();
                switch (sortOption)
                {
                    case "az":
                        temp = await genreController.GetAll();
                        genres.AddRange(temp);

                        break;

                    case "za":
                        temp = await genreController.GetAllDescending();
                        genres.AddRange(temp);

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
                        mainPage.NavView_Navigate(MainPage.GenreAddTag, new EntranceNavigationTransitionInfo(), null);

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

                    Controllers.Genre.Result selectedGenre = await genreController.Get(id);

                    switch (selectedItem.Name.ToString())
                    {
                        case "IDetails":
                            mainPage.NavView_Navigate(MainPage.GenreDetailsTag, new EntranceNavigationTransitionInfo(), selectedGenre.Id);

                            break;

                        case "IEdit":
                            mainPage.NavView_Navigate(MainPage.GenreEditTag, new EntranceNavigationTransitionInfo(), selectedGenre.Id);

                            break;

                        case "IRemove":
                            DisplayDeleteSingleDialog(selectedGenre);

                            break;
                    }
                }

                
            }
        }

        private async void DisplayDeleteSingleDialog(Controllers.Genre.Result genreToDelete)
        {
            ContentDialog deleteFileDialog = new ContentDialog
            {
                Title = "Delete '" + genreToDelete.Name + "' permanently?",
                Content = "If you delete this genre, you won't be able to recover it. Do you want to delete it?",
                PrimaryButtonText = "Delete",
                CloseButtonText = "Cancel"
            };

            ContentDialogResult result = await deleteFileDialog.ShowAsync();


            if (result == ContentDialogResult.Primary)
            {       
                // Delete

                await genreController.Delete(genreToDelete.Id);

                List<Controllers.Genre.Result> temp = await genreController.GetAll();
                genres.Clear();
                genres.AddRange(temp);

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
                Title = "Delete selected genres permanently?",
                Content = "If you delete these genres, you won't be able to recover them. Do you want to delete them?",
                PrimaryButtonText = "Delete",
                CloseButtonText = "Cancel"
            };

            ContentDialogResult result = await deleteFileDialog.ShowAsync();


            if (result == ContentDialogResult.Primary)
            {
                // Delete
                foreach(Controllers.Genre.Result genre in genresToDelete)
                    await genreController.Delete(genre.Id);
                
                List<Controllers.Genre.Result> temp = await genreController.GetAll();
                genres.Clear();
                genres.AddRange(temp);
            }
            else
            {
                // The user clicked the CLoseButton, pressed ESC, Gamepad B, or the system back button.
                // Do nothing.
            }
        }
    }
}
