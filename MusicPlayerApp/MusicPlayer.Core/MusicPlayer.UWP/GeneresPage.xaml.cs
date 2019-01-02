using MusicPlayer.UWP.Controllers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MusicPlayer.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GenresPage : Page
    {
        GenreController GenreController;
        public GenresPage()
        {
            this.InitializeComponent();

            GenreController = App.GenreController;

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
            string c = genres.Count.ToString();
            MessageDialog message = new MessageDialog(c + genre.Name, "OUTPUT:");
            await message.ShowAsync();
        }
    }
}
