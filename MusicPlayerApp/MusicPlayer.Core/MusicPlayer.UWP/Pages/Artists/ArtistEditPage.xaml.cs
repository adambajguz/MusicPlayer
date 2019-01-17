using MusicPlayer.UWP.Controllers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace MusicPlayer.UWP.Pages.Artists
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ArtistEditPage : Page
    {
        private WriteOnce<int> elementID = new WriteOnce<int>();

        private readonly MainPage mainPage;
        private ArtistController artistController;

        private ObservableRangeCollection<Controllers.Band.Result> AllBands = new ObservableRangeCollection<Controllers.Band.Result>();

        public ArtistEditPage()
        {
            this.InitializeComponent();

            artistController = new ArtistController(App.QueryDispatcher, App.CommandDispatcher);

            var frame = (Frame)Window.Current.Content;
            mainPage = (MainPage)frame.Content;
            LoadBands();
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

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            elementID.Value = (int)e.Parameter;
            Controllers.Artist.Result artist = await artistController.Get(elementID.Value);

            if (artist == null)
            {
                mainPage.GoBack();
                return;
            }


            NameTextBox.Text = artist.Name;
            SurnameTextBox.Text = artist.Surname;
            PseudonymTextBox.Text = artist.Pseudonym;

            int? bandID = artist.BandId;
            if (bandID != null)
            {
                BandToogle.IsOn = true;

                BandController bandController = new BandController(App.QueryDispatcher, App.CommandDispatcher);
                Controllers.Band.Result band = await bandController.Get((int)artist.BandId);

                foreach (Controllers.Band.Result item in BandComboBox.Items)
                    if (item.Id == bandID)
                        BandComboBox.SelectedItem = item;
            }
            else
            {
                BandComboBox.Visibility = Visibility.Collapsed;
            }

            BirthdayDatePicker.Date = artist.Birthdate;

            DescriptionRichBox.Document.SetText(Windows.UI.Text.TextSetOptions.FormatRtf, artist.Description);
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

            if (name == "" && surname == "" && pseudonym == "")
            {
                DisplayNoNameDialog();
                return;
            }

            DateTime birth = BirthdayDatePicker.SelectedDate != null ? BirthdayDatePicker.Date.DateTime : DateTime.Now;

            int? bandId = BandToogle.IsOn ? (int?)((Controllers.Band.Result)BandComboBox.SelectedItem).Id : null;

            string description = string.Empty;
            DescriptionRichBox.Document.GetText(Windows.UI.Text.TextGetOptions.FormatRtf, out description);

            await artistController.Update(elementID.Value, name, surname, pseudonym, birth, description, bandId, 1);

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
    }
}
