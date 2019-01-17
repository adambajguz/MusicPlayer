using MusicPlayer.UWP.Controllers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;


namespace MusicPlayer.UWP.Pages.Artists
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ArtistAddPage : Page
    {
        private readonly MainPage mainPage;
        private ArtistController artistController;
        private ImageController imageController;

        private ObservableRangeCollection<Controllers.Band.Result> AllBands = new ObservableRangeCollection<Controllers.Band.Result>();


        public ArtistAddPage()
        {
            this.InitializeComponent();
            artistController = new ArtistController(App.QueryDispatcher, App.CommandDispatcher);
            imageController = new ImageController(App.QueryDispatcher, App.CommandDispatcher);

            var frame = (Frame)Window.Current.Content;
            mainPage = (MainPage)frame.Content;

            LoadBands();

            BandComboBox.Visibility = Visibility.Collapsed;
        }

        private async void LoadBands()
        {
            BandController bandController = new BandController(App.QueryDispatcher, App.CommandDispatcher);

            List<Controllers.Band.Result> temp = await bandController.GetAll();
                    
            AllBands.AddRange(temp);
       
            BandComboBox.ItemsSource = AllBands;
            AllBands.CollectionChanged += AllBands_CollectionChanged;

            BandComboBox.SelectedIndex = 0;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

        }
    
        private void AllBands_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var x = e.NewItems;
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string name = NameTextBox.Text;
            string surname = SurnameTextBox.Text;
            string pseudonym = PseudonymTextBox.Text;

            if(name == "" && surname == "" && pseudonym == "")
            {
                DisplayNoNameDialog();
                return;
            }

            DateTime birth = BirthdayDatePicker.SelectedDate != null ? BirthdayDatePicker.Date.DateTime : DateTime.Now;

            int? bandId = BandToogle.IsOn ? (int?)((Controllers.Band.Result)BandComboBox.SelectedItem).Id : null;

            string description = string.Empty;
            DescriptionRichBox.Document.GetText(Windows.UI.Text.TextGetOptions.FormatRtf, out description);

            int image_id;
            if (ImageFileTextBox.Text == "")
                image_id = await imageController.Create(new MusicPlayer.Core.NullObjects.ImageNullObject().FilePath);
            else
                image_id = await imageController.Create(ImageFileTextBox.Text);

            await artistController.Create(name, surname, pseudonym, birth, description, bandId, image_id);

            mainPage.GoBack();
        }

        private async void DisplayNoNameDialog()
        {
            ContentDialog noWifiDialog = new ContentDialog
            {
                Title = "No name/surname/pseudonym provided!",
                Content = "Artist must have name, surname or pseudonym. Please correct and try again.",
                CloseButtonText = "Ok"
            };

            ContentDialogResult result = await noWifiDialog.ShowAsync();
        }


        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            mainPage.GoBack();
        }


        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            if (sender is ToggleSwitch toggleSwitch)
            {
                if (toggleSwitch.IsOn == true)
                    BandComboBox.Visibility = Visibility.Visible;
                else
                    BandComboBox.Visibility = Visibility.Collapsed;
            }
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
