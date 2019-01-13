using MusicPlayer.UWP.Controllers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;


namespace MusicPlayer.UWP.Pages.Songs
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SongAddPage : Page
    {
        private readonly MainPage mainPage;
        private SongController songController;

        private ObservableRangeCollection<Controllers.Artist.Result> AllArtists = new ObservableRangeCollection<Controllers.Artist.Result>();


        public SongAddPage()
        {
            this.InitializeComponent();
            songController = new SongController(App.QueryDispatcher, App.CommandDispatcher);

            var frame = (Frame)Window.Current.Content;
            mainPage = (MainPage)frame.Content;

            LoadBands();

            ArtistComboBox.Visibility = Visibility.Collapsed;
        }

        private async void LoadBands()
        {
            ArtistController artistController = new ArtistController(App.QueryDispatcher, App.CommandDispatcher);

            List<Controllers.Artist.Result> temp = await artistController.GetAll();
                    
            AllArtists.AddRange(temp);
       
            ArtistComboBox.ItemsSource = AllArtists;
            AllArtists.CollectionChanged += AllBands_CollectionChanged;

            ArtistComboBox.SelectedIndex = 0;
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

            DateTime birth = BirthdayDatePicker.SelectedDate != null ? BirthdayDatePicker.Date.DateTime : DateTime.Now;

            int? bandId = BandToogle.IsOn ? (int?)((Controllers.Band.Result)ArtistComboBox.SelectedItem).Id : null;

            string description = string.Empty;
            DescriptionRichBox.Document.GetText(Windows.UI.Text.TextGetOptions.FormatRtf, out description);

            await songController.Create(0, name, birth, pseudonym, null, 1);

            mainPage.GoBack();
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
                    ArtistComboBox.Visibility = Visibility.Visible;
                else
                    ArtistComboBox.Visibility = Visibility.Collapsed;
            }
        }
    }
}
