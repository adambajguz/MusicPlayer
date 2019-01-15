using MusicPlayer.UWP.Controllers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;


namespace MusicPlayer.UWP.Pages.Albums
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AlbumDetailsPage : Page
    {
        private WriteOnce<int> elementID = new WriteOnce<int>();

        private readonly MainPage mainPage;
        private BandController bandController;
        private ArtistController artistController;

        private ObservableRangeCollection<Controllers.Artist.Result> artists = new ObservableRangeCollection<Controllers.Artist.Result>();


        public AlbumDetailsPage()
        {
            this.InitializeComponent();

            bandController = new BandController(App.QueryDispatcher, App.CommandDispatcher);
            artistController = new ArtistController(App.QueryDispatcher, App.CommandDispatcher);

            artists.CollectionChanged += Genres_CollectionChanged;

            var frame = (Frame)Window.Current.Content;
            mainPage = (MainPage)frame.Content;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            elementID.Value = (int)e.Parameter;
            Controllers.Band.Result band = await bandController.Get(elementID.Value);

            if (band == null)
            {
                mainPage.GoBack();
                return;
            }


            NameTextBox.Text = band.name;

            string end = band.EndDate == null ? "..." : band.EndDate.ToString();
            CreationEndTextBox.Text = "(" + band.CreationData.ToLongDateString() + " - " + end + ")";

            artists.AddRange(await bandController.GetArtists(elementID.Value));
            GenresListView.ItemsSource = artists;

            DescriptionRichBox.Document.SetText(Windows.UI.Text.TextSetOptions.FormatRtf, band.Description);
        }

      
        private void Genres_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var x = e.NewItems;
        }

        //private async void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        //{
        //    if (sender is MenuFlyoutItem selectedItem)
        //    {
        //        List<Controllers.Artist.Result> temp;
        //        artists.Clear();

        //        string sortOption = selectedItem.Tag.ToString();
        //        switch (sortOption)
        //        {
        //            case "az":
        //                temp = await bandController.GetArtists(elementID.Value);
        //                artists.AddRange(temp);

        //                break;

        //            case "za":
        //                temp = await bandController.GetArtists(elementID.Value);
        //                artists.AddRange(temp);

        //                break;

        //        }
        //    }
        //}

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is AppBarButton selectedItem)
            {
                switch (selectedItem.Tag.ToString())
                {
                    case "Add":
                        mainPage.NavView_Navigate(MainPage.ArtistAddTag, new EntranceNavigationTransitionInfo(), null);

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

                    Controllers.Artist.Result selectedArtist = await artistController.Get(id);

                    switch (selectedItem.Name.ToString())
                    {
                        case "IDetails":
                            mainPage.NavView_Navigate(MainPage.ArtistDetailsTag, new EntranceNavigationTransitionInfo(), selectedArtist.Id);

                            break;

                        case "IEdit":
                            mainPage.NavView_Navigate(MainPage.ArtistEditTag, new EntranceNavigationTransitionInfo(), selectedArtist.Id);

                            break;

                        case "IRemove":
                            DisplayDeleteSingleDialog(selectedArtist);

                            break;
                    }
                }


            }
        }

        private async void DisplayDeleteSingleDialog(Controllers.Artist.Result bandToDelete)
        {
            ContentDialog deleteFileDialog = new ContentDialog
            {
                Title = "Delete '" + bandToDelete.Name + "' permanently?",
                Content = "If you delete this band, you won't be able to recover it. Do you want to delete it?",
                PrimaryButtonText = "Delete",
                CloseButtonText = "Cancel"
            };

            ContentDialogResult result = await deleteFileDialog.ShowAsync();


            if (result == ContentDialogResult.Primary)
            {
                // Delete

                await artistController.Delete(bandToDelete.Id);

                artists.Clear();
                artists.AddRange(await bandController.GetArtists(elementID.Value));
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
                foreach (Controllers.Artist.Result genre in genresToDelete)
                    await artistController.Delete(genre.Id);

                artists.Clear();
                artists.AddRange(await bandController.GetArtists(elementID.Value));
            }
            else
            {
                // The user clicked the CLoseButton, pressed ESC, Gamepad B, or the system back button.
                // Do nothing.
            }
        }
    }
}
