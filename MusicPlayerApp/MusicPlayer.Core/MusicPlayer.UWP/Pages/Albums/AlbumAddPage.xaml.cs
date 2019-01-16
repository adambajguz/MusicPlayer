using MusicPlayer.UWP.Controllers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;


namespace MusicPlayer.UWP.Pages.Albums
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AlbumAddPage : Page
    {
        private readonly MainPage mainPage;
        private AlbumController albumController;
        private SongController songController;

        private ObservableRangeCollection<Controllers.Song.Result> AllSongs = new ObservableRangeCollection<Controllers.Song.Result>();

        public AlbumAddPage()
        {
            this.InitializeComponent();
            albumController = new AlbumController(App.QueryDispatcher, App.CommandDispatcher);
            songController = new SongController(App.QueryDispatcher, App.CommandDispatcher);

            var frame = (Frame)Window.Current.Content;
            mainPage = (MainPage)frame.Content;

            LoadSongs();
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

        }
        private async void LoadSongs()
        {
            List<Controllers.Song.Result> temp = await songController.GetAll();

            AllSongs.AddRange(temp);

            SongsListView.ItemsSource = AllSongs;
            AllSongs.CollectionChanged += AllBands_CollectionChanged;
        }

        private void AllBands_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var x = e.NewItems;
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string name = NameTextBox.Text;

            DateTime creation = CreationDateCalendar.Date.HasValue ? CreationDateCalendar.Date.Value.DateTime : DateTime.Now;

            string description = string.Empty;
            DescriptionRichBox.Document.GetText(Windows.UI.Text.TextGetOptions.FormatRtf, out description);

            int ID = await albumController.Create(name, description, creation, 1);

            int s = 1;
            foreach (Controllers.Song.Result song in SongsListView.SelectedItems)
                await albumController.AddSong(ID, song.Id, s++);

            mainPage.GoBack();
        }


        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            mainPage.GoBack();
        }

    
    }
}
