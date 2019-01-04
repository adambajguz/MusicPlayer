using MusicPlayer.UWP.Controllers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MusicPlayer.UWP.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GenresPage : Page
    {
        private readonly MainPage mainPage;

        GenreController GenreController;
        public GenresPage()
        {
            this.InitializeComponent();

            //mainPage = this.FindParent<MainPage>();

            var frame = (Frame)Window.Current.Content;
            mainPage = (MainPage)frame.Content;

            LoadingProgress.Visibility = Visibility.Visible;
            PageContent.Visibility = Visibility.Collapsed;

            GenreController = new GenreController(App.QueryDispatcher, App.CommandDispatcher);

            var mainTask = Task.Factory.StartNew(() =>
            {
                Test();
            });
        }

        private async void Test()
        {
            string test;
            Controllers.Genre.Result genre;
            List<Controllers.Genre.Result> genres;

            test = GenreController.GetAll().ToString();
            genre = await GenreController.Get(1);
            genres = await GenreController.GetAll();

            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                Callback(genre, genres);
            });


        }

        private async void Callback(Controllers.Genre.Result genre, List<Controllers.Genre.Result> genres)
        {
            LoadingProgress.Visibility = Visibility.Collapsed;
            LoadingProgress.IsActive = false;
            PageContent.Visibility = Visibility.Visible;

            string c = genres.Count.ToString();
            MessageDialog message = new MessageDialog(c + genre.Name, "OUTPUT:");
            await message.ShowAsync();

            gridGenresView.ItemsSource = genres;
        }

        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuFlyoutItem selectedItem)
            {
                string sortOption = selectedItem.Tag.ToString();
                switch (sortOption)
                {
                    case "az":

                        break;
                    case "za":

                        break;

                }
            }
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is AppBarButton selectedItem)
            {
                string sortOption = selectedItem.Tag.ToString();
                switch (sortOption)
                {
                    case "Add":
                        mainPage.NavView_Navigate(MainPage.GenresAddEditTag, new EntranceNavigationTransitionInfo());
                                             
                        break;

                    case "Edit":

                        break;

                    case "Remove":

                        break;

                }
            }
        }
    }
}
