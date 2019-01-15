using MusicPlayer.UWP.Controllers;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;


namespace MusicPlayer.UWP.Pages.Genres
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


        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string name = NameTextBox.Text;

            string description = string.Empty;
            DescriptionRichBox.Document.GetText(Windows.UI.Text.TextGetOptions.FormatRtf, out description);

            await genreController.Create(name, description);

            mainPage.GoBack();
        }


        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            mainPage.GoBack();
        }
    }
}
