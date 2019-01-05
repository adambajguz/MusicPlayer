using MusicPlayer.UWP.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MusicPlayer.UWP.Pages.Genre
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GenreAddPage : Page
    {
        private readonly MainPage mainPage;
        private GenreController genreController;

        public GenreAddPage()
        {
            this.InitializeComponent();
            genreController = new GenreController(App.QueryDispatcher, App.CommandDispatcher);

            var frame = (Frame)Window.Current.Content;
            mainPage = (MainPage)frame.Content;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

        }


        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            String name = NameTextBox.Text;
            String description = "description";
            await genreController.Create(name, description);

            mainPage.GoBack();
        }
    }
}
