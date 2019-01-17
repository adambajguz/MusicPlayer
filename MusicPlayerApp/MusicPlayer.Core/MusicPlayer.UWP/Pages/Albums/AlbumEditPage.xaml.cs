using MusicPlayer.UWP.Controllers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace MusicPlayer.UWP.Pages.Albums
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AlbumEditPage : Page
    {
        private WriteOnce<int> elementID = new WriteOnce<int>();

        private readonly MainPage mainPage;
        private AlbumController albumController;

        private ObservableRangeCollection<Controllers.Song.Result> AllSongs = new ObservableRangeCollection<Controllers.Song.Result>();
        private SongController songController;

        public AlbumEditPage()
        {
            this.InitializeComponent();

            albumController = new AlbumController(App.QueryDispatcher, App.CommandDispatcher);
            songController = new SongController(App.QueryDispatcher, App.CommandDispatcher);

            var frame = (Frame)Window.Current.Content;
            mainPage = (MainPage)frame.Content;

            LoadSongs();
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

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            elementID.Value = (int)e.Parameter;
            Controllers.Album.Result album = await albumController.Get(elementID.Value);

            if (album == null)
            {
                mainPage.GoBack();
                return;
            }

            NameTextBox.Text = album.Title;
            DescriptionRichBox.Document.SetText(Windows.UI.Text.TextSetOptions.FormatRtf, album.Description);

            CreationDateCalendar.Date = album.PublicationDate;
           

            var selected = await albumController.GetSongs(album.Id);

            foreach (Controllers.Song.Result item in SongsListView.Items)
                foreach (var x in selected)
                    if (x.Id == item.Id)
                    {
                        SongsListView.SelectedItems.Add(item);
                        break;
                    }
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string name = NameTextBox.Text;

            DateTime creation = CreationDateCalendar.Date.HasValue ? CreationDateCalendar.Date.Value.DateTime : DateTime.Now;

            string description = string.Empty;
            DescriptionRichBox.Document.GetText(Windows.UI.Text.TextGetOptions.FormatRtf, out description);

            await albumController.Update(elementID.Value, name, description, creation, 1);

            {
                var selected = await albumController.GetSongs(elementID.Value);

                List<int> selectedSongsIds = new List<int>();
                List<int> DBSongsIds = new List<int>();

                foreach (Controllers.Song.Result item in SongsListView.SelectedItems)
                    selectedSongsIds.Add(item.Id);

                foreach (Controllers.Song.Result item in selected)
                    DBSongsIds.Add(item.Id);

                {
                    List<int> toRemove = new List<int>(DBSongsIds.Except(selectedSongsIds));
                    foreach (int i in toRemove)
                        await albumController.DeleteSong(i, elementID.Value);
                }

                {
                    int s = DBSongsIds.Count;
                    List<int> toAdd = new List<int>(selectedSongsIds.Except(DBSongsIds));
                    foreach (int i in toAdd)
                        await albumController.AddSong(elementID.Value, i, ++s);
                }
            }

            mainPage.GoBack();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            mainPage.GoBack();
        }

     
    }
}
