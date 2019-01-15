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
    public sealed partial class GenreEditPage : Page
    {
        private WriteOnce<int> elementID = new WriteOnce<int>();

        private readonly MainPage mainPage;
        private GenreController genreController;

        public GenreEditPage()
        {
            this.InitializeComponent();

            genreController = new GenreController(App.QueryDispatcher, App.CommandDispatcher);

            var frame = (Frame)Window.Current.Content;
            mainPage = (MainPage)frame.Content;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            elementID.Value = (int)e.Parameter;
            Controllers.Genre.Result genre = await genreController.Get(elementID.Value);

            if (genre == null)
            {
                mainPage.GoBack();
                return;
            }

            NameTextBox.Text = genre.Name;
            DescriptionRichBox.Document.SetText(Windows.UI.Text.TextSetOptions.FormatRtf, genre.Description);
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string name = NameTextBox.Text;

            string description = string.Empty;
            DescriptionRichBox.Document.GetText(Windows.UI.Text.TextGetOptions.FormatRtf, out description);

            await genreController.Update(elementID.Value, name, description);

            mainPage.GoBack();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            mainPage.GoBack();
        }
    }
}
