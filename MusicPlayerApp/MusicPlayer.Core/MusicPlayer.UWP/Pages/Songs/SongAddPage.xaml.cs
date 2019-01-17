﻿using MusicPlayer.UWP.Controllers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private GenreController genreController;
        private AlbumController albumController;
        private ArtistController artistController;

        private ObservableRangeCollection<Controllers.Album.Result> AllAlbums = new ObservableRangeCollection<Controllers.Album.Result>();
        private ObservableRangeCollection<Controllers.Genre.Result> AllGenres = new ObservableRangeCollection<Controllers.Genre.Result>();
        private ObservableRangeCollection<Controllers.Artist.Result> AllArtists = new ObservableRangeCollection<Controllers.Artist.Result>();


        public SongAddPage()
        {
            this.InitializeComponent();
            songController = new SongController(App.QueryDispatcher, App.CommandDispatcher);
            genreController = new GenreController(App.QueryDispatcher, App.CommandDispatcher);
            albumController = new AlbumController(App.QueryDispatcher, App.CommandDispatcher);
            artistController = new ArtistController(App.QueryDispatcher, App.CommandDispatcher);

            var frame = (Frame)Window.Current.Content;
            mainPage = (MainPage)frame.Content;

            LoadAlbums();
            LoadGenres();
            LoadArtists();
        }

        private async void LoadAlbums()
        {
            List<Controllers.Album.Result> temp = await albumController.GetAll();

            AllAlbums.AddRange(temp);

            AlbumsListView.ItemsSource = AllAlbums;
            AllAlbums.CollectionChanged += AllBands_CollectionChanged;
        }

        private async void LoadGenres()
        {
            List<Controllers.Genre.Result> temp = await genreController.GetAll();

            AllGenres.AddRange(temp);

            GenreComboBox.ItemsSource = AllGenres;
            AllGenres.CollectionChanged += AllBands_CollectionChanged;

            GenreComboBox.SelectedIndex = 0;
        }

        private async void LoadArtists()
        {
            List<Controllers.Artist.Result> temp = await artistController.GetAll();

            AllArtists.AddRange(temp);

            ArtistsListView.ItemsSource = AllArtists;
            AllArtists.CollectionChanged += AllBands_CollectionChanged;
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
            int score = Convert.ToInt32(ScoreRating.Value);
            string filePath = FileTextBox.Text;
            DateTime creation = CreationDateCalendar.Date.HasValue ? CreationDateCalendar.Date.Value.DateTime : DateTime.Now;

            Controllers.Genre.Result selectedGenre = (Controllers.Genre.Result)GenreComboBox.SelectedItem;

            int id = await songController.Create(score, name, creation, filePath, null, selectedGenre.Id);


            foreach (Controllers.Album.Result album in AlbumsListView.SelectedItems)
            {
                var selected = await albumController.GetSongs(album.Id);
                int s = selected.Count + 1;

                await albumController.AddSong(album.Id, id, s);
            }


            foreach (Controllers.Artist.Result artist in ArtistsListView.SelectedItems)
            {
                await artistController.AddSong(artist.Id, id);
            }

            mainPage.GoBack();
        }


        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            mainPage.GoBack();
        }


    }
}
