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
        private ImageController imageController;

        private ObservableRangeCollection<Controllers.Song.Result> AllSongs = new ObservableRangeCollection<Controllers.Song.Result>();
        private SongController songController;

        public AlbumEditPage()
        {
            this.InitializeComponent();

            albumController = new AlbumController(App.QueryDispatcher, App.CommandDispatcher);
            songController = new SongController(App.QueryDispatcher, App.CommandDispatcher);
            imageController = new ImageController(App.QueryDispatcher, App.CommandDispatcher);

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

            Controllers.Image.Result image = await imageController.Get(album.ImageId);
            ImageFileTextBox.Text = image.FilePath;

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
            if(name == "")
            {
                DisplayNoNameDialog();
                return;
            }


            DateTime creation = CreationDateCalendar.Date.HasValue ? CreationDateCalendar.Date.Value.DateTime : DateTime.Now;

            string description = string.Empty;
            DescriptionRichBox.Document.GetText(Windows.UI.Text.TextGetOptions.FormatRtf, out description);



            int image_id;
            {
                Controllers.Album.Result artist = await albumController.Get(elementID.Value);
                image_id = artist.ImageId;

                if (ImageFileTextBox.Text == "")
                    await imageController.Update(image_id, new MusicPlayer.Core.NullObjects.ImageNullObject().FilePath);
                else
                    await imageController.Update(image_id, ImageFileTextBox.Text);
            }


            await albumController.Update(elementID.Value, name, description, creation, image_id);

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

        private async void DisplayNoNameDialog()
        {
            ContentDialog noWifiDialog = new ContentDialog
            {
                Title = "No album name provided!",
                Content = "Please enter the name of the album and try again.",
                CloseButtonText = "Ok"
            };

            ContentDialogResult result = await noWifiDialog.ShowAsync();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            mainPage.GoBack();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker
            {
                ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail,
                SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary
            };
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");

            Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                // Application now has read/write access to the picked file

                ImageFileTextBox.Text = file.Path;
            }
            else
            {
                //Operation cancelled
            }
        }
    }
}
